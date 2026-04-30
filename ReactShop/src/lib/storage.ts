import type { AppDatabase } from "../types/domain";

const DB_KEY = "reactshop_db";

const emptyDb = (): AppDatabase => ({
  schemaVersion: 2,
  categories: [],
  products: [],
  skus: [],
  inventoryTransactions: [],
  orders: [],
});

export const readDb = (): AppDatabase => {
  const raw = localStorage.getItem(DB_KEY);
  if (!raw) return emptyDb();
  try {
    const parsed = JSON.parse(raw) as Partial<AppDatabase>;
    if (parsed.schemaVersion === 2) {
      return {
        ...emptyDb(),
        ...parsed,
      } as AppDatabase;
    }
    return emptyDb();
  } catch {
    return emptyDb();
  }
};

export const writeDb = (db: AppDatabase): void => {
  localStorage.setItem(DB_KEY, JSON.stringify(db));
};

export const updateDb = (updater: (db: AppDatabase) => AppDatabase): AppDatabase => {
  const db = readDb();
  const next = updater(db);
  writeDb(next);
  return next;
};
