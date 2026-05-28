import { api } from "./client";

export type SaveHealthLogRequest = {
  logDate?: string;
  steps?: number | null;
  calories?: number | null;
  weightKg?: number | null;
  heightCm?: number | null;
};

export type HealthLogResponse = {
  id: number;
  logDate: string;
  steps?: number | null;
  calories?: number | null;
  weightKg?: number | null;
  heightCm?: number | null;
  createdAtUtc: string;
  updatedAtUtc: string;
};

export type HealthProfileResponse = {
  weightKg?: number | null;
  heightCm?: number | null;
  logDate?: string | null;
  updatedAtUtc?: string | null;
};

export type HealthStatsResponse = {
  totalEntries: number;
  totalSteps: number;
  totalCalories: number;
  averageWeightKg?: number | null;
};

export async function saveHealthLog(req: SaveHealthLogRequest) {
  const { data } = await api.post<HealthLogResponse>("/api/health-tracker/logs", req);
  return data;
}

export async function getHealthLogs() {
  const { data } = await api.get<HealthLogResponse[]>("/api/health-tracker/logs");
  return data;
}

export async function getHealthProfile() {
  const { data } = await api.get<HealthProfileResponse>("/api/health-tracker/profile");
  return data;
}

export async function getHealthStats() {
  const { data } = await api.get<HealthStatsResponse>("/api/health-tracker/stats");
  return data;
}