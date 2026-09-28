# SSWF FoodApp

SSWF FoodApp is een webapplicatie waarmee studenten overgebleven voedselpakketten van kantines kunnen bekijken en reserveren. Kantinemedewerkers kunnen pakketten aanmaken, beheren en voorzien van producten. De applicatie is ontwikkeld als onderdeel van het vak **ServerSide Web Development Individueel (SSWPI)**.

## Doel

Het doel van SSWF FoodApp is om voedselverspilling tegen te gaan door overgebleven maaltijden en producten uit kantines aan te bieden aan studenten. Studenten kunnen eenvoudig een pakket reserveren en op een gekozen moment ophalen.

## Functionaliteiten

- Pakketten bekijken en reserveren als student
- Pakketten aanmaken, wijzigen en verwijderen als kantinemedewerker
- Producten aan pakketten koppelen
- Filteren op locatie en type maaltijd
- Automatische 18+-controle bij alcoholhoudende producten
- Voorkomen van dubbele reserveringen
- Beheren van no-shows en reserveringsbeperkingen
- Verschillende rollen voor studenten en kantinemedewerkers

## Architectuur en kwaliteit

Dit project is nadrukkelijk meer dan een eenvoudige CRUD-applicatie. De nadruk lag op architectuur, business rules en softwarekwaliteit. Enkele voorbeelden:

- **Clean/Onion Architecture** voor een heldere scheiding tussen domein, applicatie en infrastructuur
- **RESTful web API** met ASP.NET Core MVC
- **Business rules**, waaronder:
  - Maximaal één reservering per student per dag
  - Maximaal twee dagen vooruit plannen
  - Leeftijdscontrole op basis van de ophaaldatum
  - Voorkomen van dubbele reserveringen (concurrency)
- **UX-design** met oog voor gebruiksvriendelijkheid
- **Automatische unit tests**
- **Deployment via een software-ontwikkelstraat**

## Technologieën

- C#
- ASP.NET Core MVC
- RESTful Web API
- Clean/Onion Architecture
- Unit testing
- Software-ontwikkelstraat (CI/CD)
