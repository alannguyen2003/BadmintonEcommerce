import { readDb, updateDb } from "../lib/storage";
import type { InventoryAction, Order, OrderStatus } from "../types/domain";

const id = (prefix: string) => `${prefix}_${Math.random().toString(36).slice(2, 9)}`;

export const formatMoney = (value: number) =>
  new Intl.NumberFormat("vi-VN", {
    style: "currency",
    currency: "VND",
    maximumFractionDigits: 0,
  }).format(value);

export const getStatusTone = (status: string): "success" | "warning" | "danger" => {
  if (status === "active" || status === "paid") return "success";
  if (status === "draft" || status === "placed") return "warning";
  return "danger";
};

export const dashboardStats = () => {
  const db = readDb();
  const totalProducts = db.products.length;
  const totalSkus = db.skus.length;
  const totalStock = db.skus.reduce((sum, sku) => sum + sku.stock, 0);
  const monthlyRevenue = db.orders
    .filter((order) => order.status === "paid")
    .reduce((sum, order) => sum + order.total, 0);
  return { totalProducts, totalSkus, totalStock, monthlyRevenue };
};

export const listSkus = () => {
  const db = readDb();
  return db.skus.map((s) => ({
    ...s,
    productName: db.products.find((p) => p.id === s.productId)?.name ?? "-",
  }));
};

export const listInventoryTransactions = () => readDb().inventoryTransactions;

export const createInventoryTransaction = (
  skuId: string,
  action: InventoryAction,
  quantity: number,
  note: string,
) => {
  if (quantity <= 0) return;
  updateDb((db) => {
    const skus = db.skus.map((s) => {
      if (s.id !== skuId) return s;
      if (action === "import") return { ...s, stock: s.stock + quantity };
      if (action === "export") return { ...s, stock: Math.max(0, s.stock - quantity) };
      return { ...s, stock: quantity };
    });
    return {
      ...db,
      skus,
      inventoryTransactions: [
        {
          id: id("tx"),
          skuId,
          action,
          quantity,
          note,
          createdAt: new Date().toISOString(),
        },
        ...db.inventoryTransactions,
      ],
    };
  });
};

export const deleteInventoryTransaction = (transactionId: string) =>
  updateDb((db) => ({
    ...db,
    inventoryTransactions: db.inventoryTransactions.filter((t) => t.id !== transactionId),
  }));

export const listOrders = (): Order[] => readDb().orders;

export const updateOrderStatus = (orderId: string, status: OrderStatus) =>
  updateDb((db) => ({
    ...db,
    orders: db.orders.map((o) => (o.id === orderId ? { ...o, status } : o)),
  }));

export const deleteOrder = (orderId: string) =>
  updateDb((db) => ({
    ...db,
    orders: db.orders.filter((o) => o.id !== orderId),
  }));

export const revenueByDay = () => {
  const map = new Map<string, { key: string; revenue: number; orders: number }>();
  for (const order of readDb().orders) {
    if (order.status !== "paid") continue;
    const key = order.createdAt.slice(0, 10);
    const row = map.get(key) ?? { key, revenue: 0, orders: 0 };
    row.revenue += order.total;
    row.orders += 1;
    map.set(key, row);
  }
  return [...map.values()].sort((a, b) => a.key.localeCompare(b.key));
};

export const revenueByMonth = () => {
  const map = new Map<string, { key: string; revenue: number; orders: number }>();
  for (const order of readDb().orders) {
    if (order.status !== "paid") continue;
    const key = order.createdAt.slice(0, 7);
    const row = map.get(key) ?? { key, revenue: 0, orders: 0 };
    row.revenue += order.total;
    row.orders += 1;
    map.set(key, row);
  }
  return [...map.values()].sort((a, b) => a.key.localeCompare(b.key));
};