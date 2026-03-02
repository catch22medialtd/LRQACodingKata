# LRQACodingKata
Coding test for LRQA

## Overview
This is a Products API created as part of a coding kata exercise.

## Getting Started

### Prerequisites
- .NET 8 SDK
- Docker Desktop

### Setup Instructions

1. **Configure the database environment**
   
   Navigate to the `docker` folder and rename `.env.example` to `.env`:
```bash
   cd docker
   cp .env.example .env
```
   
   Update the `.env` file with your desired PostgreSQL configuration (or use the defaults).

2. **Start PostgreSQL**
   
   From the `docker` directory, run:
```bash
   docker-compose up -d
```

3. **Configure the API connection string**
   
   Add your connection string to `appsettings.Development.json` or use User Secrets.
   
   **Important:** Ensure the database name and password in your connection string match the values you set in the `.env` file.
   
4. **Run the API**
```bash
   cd ../code
   dotnet run --project src/LRQACodingKata.Api
```

## Assumptions
- Duplicate products are allowed (multiple products can have the same name)
- Product updates replace the entire entity - all fields must be provided in the request, even if unchanged