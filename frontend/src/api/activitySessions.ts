import { api } from "./client";
import type {ActivitySession, FilterActivitySessionResponse} from "../types.ts";

export type ListParams = {
  q?: string;             // namn/plats/kategori fritext
  categoryId?: number;
  locationId?: number;
  dateFrom?: string;      // yyyy-mm-dd
  dateTo?: string;        // yyyy-mm-dd
  isIndoor?: boolean;
};

export async function listActivitySessions(params: ListParams = {}) {
  const { data } = await api.get<ActivitySession[]>("/activities", { params });
  return data;
}

export async function filterActivitySessions(params: ListParams = {}) {
  const { data } = await api.post<FilterActivitySessionResponse[]>("/api/activitySessions/filter", params);
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