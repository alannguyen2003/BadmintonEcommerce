import { useState } from "react";
import { Check, CircleAlert, CircleCheck, CircleDashed, Plus, Trash2 } from "lucide-react";
import { EmptyTableRow } from "../../components/EmptyTableRow";
import { IconButton } from "../../components/IconButton";
import { Modal } from "../../components/Modal";
import { Pagination } from "../../components/Pagination";
import { createInventoryTransaction, deleteInventoryTransaction, formatMoney, listInventoryTransactions, listSkus } from "../../services/appService";
import type { InventoryAction } from "../../types/domain";

const PAGE_SIZE = 10;

export function InventoryPage() {
  const [tick, setTick] = useState(0);
  const [skuPage, setSkuPage] = useState(1);
  const [txPage, setTxPage] = useState(1);
  const [open, setOpen] = useState(false);
  const [skuId, setSkuId] = useState("");
  const [action, setAction] = useState<InventoryAction>("import");
  const [quantity, setQuantity] = useState(0);
  const [note, setNote] = useState("");

  const skus = listSkus();
  const txs = listInventoryTransactions();
  void tick;

  const skuTotal = Math.max(1, Math.ceil(skus.length / PAGE_SIZE));
  const skuRows = skus.slice((skuPage - 1) * PAGE_SIZE, skuPage * PAGE_SIZE);
  const txTotal = Math.max(1, Math.ceil(txs.length / PAGE_SIZE));
  const txRows = txs.slice((txPage - 1) * PAGE_SIZE, txPage * PAGE_SIZE);

  return (
    <div className="space-y-6">
      <div className="flex items-end justify-between">
        <div><h2 className="text-2xl font-bold">Inventory</h2><p className="text-sm text-slate-400">SKU-based stock management</p></div>
        <button className="rounded bg-cyan-600 px-3 py-2" onClick={() => setOpen(true)}><Plus className="h-4 w-4" /></button>
      </div>

      <section className="rounded-xl border border-slate-800 bg-slate-950/60 p-4">
        <table className="w-full text-left text-sm">
          <thead className="text-slate-400"><tr><th className="pb-2">SKU</th><th className="pb-2">Product</th><th className="pb-2">Price</th><th className="pb-2">Stock</th></tr></thead>
          <tbody>{skuRows.length === 0 ? <EmptyTableRow colSpan={4} /> : skuRows.map((s) => <tr key={s.id} className="border-t border-slate-800"><td className="py-2">{s.code}</td><td className="py-2">{s.productName}</td><td className="py-2">{formatMoney(s.price)}</td><td className="py-2">{s.stock}</td></tr>)}</tbody>
        </table>
        <Pagination page={skuPage} totalPages={skuTotal} onChange={setSkuPage} />
      </section>

      <section className="rounded-xl border border-slate-800 bg-slate-950/60 p-4">
        <table className="w-full text-left text-sm">
          <thead className="text-slate-400"><tr><th className="pb-2">Time</th><th className="pb-2">SKU</th><th className="pb-2">Act</th><th className="pb-2">Qty</th><th className="pb-2">Note</th><th className="pb-2">Fn</th></tr></thead>
          <tbody>{txRows.length === 0 ? <EmptyTableRow colSpan={6} /> : txRows.map((t) => <tr key={t.id} className="border-t border-slate-800"><td className="py-2">{new Date(t.createdAt).toLocaleString()}</td><td className="py-2">{skus.find((s) => s.id === t.skuId)?.code ?? "-"}</td><td className="py-2" title={t.action}>{t.action === "import" ? <CircleCheck className="h-4 w-4 text-emerald-400" /> : t.action === "export" ? <CircleAlert className="h-4 w-4 text-amber-400" /> : <CircleDashed className="h-4 w-4 text-cyan-400" />}</td><td className="py-2">{t.quantity}</td><td className="py-2">{t.note || "-"}</td><td className="py-2"><IconButton title="Delete" icon={<Trash2 size={14} />} variant="danger" onClick={() => { deleteInventoryTransaction(t.id); setTick((v) => v + 1); }} /></td></tr>)}</tbody>
        </table>
        <Pagination page={txPage} totalPages={txTotal} onChange={setTxPage} />
      </section>

      <Modal open={open} title="Inventory Transaction" onClose={() => setOpen(false)}>
        <div className="space-y-2 text-sm">
          <select value={skuId} onChange={(e) => setSkuId(e.target.value)} className="w-full rounded border border-slate-700 bg-slate-950 px-3 py-2"><option value="">Select SKU</option>{skus.map((s) => <option key={s.id} value={s.id}>{s.code}</option>)}</select>
          <div className="grid grid-cols-2 gap-2"><select value={action} onChange={(e) => setAction(e.target.value as InventoryAction)} className="rounded border border-slate-700 bg-slate-950 px-3 py-2"><option value="import">import</option><option value="export">export</option><option value="adjust">adjust</option></select><input type="number" min={0} value={quantity} onChange={(e) => setQuantity(Number(e.target.value))} className="rounded border border-slate-700 bg-slate-950 px-3 py-2" /></div>
          <input value={note} onChange={(e) => setNote(e.target.value)} className="w-full rounded border border-slate-700 bg-slate-950 px-3 py-2" placeholder="Note" />
          <button className="rounded bg-cyan-600 px-3 py-2" onClick={() => { if (!skuId || quantity <= 0) return; createInventoryTransaction(skuId, action, quantity, note); setOpen(false); setSkuId(""); setQuantity(0); setNote(""); setTick((v) => v + 1); }}><Check className="h-4 w-4" /></button>
        </div>
      </Modal>
    </div>
  );
}
