import type { PropsWithChildren } from "react";
import { X } from "lucide-react";

export function Modal({ open, title, onClose, children }: PropsWithChildren<{ open: boolean; title: string; onClose: () => void }>) {
  if (!open) return null;
  return (
    <div className="fixed inset-0 z-50 overflow-y-auto bg-black/60 p-4">
      <div className="flex min-h-full items-center justify-center">
        <div className="my-6 max-h-[85vh] w-full max-w-3xl overflow-hidden rounded-xl border border-slate-700 bg-slate-900">
          <div className="flex items-center justify-between border-b border-slate-700 p-4">
            <h3 className="text-lg font-semibold">{title}</h3>
            <button className="rounded bg-slate-800 p-2 text-sm" onClick={onClose}>
              <X className="h-4 w-4" />
            </button>
          </div>
          <div className="max-h-[calc(85vh-64px)] overflow-y-auto p-4">{children}</div>
        </div>
      </div>
    </div>
  );
}
