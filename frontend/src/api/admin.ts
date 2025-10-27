import type { CreateActivityRequest, ActivityResponse } from "../types";
import { api } from "./client";

export async function CreateActivity(filterParams: CreateActivityRequest) {
  const { data } = await api.post<ActivityResponse>("/api/admin/create/activity", filterParams);
  return data;
}