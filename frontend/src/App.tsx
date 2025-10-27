import { BrowserRouter, Routes, Route, Link, Navigate, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import ActivitySessionFilterPage from "./components/ActivitySessionFilterPage.tsx";
import CreateEntity from "./components/admin/CreateEntity.tsx";
import AuthPage from "./components/AuthPage.tsx";
import { getAuthInfo, logout } from "./api/auth.ts";
import MyBookings from "./components/MyBookings.tsx";

export default function App() {
  const [auth, setAuth] = useState(() => getAuthInfo());

  useEffect(() => {
    const onAuthChanged = () => setAuth(getAuthInfo());
    const onStorage = (e: StorageEvent) => {
      if (["access_token", "user_role", "user_name"].includes(e.key || "")) setAuth(getAuthInfo());
    };
    window.addEventListener("auth:changed", onAuthChanged as EventListener);
    window.addEventListener("storage", onStorage);
    return () => {
      window.removeEventListener("auth:changed", onAuthChanged as EventListener);
      window.removeEventListener("storage", onStorage);
    };
  }, []);

  const isAdmin = auth.role === "Admin";

  return (
    <BrowserRouter>
      <div>
        {/* Navbar */}
        <nav style={{ 
          display: "flex", 
          justifyContent: "space-between", 
          alignItems: "center",
          padding: "1rem 2rem",
          borderBottom: "1px solid #ccc"
        }}>
          <div style={{ display: "flex", gap: "2rem" }}>
            <Link to="/">Hem</Link>
            <Link to="/activities">Aktiviteter</Link>
            {isAdmin && <Link to="/admin">Admin</Link>}
            {auth.loggedIn && <Link to="/bookings">Mina bokningar</Link>}
          </div>

          {/* Right-aligned auth actions */}
          <div style={{ display: "flex", alignItems: "center", gap: 12 }}>
            {auth.loggedIn && auth.name && (
              <span style={{ color: "#444" }}>Hej, {auth.name}</span>
            )}
            {auth.loggedIn ? (
              <LogoutButton />
            ) : (
              <Link to="/auth">Logga in / Registrera</Link>
            )}
          </div>
        </nav>

        {/* Main Content */}
        <main style={{ padding: "2rem" }}>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/activities" element={<ActivitiesPage />} />
            <Route path="/admin/*" element={<RequireAdmin isAdmin={isAdmin}><AdminPage /></RequireAdmin>} />
            <Route path="/bookings" element={<RequireAuth loggedIn={auth.loggedIn}><MyBookings /></RequireAuth>} />
            <Route path="/auth" element={<AuthPage />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}

function HomePage() {
  return (
    <div>
      <h1>Välkommen till ActiviGo</h1>
      <p>Din plattform för aktiviteter</p>
    </div>
  );
}

function ActivitiesPage() {
  return <ActivitySessionFilterPage />;
}

function AdminPage() {
  return (
    <div>
      <h1>Admin Panel</h1>
      
      <nav style={{ display: "flex", gap: "1rem", marginBottom: "2rem", borderBottom: "1px solid #ccc", paddingBottom: "0.5rem" }}>
        <Link to="/admin/overview">Översikt</Link>
        <Link to="/admin/create">Lägg till</Link>
        <Link to="/admin/manage">Redigera/Ta bort</Link>
      </nav>

      <Routes>
        <Route path="overview" element={<AdminOverview />} />
        <Route path="create" element={<AdminCreate />} />
        <Route path="manage" element={<AdminManage />} />
        <Route path="/" element={<AdminOverview />} /> {/* Default */}
      </Routes>
    </div>
  );
}

function LogoutButton() {
  const nav = useNavigate();
  return (
    <button
      onClick={() => { logout(); nav("/", { replace: true }); }}
      style={{ background: "transparent", border: "1px solid #aaa", padding: "6px 12px", borderRadius: 6, cursor: "pointer" }}
    >Logga ut</button>
  );
}

function RequireAdmin({ isAdmin, children }: { isAdmin: boolean; children: React.ReactNode }) {
  if (!isAdmin) return <Navigate to="/auth" replace />;
  return <>{children}</>;
}

function RequireAuth({ loggedIn, children }: { loggedIn: boolean; children: React.ReactNode }) {
  if (!loggedIn) return <Navigate to="/auth" replace />;
  return <>{children}</>;
}

function AdminOverview() {
  return <div><h2>Översikt</h2><p>Dashboard här...</p></div>;
}

function AdminCreate() {
  return <div><h2>Lägg till</h2><p>Skapa nya entiteter här...</p>
  <div><CreateEntity /></div></div>;
}

function AdminManage() {
  return <div><h2>Redigera/Ta bort</h2><p>Hantera befintliga entiteter här...</p></div>;
}
