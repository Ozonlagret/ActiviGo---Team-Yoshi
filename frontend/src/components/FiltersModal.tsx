import { useEffect } from "react";
import "../styles/filters-modal.css";

type Props = {
  open: boolean;
  onClose: () => void;
  values: {
    categoryId?: number;
    dateFrom?: string;
    dateTo?: string;
    isOutdoor?: boolean;
    onlyWithFreeSlots?: boolean;
  };
  onChange: (patch: Partial<Props["values"]>) => void;
  categories: { id: number; name: string }[];
};

export default function FiltersModal({ open, onClose, values, onChange, categories }: Props) {
  useEffect(() => {
    const onEsc = (e: KeyboardEvent) => e.key === "Escape" && onClose();
    if (open) window.addEventListener("keydown", onEsc);
    return () => window.removeEventListener("keydown", onEsc);
  }, [open, onClose]);

  if (!open) return null;

  return (
    <div className="filters-backdrop" onClick={onClose}>
      <div className="filters-modal" onClick={(e) => e.stopPropagation()}>
        <h3 className="filters-title">Filtrera</h3>

        <label className="filters-label">Kategori</label>
        <select
          value={values.categoryId ?? ""}
          onChange={(e) => onChange({ categoryId: e.target.value ? Number(e.target.value) : undefined })}
          className="filters-input"
        >
          <option value="">Alla</option>
          {categories.map((c) => (
            <option key={c.id} value={c.id}>{c.name}</option>
          ))}
        </select>

        <div className="filters-dates">
          <div>
            <label className="filters-label">Från datum</label>
            <input
              type="date"
              value={values.dateFrom ?? ""}
              onChange={(e) => onChange({ dateFrom: e.target.value || undefined })}
              className="filters-input"
            />
          </div>
          <div>
            <label className="filters-label">Till datum</label>
            <input
              type="date"
              value={values.dateTo ?? ""}
              onChange={(e) => onChange({ dateTo: e.target.value || undefined })}
              className="filters-input"
            />
          </div>
        </div>

        <div className="filters-check">
          <label>
            <input
              type="checkbox"
              checked={!!values.isOutdoor}
              onChange={(e) => onChange({ isOutdoor: e.target.checked ? true : undefined })}
            />{" "}
            Utomhus
          </label>
        </div>
        <div className="filters-check">
          <label>
            <input
              type="checkbox"
              checked={!!values.onlyWithFreeSlots}
              onChange={(e) => onChange({ onlyWithFreeSlots: e.target.checked ? true : undefined })}
            />{" "}
            Visa endast lediga
          </label>
        </div>

        <div className="filters-actions">
          <button
            onClick={() => {
              onChange({
                categoryId: undefined,
                dateFrom: undefined,
                dateTo: undefined,
                isOutdoor: undefined,
                onlyWithFreeSlots: undefined,
              });
              onClose();
            }}
            className="btn-ghost"
          >
            Rensa
          </button>
          <button onClick={onClose} className="btn-primary">Använd filter</button>
        </div>
      </div>
    </div>
  );
}