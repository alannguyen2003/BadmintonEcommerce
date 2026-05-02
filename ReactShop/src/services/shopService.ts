import { readDb, updateDb } from "../lib/storage";
import type { Category, InventoryAction, Order, OrderStatus, Product, ProductOption, SKU } from "../types/domain";
import type { ProductCategory } from "../types/product";
import { getCategories, createCategory as apiCreateCategory, updateCategory as apiUpdateCategory, deleteCategory as apiDeleteCategory } from "./product-service";

const id = (prefix: string) => `${prefix}_${Math.random().toString(36).slice(2, 9)}`;

export const formatMoney = (value: number) =>
  new Intl.NumberFormat("vi-VN", { style: "currency", currency: "VND", maximumFractionDigits: 0 }).format(value);

export const getStatusTone = (status: string): "success" | "warning" | "danger" => {
  if (status === "active" || status === "paid") return "success";
  if (status === "draft" || status === "placed") return "warning";
  return "danger";
};

export const listCategories = async (): Promise<ProductCategory[]> => {
  try {
    return await getCategories();
  } catch (error) {
    console.error('Error fetching categories:', error);
    // Fallback to local storage if API fails
    // return readDb().categories;
    return [];
  }
};

export const createCategory = async (name: string, parentId: string | null) => {
  try {
    await apiCreateCategory({
      categoryName: name,
      parentCategoryId: parentId || undefined
    });
  } catch (error) {
    console.error('Error creating category:', error);
    // Fallback to local storage
    updateDb((db) => {
      const parent = parentId ? db.categories.find((c) => c.id === parentId) : null;
      const level = parent ? ((parent.level + 1) as 1 | 2 | 3) : 1;
      if (level > 3) return db;
      return {
        ...db,
        categories: [{ id: id("cat"), name, parentId, level }, ...db.categories],
      };
    });
  }
};

export const updateCategory = async (categoryId: string, name: string) => {
  try {
    await apiUpdateCategory({
      id: categoryId,
      categoryName: name
    });
  } catch (error) {
    console.error('Error updating category:', error);
    // Fallback to local storage
    updateDb((db) => ({ ...db, categories: db.categories.map((c) => (c.id === categoryId ? { ...c, name } : c)) }));
  }
};

export const deleteCategory = async (categoryId: string) => {
  try {
    await apiDeleteCategory(categoryId);
  } catch (error) {
    console.error('Error deleting category:', error);
    // Fallback to local storage
    updateDb((db) => {
      const childIds = db.categories.filter((c) => c.parentId === categoryId).map((c) => c.id);
      const removeIds = new Set([categoryId, ...childIds]);
      return {
        ...db,
        categories: db.categories.filter((c) => !removeIds.has(c.id)),
        products: db.products.filter((p) => !removeIds.has(p.categoryId)),
      };
    });
  }
};

const combosFromOptions = (options: ProductOption[]) => {
  if (!options.length) return [] as Record<string, string>[];
  return options.reduce<Record<string, string>[]>((acc, option) => {
    const values = option.values.filter(Boolean);
    if (!values.length) return acc;
    if (!acc.length) return values.map((v) => ({ [option.id]: v }));
    const next: Record<string, string>[] = [];
    for (const row of acc) for (const v of values) next.push({ ...row, [option.id]: v });
    return next;
  }, []);
};

export const listProducts = () => {
  const db = readDb();
  return db.products.map((p) => ({ ...p, skus: db.skus.filter((s) => s.productId === p.id) }));
};

export const upsertProduct = (input: {
  id?: string;
  name: string;
  categoryId: string;
  status: Product["status"];
  description: string;
  images: Product["images"];
  options: ProductOption[];
  skuRows: Array<{ comboKey: string; price: number; stock: number; code: string; optionValues: Record<string, string> }>;
}) => {
  const now = new Date().toISOString();
  const productId = input.id ?? id("p");
  updateDb((db) => {
    const existing = db.products.find((p) => p.id === productId);
    const product: Product = {
      id: productId,
      name: input.name,
      categoryId: input.categoryId,
      status: input.status,
      description: input.description,
      images: input.images,
      options: input.options,
      createdAt: existing?.createdAt ?? now,
      updatedAt: now,
    };
    const skus: SKU[] = input.skuRows.map((row) => ({
      id: id("sku"),
      productId,
      code: row.code,
      optionValues: row.optionValues,
      price: row.price,
      stock: row.stock,
    }));
    return {
      ...db,
      products: [product, ...db.products.filter((p) => p.id !== productId)],
      skus: [...db.skus.filter((s) => s.productId !== productId), ...skus],
    };
  });
};

export const deleteProduct = (productId: string) => {
  updateDb((db) => ({
    ...db,
    products: db.products.filter((p) => p.id !== productId),
    skus: db.skus.filter((s) => s.productId !== productId),
    inventoryTransactions: db.inventoryTransactions.filter((t) => db.skus.find((s) => s.id === t.skuId)?.productId !== productId),
  }));
};

export const buildSkuRows = (options: ProductOption[]) => {
  const combos = combosFromOptions(options);
  return combos.map((optionValues, idx) => ({
    comboKey: JSON.stringify(optionValues),
    optionValues,
    code: `SKU-${idx + 1}`,
    price: 0,
    stock: 0,
  }));
};

export const listSkus = () => {
  const db = readDb();
  return db.skus.map((s) => ({ ...s, productName: db.products.find((p) => p.id === s.productId)?.name ?? "-" }));
};

export const listInventoryTransactions = () => readDb().inventoryTransactions;

export const createInventoryTransaction = (skuId: string, action: InventoryAction, quantity: number, note: string) => {
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
      inventoryTransactions: [{ id: id("tx"), skuId, action, quantity, note, createdAt: new Date().toISOString() }, ...db.inventoryTransactions],
    };
  });
};

export const deleteInventoryTransaction = (transactionId: string) =>
  updateDb((db) => ({ ...db, inventoryTransactions: db.inventoryTransactions.filter((t) => t.id !== transactionId) }));

export const listOrders = (): Order[] => readDb().orders;

export const updateOrderStatus = (orderId: string, status: OrderStatus) =>
  updateDb((db) => ({ ...db, orders: db.orders.map((o) => (o.id === orderId ? { ...o, status } : o)) }));

export const deleteOrder = (orderId: string) =>
  updateDb((db) => ({ ...db, orders: db.orders.filter((o) => o.id !== orderId) }));

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

export const dashboardStats = () => {
  const db = readDb();
  return {
    totalProducts: db.products.length,
    totalSkus: db.skus.length,
    totalStock: db.skus.reduce((sum, s) => sum + s.stock, 0),
    paidOrders: db.orders.filter((o) => o.status === "paid").length,
    monthlyRevenue: revenueByMonth().at(-1)?.revenue ?? 0,
  };
};
