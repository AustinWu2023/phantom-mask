# Response
> The Current content is an **example template**; please edit it to fit your style and content.
## A. Required Information
### A.1. Requirement Completion Rate

- [v] **List all pharmacies open at a specific time and on a day of the week if requested.**
  - Implemented at `GET /pharmacies/open`

- [v] **List all masks sold by a given pharmacy, sorted by mask name or price.**
  - Implemented at `GET /pharmacies/masks`

- [v] **List all pharmacies with more or less than x mask products within a price range.**
  - Implemented at `GET /pharmacies/filter`

- [v] **The top x users by total transaction amount of masks within a date range.**
  - Implemented at `GET /users/top`

- [v] **The total number of masks and dollar value of transactions within a date range.**
  - Implemented at `GET /transactions/summary`

- [v] **Search for pharmacies or masks by name, ranked by relevance to the search term.**
  - Implemented at `GET /search`

- [v] **Process a user purchases a mask from a pharmacy, and handle all relevant data changes in an atomic transaction.**
  - Implemented at `POST /purchase`

### A.2. API Document

The API documentation is auto-generated using Swagger and is available at:

```
http://localhost:5000/swagger
```

This Swagger UI provides detailed request/response schemas, query parameter usage, and allows for live testing.

You may also import the OpenAPI JSON file into Postman:
- [Download Swagger/OpenAPI JSON](http://localhost:5000/swagger/v1/swagger.json) *(when container is running)*

### A.3. Import Data Commands

All data seeding is handled via JSON files and automatically executed at startup.

However, to manually import data:

```bash
# Run inside container
$ docker exec -it phantom-mask-api sh
$ dotnet phantom-mask.dll seed pharmacy /app/Data/SeedData/pharmacies.json
$ dotnet phantom-mask.dll seed user /app/Data/SeedData/users.json
```

Alternatively, update `SeedDataImporter` to run standalone.

---
## B. Bonus Information

>  If you completed the bonus requirements, please fill in your task below.
### B.1. Test Coverage Report

I wrote down the 20 unit tests for the APIs I built. Please check the test coverage report at [here](#test-coverage-report).

You can run the test script by using the command below:

```bash
bundle exec rspec spec
```

### B.2. Dockerized
The project is fully containerized using Docker + Docker Compose.

**Repository Files:**
- Dockerfile
- docker-compose.yml

To build and run:

```bash
docker-compose up --build
```

Visit Swagger UI at:
```
http://localhost:5000/swagger
```

MySQL port is mapped to host 3308 to avoid conflicts:
```yaml
ports:
  - "3308:3306"
```

Ensure your `.env` or `appsettings.json` uses:
```json
"Server=mysql;Port=3306;Database=phantom_mask;User=test;Password=Test@123456;"
```

### B.3. Demo Site URL

*(Optional placeholder)*: Available upon request or demo via local container.

---

## C. Other Information

### C.1. ERD

The ERD is available at: [ERD Diagram](#) *(insert image link or PDF)*

### C.2. Technical Document

A separate technical walkthrough is available, including:
- Folder structure (e.g. `Features/Pharmacies`, `Users/Purchase`)
- Layered architecture and service separation
- Domain-driven design principles (optional)
- Docker deployment strategy

[View Technical Document](#) *(insert Notion / hackMD / PDF link)*
