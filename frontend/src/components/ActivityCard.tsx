import type { Activity } from "../api/activities";
import "../styles/activity-card.css";

export default function ActivityCard({ a }: { a: Activity }) {
  return (
    <article className="activity-card">
      <div className="activity-card__imgHolder">
        {a.imageUrl ? (
          <img src={a.imageUrl} alt={a.name} className="activity-card__img" />
        ) : (
          <div className="activity-card__imgPlaceholder">🖼️</div>
        )}
      </div>

      <h3 className="activity-card__title">{a.name}</h3>
      <p className="activity-card__muted">{a.locationName ?? a.categoryName ?? ""}</p>
      <p className="activity-card__desc">{a.description?.slice(0, 120) ?? "—"}</p>

      <a href={`/activities/${a.id}`} className="activity-card__link">Read more</a>
    </article>
  );
}