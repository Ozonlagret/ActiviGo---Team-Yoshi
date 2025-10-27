import { useState, useEffect } from 'react';
import { listActivities } from "../../api/activities";
import type { Activity } from "../../api/activities";

// Används i CreateEntity.tsx
export default function SessionForm() {
    const [activities, setActivities] = useState<Activity[]>([]);

    useEffect(() => {
    // Fetch activities when component loads
    async function loadActivities() {
        try {
        const data = await listActivities();
        setActivities(data);
        } catch (error) {
        console.error('Failed to load activities:', error);
        }
    }
    
    loadActivities();
    }, []);

    return (
    <form className="entity-form" style={{ display: "flex", flexDirection: "column", gap: "1rem", maxWidth: "500px" }}>
      <h3>Skapa ny aktivitetssession</h3>
      
      <label style={{ display: "flex", flexDirection: "column" }}>
        Aktivitet:
        <select name="activityId" required>
        <option value="">-- Välj aktivitet --</option>
        {activities.map(activity => (
            <option key={activity.id} value={activity.id}>
            {activity.name}
            </option>
        ))}
        </select>
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Plats:
        <select name="locationId" required>
          {/* Fetch locations from API */}
        </select>
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Starttid:
        <input type="datetime-local" name="startUtc" required />
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Sluttid:
        <input type="datetime-local" name="endUtc" required />
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Kapacitet:
        <input type="number" name="capacity" required />
      </label>

      <button type="submit">Skapa session</button>
    </form>
  );
}