import axios from "axios";

//
const baseURL = import.meta.env.VITE_API_BASE_URL?.replace(/\/$/, "") || "";

console.log("🔍 VITE_API_BASE_URL:", import.meta.env.VITE_API_BASE_URL);
console.log("🔍 Final baseURL:", baseURL);

export const api = axios.create({
  baseURL: baseURL,
});

api.interceptors.request.use((cfg) => {
  const token = localStorage.getItem("access_token");
  if (token) cfg.headers.Authorization = `Bearer ${token}`;
  return cfg;
});