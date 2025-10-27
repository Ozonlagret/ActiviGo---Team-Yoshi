// import type { Activity } from "../api/activities";
import type { FilterActivitySessionResponse } from "../types";
import { bookSession } from "../api/bookings";
import { getAuthInfo } from "../api/auth";
import { useState } from "react";
import "../styles/activity-card.css";

export default function ActivityCard({ activity }: { activity: FilterActivitySessionResponse }) {
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<string | null>(null);
  const formatTime = (utcString?: string) => {
    if (!utcString) return "";
    const date = new Date(utcString);
    return date.toLocaleTimeString("sv-SE", { hour: "2-digit", minute: "2-digit" });
  };

  return (
    <article className="activity-card">
      <div className="activity-card__imgHolder">
        {activity.imageUrl ? (
          <img src={activity.imageUrl} alt={activity.activityName || "Aktivitet"} className="activity-card__img" />
        ) : (
          <div className="activity-card__imgPlaceholder">🖼️</div>
        )}
      </div>

      <h3 className="activity-card__title">{activity.activityName}</h3>
      <p className="activity-card__muted">
         {activity.locationName || "Ingen plats"} • {formatTime(activity.startUtc)}
      </p>
      <p className="activity-card__desc">{activity.description?.slice(0, 120) ?? "—"}</p>

      <div style={{ display: "flex", gap: 8, alignItems: "center", justifyContent: "space-between" }}>
        <a href={`/activities/${activity.id}`} className="activity-card__link">Visa mer</a>
        <button
          disabled={!activity.id || loading}
          onClick={async () => {
            setMessage(null);
            const auth = getAuthInfo();
            if (!auth.loggedIn) {
              window.location.href = "/auth";
              return;
            }
            try {
              setLoading(true);
              await bookSession(activity.id);
              setMessage("Bokad!");
              setTimeout(() => setMessage(null), 3000);
            } catch (e: any) {
              const errMsg = e?.response?.data?.title || e?.message || "Kunde inte boka";
              setMessage(errMsg);
            } finally {
              setLoading(false);
            }
          }}
          className="activity-card__link"
          style={{ padding: "6px 10px", border: "1px solid #aaa", borderRadius: 6, background: "#1a73e8", color: "#fff" }}
        >{loading ? "Bokar..." : "Boka"}</button>
      </div>
      {message && <div style={{ marginTop: 8, color: message === "Bokad!" ? "green" : "#b00020" }}>{message}</div>}
    </article>
  );
}