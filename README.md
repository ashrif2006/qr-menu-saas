# A7-Menu — Backend

A multi-tenant SaaS API for digital QR-code restaurant/cafe menus, built with ASP.NET Core 8 and PostgreSQL. Cafe and restaurant owners manage their menu through an authenticated dashboard API; customers scan a QR code and view the live menu through a public, unauthenticated endpoint — no app install required.

## Overview

- **Type:** Multi-tenant SaaS backend (one API serving many independent cafe accounts)
- **Core idea:** Each cafe (tenant) manages categories and menu items with bilingual content (Arabic/English). A unique QR code per cafe links to a public menu page that updates instantly — no reprinting needed.
- **Status:** Core API complete and functional; in the process of migrating to production (PostgreSQL + cloud hosting).

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 Web API (C#) |
| Database | PostgreSQL (migrated from SQL Server), hosted on Supabase |
| ORM | Entity Framework Core (Npgsql provider) |
| Auth | JWT Bearer tokens, BCrypt password hashing |
| Image storage | Cloudinary (upload, validation, auto-resize) |
| QR generation | QRCoder |
| API docs | Swagger / OpenAPI |
| Deployment target | Docker container |

## Architecture

The project follows a layered architecture to keep business logic out of controllers:

```
Controller → Service (business logic) → DbContext (EF Core) → PostgreSQL
```

- **Controllers**: thin — receive the request, call a service, return the response. No business logic.
- **Services**: own the business rules (e.g. tenant isolation, validation, image lifecycle, token generation). Each has an interface (`IAuthService`, `ICategoryService`, `IMenuItemService`, `IPublicMenuService`, `IImageUploadService`, `IQrCodeService`) registered via dependency injection.
- **DTOs**: separate request/response shapes from EF Core entities; validated with Data Annotations and custom `IValidatableObject` rules.
- **Read-optimized queries**: list/detail endpoints use EF Core `Select` projections (not `Include`) to avoid over-fetching and generate lean SQL.

## Multi-Tenancy

- Every cafe is a `Tenant`, uniquely identified by a slug used in its public menu URL (`/menu/{slug}`).
- The JWT issued at login embeds the user's `TenantId` as a claim.
- `ICurrentUserService` reads the `TenantId` from the authenticated request — never from client input — so every query is scoped to the caller's own tenant. This prevents one cafe from ever reading or modifying another cafe's data.

## Data Model

| Entity | Purpose |
|---|---|
| `Tenant` | A cafe/restaurant account (name, slug, logo, active status) |
| `User` | The cafe owner's login (linked to a Tenant) |
| `MenuCategory` / `CategoryTranslation` | Menu categories with Arabic/English names |
| `MenuItem` / `MenuItemTranslation` | Menu items — name, description, price, image, availability |
| `MenuItemVariant` / `MenuItemVariantTranslation` | Optional size/price variants per item (e.g. Small/Medium/Large), mutually exclusive with a flat item price — enforced via validation |

Translations are stored in separate tables (not fixed `NameAr`/`NameEn` columns) so additional languages can be added later without a schema change.

## Key Features

- **JWT Authentication** — register/login per cafe, with configurable token expiry.
- **Full menu CRUD** — categories and items, including bilingual fields, sort order, availability toggling, and optional size variants with per-size pricing.
- **Image upload pipeline** — images are uploaded to Cloudinary only after the parent menu item exists and belongs to the caller's tenant; old images are deleted automatically when replaced; failed uploads don't block saving the item itself.
- **Public menu endpoint** — `GET /api/public/menu/{slug}`, fully anonymous, returns the complete bilingual menu (categories + available items) in a single optimized query, filtering out inactive categories and unavailable items.
- **QR code generation** — `GET /api/Tenant/qr-code` generates a PNG pointing to the cafe's public menu URL, returned as a downloadable file.
- **Centralized exception handling** — a custom middleware catches unhandled exceptions, logs details server-side, and returns a clean, generic error response to the client.
- **CORS** — restricted to the configured frontend origin, read from configuration rather than hardcoded.
- **Validation** — Data Annotations plus custom `IValidatableObject` rules (e.g. an item must have either a flat price or variants, never both).

## API Surface (summary)

| Area | Endpoints |
|---|---|
| Auth | `POST /api/Auth/register`, `POST /api/Auth/login` |
| Categories | `GET/POST /api/Category`, `GET/PUT/DELETE /api/Category/{id}` |
| Menu Items | `GET/POST /api/MenuItem`, `GET/PUT/DELETE /api/MenuItem/{id}`, `PATCH /api/MenuItem/{id}/toggle-availability`, `POST /api/MenuItem/{id}/upload-image` |
| Public | `GET /api/public/menu/{slug}` (no auth) |
| Tenant | `GET /api/Tenant/qr-code` |

All authenticated endpoints require a `Bearer` JWT and are automatically scoped to the caller's tenant.

## Database Migration

The backend was originally built against SQL Server (LocalDB) during development, then migrated to PostgreSQL to use Supabase's free managed database for production — swapping the EF Core provider (`Npgsql.EntityFrameworkCore.PostgreSQL`) and regenerating migrations. The rest of the codebase (models, services, controllers) required no changes, since EF Core abstracts the database-specific logic.

## What's Next

- Production deployment (containerized via Docker)
- Subscription/billing tier (deferred until there's validated demand)
- Per-table QR codes and branch support
- Basic analytics (scan counts, popular items)

---

*Paired with an Angular 18 frontend (standalone components, signals-based state) covering the owner dashboard and the public customer-facing menu.*
