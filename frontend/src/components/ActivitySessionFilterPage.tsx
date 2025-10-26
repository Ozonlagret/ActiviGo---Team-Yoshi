import React, { useEffect, useState } from "react";
import { filterActivitySessions } from "../api/activitySessions";
import ActivityCard from "./ActivityCard";
import type { ActivitySession, FilterActivitySessionResponse } from "../types";

function formatDate(date: Date) {
  return date.toISOString().split("T")[0]; // yyyy-mm-dd
}

function ActivityPage() {
  const [currentDate, setCurrentDate] = useState(new Date());
  const [sessions, setSessions] = useState<FilterActivitySessionResponse[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setLoading(true);
    setError(null);
    filterActivitySessions({ dateFrom: formatDate(currentDate), dateTo: formatDate(currentDate) })
      .then(setSessions)
      .catch((error) => setError(error.message))
      .finally(() => setLoading(false));
  }, [currentDate]);

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
      <div style={{ display: "flex", alignItems: "center", marginBottom: 16 }}>
        <button onClick={goToPreviousDay}>&lt;</button>
        <h2 style={{ flex: 1, textAlign: "center" }}>
          {currentDate.toLocaleDateString()}
        </h2>
        <button onClick={goToNextDay}>&gt;</button>
      </div>
      {loading && <div>Loading...</div>}
      {error && <div style={{ color: "red" }}>{error}</div>}
      {sessions.length === 0 && !loading && <div>No activities for this day.</div>}
      <div style={{ display: "flex", flexDirection: "column", gap: 12 }}>
        {sessions.map((session) => (
          <ActivityCard key={session.sessionId} activity={session} />
        ))}
      </div>
    </div>
  );
}

export default ActivityPage;