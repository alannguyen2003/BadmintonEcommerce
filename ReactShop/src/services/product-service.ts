import axios from "axios";
import type { ProductOption } from "../types/domain";
import type { Category } from "../types/category";
import type { ProductImage } from "../types/productImage";
import type { Product } from "../types/product";

const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5080";

// Helper function to calculate category level based on parent
const calculateLevel = (parentId: string | null): 1 | 2 | 3 => {
  if (!parentId) return 1;
  // For simplicity, assume level 2 if has parent, but this might need adjustment
  // In a real scenario, you might need to fetch parent category to determine level
  return 2;
};

export const getCategories = async (): Promise<Category[]> => {
  try {
    const response = await axios.get(`${API_BASE_URL}/categories`);
    console.log(response);
    return response.data;
  } catch (error) {
    console.error("Error fetching categories:", error);
    throw error;
  }
};

export const createCategory = async (data: {
  categoryName: string;
  parentCategoryId?: string;
}): Promise<Category> => {
  try {
    const response = await axios.post(`${API_BASE_URL}/categories`, data);
    const item = response.data;
    return {
      id: item.id,
      categoryName: item.categoryName,
      parentCategoryId: item.parentCategoryId || null,
      parentCategoryName: item.parentCategoryName || null,
      level: calculateLevel(item.parentCategoryId),
    };
  } catch (error) {
    console.error("Error creating category:", error);
    throw error;
  }
};

export const updateCategory = async (data: {
  id: string;
  categoryName: string;
  parentCategoryId?: string;
}): Promise<Category> => {
  try {
    const response = await axios.put(`${API_BASE_URL}/categories`, data);
    const item = response.data;
    return {
      id: item.id,
      categoryName: item.categoryName,
      parentCategoryId: item.parentCategoryId || null,
      parentCategoryName: item.parentCategoryName || null,
      level: calculateLevel(item.parentCategoryId),
    };
  } catch (error) {
    console.error("Error updating category:", error);
    throw error;
  }
};

export const deleteCategory = async (id: string): Promise<void> => {
  try {
    await axios.delete(`${API_BASE_URL}/categories/${id}`);
  } catch (error) {
    console.error("Error deleting category:", error);
    throw error;
  }
};
// Products API functions - Get List
export const getProducts = async (): Promise<
  Array<Product & { totalVariants?: number }>
> => {
  try {
    const response = await axios.get(`${API_BASE_URL}/products`);
    console.log("Service: ", response.data);
    return response.data.map((item: any) => ({
      id: item.id,
      name: item.productName,
      brand: item.brand,
      categoryId: item.categoryId,
      categoryName: item.categoryName,
      status: item.status ? "active" : "inactive",
      description: item.productDescription,
      options: [],
      images: item.primaryImage
        ? [
            {
              id: item.primaryImage.id,
              name: "",
              dataUrl: item.primaryImage.url,
              isPrimary: true,
            },
          ]
        : [],
      createdAt: "",
      updatedAt: "",
      totalVariants: item.totalVariants || 0,
    }));
  } catch (error) {
    console.error("Error fetching products:", error);
    throw error;
  }
};

export const getProductDetail = async (
  id: string,
): Promise<
  Product & {
    variants: Array<{
      id: string;
      code: string;
      price: number;
      stock: number;
      optionValues: Record<string, string>;
    }>;
  }
