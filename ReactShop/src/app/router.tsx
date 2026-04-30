import { Navigate, Route, Routes } from "react-router-dom";
import { AdminLayout } from "../layouts/AdminLayout";
import { CategoriesPage } from "../modules/categories/CategoriesPage";
import { InventoryPage } from "../modules/inventory/InventoryPage";
import { OrdersPage } from "../modules/orders/OrdersPage";
import { ProductsPage } from "../modules/products/ProductsPage";
import { ReportsPage } from "../modules/reports/ReportsPage";

export function AppRouter() {
  return (
    <Routes>
      <Route element={<AdminLayout />}>
        <Route path="/categories" element={<CategoriesPage />} />
        <Route path="/products" element={<ProductsPage />} />
        <Route path="/inventory" element={<InventoryPage />} />
        <Route path="/orders" element={<OrdersPage />} />
        <Route path="/reports" element={<ReportsPage />} />
        <Route path="*" element={<Navigate to="/categories" replace />} />
      </Route>
    </Routes>
  );
}
