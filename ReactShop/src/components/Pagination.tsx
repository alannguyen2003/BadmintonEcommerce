export function Pagination({ page, totalPages, onChange }: { page: number; totalPages: number; onChange: (page: number) => void }) {
  if (totalPages <= 1) return null;
  return (
    <div className="mt-3 flex items-center justify-end gap-2 text-sm">
      <button className="rounded bg-slate-800 px-2 py-1 disabled:opacity-50" onClick={() => onChange(Math.max(1, page - 1))} disabled={page <= 1}>
        Prev
      </button>
      <span className="text-slate-400">{page}/{totalPages}</span>
      <button className="rounded bg-slate-800 px-2 py-1 disabled:opacity-50" onClick={() => onChange(Math.min(totalPages, page + 1))} disabled={page >= totalPages}>
        Next
      </button>
    </div>
  );
}
