import { api } from "./client";

export type Category = {
  id: number;
  name: string;
};

export async function listLocations() {
  const { data } = await api.get<Location[]>("/api/locations");
  return data;
}