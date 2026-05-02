export type ProductStatus = "draft" | "active";

export interface ProductCategory {
		id: string;
		categoryName: string;
		parentCategoryName: string;
		parentCategoryId: string;
		level: number
}

export interface Product {
	id: string;
	name: string;
	categoryName: string;
	status: ProductStatus;
	description: string;
	images: [];
	options: [];
	createdAt: string;
	updatedAt: string;
}

