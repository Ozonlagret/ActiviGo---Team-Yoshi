import { useState } from "react";
import ActivityForm from "./ActivityForm.tsx";
import SessionForm from "./SessionForm.tsx";
import CategoryForm from "./CategoryForm.tsx";
import LocationForm from "./LocationForm.tsx";

export default function CreateEntity() {
  const [entityType, setEntityType] = useState<string>("");

  return (
    <div className="create-entity">
      <div className="create-entity__selector">
        <label htmlFor="entity-type">Välj entitet att skapa:</label>
        <select 
          id="entity-type"
          value={entityType} 
          onChange={(e) => setEntityType(e.target.value)}
        >
          <option value="">-- Välj typ --</option>
          <option value="activity">Aktivitet</option>
          <option value="session">Aktivitetssession</option>
          <option value="category">Kategori</option>
          <option value="location">Plats/Lokal</option>
        </select>
      </div>

      <div className="create-entity__form">
        {entityType === "activity" && <ActivityForm />}
        {entityType === "session" && <SessionForm />}
        {entityType === "category" && <CategoryForm />}
        {entityType === "location" && <LocationForm />}
      </div>
    </div>
  );
}