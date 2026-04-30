import { readDb, writeDb } from "../lib/storage";
import type { AppDatabase, Product, SKU } from "../types/domain";

const now = new Date().toISOString();

const categories = [
  { id: "c1", name: "Rackets", parentId: null, level: 1 as const },
  { id: "c2", name: "Yonex", parentId: "c1", level: 2 as const },
  { id: "c3", name: "Victor", parentId: "c1", level: 2 as const },
  { id: "c4", name: "Professional", parentId: "c2", level: 3 as const },
  { id: "c5", name: "Shoes", parentId: null, level: 1 as const },
  { id: "c6", name: "Apparel", parentId: null, level: 1 as const },
  { id: "c7", name: "Accessories", parentId: null, level: 1 as const },
];

const names = [
  "Astrox 100ZZ", "Nanoflare 700", "Arcsaber 11 Pro", "Thruster Ryuga",
  "Auraspeed 90K", "Hypernano X", "Power Cushion 65Z", "Aerus Z",
  "Tournament Polo", "Dry Fit Tee", "Overgrip AC102", "BG80 String",
  "Pro Backpack", "Wristband Set", "Shuttle AS50", "Training Cone",
  "Grip Powder", "Knee Support", "Court Socks", "Elite Cap",
];

const products: Product[] = names.map((name, idx) => {
  const isRacket = idx < 6;
  const categoryId = isRacket ? (idx % 2 === 0 ? "c4" : "c3") : idx < 8 ? "c5" : idx < 10 ? "c6" : "c7";
  return {
    id: `p${idx + 1}`,
    name,
    categoryId,
    status: idx % 4 === 0 ? "draft" : "active",
    description: `${name} for badminton shop management demo`,
    options: isRacket
      ? [
          { id: `opt_w_${idx}`, name: "Weight", values: ["3U", "4U"] },
          { id: `opt_g_${idx}`, name: "Grip", values: ["G5", "G6"] },
        ]
      : [{ id: `opt_size_${idx}`, name: "Size", values: ["S", "M", "L"] }],
    images: [],
    createdAt: now,
    updatedAt: now,
  };
});

const skus: SKU[] = products.flatMap((product, idx): SKU[] => {
  if (idx < 6) {
    return [
      { id: `s${idx + 1}a`, productId: product.id, code: `${product.name.replace(/\s+/g, "-").toUpperCase()}-3U-G5`, optionValues: { w: "3U", g: "G5" } as Record<string, string>, price: 2500000 + idx * 150000, stock: 8 + (idx % 5) },
      { id: `s${idx + 1}b`, productId: product.id, code: `${product.name.replace(/\s+/g, "-").toUpperCase()}-4U-G6`, optionValues: { w: "4U", g: "G6" } as Record<string, string>, price: 2400000 + idx * 150000, stock: 6 + (idx % 4) },
    ];
  }
  return [
    { id: `s${idx + 1}`, productId: product.id, code: `${product.name.replace(/\s+/g, "-").toUpperCase()}-STD`, optionValues: { size: "M" } as Record<string, string>, price: 250000 + idx * 50000, stock: 10 + (idx % 8) },
  ];
});

const seedData: AppDatabase = {
  schemaVersion: 2,
  categories,
  products,
  skus,
  inventoryTransactions: [],
  orders: [
    {
      id: "o1",
      customerName: "Nguyen Van A",
      status: "paid",
      items: [{ id: "oi1", skuId: "s1a", skuCode: skus[0].code, productName: products[0].name, quantity: 1, unitPrice: skus[0].price }],
      total: skus[0].price,
      createdAt: now,
    },
    {
      id: "o2",
      customerName: "Tran Thi B",
      status: "placed",
      items: [{ id: "oi2", skuId: "s2a", skuCode: skus[2].code, productName: products[1].name, quantity: 2, unitPrice: skus[2].price }],
      total: skus[2].price * 2,
      createdAt: now,
    },
  ],
};

export const seedIfNeeded = () => {
  const db = readDb();
  if (!db.products.length && !db.orders.length) writeDb(seedData);
};
