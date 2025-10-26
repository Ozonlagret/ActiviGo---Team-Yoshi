import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import ActivityCard from "./ActivityCard";
import type { ActivitySession, Activity } from "../types";
import { api } from "../api/client"; // or your actual API import

// Example API call (replace with your actual API function)
async function fetchActivityById(activityId: number): Promise<Activity & { sessions: ActivitySession[] }> {
  const { data } = await api.get(`/activities/${activityId}`);
  return data;
}

export default function ActivityDetails() {
  const { activityId } = useParams<{ activityId: string }>();
  const [activity, setActivity] = useState<Activity | null>(null);
  const [sessions, setSessions] = useState<ActivitySession[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!activityId) return;
    setLoading(true);
    setError(null);
    fetchActivityById(Number(activityId))
      .then((data) => {
        setActivity(data);
        setSessions(data.sessions || []);
      })
      .catch((err) => setError(err.message))
      .finally(() => setLoading(false));
  }, [activityId]);

  if (loading) return <div>Loading...</div>;
  if (error) return <div style={{ color: "red" }}>{error}</div>;
  if (!activity) return <div>No activity found.</div>;

  return (
    <div style={{ maxWidth: 600, margin: "0 auto", padding: 16 }}>
      <h1>{activity.name}</h1>
      {activity.imageUrl && (
        <img src={activity.imageUrl} alt={activity.name} style={{ width: "100%", maxHeight: 300, objectFit: "cover" }} />
      )}
      <p>{activity.description}</p>
      <h2>Upcoming Sessions</h2>
      {sessions.length === 0 && <div>No upcoming sessions.</div>}
      <div style={{ display: "flex", flexDirection: "column", gap: 12 }}>
        {sessions.map((session) => (
          <ActivityCard key={session.id} activitySession={session} />
        ))}
      </div>
    </div>
  );
}