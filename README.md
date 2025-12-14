### TimeSeries
Time-Series Data Processor & API


**Kortfattade lösningsnoteringar**

- **Datalager**

   Använder SQLite och EF Core samt
  3:e partskomponenten Entity Framework Extensions för att uppnå bulkade inserts/updates med hög prestanda

- **Validering**

  FluentValidation används dels för grundläggande validering av request-databärare samt för validering av poster i listor i LoadProfileService

- **Loggning**

  SeriLog med fil-loggar i utvecklingsmiljö och stubb för produktionsmiljö som skriver loggarna till Seq

- **Tester**

  Ett unit-test vardera mot service- resp. repository-lagren samt ett integrations-test
  
  
