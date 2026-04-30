import type { ReactNode } from "react";

export function IconButton({ icon, onClick, title, variant = "default" }: { icon: ReactNode; onClick: () => void; title: string; variant?: "default" | "danger" | "accent"; }) {
  const style =
    variant === "danger"
      ? "bg-rose-500/20 text-rose-300 hover:bg-rose-500/30"
      : variant === "accent"
        ? "bg-cyan-500/20 text-cyan-300 hover:bg-cyan-500/30"
        : "bg-slate-700 text-slate-200 hover:bg-slate-600";
  return (
    <button title={title} aria-label={title} onClick={onClick} className={`rounded p-2 transition ${style}`}>
      {icon}
    </button>
  );
}
