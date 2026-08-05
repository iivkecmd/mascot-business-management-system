"use client";

import { useParams } from "next/navigation";
import Link from "next/link";
import { useEffect, useState } from "react";

type MascotDetails = {
  id: number;
  name: string;
  description: string;
  imageUrl: string;
  rentalPrice: number;
  salePrice: number | null;
  isAvailableForRent: boolean;
  isAvailableForSale: boolean;
};

const apiUrl = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5004";

export default function MascotDetailsPage() {
  const params = useParams<{ id: string }>();
  const [mascot, setMascot] = useState<MascotDetails | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function loadMascot() {
      try {
        const response = await fetch(`${apiUrl}/api/mascots/${params.id}`);

        if (response.status === 404) {
          setError("Tražena maskota ne postoji.");
          return;
        }

        if (!response.ok) {
          throw new Error("Detalji trenutno nisu dostupni.");
        }

        const data: MascotDetails = await response.json();
        setMascot(data);
      } catch {
        setError("Nismo uspeli da učitamo detalje. Proveri da li je API pokrenut.");
      } finally {
        setIsLoading(false);
      }
    }

    loadMascot();
  }, [params.id]);

  return (
    <main>
      <header className="site-header">
        <Link className="brand" href="/" aria-label="Družbalica početna">
          Družbalica
        </Link>
        <Link className="back-link" href="/">
          ← Nazad na katalog
        </Link>
      </header>

      <section className="details-shell">
        {isLoading && <p className="status-message">Učitavamo detalje…</p>}

        {!isLoading && error && (
          <div className="details-error">
            <p className="eyebrow">Nije pronađeno</p>
            <h1>{error}</h1>
            <Link className="primary-link" href="/">
              Vrati se na katalog
            </Link>
          </div>
        )}

        {!isLoading && !error && mascot && (
          <article className="details-layout">
            <div className="details-visual" aria-hidden="true">
              {mascot.name.charAt(0).toUpperCase()}
            </div>

            <div className="details-content">
              <p className="eyebrow">Maskota #{mascot.id}</p>
              <h1>{mascot.name}</h1>
              <p className="details-description">{mascot.description}</p>

              <dl className="details-facts">
                <div>
                  <dt>Cena iznajmljivanja</dt>
                  <dd>
                    {mascot.rentalPrice.toLocaleString("sr-RS")} RSD
                  </dd>
                </div>
                {mascot.isAvailableForSale && mascot.salePrice !== null && (
                  <div>
                    <dt>Prodajna cena</dt>
                    <dd>{mascot.salePrice.toLocaleString("sr-RS")} RSD</dd>
                  </div>
                )}
                <div>
                  <dt>Dostupnost</dt>
                  <dd>
                    {mascot.isAvailableForRent
                      ? "Dostupna za iznajmljivanje"
                      : "Trenutno nije dostupna"}
                  </dd>
                </div>
              </dl>

              <button
                className="primary-button"
                type="button"
                disabled={!mascot.isAvailableForRent}
              >
                {mascot.isAvailableForRent
                  ? "Pošalji zahtev za iznajmljivanje"
                  : "Maskota nije dostupna"}
              </button>
              <p className="button-note">
                Forma za rezervaciju biće dodata u sledećoj funkcionalnoj celini.
              </p>
            </div>
          </article>
        )}
      </section>
    </main>
  );
}
