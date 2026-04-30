import { useState } from "react";
import { EmptyTableRow } from "../../components/EmptyTableRow";
import { Pagination } from "../../components/Pagination";
import { formatMoney, revenueByDay, revenueByMonth } from "../../services/shopService";

const PAGE_SIZE = 10;

export function ReportsPage() {
  const [dayPage, setDayPage] = useState(1);
  const [monthPage, setMonthPage] = useState(1);
  const byDay = revenueByDay();
  const byMonth = revenueByMonth();

  const dayTotal = Math.max(1, Math.ceil(byDay.length / PAGE_SIZE));
  const monthTotal = Math.max(1, Math.ceil(byMonth.length / PAGE_SIZE));
  const dayRows = byDay.slice((dayPage - 1) * PAGE_SIZE, dayPage * PAGE_SIZE);
  const monthRows = byMonth.slice((monthPage - 1) * PAGE_SIZE, monthPage * PAGE_SIZE);

  return (
    <div className="space-y-6">
      <div><h2 className="text-2xl font-bold">Reports</h2><p className="text-sm text-slate-400">Revenue tables by day and month</p></div>
      <section className="rounded-xl border border-slate-800 bg-slate-950/60 p-4"><table className="w-full text-left text-sm"><thead className="text-slate-400"><tr><th className="pb-2">Day</th><th className="pb-2">Orders</th><th className="pb-2">Revenue</th></tr></thead><tbody>{dayRows.length === 0 ? <EmptyTableRow colSpan={3} /> : dayRows.map((d) => <tr key={d.key} className="border-t border-slate-800"><td className="py-2">{d.key}</td><td className="py-2">{d.orders}</td><td className="py-2">{formatMoney(d.revenue)}</td></tr>)}</tbody></table><Pagination page={dayPage} totalPages={dayTotal} onChange={setDayPage} /></section>
      <section className="rounded-xl border border-slate-800 bg-slate-950/60 p-4"><table className="w-full text-left text-sm"><thead className="text-slate-400"><tr><th className="pb-2">Month</th><th className="pb-2">Orders</th><th className="pb-2">Revenue</th></tr></thead><tbody>{monthRows.length === 0 ? <EmptyTableRow colSpan={3} /> : monthRows.map((m) => <tr key={m.key} className="border-t border-slate-800"><td className="py-2">{m.key}</td><td className="py-2">{m.orders}</td><td className="py-2">{formatMoney(m.revenue)}</td></tr>)}</tbody></table><Pagination page={monthPage} totalPages={monthTotal} onChange={setMonthPage} /></section>
    </div>
  );
}
