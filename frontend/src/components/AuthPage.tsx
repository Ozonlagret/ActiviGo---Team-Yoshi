import { useState } from "react";
import { login, register } from "../api/auth";
import { useNavigate } from "react-router-dom";

export default function AuthPage() {
  const [mode, setMode] = useState<"login" | "register">("login");
  return (
    <div style={{ maxWidth: 460, margin: "0 auto" }}>
      <h1 style={{ marginBottom: 16 }}>{mode === "login" ? "Logga in" : "Skapa konto"}</h1>

      <div style={{ display: "flex", gap: 12, marginBottom: 24 }}>
        <button
          onClick={() => setMode("login")}
          style={{ padding: "6px 12px", background: mode === "login" ? "#222" : "#eee", color: mode === "login" ? "#fff" : "#222", border: "1px solid #aaa", borderRadius: 6 }}
        >Logga in</button>
        <button
          onClick={() => setMode("register")}
          style={{ padding: "6px 12px", background: mode === "register" ? "#222" : "#eee", color: mode === "register" ? "#fff" : "#222", border: "1px solid #aaa", borderRadius: 6 }}
        >Registrera</button>
      </div>

      {mode === "login" ? <LoginForm onSwitch={() => setMode("register")} /> : <RegisterForm onSwitch={() => setMode("login")} />}
    </div>
  );
}

function LoginForm({ onSwitch }: { onSwitch: () => void }) {
  const nav = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    try {
      await login({ email, password });
      nav("/", { replace: true });
    } catch (err: any) {
      setError(err?.response?.data?.title || err?.message || "Kunde inte logga in");
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={onSubmit} style={{ display: "grid", gap: 12 }}>
      <label>
        E-post
        <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required style={inputStyle} />
      </label>
      <label>
        Lösenord
        <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required style={inputStyle} />
      </label>
      {error && <div style={{ color: "#b00020" }}>{error}</div>}
      <button type="submit" disabled={loading} style={primaryBtnStyle}>{loading ? "Loggar in..." : "Logga in"}</button>
      <button type="button" onClick={onSwitch} style={linkBtnStyle}>Har du inget konto? Registrera dig</button>
    </form>
  );
}

function RegisterForm({ onSwitch }: { onSwitch: () => void }) {
  const nav = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    try {
      await register({ email, password, firstName, lastName });
      nav("/", { replace: true });
    } catch (err: any) {
      // If backend returns validation problem, surface it
      const detail = err?.response?.data;
      const msg = detail?.title || detail?.register?.join?.("\n") || err?.message || "Kunde inte registrera";
      setError(msg);
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={onSubmit} style={{ display: "grid", gap: 12 }}>
      <label>
        Förnamn
        <input type="text" value={firstName} onChange={(e) => setFirstName(e.target.value)} style={inputStyle} />
      </label>
      <label>
        Efternamn
        <input type="text" value={lastName} onChange={(e) => setLastName(e.target.value)} style={inputStyle} />
      </label>
      <label>
        E-post
        <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required style={inputStyle} />
      </label>
      <label>
        Lösenord
        <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required style={inputStyle} />
      </label>
      {error && <div style={{ color: "#b00020" }}>{error}</div>}
      <button type="submit" disabled={loading} style={primaryBtnStyle}>{loading ? "Skapar konto..." : "Registrera"}</button>
      <button type="button" onClick={onSwitch} style={linkBtnStyle}>Har du redan konto? Logga in</button>
    </form>
  );
}

const inputStyle: React.CSSProperties = {
  width: "100%",
  padding: "8px 10px",
  borderRadius: 6,
  border: "1px solid #ccc",
  marginTop: 4,
};

const primaryBtnStyle: React.CSSProperties = {
  padding: "10px 14px",
  background: "#1a73e8",
  color: "white",
  border: "none",
  borderRadius: 6,
  cursor: "pointer",
};

const linkBtnStyle: React.CSSProperties = {
  padding: 0,
  border: "none",
  background: "transparent",
  color: "#1a73e8",
  textAlign: "left",
  cursor: "pointer",
};
