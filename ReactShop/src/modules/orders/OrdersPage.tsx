import { useState } from "react";
import { Circle, CircleAlert, CircleCheck, Edit3, Trash2 } from "lucide-react";
import { EmptyTableRow } from "../../components/EmptyTableRow";
import { IconButton } from "../../components/IconButton";
import { Modal } from "../../components/Modal";
import { Pagination } from "../../components/Pagination";
import { deleteOrder, formatMoney, getStatusTone, listOrders, updateOrderStatus } from "../../services/appService";
import type { OrderStatus } from "../../types/domain";

const PAGE_SIZE = 10;

export function OrdersPage() {
  const [tick, setTick] = useState(0);
  const [page, setPage] = useState(1);
  const [open, setOpen] = useState(false);
  const [currentOrderId, setCurrentOrderId] = useState("");
  const [nextStatus, setNextStatus] = useState<OrderStatus>("placed");

  const orders = listOrders();
  void tick;
  const totalPages = Math.max(1, Math.ceil(orders.length / PAGE_SIZE));
  const rows = orders.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);

  return (
    <div className="space-y-6">
      <div><h2 className="text-2xl font-bold">Orders</h2><p className="text-sm text-slate-400">No create action, only status update and delete</p></div>
      <section className="rounded-xl border border-slate-800 bg-slate-950/60 p-4">
        <table className="w-full text-left text-sm">
          <thead className="text-slate-400"><tr><th className="pb-2">Order</th><th className="pb-2">Customer</th><th className="pb-2">Items</th><th className="pb-2">Total</th><th className="pb-2">Status</th><th className="pb-2">Fn</th></tr></thead>
          <tbody>{rows.length === 0 ? <EmptyTableRow colSpan={6} /> : rows.map((o) => <tr key={o.id} className="border-t border-slate-800"><td className="py-2 text-xs text-slate-400">{o.id}</td><td className="py-2">{o.customerName}</td><td className="py-2">{o.items.map((i) => `${i.skuCode} x${i.quantity}`).join(", ")}</td><td className="py-2">{formatMoney(o.total)}</td><td className="py-2" title={o.status}>{getStatusTone(o.status) === "success" ? <CircleCheck className="h-4 w-4 text-emerald-400" /> : getStatusTone(o.status) === "warning" ? <Circle className="h-4 w-4 text-amber-400" /> : <CircleAlert className="h-4 w-4 text-rose-400" />}</td><td className="py-2"><div className="flex gap-2"><IconButton title="Status" icon={<Edit3 size={14} />} onClick={() => { setCurrentOrderId(o.id); setNextStatus(o.status); setOpen(true); }} /><IconButton title="Delete" icon={<Trash2 size={14} />} variant="danger" onClick={() => { deleteOrder(o.id); setTick((v) => v + 1); }} /></div></td></tr>)}</tbody>
        </table>
        <Pagination page={page} totalPages={totalPages} onChange={setPage} />
      </section>
      <Modal open={open} title="Update Order Status" onClose={() => setOpen(false)}><div className="space-y-2 text-sm"><select value={nextStatus} onChange={(e) => setNextStatus(e.target.value as OrderStatus)} className="w-full rounded border border-slate-700 bg-slate-950 px-3 py-2"><option value="placed">placed</option><option value="paid">paid</option><option value="cancelled">cancelled</option></select><button className="rounded bg-cyan-600 px-3 py-2" onClick={() => { updateOrderStatus(currentOrderId, nextStatus); setOpen(false); setTick((v) => v + 1); }}><CircleCheck className="h-4 w-4" /></button></div></Modal>
    </div>
  );
}
