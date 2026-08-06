# Družbalica — kontekst projekta

## 1. Svrha dokumenta

Ovaj dokument je glavni kontekst za rad na projektu. Pre svake veće izmene prvo ga pročitati, zatim pregledati postojeći kod i nastaviti od trenutnog stanja.

Ne praviti celu aplikaciju odjednom. Projekat razvijati kroz male funkcionalne celine koje mogu da se pokrenu, provere i objasne.

## 2. Ideja projekta

**Družbalica** je web aplikacija za:

- iznajmljivanje maskota za događaje;
- prodaju maskota;
- interno upravljanje zahtevima, porudžbinama i poslovnim podacima.

Aplikacija je portfolio projekat Ive Samopjan. Treba da pokaže praktično znanje iz razvoja full-stack aplikacija, ali kod mora ostati razumljiv kako bi Iva mogla samostalno da ga predstavi i objasni na razgovoru za posao.

## 3. Korisnici aplikacije

### Klijent

Klijent koristi javni deo aplikacije i **ne mora da ima nalog niti da se prijavi**.

Može da:

- pregleda ponudu maskota;
- otvori detalje maskote;
- izabere kupovinu ili iznajmljivanje;
- pošalje zahtev sa svojim kontakt podacima;
- dobije broj zahteva ili porudžbine;
- kasnije proveri status zahteva.

### Zaposleni

Zaposleni se prijavljuje u interni deo i obrađuje rezervacije i porudžbine.

### Administrator

Administrator ima sve mogućnosti zaposlenog, a kasnije i upravljanje korisnicima, finansijama, izveštajima i podešavanjima.

U prvom MVP-u nije neophodno odmah implementirati potpuno razdvojene dozvole administratora i zaposlenog. Najpre je dovoljno zaštititi interni deo aplikacije.

## 4. Dva dela aplikacije

### Javni deo

- početna stranica;
- katalog maskota;
- detalji maskote;
- forma za zahtev za iznajmljivanje;
- kupovina i porudžbina;
- potvrda uspešnog slanja;
- praćenje statusa bez naloga.

### Interni deo

- prijava;
- kontrolna tabla;
- pregled rezervacija;
- detalji rezervacije;
- potvrđivanje, odbijanje i promena statusa;
- pregled i obrada prodajnih porudžbina.

## 5. Tehnologije

Planirani tehnološki stek:

- **Backend:** C# i ASP.NET Core Web API;
- **Frontend:** React;
- **Baza:** SQL Server uz Entity Framework Core;
- **Autentifikacija:** JWT, kada dođemo do prijave zaposlenih;
- **Dokumentacija API-ja:** Swagger/OpenAPI;
- **Kontrola verzija:** Git i GitHub;
- **Opcionalno kasnije:** Docker.

Ne uvoditi novu biblioteku ili složenu arhitekturu bez jasne potrebe.

## 6. Prvi MVP

Prva stvarno upotrebljiva verzija treba da podrži ovaj tok:

```text
Klijent pregleda maskote
→ otvara detalje maskote
→ šalje zahtev za iznajmljivanje bez prijave
→ zahtev se čuva u bazi
→ interni korisnik vidi zahtev
→ potvrđuje ga, odbija ili menja status
```

Prodavnica se dodaje tek kada tok iznajmljivanja radi od početka do kraja.

### Modeli dovoljni za prvi deo

Za početak koristiti samo:

- `Mascot`;
- `Customer`;
- `Reservation`.

Kada se uvede zaštićen interni deo, dodati `User`.

Kada počne izrada prodavnice, dodati:

- `SalesOrder`;
- `SalesOrderItem`.

Za sada nije potrebno praviti posebne tabele `MascotRentalInfo`, `MascotSaleInfo`, `MascotSize`, `Payment`, `Supplier`, `ImportBatch`, `Expense`, `Notification` i `ActivityLog`.

## 7. Početni podaci modela

Ovo je smernica, a ne nepromenljiva konačna šema.

### Mascot

- `Id`;
- `Name`;
- `Description`;
- `ImageUrl`;
- `RentalPrice`;
- `SalePrice` (može biti prazno ako se maskota ne prodaje);
- `IsAvailableForRent`;
- `IsAvailableForSale`;
- `StockQuantity`.

