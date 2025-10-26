import { Link } from "react-router-dom";
import "../styles/header.css";

export default function Header() {
  return (
    <header className="ag-header">
      <div className="ag-header__inner">
        <Link to="/" className="ag-brand">ActiviGo</Link>

        <nav className="ag-nav">
          <Link to="/bookings" className="ag-nav__link">Mina Bokningar</Link>
          <Link to="/admin" className="ag-nav__link">Admin</Link>
        </nav>

        <div className="ag-auth">
          <Link to="/auth" className="ag-btn ag-btn--dark">Logga in / Registrera dig</Link>
        </div>
      </div>
    </header>
  );
}