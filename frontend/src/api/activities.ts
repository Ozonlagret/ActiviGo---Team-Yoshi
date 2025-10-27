import { api } from "./client";
import type { ActivitySession } from "../types.ts";

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
  const { data } = await api.get<Activity[]>("/api/activities", { params });
  return data;
}

export async function fetchActivitySessions(filterParams = {}) {
  const { data } = await api.post<ActivitySession[]>("/api/activitySessions/filter", filterParams);
  return data;
}

export async function listCategories() {
  const { data } = await api.get<Array<{ id:number; name:string }>>("/api/categories");
  return data;
}

export async function listLocations() {
  const { data } = await api.get<Array<{ id:number; name:string }>>("/api/locations");
  return data;
}