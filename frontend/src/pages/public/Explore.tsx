// Startsidan: hämtar aktiviteter, håller sök- och filter-state, visar grid + modal.

import { useMemo, useState } from "react";
import { useQuery } from "@tanstack/react-query";

// UI-komponenter
import Header from "../../components/Header";
import SearchBar from "../../components/SearchBar";
import ActivityCard from "../../components/ActivityCard";
import FiltersModal from "../../components/FiltersModal";

// API-funktioner (värden)
import { listActivities, listCategories } from "../../api/activities";

// Typer
import type { ListParams } from "../../api/activities";

import "../../styles/explore.css";

export default function Explore() {
  // Söktext
  const [q, setQ] = useState("");
  // Styr visning av filtermodal
  const [showFilters, setShowFilters] = useState(false);
  // Aktiva filter (kategori, datum, mm)
  const [filters, setFilters] = useState<Partial<ListParams>>({});

  // Memoisera params-objekt så react-querys key inte skapas om i onödan
  const queryParams = useMemo(
    () => ({ q: q || undefined, ...filters }),
    [q, filters]
  );

  // Hämtar aktiviteter varje gång queryParams ändras.
  // Första render: {} -> hämtar alla aktiviteter (utan filter).
  const { data: activities, isLoading } = useQuery({
    queryKey: ["activities", queryParams],
    queryFn: () => listActivities(queryParams),
  });

  // Hämtar kategorier för filterlistan (cachas av react-query)
  const { data: categories } = useQuery({
    queryKey: ["categories"],
    queryFn: () => listCategories(),
  });

  return (
    <>
      <Header />

      {/* Sök-raden under headern */}
      <SearchBar
        value={q}
        onChange={setQ}
        onOpenFilters={() => setShowFilters(true)}
      />

      {/* Huvudinnehåll: laddning, tomt-state, kort-grid */}
      <main className="explore">
        {isLoading && <div className="explore__loading">Laddar…</div>}

        {!isLoading && (!activities || activities.length === 0) && (
          <div className="explore__empty">Inga aktiviteter matchade din sökning.</div>
        )}

        <div className="explore__grid">
          {/* Rendera kort för varje aktivitet */}
          {activities?.map((a) => <ActivityCard key={a.id} a={a} />)}
        </div>
      </main>

      {/* Filtermodalen sätter patchar i filters-state när användaren ändrar. */}
      <FiltersModal
        open={showFilters}
        onClose={() => setShowFilters(false)}
        values={{
          categoryId: filters.categoryId,
          dateFrom: filters.dateFrom as string | undefined,
          dateTo: filters.dateTo as string | undefined,
          isOutdoor: filters.isOutdoor,
          onlyWithFreeSlots: filters.onlyWithFreeSlots,
        }}
        onChange={(patch) => setFilters((prev) => ({ ...prev, ...patch }))}
        categories={categories ?? []}
      />
    </>
  );
}