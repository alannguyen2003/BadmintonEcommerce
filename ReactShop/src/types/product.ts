export type ProductStatus = "draft" | "active";

export interface ProductCategory {
		id: string;
		name: string;
		parentCategory: string;
		level: number
}