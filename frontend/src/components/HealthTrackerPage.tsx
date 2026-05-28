import { useEffect, useState } from "react";
import { getHealthLogs, getHealthProfile, saveHealthLog, type HealthLogResponse, type HealthProfileResponse } from "../api/healthTracker";

function todayValue() {
  return new Date().toISOString().slice(0, 10);
}

export default function HealthTrackerPage() {
  const [logDate, setLogDate] = useState(todayValue());
  const [steps, setSteps] = useState("");
  const [calories, setCalories] = useState("");
  const [weightKg, setWeightKg] = useState("");
  const [heightCm, setHeightCm] = useState("");

  const [message, setMessage] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const [profile, setProfile] = useState<HealthProfileResponse | null>(null);
  const [logs, setLogs] = useState<HealthLogResponse[]>([]);

  useEffect(() => {
    void loadData();
  }, []);

  async function loadData() {
    try {
      const [profileData, logData] = await Promise.allSettled([getHealthProfile(), getHealthLogs()]);

      if (profileData.status === "fulfilled") setProfile(profileData.value);
      if (logData.status === "fulfilled") setLogs(logData.value);
    } catch {
      // Intentionally silent for the first basic version.
    }
  }

  async function onSave(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);
    setMessage(null);

    try {
      await saveHealthLog({
        logDate,
        steps: steps ? Number(steps) : null,
        calories: calories ? Number(calories) : null,
        weightKg: weightKg ? Number(weightKg) : null,
        heightCm: heightCm ? Number(heightCm) : null,
      });

      setMessage("Sparat.");
      await loadData();
    } catch (err: any) {
      setMessage(err?.response?.data || err?.message || "Kunde inte spara");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div style={{ maxWidth: 720 }}>
      <h1>Health Tracker</h1>
      <p style={{ marginBottom: 24 }}>En minimal sida för att logga steg, kalorier, vikt och längd.</p>

      <form onSubmit={onSave} className="form" style={{ marginBottom: 28 }}>
        <label className="label">
          Datum
          <input className="input" type="date" value={logDate} onChange={(e) => setLogDate(e.target.value)} />
        </label>

        <label className="label">
          Steg
          <input className="input" type="number" min="0" value={steps} onChange={(e) => setSteps(e.target.value)} />
        </label>

        <label className="label">
          Kalorier
          <input className="input" type="number" min="0" value={calories} onChange={(e) => setCalories(e.target.value)} />
        </label>

        <label className="label">
          Vikt (kg)
          <input className="input" type="number" min="0" step="0.1" value={weightKg} onChange={(e) => setWeightKg(e.target.value)} />
        </label>

        <label className="label">
          Längd (cm)
          <input className="input" type="number" min="0" value={heightCm} onChange={(e) => setHeightCm(e.target.value)} />
        </label>

        <button type="submit" className="btn btn-primary" disabled={loading}>
          {loading ? "Sparar..." : "Spara"}
        </button>
        {message && <div className="muted" style={{ marginTop: 12 }}>{message}</div>}
      </form>

      <section style={{ marginBottom: 24 }}>
        <h2>Profil</h2>
        {profile ? (
          <div>
            <div>Vikt: {profile.weightKg ?? "-"} kg</div>
            <div>Längd: {profile.heightCm ?? "-"} cm</div>
            <div>Senast uppdaterad: {profile.updatedAtUtc ? new Date(profile.updatedAtUtc).toLocaleString("sv-SE") : "-"}</div>
          </div>
        ) : (
          <div className="muted">Ingen profilhistorik ännu.</div>
        )}
      </section>

      <section>
        <h2>Senaste loggar</h2>
        {logs.length === 0 ? (
          <div className="muted">Inga loggar ännu.</div>
        ) : (
          <div style={{ display: "grid", gap: 12 }}>
            {logs.slice(0, 10).map((log) => (
              <article key={log.id} style={{ border: "1px solid #d8d8d8", borderRadius: 10, padding: 12 }}>
                <strong>{log.logDate}</strong>
                <div>Steg: {log.steps ?? "-"}</div>
                <div>Kalorier: {log.calories ?? "-"}</div>
                <div>Vikt: {log.weightKg ?? "-"} kg</div>
                <div>Längd: {log.heightCm ?? "-"} cm</div>
              </article>
            ))}
          </div>
        )}
      </section>
    </div>
  );
}