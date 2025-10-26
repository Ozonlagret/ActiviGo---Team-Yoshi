import { api } from "./client";
import type {ActivitySession, FilterActivitySessionResponse} from "../types.ts";

export type ListParams = {
  q?: string;             // namn/plats/kategori fritext
  categoryId?: number;
  dateFrom?: string;      // yyyy-mm-dd
  dateTo?: string;        // yyyy-mm-dd
  isOutdoor?: boolean;
  onlyWithFreeSlots?: boolean;
};

export async function listActivitySessions(params: ListParams = {}) {
  const { data } = await api.get<ActivitySession[]>("/activities", { params });
  return data;
}

export async function filterActivitySessions(filterParams: Record<string, any>) {
  const { data } = await api.post<ActivitySession[]>("/api/activitysessions/filter", filterParams);
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