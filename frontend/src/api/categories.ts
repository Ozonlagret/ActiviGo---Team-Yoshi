import { api } from "./client";

export type Category = {
  id: number;
  name: string;
};

export async function listCategories() {
  const { data } = await api.get<Category[]>("/api/categories");
  return data;
}