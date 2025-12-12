### TimeSeries
Time-Series Data Processor & API


**Kortfattade lösningsnoteringar**

- **Arkitektur/Allmänt**

  - Jag har löst det med en Clean–Onion hybrid, där jag bryter mot Onion genom att ha alla interface i applikationslagret (alltså även interface som hör till infrastruktur-lagret)
  - CQRS-baserad struktur, med Queries, Commands och Handlers som lager mellan api-lagret och applikationslagret. Använder MediatR för detta.
  - Swagger-support
  - Notera att kod-kommentarer är på svenska, normalt använder jag uteslutande engelska när jag får
  - Det blev en ganska extensiv lösning då jag ville få med så mycket som möjligt av de patterns jag brukar arbeta med.
  
- **Datalager**

   SQLite och EF Core
  3:e partskomponenten Entity Framework Extensions används då det är den främst använda utökningen av EF Core för att hantera stora datamängder och som även erbjuder metoder för bulkade inserts/updates med väldigt få round-trips.

- **Loggning**

  SeriLog med icke-buffrade (av felsökningsbekvänlighet) fil-loggar i utvecklingsmiljö. Stubb för produktionsmiljö skriver loggarna till Seq

- **Validering**

  FluentValidation används dels för grundläggande validering av request-databärare (för att avgöra 400-fall) samt för validering av poster i listor i LoadProfileService, där misslyckade valideringar inte skall förkasta all data.

- **Tester**

  Ett unit-test vardera för en service-metod och en repository-metod samt ett integrations-test. Samtliga med parametriserad test-data och mockning av        Validation, Repository och databas.
  
  