> => {
  try {
    const response = await axios.get(`${API_BASE_URL}/products/${id}`);
    const item = response.data;
    console.log(response.data);
    // Transform options: flatten values array
    const options = (item.options || []).map((option: any) => ({
      id: option.id,
      name: option.name,
      values: (option.values || []).map((v: any) => v.value),
    }));

    // Create a map of value ID -> value text and option ID for lookup
    const valueIdMap = new Map<string, { optionId: string; value: string }>();

    item.options.forEach((option: any) => {
      option.values.forEach((v: any) => {
        // v ở đây là { id: string, value: string }
        valueIdMap.set(v.id, {
          optionId: option.id,
          value: v.value,
        });
      });
    });

    // Transform variants to SKUs
    const variants = (item.variants || []).map((variant: any) => {
      const optionValues: Record<string, string> = {};
      (variant.optionValues || []).forEach((valueId: string) => {
        const mapped = valueIdMap.get(valueId);
        if (mapped) {
          optionValues[mapped.optionId] = mapped.value;
        }
      });

      return {
        id: variant.id,
        code: variant.sku,
        price: variant.price,
        stock: 0,
        optionValues,
      };
    });

    return {
      id: item.id,
      name: item.name,
      categoryId: item.categoryId,
      categoryName: item.categoryName,
      status: item.status ? "active" : "inactive",
      description: item.description,
      options,
      images: (item.images || []).map((img: any) => ({
        id: img.id,
        name: "",
        dataUrl: img.imageUrl,
        isPrimary: img.isPrimary,
      })),
      createdAt: "",
      updatedAt: "",
      variants,
    };
  } catch (error) {
    console.error("Error fetching product detail:", error);
    throw error;
  }
};

export const createProduct = async (data: {
  name: string;
  categoryId: string;
  status: boolean;
  description: string;
  brand?: string;
  options: ProductOption[];
  variants: Array<{
    code: string;
    price: number;
    stock: number;
    optionValues: Record<string, string>;
  }>;
  images: ProductImage[];
}): Promise<Product> => {
  try {
    const formData = new FormData();
    console.log(data);

    // Add text fields
    formData.append("name", data.name);
    formData.append("brand", data.name);
    formData.append("categoryId", data.categoryId);
    formData.append("status", data.status ? "true" : "false");
    formData.append("description", data.description);
    if (data.brand) {
      formData.append("brand", data.brand);
    }

    // Serialize options in API shape
    const requestOptions = data.options.map((option) => ({
      name: option.name,
      code: option.id,
      values: option.values || [],
    }));
    formData.append("options", JSON.stringify(requestOptions));

    // Serialize skuRows in API shape
    const requestSkuRows = data.variants.map((row) => ({
      code: row.code,
      price: row.price,
      stock: row.stock,
      values: Object.entries(row.optionValues).map(([optionId, value]) => {
        const option = data.options.find((o) => o.id === optionId);
        return {
          code: option?.id || optionId,
          name: option?.name || optionId,
          value,
        };
      }),
    }));
    formData.append("skuRows", JSON.stringify(requestSkuRows));

    // Add images
    data.images.forEach((img) => {
      if (img.dataUrl.startsWith("data:")) {
        const blob = dataURLToBlob(img.dataUrl);
        formData.append("images", blob, img.name);
      }
    });

    const response = await axios.post(
      `${API_BASE_URL}/products/create-products`,
      formData,
    );

    const item = response.data;
    return {
      id: item.id,
      name: item.name,
      categoryId: item.categoryId,
      status: item.status ? "active" : "inactive",
      description: item.description,
      options: (item.options || []).map((option: any) => ({
        id: option.code || option.id || option.name || crypto.randomUUID(),
        name: option.name,
        values: option.values || [],
      })),
      images: item.images || [],
      createdAt: item.createdAt,
      updatedAt: item.updatedAt,
    };
  } catch (error) {
    console.error("Error creating product:", error);
    throw error;
  }
};

// Helper function to convert base64 data URL to Blob
function dataURLToBlob(dataURL: string): Blob {
  const arr = dataURL.split(",");
  const mime = arr[0].match(/:(.*?);/)![1];
  const bstr = atob(arr[1]);
  let n = bstr.length;
  const u8arr = new Uint8Array(n);
  while (n--) {
    u8arr[n] = bstr.charCodeAt(n);
  }
  return new Blob([u8arr], { type: mime });
}

export const deleteProduct = async (id: string): Promise<void> => {
  try {
    await axios.delete(`${API_BASE_URL}/products`, { data: { id } });
  } catch (error) {
    console.error("Error deleting product:", error);
    throw error;
  }
};