### Customer

- `Id`;
- `FirstName`;
- `LastName`;
- `Phone`;
- `Email`;
- opciono adresa i napomena kada budu potrebne.

Telefon i email ne moraju biti strogo jedinstveni u bazi. Pre dodavanja klijenta aplikacija kasnije može upozoriti na mogućeg duplikata.

### Reservation

- `Id`;
- javni broj zahteva;
- `MascotId`;
- `CustomerId`;
- `StartAt`;
- `EndAt`;
- lokacija događaja;
- napomena;
- status;
- datum kreiranja.

Koristiti `StartAt` i `EndAt`, a ne odvojene kolone za datum i vreme.

## 8. Statusi

Za prvi MVP statusi rezervacije mogu biti:

- `Pending` — novi zahtev;
- `Confirmed` — potvrđena rezervacija;
- `Rejected` — odbijena;
- `Cancelled` — otkazana;
- `Completed` — završena.

Ne dodavati dodatne statuse dok za njih ne postoji ekran i stvarna poslovna potreba.

## 9. Najvažnije poslovno pravilo

Ista maskota ne sme imati dve potvrđene rezervacije čiji se termini preklapaju.

Preklapanje postoji kada važi:

```text
newStart < existingEnd
AND
newEnd > existingStart
```

Klijenti mogu poslati više zahteva za isti termin, ali pri promeni statusa u `Confirmed` sistem mora ponovo proveriti dostupnost. Naprednu transakcijsku i concurrency zaštitu dodati kada osnovni tok već radi.

## 10. Redosled razvoja

1. Kreirati osnovni backend i frontend i povezati bazu.
2. Napraviti `Mascot` model i prikazati katalog maskota.
3. Napraviti stranicu sa detaljima maskote.
4. Dodati `Customer` i `Reservation` modele.
5. Napraviti javnu formu za iznajmljivanje i sačuvati zahtev u bazi.
6. Napraviti jednostavan interni pregled rezervacija i promenu statusa.
7. Dodati prijavu i zaštititi interni deo.
8. Dodati prodavnicu, korpu i prodajne porudžbine.
9. Tek zatim dodavati kalendar, uplate, zalihe, zaposlene i izveštaje.

Svaki korak mora biti pokrenut i proveren pre prelaska na sledeći.

## 11. Funkcionalnosti za kasnije

Kada osnovna aplikacija bude stabilna, mogu se dodati:

- praćenje zahteva preko broja zahteva i emaila ili telefona;
- kalendar rezervacija;
- evidencija avansa i drugih uplata;
- više veličina i zalihe po veličini;
- dobavljači i nabavne ture;
- troškovi i finansijski izveštaji;
- obaveštenja;
- audit log;
- izvoz u Excel ili PDF;
- email potvrde;
- napredne dozvole za zaposlene i administratora;
- Docker i objavljivanje aplikacije.

Ovo nisu zahtevi za početnu verziju.

## 12. Pravila rada za Codex/AI

- Pre izmene pregledati postojeću strukturu projekta i ovaj dokument.
- Ne pretpostavljati da treba implementirati sve planirane funkcionalnosti.
- Raditi samo trenutno dogovorenu celinu.
- Ne menjati postojeću arhitekturu bez objašnjenja i jasnog razloga.
- Ne uvoditi nepotrebne obrasce, apstrakcije i pakete.
- Kod pisati jasno, početnički razumljivo i dovoljno kvalitetno za portfolio.
- Važnu poslovnu logiku držati na backendu, ne samo na frontendu.
- Dodati validaciju unosa i razumljive poruke o grešci.
- Posle svake izmene pokrenuti relevantan build i testove.
- Ne brisati ili prepisivati korisnički kod koji nije povezan sa zadatkom.
- Na kraju svakog koraka objasniti Ivi šta je napravljeno, gde se nalazi i kako može da proveri da radi.

## 13. Trenutno stanje

Završene su katalog i detalji maskota od baze do React frontenda, kao i backend deo javnog toka za slanje zahteva za iznajmljivanje.

