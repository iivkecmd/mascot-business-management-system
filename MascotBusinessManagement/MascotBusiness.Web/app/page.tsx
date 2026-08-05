"use client";

import { useEffect, useState } from "react";
import Link from "next/link";

type MascotCatalogItem = {
  id: number;
  name: string;
  imageUrl: string;
  rentalPrice: number;
  salePrice: number | null;
  isAvailableForRent: boolean;
  isAvailableForSale: boolean;
};

const apiUrl = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5004";

export default function Home() {
  const [mascots, setMascots] = useState<MascotCatalogItem[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function loadMascots() {
      try {
        const response = await fetch(`${apiUrl}/api/mascots`);

        if (!response.ok) {
          throw new Error("Katalog trenutno nije dostupan.");
        }

        const data: MascotCatalogItem[] = await response.json();
        setMascots(data);
      } catch {
        setError("Nismo uspeli da učitamo maskote. Proveri da li je API pokrenut.");
      } finally {
        setIsLoading(false);
      }
    }

    loadMascots();
  }, []);

  return (
    <main>
      <header className="site-header">
        <Link className="brand" href="/" aria-label="Družbalica početna">
          Družbalica
        </Link>
        <span className="header-note">Maskote za događaje koji se pamte</span>
      </header>

      <section className="hero">
        <p className="eyebrow">Katalog maskota</p>
        <h1>Izaberi junaka za svoju sledeću proslavu.</h1>
        <p className="hero-copy">
          Pregledaj ponudu maskota za iznajmljivanje i pronađi onu koja će
          obradovati tvoje goste.
        </p>
      </section>

      <section className="catalog" aria-labelledby="catalog-title">
        <div className="catalog-heading">
          <h2 id="catalog-title">Naše maskote</h2>
          {!isLoading && !error && (
            <span>{mascots.length} u ponudi</span>
          )}
        </div>

        {isLoading && <p className="status-message">Učitavamo katalog…</p>}
        {error && <p className="status-message error-message">{error}</p>}
        {!isLoading && !error && mascots.length === 0 && (
          <p className="status-message">Trenutno nema maskota u ponudi.</p>
        )}

        <div className="mascot-grid">
          {mascots.map((mascot) => (
            <article className="mascot-card" key={mascot.id}>
              <div className="mascot-visual" aria-hidden="true">
                {mascot.name.charAt(0).toUpperCase()}
              </div>
              <div className="mascot-content">
                <div className="availability-row">
                  <span
                    className={
                      mascot.isAvailableForRent
                        ? "availability available"
                        : "availability unavailable"
                    }
                  >
                    {mascot.isAvailableForRent
                      ? "Dostupna za najam"
                      : "Trenutno nedostupna"}
                  </span>
                </div>
                <h3>{mascot.name}</h3>
                <p className="price">
                  {mascot.rentalPrice.toLocaleString("sr-RS")} RSD
                  <span> / događaj</span>
                </p>
                <Link className="details-link" href={`/mascots/${mascot.id}`}>
                  Pogledaj detalje <span aria-hidden="true">→</span>
                </Link>
              </div>
            </article>
          ))}
        </div>
      </section>
    </main>
  );
}
