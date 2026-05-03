import axios from 'axios';
import type { Category, Product, ProductImage, ProductOption } from '../types/domain';
import type { ProductCategory } from '../types/product';

const API_BASE_URL = 'http://localhost:5080';

// Helper function to calculate category level based on parent
const calculateLevel = (parentId: string | null): 1 | 2 | 3 => {
  if (!parentId) return 1;
  // For simplicity, assume level 2 if has parent, but this might need adjustment
  // In a real scenario, you might need to fetch parent category to determine level
  return 2;
};

export const getCategories = async (): Promise<ProductCategory[]> => {
  try {
    const response = await axios.get(`${API_BASE_URL}/categories`);
    return response.data;
  } catch (error) {
    console.error('Error fetching categories:', error);
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
      name: item.categoryName,
      parentId: item.parentCategoryId || null,
      level: calculateLevel(item.parentCategoryId)
    };
  } catch (error) {
    console.error('Error creating category:', error);
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
      name: item.categoryName,
      parentId: item.parentCategoryId || null,
      level: calculateLevel(item.parentCategoryId)
    };
  } catch (error) {
    console.error('Error updating category:', error);
    throw error;
  }
};

export const deleteCategory = async (id: string): Promise<void> => {
  try {
    await axios.delete(`${API_BASE_URL}/categories/${id}`);
  } catch (error) {
    console.error('Error deleting category:', error);
    throw error;
  }
};
// Products API functions
export const getProducts = async (): Promise<Product[]> => {
  try {
    const response = await axios.get(`${API_BASE_URL}/products`);
    return response.data.map((item: any) => ({
      id: item.id,
      name: item.name,
      categoryId: item.categoryId,
      status: item.status ? 'active' : 'draft', // Convert boolean to string
      description: item.description,
      options: item.options || [],
      images: item.images || [],
      createdAt: item.createdAt,
      updatedAt: item.updatedAt
    }));
  } catch (error) {
    console.error('Error fetching products:', error);
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
  skuRows: Array<{
    code: string;
    price: number;
    stock: number;
    optionValues: Record<string, string>;
  }>;
  images: ProductImage[];
}): Promise<Product> => {
  try {
    const formData = new FormData();

    // Add text fields
    formData.append('name', data.name);
    formData.append('brand', data.name);
    formData.append('categoryId', data.categoryId);
    formData.append('status', data.status.toString());
    formData.append('description', data.description);
    if (data.brand) {
      formData.append('brand', data.brand);
    }

    // Serialize options and skuRows to JSON
    formData.append('options', JSON.stringify(data.options));
    formData.append('skuRows', JSON.stringify(data.skuRows));

    // Add images
    data.images.forEach((img) => {
      if (img.dataUrl.startsWith('data:')) {
        // Convert base64 to blob
        const blob = dataURLToBlob(img.dataUrl);
        formData.append('images', blob, img.name);
      }
    });

    const response = await axios.post(`${API_BASE_URL}/products/create-products`, formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    });

    const item = response.data;
    return {
      id: item.id,
      name: item.name,
      categoryId: item.categoryId,
      status: item.status ? 'active' : 'draft',
      description: item.description,
      options: item.options || [],
      images: item.images || [],
      createdAt: item.createdAt,
      updatedAt: item.updatedAt
    };
  } catch (error) {
    console.error('Error creating product:', error);
    throw error;
  }
};

// Helper function to convert base64 data URL to Blob
function dataURLToBlob(dataURL: string): Blob {
  const arr = dataURL.split(',');
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
    console.error('Error deleting product:', error);
    throw error;
  }
};