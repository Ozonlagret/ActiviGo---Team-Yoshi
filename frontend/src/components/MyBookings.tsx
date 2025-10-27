import { useEffect, useState } from "react";
import { cancelBooking, getMyBookings, type BookingResponse, type MyBookingsResponse } from "../api/bookings";
import { getAuthInfo } from "../api/auth";
import { Link, useNavigate } from "react-router-dom";

export default function MyBookingsPage() {
	const nav = useNavigate();
	const [data, setData] = useState<MyBookingsResponse | null>(null);
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState<string | null>(null);
	const auth = getAuthInfo();

	useEffect(() => {
		if (!auth.loggedIn) {
			nav("/auth", { replace: true });
			return;
		}
		setLoading(true);
		setError(null);
		getMyBookings()
			.then(setData)
			.catch((e) => setError(e?.response?.data?.title || e?.message || "Kunde inte hämta bokningar"))
			.finally(() => setLoading(false));
	}, []);

	return (
		<div style={{ maxWidth: 700, margin: "0 auto", padding: 16 }}>
			<h1>Mina bokningar</h1>
			{loading && <div>Laddar...</div>}
			{error && <div style={{ color: "#b00020" }}>{error}</div>}
			{data && (
				<>
					<Section title="Kommande" emptyText="Inga kommande bokningar" items={data.upcoming} />
					<Section title="Tidigare" emptyText="Inga tidigare bokningar" items={data.past} />
					<Section title="Avbokade" emptyText="Inga avbokade" items={data.canceled} allowCancel={false} />
				</>
			)}
		</div>
	);
}

function Section({ title, emptyText, items, allowCancel = true }: { title: string; emptyText: string; items: BookingResponse[]; allowCancel?: boolean; }) {
	return (
		<section style={{ marginTop: 24 }}>
			<h2 style={{ marginBottom: 12 }}>{title}</h2>
			{items.length === 0 ? (
				<div style={{ color: "#666" }}>{emptyText}</div>
			) : (
				<ul style={{ listStyle: "none", padding: 0, display: "grid", gap: 12 }}>
					{items.map((b) => (
						<li key={b.id} style={{ border: "1px solid #ddd", borderRadius: 8, padding: 12, display: "flex", alignItems: "center", justifyContent: "space-between" }}>
							<div>
								<div style={{ fontWeight: 600 }}>{b.activityName}</div>
								<div style={{ fontSize: 14, color: "#555" }}>{formatRange(b.startDateUtc, b.endDateUtc)} • Status: {b.status}</div>
							</div>
							<div style={{ display: "flex", gap: 8 }}>
								<Link to={`/activities/${b.activitySessionId}`} style={{ textDecoration: "none", padding: "6px 10px", border: "1px solid #aaa", borderRadius: 6 }}>Visa</Link>
								{allowCancel && b.status === "Active" && (
									<CancelBtn id={b.id} />
								)}
							</div>
						</li>
					))}
				</ul>
			)}
		</section>
	);
}

function CancelBtn({ id }: { id: number }) {
	const [loading, setLoading] = useState(false);
	const [done, setDone] = useState(false);
	return (
		<button
			onClick={async () => {
				if (!confirm("Vill du avboka?")) return;
				try {
					setLoading(true);
					await cancelBooking(id);
					setDone(true);
				} finally {
					setLoading(false);
					// soft refresh
					window.location.reload();
				}
			}}
			disabled={loading || done}
			style={{ padding: "6px 10px", border: "1px solid #aaa", borderRadius: 6, background: "#eee" }}
		>{loading ? "Avbokar..." : done ? "Avbokad" : "Avboka"}</button>
	);
}

function formatRange(startIso: string, endIso: string) {
	const s = new Date(startIso);
	const e = new Date(endIso);
	const date = s.toLocaleDateString("sv-SE");
	const t1 = s.toLocaleTimeString("sv-SE", { hour: "2-digit", minute: "2-digit" });
	const t2 = e.toLocaleTimeString("sv-SE", { hour: "2-digit", minute: "2-digit" });
	return `${date} ${t1}–${t2}`;
}
