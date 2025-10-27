import { useEffect, useState } from "react";
import { filterActivitySessions } from "../api/activitySessions";
import { listCategories} from "../api/categories";
import { listLocations} from "../api/activitySessions";
import ActivityCard from "./ActivityCard";
import type { Category, FilterActivitySessionResponse, Location} from "../types";

function formatDate(date: Date) {
  return date.toISOString().split("T")[0]; // yyyy-mm-dd
}

function ActivityPage() {
  const [currentDate, setCurrentDate] = useState(new Date());

  const [sessions, setSessions] = useState<FilterActivitySessionResponse[]>([]);

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [categories, setCategories] = useState<Category[]>([]);
  const [locations, setLocations] = useState<Location[]>([]);

  const [categoryId, setCategoryId] = useState<number | undefined>(undefined);
  const [locationId, setLocationId] = useState<number | undefined>(undefined);
  const [isIndoor, setIsIndoor] = useState<boolean | undefined>(undefined);

    useEffect(() => {
    listCategories().then(setCategories);
    listLocations().then(setLocations);
  }, []);

  useEffect(() => {
    setLoading(true);
    setError(null);
    
    const dateStr = formatDate(currentDate);
    console.log('🔍 Filtering with date:', dateStr, 'categoryId:', categoryId, 'locationId:', locationId, 'isIndoor:', isIndoor);
    
    filterActivitySessions({ 
      dateFrom: dateStr, 
      dateTo: dateStr,
      categoryId,
      locationId,
      isIndoor
     })
      .then((data) => {
        setSessions(data);
      })
      .catch((error) => {
        console.error('❌ Filter error:', error);
        setError(error.message);
      })
      .finally(() => setLoading(false));
  }, [currentDate, categoryId, locationId, isIndoor]);

  const goToPreviousDay = () => {
    setCurrentDate((prev) => {
      const d = new Date(prev);
      d.setDate(d.getDate() - 1);
      return d;
    });
  };

  const goToNextDay = () => {
    setCurrentDate((prev) => {
      const d = new Date(prev);
      d.setDate(d.getDate() + 1);
      return d;
    });
  };

  return (
    <div style={{ maxWidth: 600, margin: "0 auto", padding: 16 }}>
      <div style={{ display: "flex", gap: 12, marginBottom: 16, flexWrap: "wrap" }}>
        <select value={categoryId || ""} onChange={(e) => setCategoryId(e.target.value ? Number(e.target.value) : undefined)}>
          <option value="">Alla Kategorier</option>
          {categories.map((cat) => (
            <option key={cat.id} value={cat.id}>{cat.name}</option>
          ))}
        </select>

        <select value={locationId || ""} onChange={(e) => setLocationId(e.target.value ? Number(e.target.value) : undefined)}>
          <option value="">Alla lokaler</option>
          {locations.map((loc) => (
            <option key={loc.id} value={loc.id}>{loc.name}</option>
          ))}
        </select>

        <select value={isIndoor === undefined ? "" : String(isIndoor)} onChange={(e) => setIsIndoor(e.target.value === "" ? undefined : e.target.value === "true")}>
          <option value="">Inomhus/Utomhus</option>
          <option value="true">Inomhus</option>
          <option value="false">Utomhus</option>
        </select>
      </div>

      <div style={{ display: "flex", alignItems: "center", marginBottom: 16 }}>
        <button onClick={goToPreviousDay}>&lt;</button>
        <h2 style={{ flex: 1, textAlign: "center" }}>
          {currentDate.toLocaleDateString()}
        </h2>
        <button onClick={goToNextDay}>&gt;</button>
      </div>
      {loading && <div>Loading...</div>}
      {error && <div style={{ color: "red" }}>{error}</div>}
      {sessions.length === 0 && !loading && <div>Inga aktiviteter för denna dag.</div>}
      <div style={{ display: "flex", flexDirection: "column", gap: 12 }}>
        {sessions.map((session) => (
          <ActivityCard key={session.sessionId} activity={session} />
        ))}
      </div>
    </div>
  );
}

export default ActivityPage;