// Enkel detaljsida: läser :id från URLen, hämtar aktivitet och visar den.

import { useParams, Link } from "react-router-dom";
import { useQuery } from "@tanstack/react-query";
import { getActivity } from "../../api/activities";
import "../../styles/activity-details.css";

export default function ActivityDetails() {
  // :id kommer som string → parsa till number (fallback -1 om saknas/ogiltig)
  const { id } = useParams();
  const numericId = Number(id);

  const { data: activity, isLoading, isError } = useQuery({
    queryKey: ["activity", numericId],
    queryFn: () => getActivity(numericId),
    enabled: Number.isFinite(numericId) && numericId > 0, // undvik fetch på ogiltigt id
  });

  return (
    <main className="ad">
      <div className="ad__container">
        <div className="ad__toprow">
          <Link to="/" className="ad__back">← Tillbaka</Link>
        </div>

        {isLoading && <div className="ad__status">Laddar…</div>}
        {isError && <div className="ad__status ad__status--error">Kunde inte hämta aktiviteten.</div>}
        {!isLoading && !isError && activity && (
          <article className="ad__content">
            <div className="ad__media">
              {activity.imageUrl ? (
                <img src={activity.imageUrl} alt={activity.name} className="ad__img" />
              ) : (
                <div className="ad__placeholder">🖼️</div>
              )}
            </div>

            <div className="ad__body">
              <h1 className="ad__title">{activity.name}</h1>
              <p className="ad__meta">
                {activity.locationName ?? activity.categoryName ?? "—"}
                {activity.isOutdoor ? " • Utomhus" : ""}
              </p>
              <p className="ad__desc">{activity.description ?? "Ingen beskrivning."}</p>

              <div className="ad__actions">
                {/* här ska en “Boka” knapp läggas till senare */}
                <Link to="/" className="ad__btn">Till listan</Link>
              </div>
            </div>
          </article>
        )}
      </div>
    </main>
  );
}