Na backendu je završeno:

- kreiran je ASP.NET Core Web API projekat na .NET 10;
- povezan je lokalni SQL Server Express preko Entity Framework Core-a;
- dodat je `ApplicationDbContext` i razvojni connection string;
- napravljen je `Mascot` model sa osnovnom validacijom;
- kreirana je i primenjena migracija `InitialCreate`;
- napravljeni su GET, POST, PUT i DELETE endpointi za maskote;
- dodati su request DTO-i za kreiranje i izmenu maskote;
- dodati su posebni response DTO-i za kartice kataloga i detalje maskote;
- uklonjen je početni `WeatherForecast` primer;
- relevantni build i API zahtevi uspešno su provereni.

Za tok iznajmljivanja na backendu je završeno:

- dodati su modeli `Customer` i `Reservation`;
- dodat je `ReservationStatus` sa statusima `Pending`, `Confirmed`, `Rejected`, `Cancelled` i `Completed`;
- rezervacija je povezana sa klijentom i maskotom;
- dodat je jedinstveni javni broj zahteva;
- Entity Framework konfiguracija je proširena za nove modele i njihove veze;
- kreirana je i primenjena migracija `AddCustomerAndReservation`;
- dodati su `CreateReservationRequest` i `CreateReservationResponse` DTO-i;
- napravljen je javni `POST /api/reservations` endpoint;
- endpoint proverava da maskota postoji i da je dostupna za iznajmljivanje;
- endpoint proverava da je termin u budućnosti i da je kraj posle početka;
- novi zahtev dobija status `Pending`, a klijentu se vraća javni broj zahteva;
- status rezervacije se u JSON odgovoru prikazuje kao čitljiv tekst;
- endpoint je ručno testiran stvarnim zahtevom i uspešno je sačuvao klijenta i rezervaciju;
- relevantni backend build je uspešno završen bez grešaka i upozorenja.

Na frontendu je završeno:

- kreiran je osnovni React/Next.js frontend;
- frontend je povezan sa postojećim API-jem;
- početna stranica prikazuje katalog maskota iz baze;
- dodata je dinamička stranica detalja na ruti `/mascots/[id]`;
- obrađena su loading, empty, error i not-found stanja;
- prikazano je da li je maskota dostupna za iznajmljivanje ili prodaju;
- API adresa se čita iz `NEXT_PUBLIC_API_URL`, uz lokalnu rezervnu vrednost;
- frontend i backend povezani su kontrolisanom CORS politikom;
- relevantni frontend build i ručne provere uspešno su završeni.

Backend se razvija kao jednostavan modularni monolit sa feature-based organizacijom. Kod jedne funkcionalnosti drži se zajedno unutar `Features` foldera, dok zajednički EF Core kontekst i migracije ostaju izdvojeni. Ne uvoditi mikroservise, MediatR, generičke repozitorijume ili druge apstrakcije bez stvarne potrebe.

Detaljan veliki ER model i dalje postoji samo kao mogući pravac za kasnije i ne treba ga odmah u potpunosti implementirati.

Sledeći praktični cilj je završetak javnog toka iznajmljivanja na frontendu:

1. napraviti React formu za iznajmljivanje na stranici detalja maskote;
2. automatski proslediti `MascotId` iz otvorene stranice;
3. povezati formu sa `POST /api/reservations` endpointom;
4. prikazati validacione i serverske greške korisniku;
5. nakon uspešnog slanja prikazati javni broj zahteva i status `Pending`;
6. pokrenuti frontend build i ručno proveriti ceo tok od stranice maskote do baze.

Posle toga nastaviti sa jednostavnim internim pregledom rezervacija i promenom njihovog statusa. Pri promeni statusa u `Confirmed` obavezno proveriti preklapanje termina.

## 14. Definicija uspeha

Projekat je uspešan ako:

- radi od frontenda do baze;
- ima jasan i realan tok iznajmljivanja i kasnije prodaje;
- može lako da se pokrene i demonstrira;
- Iva razume ključne modele, API pozive i poslovnu logiku;
- kod i README jasno pokazuju njeno znanje poslodavcu.
