import { api } from "./client";

export type Activity = {
  id: number;
  name: string;
  description?: string;
  categoryId?: number;
  categoryName?: string;
  isOutdoor?: boolean;
  imageUrl?: string;
  locationName?: string;
};

export type ListParams = {
  q?: string;             // namn/plats/kategori fritext
  categoryId?: number;
  dateFrom?: string;      // yyyy-mm-dd
  dateTo?: string;        // yyyy-mm-dd
  isOutdoor?: boolean;
  onlyWithFreeSlots?: boolean;
};

export async function listActivities(params: ListParams = {}) {
  const { data } = await api.get<Activity[]>("/activities", { params });
  return data;
}

export async function listCategories() {
  const { data } = await api.get<Array<{ id:number; name:string }>>("/categories");
  return data;
}

export async function listLocations() {
  const { data } = await api.get<Array<{ id:number; name:string }>>("/locations");
  return data;
}

export async function getActivity(id: number) {
  const { data } = await api.get<Activity>(`/activities/${id}`);
  return data;
}
