import { useMemo, useState } from "react";
import { Check, Circle, CircleAlert, CircleCheck, Edit3, ImagePlus, Plus, Star, Trash2 } from "lucide-react";
import { EmptyTableRow } from "../../components/EmptyTableRow";
import { IconButton } from "../../components/IconButton";
import { Modal } from "../../components/Modal";
import { Pagination } from "../../components/Pagination";
import { deleteProduct, getStatusTone, listCategories, listProducts, upsertProduct } from "../../services/shopService";
import type { Product, ProductImage, ProductOption } from "../../types/domain";

const PAGE_SIZE = 8;
const emptyOption = (): ProductOption => ({ id: crypto.randomUUID(), name: "", values: [] });
const formatInteger = (value: number) => new Intl.NumberFormat("vi-VN").format(value || 0);

export function ProductsPage() {
  const [tick, setTick] = useState(0);
  const [query, setQuery] = useState("");
  const [page, setPage] = useState(1);

  const [productModalOpen, setProductModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [editing, setEditing] = useState<(Product & { skus: Array<{ id: string; code: string; price: number; stock: number; optionValues: Record<string, string> }> }) | null>(null);
  const [deletingProductId, setDeletingProductId] = useState("");

  const [name, setName] = useState("");
  const [categoryId, setCategoryId] = useState("");
  const [status, setStatus] = useState<"draft" | "active">("draft");
  const [description, setDescription] = useState("");
  const [images, setImages] = useState<ProductImage[]>([]);
  const [options, setOptions] = useState<ProductOption[]>([emptyOption()]);
  const [skuRows, setSkuRows] = useState<Array<{ comboKey: string; code: string; price: number; stock: number; optionValues: Record<string, string> }>>([]);

  const categories = listCategories();
  const products = listProducts();
  void tick;

  const filteredProducts = useMemo(() => products.filter((p) => p.name.toLowerCase().includes(query.toLowerCase())), [products, query]);
  const totalPages = Math.max(1, Math.ceil(filteredProducts.length / PAGE_SIZE));
  const rows = filteredProducts.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);

  const categoryPath = (id: string) => {
    const node = categories.find((c) => c.id === id);
    if (!node) return "-";
    const chain = [node.name];
    let current = node;
    while (current.parentId) {
      const parent = categories.find((c) => c.id === current.parentId);
      if (!parent) break;
      chain.unshift(parent.name);
      current = parent;
    }
    return chain.join(" > ");
  };

  const resetProductForm = () => {
    setEditing(null);
    setName("");
    setCategoryId("");
    setStatus("draft");
    setDescription("");
    setImages([]);
    setOptions([emptyOption()]);
    setSkuRows([]);
  };

  const onUploadImages = (files: FileList | null) => {
    if (!files) return;
    const tasks = Array.from(files).map(
      (file) =>
        new Promise<ProductImage>((resolve) => {
          const reader = new FileReader();
          reader.onload = () => resolve({ id: crypto.randomUUID(), name: file.name, dataUrl: String(reader.result), isPrimary: false });
          reader.readAsDataURL(file);
        }),
    );
    Promise.all(tasks).then((newItems) => {
      setImages((prev) => {
        const merged = [...prev, ...newItems];
        if (!merged.some((x) => x.isPrimary) && merged[0]) merged[0].isPrimary = true;
        return [...merged];
      });
    });
  };

  const addOptionValue = (optionId: string, value = "") => {
    setOptions((prev) => prev.map((o) => (o.id === optionId ? { ...o, values: [...o.values, value] } : o)));
  };

  const removeOptionValue = (optionId: string, valueIdx: number) => {
    setOptions((prev) =>
      prev.map((o) =>
        o.id === optionId
          ? { ...o, values: o.values.filter((_, idx) => idx !== valueIdx) }
          : o,
      ),
    );
  };

  const regenerateSkuFromOptions = () => {
    const validOptions = options
      .map((o) => ({ ...o, name: o.name.trim(), values: o.values.map((v) => v.trim()).filter(Boolean) }))
      .filter((o) => o.name && o.values.length);

    if (!validOptions.length) {
      setSkuRows([]);
      return;
    }

    const combos = validOptions.reduce<Record<string, string>[]>((acc, option) => {
      if (!acc.length) return option.values.map((v) => ({ [option.id]: v }));
      const next: Record<string, string>[] = [];
      for (const row of acc) for (const value of option.values) next.push({ ...row, [option.id]: value });
      return next;
    }, []);

    setSkuRows(
      combos.map((optionValues, idx) => ({
        comboKey: JSON.stringify(optionValues),
        optionValues,
        code: `SKU-${idx + 1}`,
        price: 0,
        stock: 0,
      })),
    );
  };

  const addManualSku = () => {
    setSkuRows((prev) => [
      ...prev,
      {
        comboKey: `manual_${crypto.randomUUID()}`,
        optionValues: {},
        code: `SKU-M-${prev.length + 1}`,
        price: 0,
        stock: 0,
      },
    ]);
  };

  const submitProduct = () => {
    if (!name.trim() || !categoryId) return;
    const normalizedImages = images.map((img, idx) => ({ ...img, isPrimary: idx === images.findIndex((i) => i.isPrimary) }));
    upsertProduct({
      id: editing?.id,
      name: name.trim(),
      categoryId,
      status,
      description: description.trim(),
      images: normalizedImages,
      options: options.map((o) => ({ ...o, name: o.name.trim(), values: o.values.map((v) => v.trim()).filter(Boolean) })).filter((o) => o.name && o.values.length),
      skuRows,
    });
    setProductModalOpen(false);
    resetProductForm();
    setTick((v) => v + 1);
  };

  return (
    <div className="space-y-6">
      <div className="flex items-end justify-between">
        <div><h2 className="text-2xl font-bold">Products</h2><p className="text-sm text-slate-400">Product table and modal CRUD</p></div>
        <div className="flex gap-2">
          <input value={query} onChange={(e) => { setPage(1); setQuery(e.target.value); }} placeholder="Search" className="rounded border border-slate-700 bg-slate-900 px-3 py-2 text-sm" />
          <button className="rounded bg-cyan-600 px-3 py-2" onClick={() => { resetProductForm(); setProductModalOpen(true); }}><Plus className="h-4 w-4" /></button>
        </div>
      </div>

      <section className="rounded-xl border border-slate-800 bg-slate-950/60 p-4">
        <table className="w-full text-left text-sm">
          <thead className="text-slate-400"><tr><th className="pb-2">#</th><th className="pb-2">Name</th><th className="pb-2">Category</th><th className="pb-2">SKU</th><th className="pb-2">Stock</th><th className="pb-2">Status</th><th className="pb-2">Actions</th></tr></thead>
          <tbody>
            {rows.length === 0 ? <EmptyTableRow colSpan={7} /> : rows.map((p, idx) => {
              const totalStock = p.skus.reduce((sum, s) => sum + s.stock, 0);
              return <tr key={p.id} className="border-t border-slate-800"><td className="py-2">{(page - 1) * PAGE_SIZE + idx + 1}</td><td className="py-2">{p.name}</td><td className="py-2">{categoryPath(p.categoryId)}</td><td className="py-2">{p.skus.length}</td><td className="py-2">{formatInteger(totalStock)}</td><td className="py-2" title={p.status}>{getStatusTone(p.status) === "success" ? <CircleCheck className="h-4 w-4 text-emerald-400" /> : getStatusTone(p.status) === "warning" ? <Circle className="h-4 w-4 text-amber-400" /> : <CircleAlert className="h-4 w-4 text-rose-400" />}</td><td className="py-2"><div className="flex gap-2"><IconButton title="Edit" icon={<Edit3 size={14} />} variant="accent" onClick={() => { setEditing(p); setName(p.name); setCategoryId(p.categoryId); setStatus(p.status); setDescription(p.description); setImages(p.images); setOptions(p.options.length ? p.options : [emptyOption()]); setSkuRows(p.skus.map((s) => ({ comboKey: JSON.stringify(s.optionValues), code: s.code, price: s.price, stock: s.stock, optionValues: s.optionValues }))); setProductModalOpen(true); }} /><IconButton title="Delete" icon={<Trash2 size={14} />} variant="danger" onClick={() => { setDeletingProductId(p.id); setDeleteModalOpen(true); }} /></div></td></tr>;
            })}
          </tbody>
        </table>
        <Pagination page={page} totalPages={totalPages} onChange={setPage} />
      </section>

      <Modal open={productModalOpen} title={editing ? "Update Product" : "Create Product"} onClose={() => setProductModalOpen(false)}>
        <div className="space-y-3 text-sm">
          <div className="grid grid-cols-2 gap-2">
            <input value={name} onChange={(e) => setName(e.target.value)} placeholder="Product name" className="rounded border border-slate-700 bg-slate-950 px-3 py-2" />
            <select value={categoryId} onChange={(e) => setCategoryId(e.target.value)} className="rounded border border-slate-700 bg-slate-950 px-3 py-2"><option value="">Select category</option>{categories.map((c) => <option key={c.id} value={c.id}>{"-".repeat(c.level - 1)} {c.name}</option>)}</select>
          </div>
          <textarea value={description} onChange={(e) => setDescription(e.target.value)} rows={2} placeholder="Description" className="w-full rounded border border-slate-700 bg-slate-950 px-3 py-2" />
          <select value={status} onChange={(e) => setStatus(e.target.value as "draft" | "active")} className="rounded border border-slate-700 bg-slate-950 px-3 py-2"><option value="draft">draft</option><option value="active">active</option></select>

          <div className="rounded border border-slate-700 p-3">
            <div className="mb-2 flex items-center justify-between"><p className="text-xs text-slate-400">Product Images</p><label className="cursor-pointer rounded bg-slate-700 px-2 py-1 text-xs"><ImagePlus className="inline h-3 w-3" /><input type="file" multiple accept="image/*" onChange={(e) => onUploadImages(e.target.files)} className="hidden" /></label></div>
            <div className="grid grid-cols-5 gap-2">
              {images.length === 0 ? <p className="col-span-5 text-center text-xs text-slate-400">Khong co ban ghi nao hien co</p> : images.map((img) => <div key={img.id} className={`overflow-hidden rounded border ${img.isPrimary ? "border-cyan-400" : "border-slate-700"}`}><img src={img.dataUrl} alt={img.name} className="h-16 w-full object-cover" /><div className="flex items-center justify-between p-1"><button className={`rounded px-2 py-0.5 text-xs ${img.isPrimary ? "bg-cyan-700" : "bg-slate-700"}`} onClick={() => setImages((prev) => prev.map((x) => ({ ...x, isPrimary: x.id === img.id })))}><Star className="inline h-3 w-3" /></button><button className="rounded bg-rose-600 px-2 py-0.5 text-xs" onClick={() => setImages((prev) => prev.filter((x) => x.id !== img.id))}><Trash2 className="inline h-3 w-3" /></button></div></div>)}
            </div>
          </div>

          <div className="rounded border border-slate-700 p-3">
            <div className="mb-2 flex items-center justify-between"><p className="text-xs text-slate-400">Product Options and Values</p><button className="rounded bg-slate-700 px-2 py-1 text-xs" onClick={() => setOptions((prev) => [...prev, emptyOption()])}><Plus size={12} /></button></div>
            <div className="space-y-2">
              {options.map((option) => (
                <div key={option.id} className="rounded border border-slate-700 p-2">
                  <label className="mb-1 block text-xs text-slate-400">Option</label>
                  <input value={option.name} onChange={(e) => setOptions((prev) => prev.map((o) => (o.id === option.id ? { ...o, name: e.target.value } : o)))} placeholder="e.g. Color / Size / Weight" className="mb-2 w-full rounded border border-slate-600 bg-slate-900 px-2 py-1" />
                  <label className="mb-1 block text-xs text-slate-400">Option Values</label>
                  <div className="space-y-1">
                    {option.values.map((value, idx) => (
                      <div key={`${option.id}-${idx}`} className="flex gap-2">
                        <input value={value} onChange={(e) => setOptions((prev) => prev.map((o) => (o.id === option.id ? { ...o, values: o.values.map((v, i) => (i === idx ? e.target.value : v)) } : o)))} placeholder="value" className="w-full rounded border border-slate-600 bg-slate-900 px-2 py-1" />
                        <button className="rounded bg-rose-600 px-2 py-1 text-xs" onClick={() => removeOptionValue(option.id, idx)}><Trash2 className="h-3 w-3" /></button>
                      </div>
                    ))}
                  </div>
                  <button className="mt-2 rounded bg-slate-700 px-2 py-1 text-xs" onClick={() => addOptionValue(option.id)}>Add Value</button>
                </div>
              ))}
            </div>
            <div className="mt-2 flex gap-2"><button className="rounded bg-slate-700 px-3 py-1 text-xs" onClick={regenerateSkuFromOptions}><Check className="inline h-3 w-3" /></button><button className="rounded bg-slate-700 px-3 py-1 text-xs" onClick={addManualSku}><Plus className="inline h-3 w-3" /></button></div>
          </div>

          <div className="rounded border border-slate-700 p-3">
            <p className="mb-2 text-xs text-slate-400">SKU Variant Table</p>
            <div className="max-h-56 overflow-auto">
              <table className="w-full text-center text-xs">
                <thead><tr><th className="pb-1">Combination</th><th className="pb-1">Code</th><th className="pb-1">Price</th><th className="pb-1">Stock</th><th className="pb-1">Fn</th></tr></thead>
                <tbody>
                  {skuRows.length === 0 ? <tr><td colSpan={5} className="py-3 text-center text-slate-400">Khong co ban ghi nao hien co</td></tr> : skuRows.map((row, idx) => <tr key={row.comboKey} className="border-t border-slate-800"><td className="py-1">{Object.values(row.optionValues).join(" / ") || "Manual"}</td><td><input value={row.code} onChange={(e) => setSkuRows((prev) => prev.map((r, i) => (i === idx ? { ...r, code: e.target.value } : r)))} className="mx-auto w-28 rounded border border-slate-700 bg-slate-900 px-1 text-center" /></td><td><input type="number" value={row.price} onChange={(e) => setSkuRows((prev) => prev.map((r, i) => (i === idx ? { ...r, price: Number(e.target.value) } : r)))} className="mx-auto w-24 rounded border border-slate-700 bg-slate-900 px-1 text-center" /><p className="text-[10px] text-slate-500">{formatInteger(row.price)}</p></td><td><input type="number" value={row.stock} onChange={(e) => setSkuRows((prev) => prev.map((r, i) => (i === idx ? { ...r, stock: Number(e.target.value) } : r)))} className="mx-auto w-20 rounded border border-slate-700 bg-slate-900 px-1 text-center" /><p className="text-[10px] text-slate-500">{formatInteger(row.stock)}</p></td><td><IconButton title="Delete SKU" icon={<Trash2 size={12} />} variant="danger" onClick={() => setSkuRows((prev) => prev.filter((_, i) => i !== idx))} /></td></tr>)}
                </tbody>
              </table>
            </div>
          </div>

          <button className="rounded bg-cyan-600 px-3 py-2" onClick={submitProduct}><Check className="inline h-4 w-4" /></button>
        </div>
      </Modal>

      <Modal open={deleteModalOpen} title="Delete Product" onClose={() => setDeleteModalOpen(false)}>
        <div className="space-y-2"><p className="text-sm text-slate-300">Confirm delete?</p><button className="rounded bg-rose-600 px-3 py-2" onClick={() => { deleteProduct(deletingProductId); setDeleteModalOpen(false); setTick((v) => v + 1); }}><Trash2 className="inline h-4 w-4" /></button></div>
      </Modal>
    </div>
  );
}
