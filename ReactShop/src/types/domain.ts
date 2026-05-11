import type { Category } from "./category";
import type { Product } from "./product";

export type ProductStatus = "inactive" | "active";
export type InventoryAction = "import" | "export" | "adjust";
export type OrderStatus = "placed" | "paid" | "cancelled";

// export interface Category {
//   id: string;
//   name: string;
//   parentId: string | null;
//   level: 1 | 2 | 3;
// }

// export interface ProductImage {
//   id: string;
//   name: string;
//   dataUrl: string;
//   isPrimary: boolean;
// }

export interface ProductOption {
  id: string;
  name: string;
  values: string[];
}

export interface SKU {
  id: string;
  productId: string;
  code: string;
  optionValues: Record<string, string>;
  price: number;
  stock: number;
}

// export interface Product {
//   id: string;
//   name: string;
//   brand: string;
//   categoryId: string;
//   categoryName: string;
//   status: ProductStatus;
//   description: string;
//   options: ProductOption[];
//   images: ProductImage[];
//   createdAt: string;
//   updatedAt: string;
// }

export interface InventoryTransaction {
  id: string;
  skuId: string;
  action: InventoryAction;
  quantity: number;
  note: string;
  createdAt: string;
}

export interface OrderItem {
  id: string;
  skuId: string;
  skuCode: string;
  productName: string;
  quantity: number;
  unitPrice: number;
}

export interface Order {
  id: string;
  customerName: string;
  status: OrderStatus;
  items: OrderItem[];
  total: number;
  createdAt: string;
}

export interface AppDatabase {
  schemaVersion: 2;
  categories: Category[];
  products: Product[];
  skus: SKU[];
  inventoryTransactions: InventoryTransaction[];
  orders: Order[];
}
