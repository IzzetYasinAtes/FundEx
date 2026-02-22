# FundEx

**Status: In Development** · A public API exposing Turkish fund data sourced from [TEFAS](https://tefas.takasbank.com.tr/) (Takasbank Electronic Fund Trading Platform).

---

## Overview

FundEx aggregates fund metadata and daily performance data from TEFAS via its public XHR endpoints, normalizes and persists it in MSSQL, and exposes a clean REST API. The project fetches data over the network directly from TEFAS services—no manual imports or third-party dumps. This API will be made publicly available once development is complete.

---

## Architecture

| Layer | Description |
|-------|-------------|
| **Domain** | Entities, value objects, business rules, enums. No external dependencies. |
| **Application** | CQRS (Commands/Queries), MediatR handlers, DTOs, validation, mapping, pipeline behaviors. |
| **Persistence** | EF Core 10 Code-First, MSSQL, repositories, unit of work, migrations, seed data. |
| **Infrastructure** | TEFAS HTTP client, background sync services, caching, Serilog, JWT. |
| **WebApi** | Controllers, global exception handling, Swagger. |

**Principles:** DDD, Clean Architecture, SOLID, DRY, KISS. **Mapping:** Mapster. **Validation:** FluentValidation. **Pipelines:** Validation, Logging, Caching, Transaction.

---

## Technical Stack

- **.NET 10** · C# 14
- **Entity Framework Core 10** · MSSQL
- **MediatR** · CQRS
- **Mapster** · Object mapping
- **FluentValidation** · Request validation
- **Serilog** · Structured logging
- **Polly** · HTTP resilience (retry, circuit breaker)
- **JWT** · Authentication

---

## Data Source & Sync Strategy

Data is retrieved from TEFAS APIs over HTTPS. Endpoints used:

| API | Purpose |
|-----|---------|
| `fonKurucuGetir` | Fund founders (portfolio management firms) |
| `fonTurGetir` / `fonDetayGetir` | Fund categories (by type) |
| `fonUnvanGetir` / `fonGrupGetir` | Fund titles (groupings) |
| `fonGnlBlgSiraliGetir` | Daily general fund info (price, NAV, investor count) |
| `dagilimSiraliGetirT` | Daily portfolio distribution (asset allocation) |

**Persistence:** Upsert (insert or update on conflict). Existing records for the same fund and date are overwritten.

---

## Startup Flow

```
WebApi Startup
    │
    ├── Migrate database (EF Core)
    ├── Seed FundType (5 fixed records: YAT, EMK, BYF, GYF, GSYF)
    └── Trigger Initial Sync (DataSyncBackgroundService)
            │
            └── DataSyncOrchestrator.RunFullSyncAsync()
                    │
                    ├── Sync Metadata (per fund type)
                    │   ├── fonKurucuGetir  → Upsert Founders
                    │   ├── fonTurGetir / fonDetayGetir → Upsert FundCategories
                    │   └── fonUnvanGetir / fonGrupGetir → Upsert FundTitles
                    │
                    └── Sync Historical Daily Data (monthly loop, 5 years back)
                        │
                        └── For each (month, fund type):
                            ├── fonGnlBlgSiraliGetir  ──┐
                            ├── dagilimSiraliGetirT  ──┼── Upsert Fund + FundDailyData
                            └─────────────────────────┘

Nightly 00:00 (scheduled)
    └── Same flow, but only last day’s data
```

---

## Fund Types (TEFAS)

| Code | Description |
|------|-------------|
| YAT | Securities Investment Funds |
| EMK | Pension Funds |
| BYF | Exchange Traded Funds |
| GYF | Real Estate Investment Funds |
| GSYF | Venture Capital Investment Funds |

---

## Project Structure

```
src/
├── FundEx.Domain/        # Entities, enums, business rules
├── FundEx.Application/   # CQRS, pipelines, interfaces
├── FundEx.Persistence/   # DbContext, repositories, migrations
├── FundEx.Infrastructure/# TEFAS client, sync services, cache, auth
└── FundEx.WebApi/        # API surface

tests/
├── FundEx.Domain.Tests/
├── FundEx.Application.Tests/
└── FundEx.Infrastructure.Tests/
```

---

**Note:** This project is still under active development. Features, endpoints, and behavior may change.
