import { useEffect, useState } from "react";

export default function App() {
  const [msg, setMsg] = useState("Testar API…");
  useEffect(() => {
    const base = import.meta.env.VITE_API_BASE_URL as string;
    fetch(`${base}/swagger/v1/swagger.json`)
      .then(r => setMsg(r.ok ? `OK ✅  ${base}` : `HTTP ${r.status} från ${base}`))
      .catch(e => setMsg(`Kunde inte nå API ❌ (${e})`));
  }, []);
  return <div style={{ padding: 24 }}>{msg}</div>;
}
