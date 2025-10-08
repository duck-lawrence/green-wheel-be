# Green Wheel

## [Documentations](https://docs.google.com/document/d/1YYFCutl6D6C-bexIc14sZuamJwadSp8Y/edit?usp=sharing&ouid=102744078799902508261&rtpof=true&sd=true)
Green Wheel is a self-drive vehicle rental platform that connects a customer-facing booking experience with an internal staff workspace. The monorepo houses a Next.js 15 front end and an ASP.NET Core 8 back end, built around layered architecture, rich domain modelling, and integrations for identity verification, payments, and cloud media.

## Repository Overview

- `backend/` - ASP.NET Core 8 Web API (API / Application / Domain / Infrastructure layers)
- `frontend/` - Next.js 15 (App Router) customer & staff portals
- `init-db/` - SQL Server schema & seed scripts for local development

## Project Highlights

- **Dual-portals:** Contemporary customer site plus an internal staff console sharing a common design language.
- **Automated identity checks:** Driver licence and citizen ID extraction powered by Google Gemini with Cloudinary asset storage.
- **End-to-end rental lifecycle:** Vehicle catalogue, rental contracts, checklists, invoicing, payments, and customer support.
- **Payment-ready:** Integrated MoMo gateway, invoice flows, and Redis-backed caching for short-lived artefacts.
- **Secure authentication:** JWT access/refresh tokens, OTP flows, Google OAuth sign-in, and global error handling middleware.
- **Localization-first:** English and Vietnamese translations via `i18next`, with middleware-guarded routes and persistent stores.



## 💻 Tech Stack

#### 🖥️ Frontend

