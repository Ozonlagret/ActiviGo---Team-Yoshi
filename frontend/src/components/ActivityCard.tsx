// import type { Activity } from "../api/activities";
import type { FilterActivitySessionResponse } from "../types";
import "../styles/activity-card.css";

export default function ActivityCard({ activity }: { activity: FilterActivitySessionResponse }) {
  return (
    <article className="activity-card">
      <div className="activity-card__imgHolder">
        {activity.imageUrl ? (
          <img src={activity.imageUrl} alt={activity.name} className="activity-card__img" />
        ) : (
          <div className="activity-card__imgPlaceholder">🖼️</div>
        )}
      </div>

      <h3 className="activity-card__title">{activity.name}</h3>
      <p className="activity-card__muted">{activity.location ?? activity.category ?? ""}</p>
      <p className="activity-card__desc">{activity.description?.slice(0, 120) ?? "—"}</p>

      <a href={`/activities/${activity.sessionId}`} className="activity-card__link">Read more</a>
    </article>
  );
}