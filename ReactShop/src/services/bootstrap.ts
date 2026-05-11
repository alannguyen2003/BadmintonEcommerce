// import { readDb, writeDb } from "../lib/storage";
// import type { AppDatabase, Product, SKU } from "../types/domain";

// const now = new Date().toISOString();

// const categories = [
//   { id: "c1", name: "Rackets", parentId: null, level: 1 as const },
//   { id: "c2", name: "Yonex", parentId: "c1", level: 2 as const },
//   { id: "c3", name: "Victor", parentId: "c1", level: 2 as const },
//   { id: "c4", name: "Professional", parentId: "c2", level: 3 as const },
//   { id: "c5", name: "Shoes", parentId: null, level: 1 as const },
//   { id: "c6", name: "Apparel", parentId: null, level: 1 as const },
//   { id: "c7", name: "Accessories", parentId: null, level: 1 as const },
// ];

// const names = [
//   "Astrox 100ZZ", "Nanoflare 700", "Arcsaber 11 Pro", "Thruster Ryuga",
//   "Auraspeed 90K", "Hypernano X", "Power Cushion 65Z", "Aerus Z",
//   "Tournament Polo", "Dry Fit Tee", "Overgrip AC102", "BG80 String",
//   "Pro Backpack", "Wristband Set", "Shuttle AS50", "Training Cone",
//   "Grip Powder", "Knee Support", "Court Socks", "Elite Cap",
// ];

// const products: Product[] = names.map((name, idx) => {
//   const isRacket = idx < 6;
//   const categoryId = isRacket ? (idx % 2 === 0 ? "c4" : "c3") : idx < 8 ? "c5" : idx < 10 ? "c6" : "c7";
//   return {
//     id: `p${idx + 1}`,
//     name,
//     categoryId,
//     status: idx % 4 === 0 ? "inactive" : "active",
//     description: `${name} for badminton shop management demo`,
//     options: isRacket
//       ? [
//           { id: `opt_w_${idx}`, name: "Weight", values: ["3U", "4U"] },
//           { id: `opt_g_${idx}`, name: "Grip", values: ["G5", "G6"] },
//         ]
//       : [{ id: `opt_size_${idx}`, name: "Size", values: ["S", "M", "L"] }],
//     images: [],
//     createdAt: now,
//     updatedAt: now,
//   };
// });

// const seedData: AppDatabase = {
//   schemaVersion: 2,
//   categories,
//   products,
//   skus,
//   inventoryTransactions: [],
//   orders: [
//   ],
// };

// export const seedIfNeeded = () => {
//   const db = readDb();
//   if (!db.products.length && !db.orders.length) writeDb(seedData);
// };