![Next.js](https://img.shields.io/badge/Next.js-000000?style=for-the-badge&logo=next.js&logoColor=white)
![React](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
![npm](https://img.shields.io/badge/npm-9%2B-CB3837?style=for-the-badge&logo=npm&logoColor=white)
![ESLint 9](https://img.shields.io/badge/ESLint-9-4B32C3?style=for-the-badge&logo=eslint&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-06B6D4?style=for-the-badge&logo=tailwindcss&logoColor=white)
![Styled Components](https://img.shields.io/badge/styled--components-DB7093?style=for-the-badge&logo=styledcomponents&logoColor=white)
![Prettier Config](https://img.shields.io/badge/Prettier-Config-F7B93E?style=for-the-badge&logo=prettier&logoColor=black)
![TypeScript%20ESLint](https://img.shields.io/badge/TypeScript-ESLint-3178C6?style=for-the-badge&logo=typescript&logoColor=white)


#### 🧠 Backend

![.NET 8](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-68217A?style=for-the-badge&logo=efcore&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)
![Cloudinary](https://img.shields.io/badge/Cloudinary-3448C5?style=for-the-badge&logo=cloudinary&logoColor=white)


#### 🛢️ Database & Cache

![Microsoft SQL Server 2022](https://img.shields.io/badge/Microsoft%20SQL%20Server-2022-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)
![Cloudinary Media Storage](https://img.shields.io/badge/Cloudinary-Media%20Storage-3448C5?style=for-the-badge&logo=cloudinary&logoColor=white)


#### ☁️ Infrastructure & Cloud Services


![Vercel](https://img.shields.io/badge/Vercel-000000?style=for-the-badge&logo=vercel&logoColor=white)
![Mailgun](https://img.shields.io/badge/Mailgun-EA4335?style=for-the-badge&logo=mailgun&logoColor=white)
![Gemini AI API](https://img.shields.io/badge/Gemini_AI-4285F4?style=for-the-badge&logo=google&logoColor=white)
![Momo Gateway](https://img.shields.io/badge/MoMo_Payment-DD0A73?style=for-the-badge)

#### 🚀 DevOps / Monitoring / CI-CD

![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Docker Compose](https://img.shields.io/badge/Docker_Compose-1488C6?style=for-the-badge&logo=docker&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/GitHub_Actions-2088FF?style=for-the-badge&logo=githubactions&logoColor=white)
![DockerHub](https://img.shields.io/badge/DockerHub-2496ED?style=for-the-badge&logo=docker&logoColor=white)


#### ⚙️ Dev Tools

![Visual Studio](https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visualstudio&logoColor=white)
![VS Code](https://img.shields.io/badge/VS_Code-007ACC?style=for-the-badge&logo=visual-studio-code&logoColor=white)
![GitHub Desktop](https://img.shields.io/badge/GitHub_Desktop-FFFFFF?style=for-the-badge&logo=github&logoColor=blue)
![Git](https://img.shields.io/badge/Git-F05032?style=for-the-badge&logo=git&logoColor=white)
![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)
![Discord](https://img.shields.io/badge/Discord-5865F2?style=for-the-badge&logo=discord&logoColor=white)



## Architecture Overview

**Layered backend design**

- `API` exposes RESTful controllers, Swagger UI, CORS policy, middleware, and global exception handling.
- `Application` hosts business logic, DTOs, validators, mappers, services (Gemini, MoMo, rental, support, vehicle, user), and unit-of-work abstractions.
- `Domain` defines aggregate roots (vehicles, contracts, invoices, stations, staff, support requests, identity documents) and shared primitives.
- `Infrastructure` implements EF Core context, repositories, migrations, caching, and Cloudinary services.

**Runtime flow**

1. Next.js uses React Query hooks and Zustand stores to orchestrate customer/staff experiences and guard protected routes.
2. Requests reach the ASP.NET Core API, validated via FluentValidation filters and orchestrated through service layers.
3. EF Core persists to SQL Server; Redis and in-memory caches accelerate role lookups, OTP state, and payment sessions.
4. Cloudinary hosts uploaded assets, Gemini validates identity documents, and MoMo completes payment link lifecycles.
5. Swagger at `http://localhost:5160/swagger` provides interactive API documentation during development.

## Key Features

**Customer experience**
- Localised landing pages, vehicle catalogue, and rental contract registration (`frontend/src/app/(public)` & `/ (private)/(customer)`).
- JWT/refresh token handling with silent refresh and Google OAuth sign-in.
- Profile management, document uploads, and rental contract status tracking.

**Staff workspace**
- Dedicated dashboard with modules for contracts, handovers, fleet, reports, and notifications.
- Sidebar-driven navigation (`frontend/src/components/modules/Staff`) with HeroUI widgets and charting.
- Report submission and issue tracking with status-driven tables and filters.

**Platform services**
- OTP-driven registration, password recovery, and account activation via SMTP.
- Identity verification: Gemini service parses citizen ID and driver licence images stored in Cloudinary.
- Payment integration: MoMo payment link creation, IPN validation, and invoice settlement.
- Vehicle lifecycle: segmentation, model imagery, checklist management, rental contract workflows, and dispatch coordination.
- Support centre: staff reports, support requests, and caching of role metadata for rapid lookups.

## Getting Started

### Prerequisites

- Node.js >= 20 and npm (or pnpm/yarn)  
- .NET 8 SDK  
- Docker Desktop (for containerised setup)  
- SQL Server tooling (SQLCMD, Azure Data Studio, or SSMS) if you wish to inspect or seed the database manually

### Clone & install

```bash
git clone https://github.com/<your-org>/green-wheel.git
cd green-wheel
```

Install dependencies:

```bash
# Frontend
cd frontend
npm install

# Backend
cd ../backend
dotnet restore
```

## Environment Configuration

Copy `.env.example` files to `.env` and update values based on your environment.

### Backend (`backend/.env`)

| Variable | Description | Example |
| --- | --- | --- |
| `FRONTEND_ORIGIN` | Allowed CORS origin for the web client | `http://localhost:3000` |
| `MSSQL_CONNECTION_STRING` | Full SQL Server connection string used by EF Core | `"Server=localhost,1434;Database=green_wheel_db;User Id=sa;Password=<YourStrong!Passw0rd>;TrustServerCertificate=True;"` |
| `MSSQL_HOST` / `MSSQL_PORT` / `MSSQL_DB` | Individual SQL Server settings (used by Docker Compose) | `localhost` / `1434` / `green_wheel_db` |
| `MSSQL_USER` / `MSSQL_PASSWORD` | Database credentials | `sa` / `<YourStrong!Passw0rd>` |
| `REDIS_CONFIGURATION` | Redis connection string for distributed cache | `localhost:6379` |
| `REDIS_HOST` / `REDIS_PORT` / `REDIS_INSTANCE` | Redis docker settings & key prefix | `redis` / `6379` / `GreenWheel` |

> Additional secrets (JWT, OTP, email, Cloudinary, Gemini, MoMo, Google OAuth) ship in `API/appsettings.json`. Override them via environment variables or secret stores before deploying to any shared environment.

### Frontend (`frontend/.env`)

| Variable | Description | Example |
| --- | --- | --- |
| `NEXT_PUBLIC_BACKEND_API_URL` | Base URL for REST calls (must include `/api/`) | `http://localhost:5160/api/` |
| `NEXT_PUBLIC_GOOGLE_CLIENT_ID` | Google OAuth client ID surfaced to the browser | `617684376840-e90f14r0gchfni0rhokrql21i18lst1v.apps.googleusercontent.com` |

## Run the Project

### Option 1 - Docker Compose (backend stack)

```bash
cd backend
docker compose up --build
```

Services started:

- `greenwheel-backend`: ASP.NET Core API on `http://localhost:5160`
- `greenwheel-mssql`: SQL Server 2022 on `localhost:1434`
- `greenwheel-redis`: Redis cache on `localhost:6379`

### Option 2 - Run everything manually

```bash
# Backend API
cd backend
dotnet ef database update     # applies EF Core migrations
dotnet run --project API      # serves Swagger at http://localhost:5160/swagger

# Frontend (new terminal)
cd frontend
npm run dev                   # runs on http://localhost:3000
```

When using the manual approach, ensure SQL Server and Redis are running locally (or update connection strings accordingly).

## Database & Migrations

- EF Core migrations live in `backend/Infrastructure/Migrations`. Apply them with `dotnet ef database update`.
- SQL seed scripts (`01-schema.sql`, `02-seeding.sql`) live under `init-db/` for manual provisioning or inspection.
- Example PowerShell execution using SQLCMD:

```powershell
sqlcmd -S localhost,1434 -U sa -P '<YourStrong!Passw0rd>' -i ..\init-db\01-schema.sql
sqlcmd -S localhost,1434 -U sa -P '<YourStrong!Passw0rd>' -i ..\init-db\02-seeding.sql
```

## Development Workflow

- **Linting:** `npm run lint` (Next.js + ESLint + TypeScript rules)
- **Frontend build:** `npm run build` / `npm run start`
- **Backend build:** `dotnet build` or `dotnet publish -c Release`
- **Environment loading:** `DotNetEnv` hydrates configuration from `.env`; secrets can be promoted to production vaults.
- **Error handling:** `GlobalErrorHandlerMiddleware` wraps HTTP responses and logs; validation errors bubble via `ValidationFilter`.
- **Caching:** Roles and OTP flows use Redis/in-memory caches. Clear Redis when changing critical auth data in dev.
- **Tests:** Automated tests are not yet defined. Add unit/integration test projects under `backend/` and jest/vitest suites for the frontend as the next improvement.

## Documentation & Assets

Design artefacts (system architecture, CI/CD workflow, and ERD diagrams) accompany the project documentation package shared with the repository. Place them under a `docs/` directory (e.g., `docs/system-architecture.png`) to surface them in the README if you need in-repo previews.

## Project Credits

<div align="center">

| No | Student ID | Name | GitHub URL |
| --- | --- | --- | --- |
| 1 | SE192160 | Trang Thuận Đức | [ducklawrence05](https://github.com/ducklawrence05) |
| 2 | SE183274 | Ngô Gia Huy | [devgiahuy](https://github.com/devgiahuy) |
| 3 | SE192427 | Lê Hoàng Duy | [MavilH](https://github.com/MavilH) |
| 4 | SE194177 | Nguyễn Quang Huy | [WuagHy](https://github.com/WuagHy) |
| 5 | QE190133 | Phạm Hồng Phúc | [PhucPhamHong-dev](https://github.com/PhucPhamHong-dev) |


## </div>

<div align="center">

### 🙏 Thank You for Being Part of Green Wheel!

</div>

We appreciate your contributions and dedication to the Green Wheel project.

- For questions or suggestions, please communicate within the project team or use the repository's [issue tracker](../../issues).
- **Note:** External contributions are not accepted, as this is a closed group project.

<div align="center">

Let's build something great together! 🚀

</div>
