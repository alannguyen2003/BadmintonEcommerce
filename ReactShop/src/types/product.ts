export type ProductStatus = "inactive" | "active";

export interface Product {
	id: string;
	name: string;
	categoryId: string;
	categoryName: string;
	status: ProductStatus;
	description: string;
	images: [];
	options: [];
	variants: [];
	createdAt: string;
	updatedAt: string;
}

