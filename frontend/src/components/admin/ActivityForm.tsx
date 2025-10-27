import { useState } from "react";
import { useEffect } from "react";
import type { CreateActivityRequest } from "../../types";
import { listCategories } from "../../api/categories";
import { CreateActivity } from "../../api/admin";

export default function ActivityForm() {
    const [categories, setCategories] = useState<Array<{id: number, name: string}>>([]);

    useEffect(() => {
        async function loadCategories() {
            try {
            const data = await listCategories();
            setCategories(data);
            } catch (error) {
            console.error('Laddning av kategorier misslyckades:', error);
            }
        }
        loadCategories();
        }, []);

    const [formData, setFormData] = useState(() => ({
        name: '',
        description: '',
        categoryId: 0,
        imageUrl: '',
        isActive: true,
        price: 0,
        isOutdoor: false
    }));

    const [durationMinutes, setDurationMinutes] = useState(60);

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);
        setSuccess(false);

        try {
            // Convert minutes to TimeSpan format
            const hours = Math.floor(durationMinutes / 60);
            const mins = durationMinutes % 60;
            const standardDuration = `${String(hours).padStart(2, '0')}:${String(mins).padStart(2, '0')}:00`;

            const request: CreateActivityRequest = {
                id: 0,
                ...formData,
                standardDuration
            };

            const response = await CreateActivity(request);
            console.log("Activity created:", response);
            setSuccess(true);
            // Reset form
            setFormData({
                name: '',
                description: '',
                categoryId: 0,
                imageUrl: '',
                isActive: true,
                price: 0,
                isOutdoor: false
            });
            setDurationMinutes(60);
        } catch (err: any) {
            console.error("Failed to create activity:", err);
            setError(err?.response?.data?.message || "Misslyckades med att skapa aktivitet");
        } finally {
            setLoading(false);
        }
    };

  return (
    <form className="entity-form" onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: "1rem", maxWidth: "500px" }}>
      <h3>Skapa ny aktivitet</h3>
      
      {error && <div style={{ color: "red", padding: "0.5rem", background: "#fee" }}>{error}</div>}
      {success && <div style={{ color: "green", padding: "0.5rem", background: "#efe" }}>Aktivitet skapad!</div>}
      
      <label style={{ display: "flex", flexDirection: "column" }}>
        Namn:
        <input type="text" name="name" required 
        value={formData.name} onChange={(e) => setFormData({ ...formData, name: e.target.value })} />
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Beskrivning:
        <textarea 
            name="description" 
            rows={3} 
            value={formData.description} onChange={(e) => setFormData({ ...formData, description: e.target.value })} />
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Standard tidslängd (minuter):
        <input 
            type="number" 
            name="duration"
            min="1"
            value={durationMinutes}
            onChange={(e) => setDurationMinutes(Number(e.target.value))} />
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Kategori:
        <select name="categoryId" 
            value={formData.categoryId} 
            onChange={(e) => setFormData({ ...formData, categoryId: Number(e.target.value) })}>
        <option value="">-- Välj kategori --</option>
        {categories.map(category => (
            <option key={category.id} value={category.id}>
            {category.name}
            </option>
        ))}
        </select>
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Standardkostnad (kr):
        <input type="number" name="price" 
        value={formData.price} onChange={(e) => setFormData({ ...formData, price: Number(e.target.value) })} />
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Inom/utomhus:
        <select name="isOutdoor" 
        value={formData.isOutdoor ? "true" : "false"} onChange={(e) => setFormData({ ...formData, isOutdoor: e.target.value === "true" })}>
          <option value="false">Inomhus</option>
          <option value="true">Utomhus</option>
        </select>
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Bild URL:
        <input type="text" name="imageUrl" 
        value={formData.imageUrl} onChange={(e) => setFormData({ ...formData, imageUrl: e.target.value })} />
      </label>

      <button type="submit" disabled={loading}>
        {loading ? 'Skapar...' : 'Skapa aktivitet'}
      </button>
    </form>
  );
}