export default function CategoryForm() {
  return (
    <form className="entity-form" style={{ display: "flex", flexDirection: "column", gap: "1rem", maxWidth: "500px" }}>
      <h3>Skapa ny kategori</h3>
      
      <label style={{ display: "flex", flexDirection: "column" }}>
        Namn:
        <input type="text" name="name" required />
      </label>

      <button type="submit">Skapa kategori</button>
    </form>
  );
}
