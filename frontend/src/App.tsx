import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import ActivitySessionFilterPage from "./components/ActivitySessionFilterPage.tsx";
import CreateEntity from "./components/admin/CreateEntity.tsx";

export default function App() {
  const isAdmin = true; // TODO: Replace with actual auth check

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
          </div>
          
          {isAdmin && (
            <Link to="/admin">Admin</Link>
          )}
        </nav>

        {/* Main Content */}
        <main style={{ padding: "2rem" }}>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/activities" element={<ActivitiesPage />} />
            {isAdmin && (<Route path="/admin/*" element={<AdminPage />} />)}
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
