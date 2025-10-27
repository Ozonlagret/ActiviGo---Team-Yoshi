import { api } from "./client";

export type LoginRequest = {
  email: string;
  password: string;
};

export type RegisterRequest = {
  email: string;
  password: string;
  firstName?: string;
  lastName?: string;
  role?: string; // optional; server defaults typically to User
};

export type AuthResponse = {
  token: string;
  expiresAtUtc: string; // ISO date
  role?: string | null;
  name: string;
  userId: number;
};

export async function login(req: LoginRequest): Promise<AuthResponse> {
  const { data } = await api.post<AuthResponse>("/api/auth/login", req);
  // persist token and basic user info for UI
  localStorage.setItem("access_token", data.token);
  localStorage.setItem("user_name", data.name ?? "");
  if (data.role) localStorage.setItem("user_role", data.role);
  // notify app
  window.dispatchEvent(new CustomEvent("auth:changed", { detail: { loggedIn: true, role: data.role, name: data.name } }));
  return data;
}

export async function register(req: RegisterRequest): Promise<AuthResponse> {
  const { data } = await api.post<AuthResponse>("/api/auth/register", req);
  // persist token and basic user info for UI (API logs in on register)
  localStorage.setItem("access_token", data.token);
  localStorage.setItem("user_name", data.name ?? "");
  if (data.role) localStorage.setItem("user_role", data.role);
  window.dispatchEvent(new CustomEvent("auth:changed", { detail: { loggedIn: true, role: data.role, name: data.name } }));
  return data;
}

export function logout() {
  localStorage.removeItem("access_token");
  localStorage.removeItem("user_name");
  localStorage.removeItem("user_role");
  window.dispatchEvent(new CustomEvent("auth:changed", { detail: { loggedIn: false } }));
}

export function getAuthInfo() {
  const token = localStorage.getItem("access_token");
  const role = localStorage.getItem("user_role") || undefined;
  const name = localStorage.getItem("user_name") || undefined;
  return { token, role, name, loggedIn: !!token } as const;
}
