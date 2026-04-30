import { useState } from "react";
import { Check, Edit3, Plus, Trash2 } from "lucide-react";
import { EmptyTableRow } from "../../components/EmptyTableRow";
import { IconButton } from "../../components/IconButton";
import { Modal } from "../../components/Modal";
import { Pagination } from "../../components/Pagination";
import { createCategory, deleteCategory, listCategories, updateCategory } from "../../services/shopService";

const PAGE_SIZE = 10;

export function CategoriesPage() {
  const [tick, setTick] = useState(0);
  const [page, setPage] = useState(1);
  const [open, setOpen] = useState(false);
  const [name, setName] = useState("");
  const [parentId, setParentId] = useState("");
  const [editingId, setEditingId] = useState("");

  const categories = listCategories();
  void tick;

  const totalPages = Math.max(1, Math.ceil(categories.length / PAGE_SIZE));
  const rows = categories.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);

  return (
    <div className="space-y-6">
      <div className="flex items-end justify-between">
        <div><h2 className="text-2xl font-bold">Categories</h2><p className="text-sm text-slate-400">Separate category management</p></div>
        <button className="rounded bg-cyan-600 px-3 py-2" onClick={() => setOpen(true)}><Plus className="h-4 w-4" /></button>
      </div>

      <section className="rounded-xl border border-slate-800 bg-slate-950/60 p-4">
        <table className="w-full text-left text-sm">
          <thead className="text-slate-400"><tr><th className="pb-2">Name</th><th className="pb-2">Level</th><th className="pb-2">Parent</th><th className="pb-2">Actions</th></tr></thead>
          <tbody>
            {rows.length === 0 ? <EmptyTableRow colSpan={4} /> : rows.map((c) => (
              <tr key={c.id} className="border-t border-slate-800">
                <td className="py-2">{c.name}</td>
                <td className="py-2">{c.level}</td>
                <td className="py-2">{c.parentId ? categories.find((x) => x.id === c.parentId)?.name ?? "-" : "-"}</td>
                <td className="py-2"><div className="flex gap-2"><IconButton title="Rename" icon={<Edit3 size={14} />} onClick={() => { setEditingId(c.id); setName(c.name); setParentId(c.parentId ?? ""); setOpen(true); }} /><IconButton title="Delete" icon={<Trash2 size={14} />} variant="danger" onClick={() => { deleteCategory(c.id); setTick((v) => v + 1); }} /></div></td>
              </tr>
            ))}
          </tbody>
        </table>
        <Pagination page={page} totalPages={totalPages} onChange={setPage} />
      </section>

      <Modal open={open} title="Category" onClose={() => setOpen(false)}>
        <div className="space-y-2 text-sm">
          <input value={name} onChange={(e) => setName(e.target.value)} placeholder="Category name" className="w-full rounded border border-slate-700 bg-slate-950 px-3 py-2" />
          <select value={parentId} onChange={(e) => setParentId(e.target.value)} className="w-full rounded border border-slate-700 bg-slate-950 px-3 py-2">
            <option value="">No parent (level 1)</option>
            {categories.filter((c) => c.level < 3).map((c) => <option key={c.id} value={c.id}>{"-".repeat(c.level - 1)} {c.name}</option>)}
          </select>
          <button className="rounded bg-cyan-600 px-3 py-2" onClick={() => { if (!name.trim()) return; if (editingId) updateCategory(editingId, name.trim()); else createCategory(name.trim(), parentId || null); setOpen(false); setName(""); setParentId(""); setEditingId(""); setTick((v) => v + 1); }}><Check className="h-4 w-4" /></button>
        </div>
      </Modal>
    </div>
  );
}
