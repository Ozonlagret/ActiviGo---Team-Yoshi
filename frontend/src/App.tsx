import { useEffect, useState } from "react";
import ActivityPage from "./components/ActivitySessionFilterPage.tsx"

export default function App() {
  const [msg, setMsg] = useState("Testar API…");
  useEffect(() => {
    const base = import.meta.env.VITE_API_BASE_URL as string;
    // Use a real API endpoint, not swagger JSON
    fetch(`${base}/api/activities`)
      .then(r => r.ok ? r.json() : Promise.reject(`HTTP ${r.status}`))
      .then(data => setMsg(`OK ✅ ${base} - Found ${data.length} activities`))
      .catch(e => setMsg(`Kunde inte nå API ❌ (${e})`));
  }, []);


  return <div 
  
  
    style={{ padding: 24 }}>{msg}
    
    <div>
      <ActivityPage />
    </div>
    </div>;
}
