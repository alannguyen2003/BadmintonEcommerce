export function EmptyTableRow({ colSpan, message }: { colSpan: number; message?: string }) {
  return (
    <tr>
      <td colSpan={colSpan} className="py-6 text-center text-sm text-slate-400">
        {message ?? "Khong co ban ghi nao hien co"}
      </td>
    </tr>
  );
}
