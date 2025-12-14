Time-Series Data Processor & API
---
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
  
---  
**Test-instruktioner**

- **Bulk-importer**
   - Sökväg till drop-mappen anges i "CollectionPath" i appsettings.json
   - Endast CSV-filer
   - Endast filer exporterade från https://opendata.esett.com/load_profile
      där både MBA och MGA angetts. Detta då nyckel vid db-merge består av (Timestamp, MBA, MGA)

- **Parse enskild fil**
  - Energi-enhet (kWh eller MWh) måste läggas till i json-payload enligt:
   ```
  "unit": "MWh",
  "timeSeries": [
  {
    "timestamp": "2025-11-02T01:00:00",
    "timestampUTC": "2025-11-02T00:00:00Z",
    "mgaCode": "ALS",
    "mgaName": "Alingsås",
    "mba": "SE3",
    "quantity": -2.13466
  }
  ]
}
   ```
