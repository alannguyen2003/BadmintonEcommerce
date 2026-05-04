import { NavLink, Outlet } from "react-router-dom";
import { Box, Boxes, ChartColumn, FolderTree, Receipt } from "lucide-react";
import { seedIfNeeded } from "../services/bootstrap";
import { dashboardStats, formatMoney } from "../services/appService";

seedIfNeeded();

const links = [
  { to: "/categories", icon: FolderTree },
  { to: "/products", icon: Box },
  { to: "/inventory", icon: Boxes },
  { to: "/orders", icon: Receipt },
  { to: "/reports", icon: ChartColumn },
];

export function AdminLayout() {
  const stats = dashboardStats();

  return (
    <div className="min-h-screen bg-slate-950 text-slate-100">
      <div className="mx-auto flex max-w-7xl gap-4 p-4">
        <aside className="sticky top-4 h-[calc(100vh-2rem)] w-72 rounded-2xl border border-slate-800 bg-slate-900/80 p-4 backdrop-blur">
          <h1 className="mb-1 text-xl font-bold">ReactShop Admin</h1>
          <p className="mb-6 text-xs text-slate-400">Badminton shop dashboard</p>

          <div className="mb-6 grid grid-cols-2 gap-2 text-xs">
            <div className="rounded-lg bg-slate-800 p-2"><p className="text-slate-400">Products</p><p className="text-base font-semibold">{stats.totalProducts}</p></div>
            <div className="rounded-lg bg-slate-800 p-2"><p className="text-slate-400">SKUs</p><p className="text-base font-semibold">{stats.totalSkus}</p></div>
            <div className="rounded-lg bg-slate-800 p-2"><p className="text-slate-400">Stock</p><p className="text-base font-semibold">{stats.totalStock}</p></div>
            <div className="rounded-lg bg-slate-800 p-2"><p className="text-slate-400">Revenue</p><p className="text-base font-semibold">{formatMoney(stats.monthlyRevenue)}</p></div>
          </div>

          <nav className="grid grid-cols-5 gap-2">
            {links.map((link) => (
              <NavLink key={link.to} to={link.to} title={link.to} className={({ isActive }) => `flex h-10 items-center justify-center rounded-lg text-lg ${isActive ? "bg-cyan-500/20" : "bg-slate-800 hover:bg-slate-700"}`}>
                <link.icon className="h-5 w-5" />
              </NavLink>
            ))}
          </nav>
        </aside>

        <main className="min-h-[calc(100vh-2rem)] flex-1 rounded-2xl border border-slate-800 bg-slate-900/50 p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
