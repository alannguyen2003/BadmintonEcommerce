import axios from 'axios';
import type { Category } from '../types/domain';
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