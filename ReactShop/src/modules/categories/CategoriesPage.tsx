import { useState, useEffect } from "react";
import { Check, Edit3, Plus, Trash2 } from "lucide-react";
import { EmptyTableRow } from "../../components/EmptyTableRow";
import { IconButton } from "../../components/IconButton";
import { Modal } from "../../components/Modal";
import { Pagination } from "../../components/Pagination";
import {
  createCategory,
  deleteCategory,
  listCategories,
  updateCategory,
} from "../../services/shopService";
import type { ProductCategory } from "../../types/product";

const PAGE_SIZE = 10;

export function CategoriesPage() {
  const [categories, setCategories] = useState<ProductCategory[]>([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [open, setOpen] = useState(false);
  const [name, setName] = useState("");
  const [parentId, setParentId] = useState("");
  const [editingId, setEditingId] = useState("");

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const data = await listCategories();
        setCategories(data);
      } catch (error) {
        console.error("Failed to fetch categories:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchCategories();
  }, []);

  const handleDelete = async (id: string) => {
    try {
      await deleteCategory(id);
      const data = await listCategories();
      setCategories(data);
    } catch (error) {
      console.error("Failed to delete category:", error);
    }
  };

  const handleSave = async () => {
    if (!name.trim()) return;
    try {
      if (editingId) {
        await updateCategory(editingId, name.trim());
      } else {
        console.log(parentId);
        await createCategory(name.trim(), parentId || null);
      }
      const data = await listCategories();
      setCategories(data);
      setOpen(false);
      setName("");
      setParentId("");
      setEditingId("");
    } catch (error) {
      console.error("Failed to save category:", error);
    }
  };

  const totalPages = Math.max(1, Math.ceil(categories.length / PAGE_SIZE));
  const rows = categories.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);
  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <div className="space-y-6">
      <div className="flex items-end justify-between">
        <div>
          <h2 className="text-2xl font-bold">Categories</h2>
          <p className="text-sm text-slate-400">Separate category management</p>
        </div>
        <button
          className="rounded bg-cyan-600 px-3 py-2"
          onClick={() => setOpen(true)}
        >
          <Plus className="h-4 w-4" />
        </button>
      </div>

      <section className="rounded-xl border border-slate-800 bg-slate-950/60 p-4">
        <table className="w-full text-left text-sm">
          <thead className="text-slate-400">
            <tr>
              <th className="pb-2">Name</th>
              <th className="pb-2">Level</th>
              <th className="pb-2">Parent</th>
              <th className="pb-2">Actions</th>
            </tr>
          </thead>
          <tbody>
            {rows.length === 0 ? (
              <EmptyTableRow colSpan={4} />
            ) : (
              rows.map((c) => (
                <tr key={c.id} className="border-t border-slate-800">
                  <td className="py-2">{c.categoryName}</td>
                  <td className="py-2">{c.level}</td>
                  <td className="py-2">
                    {c.parentCategoryId 
                      ? (categories.find(
                          (x) => x.parentCategoryId === c.parentCategoryId,
                        )?.parentCategoryName ?? "-")
                      : "-"}
                  </td>
                  <td className="py-2">
                    <div className="flex gap-2">
                      <IconButton
                        title="Rename"
                        icon={<Edit3 size={14} />}
                        onClick={() => {
                          setEditingId(c.id);
                          setName(c.categoryName);
                          setParentId(c.parentCategoryId ?? "");
                          setOpen(true);
                        }}
                      />
                      <IconButton
                        title="Delete"
                        icon={<Trash2 size={14} />}
                        variant="danger"
                        onClick={() => handleDelete(c.id)}
                      />
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
        <Pagination page={page} totalPages={totalPages} onChange={setPage} />
      </section>

      <Modal open={open} title="Category" onClose={() => setOpen(false)}>
        <div className="space-y-2 text-sm">
          <input
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="Category name"
            className="w-full rounded border border-slate-700 bg-slate-950 px-3 py-2"
          />
          <select
            value={parentId}
            onChange={(e) => setParentId(e.target.value)}
            className="w-full rounded border border-slate-700 bg-slate-950 px-3 py-2"
          >
            <option value="">No parent (level 1)</option>
            {categories
              .filter((c) => c.level < 3)
              .map((c) => (
                <option key={c.id} value={c.id}>
                  {"-".repeat(c.level - 1)} {c.categoryName}
                </option>
              ))}
          </select>
          <button
            className="rounded bg-cyan-600 px-3 py-2"
            onClick={handleSave}
          >
            <Check className="h-4 w-4" />
          </button>
        </div>
      </Modal>
    </div>
  );
}
