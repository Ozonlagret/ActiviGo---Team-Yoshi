export default function LocationForm() {
  return (
    <form className="entity-form" style={{ display: "flex", flexDirection: "column", gap: "1rem", maxWidth: "500px" }}>
      <h3>Skapa ny plats</h3>
      
      <label style={{ display: "flex", flexDirection: "column" }}>
        Namn:
        <input type="text" name="name" required />
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Adress:
        <input type="text" name="address" />
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Kapacitet:
        <input type="number" name="capacity" />
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Inom/utomhus:
        <select name="isOutdoor">
          <option value="false">Inomhus</option>
          <option value="true">Utomhus</option>
        </select>
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Latitude:
        <input type="number" step="0.000001" name="latitude" />
      </label>

      <label style={{ display: "flex", flexDirection: "column" }}>
        Longitude:
        <input type="number" step="0.000001" name="longitude" />
      </label>

      <button type="submit">Skapa plats</button>
    </form>
  );
}
