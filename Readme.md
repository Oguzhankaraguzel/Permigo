# Permigo

Permigo is a .NET 9 based leave management system built with a modular, layered architecture. The solution models core HR concepts such as users, roles and leave policies while providing infrastructure for authentication, mailing and database seeding. A web-based MVC UI acts as the main presentation layer.

## Solution Layout

```
Permigo.sln
├── src
│   ├── Core
│   │   ├── Domain           // Entity models and domain events
│   │   └── Application      // Use cases, DTOs, MediatR behaviours
│   ├── Infrastructure
│   │   ├── Infrastructure   // infrastructure services (auth, mail, etc.)
│   │   └── Persistence      // EF Core & migrations
│   ├── Presentation         // ASP.NET Core MVC UI
│   └── SharedKernel         // Cross‑cutting primitives (Result, Error, etc.)
└── test
    └── ArchitectureTest     // Placeholder for future architecture tests
```

Each project can be built independently and is wired together through dependency injection.

## Domain Layer (`src/Core/Domain`)

The domain layer holds the business model:

* **BaseEntity** – common audit fields and navigation to creator/updater/deleter users. IDs are generated with GUID version 7 for sequential performance.
* **User entities** – `AppUser` extends `IdentityUser<Guid>` and raises domain events, `AppRole` models roles, `Gender` and `Roles` enums describe user metadata and predefined roles.
* **Leave entities** – `LeaveType`, `LeaveEntitlement`, and `LeaveRequest` capture leave policies, allocations and requests. Requests can be created, approved or rejected, emitting corresponding domain events.
* **Calendar** – `WorkingDay` records whether a given date is a working day.

Domain events bubble up to the application layer where they can be handled asynchronously.

## Application Layer (`src/Core/Application`)

This layer implements use cases using **MediatR** and provides pipeline behaviours:

* `DependencyInjection.AddApplication` registers MediatR handlers, a request logging behaviour and a validation behaviour that short‑circuits on failures.
* **DTOs** provide lightweight shapes for API responses (e.g., `LeaveTypeDto`, `LeaveRequestDto`).
* **DomainSeeds** centralises constant IDs and seed data for roles, users, leave types and working days.
* **Features** encapsulate commands/queries such as:
  * User: create and list employees.
  * Authentication: `LoginUserCommand` issues JWT tokens via `ITokenProvider`.
  * Leave types: CRUD operations and paged listing.
  * Leave entitlements: assign or adjust allocations and query remaining days.
  * Leave requests: create, approve or reject requests with validation against entitlements and working-day rules.
* Validators built with **FluentValidation** protect commands from invalid input.

## Infrastructure Layer (`src/Infrastructure`)

Split into two assemblies:

### Persistence (`src/Infrastructure/Persistence`)

* `PermigoDbContext` inherits from `IdentityDbContext` and publishes domain events after `SaveChangesAsync`.
* `DependencyInjection.AddPersistence` configures EF Core with PostgreSQL, identity services and health checks.
* `DataSeeder` seeds default roles, users, leave types, entitlements, requests and working days during startup.

### Infrastructure (`src/Infrastructure/Infrastructure`)

* JWT authentication is configured via `AddAuthenticationInternal`, wiring `IUserContext`, `ITokenProvider` and utility services like mailing and text normalisation.
* Concrete services include:
  * **TokenProvider** – builds JWT tokens with roles and standard claims.
  * **UserContext** – exposes the current user ID from the HTTP context.
  * **MailService** – sends SMTP emails and supports attachments.
  * **WorkingDayService** – calculates leave durations based on cached working-day records.
  * **TextUtilityService** – normalises names, validates emails and generates usernames.

## Presentation Layer (`src/Presentation/MVCUI`)

An ASP.NET Core MVC application provides the user interface:

* `Program.cs` composes the application by adding the Application, Infrastructure and Persistence layers, sets up Serilog and health checks, and configures middleware such as session, authentication and request logging.
* Controllers expose endpoints for authentication (`AccountController`), user management (`UsersController`), leave types, entitlements and leave requests. For example, `UsersController.Create` handles user creation and returns validation details if a request fails.
* Razor Views under `Views/` render the pages; `wwwroot/` contains static assets.
* Custom middlewares (`RequestContextLoggingMiddleware`, `AuthorizationHeaderInjectionMiddleware`) are registered via `AddPresentation` and applied in the HTTP pipeline.

## Shared Kernel (`src/SharedKernel`)

Utility abstractions reused across layers:

* `Result` and `Error` types provide a functional error-handling model with helpers for validation and paging.
* Domain primitives such as `Entity` base class and `IDomainEvent` interface.
* Enum `ErrorType` distinguishes failure categories.

## Authentication & Authorization

JWT bearer authentication is enabled. Tokens are generated with configurable issuer, audience and secret and include role claims. Default roles are *SuperAdmin*, *Manager* and *Employee*. Controllers use `[Authorize]` attributes to restrict access (e.g., `UsersController` is limited to managers and super admins).

## Database & Migrations

EF Core uses PostgreSQL and snake_case naming. Migrations live in `Persistence/Migrations`. A SQL dump (`permigo_full.sql`) is included for restoring a pre-seeded database. The `docker-compose.yml` file provisions PostgreSQL and a Seq log server alongside the MVC UI for local development.

## Building and Running

### Without Docker

```bash
dotnet restore
dotnet build
dotnet run --project src/Presentation/MVCUI
```

Configuration values come from `appsettings.json` files and environment variables (e.g., `ConnectionStrings:Database`).

### With Docker

```bash
docker compose up --build
```

This starts the MVC UI on port `8080`, PostgreSQL on `5432` and Seq on `5341`.

## Testing

Architecture tests are scaffolded but not yet implemented. Run the full test suite with:

```bash
dotnet test
```

## Contributing

1. Ensure **.NET 9 SDK** and **Docker** (optional) are installed.
2. Restore packages and run migrations if needed.
3. Follow the established layering: domain logic in `Domain`, use cases in `Application`, infrastructure concerns in `Infrastructure`, and presentation code in `MVCUI`.

---
This README provides a comprehensive overview of the Permigo project structure, features and runtime instructions.
