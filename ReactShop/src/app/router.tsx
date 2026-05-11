import { Navigate, Route, Routes } from "react-router-dom";
import { AdminLayout } from "../layouts/AdminLayout";
import { CategoriesPage } from "../modules/categories/CategoriesPage";
import { InventoryPage } from "../modules/inventory/InventoryPage";
import { OrdersPage } from "../modules/orders/OrdersPage";
import { ProductsPage } from "../modules/products/ProductsPage";
import { ReportsPage } from "../modules/reports/ReportsPage";
import { LoginPage } from "./LoginPage";
import { RequireAuth } from "./RequireAuth";
import { authService } from "../services/authService";

export function AppRouter() {
  const defaultRoute = authService.isAuthenticated() ? "/categories" : "/login";

  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />

      <Route element={<RequireAuth />}>
        <Route element={<AdminLayout />}>
          <Route path="/categories" element={<CategoriesPage />} />
          <Route path="/products" element={<ProductsPage />} />
          <Route path="/inventory" element={<InventoryPage />} />
          <Route path="/orders" element={<OrdersPage />} />
          <Route path="/reports" element={<ReportsPage />} />
        </Route>
      </Route>

      <Route path="*" element={<Navigate to={defaultRoute} replace />} />
    </Routes>
  );
}
