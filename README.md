Time-Series Data Processor & API
---
**Kortfattade lösningsnoteringar**
- **Arkitektur**
  
  Jag har länge arbetat mot att följa Clean/Onion i min arkitektur. Mitt inlämnade prov gjorde jag därför lite större än kanske var avsikten. Detta för att jag vill belysa vad jag ser som essentiellt i ett stort system.
  Vad principerna är i Clean/Onion känner ni säkert till så lämnar diskussion kring detta till mötet där jag belyser hur jag följt detta. Även CQRS följer jag så strikt jag kan.
  Nedan några korta tekniska punkter och längre ned lite kommentarer/instruktioner för när ni testar.

  Arkitekturen i detta projekt är Clean med teknik-orienterad mappstruktur, men i ett verkligt scenario hade jag valt Vertical-slice med Feature-baserad struktur då det helt grundar sig i Use-cases men med Clean-arkitektur bibehållen inom varje feature. 

- **Datalager**

  Använder SQLite och EF Core samt
  3:e partskomponenten Entity Framework Extensions för att uppnå bulkade inserts/updates med hög prestanda

- **Validering**

  FluentValidation används dels för grundläggande validering av indata samt för validering av data vid importer

- **Loggning**

  SeriLog med fil-loggar i utvecklingsmiljö och stubb för produktionsmiljö som skriver loggarna till Seq

- **Tester**

  Ett unit-test vardera mot service- resp. repository-lagren samt ett integrations-test
  
---  
**Test-instruktioner**
- **NOTERA: Koden har kommentarer där jag anser av nytta. Men läs speciellt kommentarerna i /Infrastructure/Repositories/TimeSeriesRepository.cs , /Application/Imports/TimeSeriesProcessor.cs samt /Domain/Entities/TimeSeries.cs
  gällande slutsatser jag fick dra om databas-designen, serie-id och upsert-förfarandet.**

  - Lösningen är i Net 8, men jag kodade i nya VS2026. Solution-filen är i formatet *.slnx som jag tror fungerar även i VS2022. 
  - Om ni vill rensa databasen mellan körningar så hamnar db-filerna i samma mapp som projekt-filerna. Annars DB Browser (SQLite) 
  - Jag lade till SwaggerUI att testa API:t med.
  
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
  ]}
   ```
- **Hämta datapunkter**
  - start/end
    Dessa kan anges på formerna "hh", "hh:mm" samt "hh:mm:ss"
- **Beräkna KPI:er**
  - periodStart/periodEnd
    Dessa  anges på formen yyyy-mm-dd
    
  Lade till dessa i all hast imorse, så om matematiken inte stämmer väljer jag att skylla på det ;)
   ```
   {
  "kpis": [
    {
      "description": "Total energi (MWh)",
      "value": 4122.214573000002
    },
    {
      "description": "Snittbelastning (MW)",
      "value": 13.212226195512827
    },
    {
      "description": "Maxbelastning (MW)",
      "value": 17.772036
    },
    {
      "description": "Tidpunkt för maxbelastning = 2025-12-12 02:00:00",
      "value": 0
    },
    {
      "description": "Minbelastning (MW)",
      "value": 8.182108
    },
    {
      "description": "Tidpunkt för minbelastning = 2025-12-12 18:15:00",
      "value": 0
    },
    {
      "description": "Belastningsgrad (Snitt/Max)",
      "value": 0.7434278321016695
    },
    {
      "description": "Standardavvikelse",
      "value": 0.2776158705412206
    }
  }
   ```
