# Družbalica frontend

React i TypeScript frontend za javni deo aplikacije Družbalica.

## Pokretanje

Backend treba da radi na `http://localhost:5004`, a zatim pokrenuti frontend:

```powershell
npm.cmd install
npm.cmd run dev
```

Frontend je dostupan na `http://localhost:3000`.

## Provera builda

```powershell
npm.cmd run build
```

## Trenutne stranice

- `/` — katalog maskota;
- `/mascots/{id}` — detalji izabrane maskote.

Podaci se učitavaju iz ASP.NET Core API-ja. Lokalna API adresa može se promeniti pomoću `NEXT_PUBLIC_API_URL` promenljive okruženja.
