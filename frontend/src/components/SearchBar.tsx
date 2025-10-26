import "../styles/searchbar.css";

type Props = {
  value: string;
  onChange: (v: string) => void;
  onOpenFilters: () => void;
};

export default function SearchBar({ value, onChange, onOpenFilters }: Props) {
  return (
    <div className="ag-searchbar">
      <div className="ag-searchbar__inner">
        <button className="pill-btn" onClick={() => onChange("")}>Alla aktiviteter</button>

        <input
          className="search-input"
          placeholder="Sök på namn, plats, kategori …"
          value={value}
          onChange={(e) => onChange(e.target.value)}
          onKeyDown={(e) => e.key === "Enter" && (e.currentTarget as HTMLInputElement).blur()}
        />

        <button className="filter-btn" onClick={onOpenFilters}>
          Filtrera ▾
        </button>
      </div>
    </div>
  );
}