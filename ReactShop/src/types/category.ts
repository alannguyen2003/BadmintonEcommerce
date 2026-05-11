export interface Category {
	id: string;
	categoryName: string;
	parentCategoryName : string;
	parentCategoryId: string | null;
	level: number;
}