import { useState } from "react";
import { login, register } from "../api/auth";
import { useNavigate } from "react-router-dom";

export default function AuthPage() {
  const [mode, setMode] = useState<"login" | "register">("login");
  return (
    <div className="container" style={{ maxWidth: 560 }}>
      <h1 style={{ marginBottom: 16 }}>{mode === "login" ? "Logga in" : "Skapa konto"}</h1>

      <div className="row" style={{ marginBottom: 24 }}>
        <button
          onClick={() => setMode("login")}
          className={`btn btn-toggle ${mode === "login" ? "is-active" : ""}`}
        >Logga in</button>
        <button
          onClick={() => setMode("register")}
          className={`btn btn-toggle ${mode === "register" ? "is-active" : ""}`}
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
    <form onSubmit={onSubmit} className="form">
      <label className="label">
        E-post
        <input className="input" type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
      </label>
      <label className="label">
        Lösenord
        <input className="input" type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
      </label>
      {error && <div className="error">{error}</div>}
      <button type="submit" disabled={loading} className="btn btn-primary">{loading ? "Loggar in..." : "Logga in"}</button>
      <button type="button" onClick={onSwitch} className="btn btn-link">Har du inget konto? Registrera dig</button>
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
    <form onSubmit={onSubmit} className="form">
      <label className="label">
        Förnamn
        <input className="input" type="text" value={firstName} onChange={(e) => setFirstName(e.target.value)} />
      </label>
      <label className="label">
        Efternamn
        <input className="input" type="text" value={lastName} onChange={(e) => setLastName(e.target.value)} />
      </label>
      <label className="label">
        E-post
        <input className="input" type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
      </label>
      <label className="label">
        Lösenord
        <input className="input" type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
      </label>
      {error && <div className="error">{error}</div>}
      <button type="submit" disabled={loading} className="btn btn-primary">{loading ? "Skapar konto..." : "Registrera"}</button>
      <button type="button" onClick={onSwitch} className="btn btn-link">Har du redan konto? Logga in</button>
    </form>
  );
}

