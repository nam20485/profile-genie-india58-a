# Profile Genie

A modern profile management application built with .NET 9.0, Blazor Server, and ASP.NET Core, orchestrated with .NET Aspire.

## Technology Stack

| Component | Technology |
|-----------|------------|
| Language | C# .NET 9.0 |
| UI Framework | Blazor Server |
| Backend | ASP.NET Core Web API |
| Orchestration | .NET Aspire |
| Database | PostgreSQL |
| Testing | xUnit, FluentAssertions, Moq, bUnit, Playwright |
| Containerization | Docker |

## Project Structure

```
ProfileGenie/
├── src/
│   ├── ProfileGenie.Core/           # Domain logic, entities, services
│   ├── ProfileGenie.Data/           # PostgreSQL / EF Core data access
│   ├── ProfileGenie.ApiService/     # REST API endpoints
│   ├── ProfileGenie.PlaywrightService/ # Browser automation service
│   ├── ProfileGenie.Web/            # Blazor Server UI
│   ├── ProfileGenie.Shared/         # Shared DTOs, contracts
│   ├── ProfileGenie.ServiceDefaults/ # Aspire service defaults
│   └── ProfileGenie.AppHost/        # .NET Aspire host
├── tests/
│   ├── ProfileGenie.Core.Tests/
│   ├── ProfileGenie.ApiService.Tests/
│   └── ProfileGenie.Web.Tests/
└── assets/
```

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- PostgreSQL 15+ (or use Docker)
- Docker (optional, for containerized development)
- .NET Aspire workload (optional, for full orchestration)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/nam20485/profile-genie-india58-a.git
cd profile-genie-india58-a/ProfileGenie
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Solution

```bash
dotnet build
```

### 4. Run Tests

```bash
dotnet test
```

### 5. Run the Application

#### Option A: Run Individual Services

```bash
# Run the API
dotnet run --project src/ProfileGenie.ApiService

# Run the Web frontend (in another terminal)
dotnet run --project src/ProfileGenie.Web
```

#### Option B: Run with Aspire (requires workload)

```bash
# Install Aspire workload (requires elevated privileges)
dotnet workload install aspire

# Run the AppHost
dotnet run --project src/ProfileGenie.AppHost
```

### 6. Run with Docker

```bash
cd ../docker
docker-compose up -d
```

## Development

### Database Migrations

```bash
cd src/ProfileGenie.Data
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### API Documentation

When running the API, access Swagger UI at:
- Development: `http://localhost:5000/swagger`
- Production: `https://your-domain/swagger`

### Health Checks

- API Health: `http://localhost:5000/health`
- API Liveness: `http://localhost:5000/alive`

## Configuration

### Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `ConnectionStrings__profilegenie-db` | PostgreSQL connection string | localhost |
| `ASPNETCORE_ENVIRONMENT` | Environment (Development/Production) | Development |

### appsettings.json

Configuration files are located in each project's directory:
- `src/ProfileGenie.ApiService/appsettings.json`
- `src/ProfileGenie.Web/appsettings.json`

## Testing

### Unit Tests

```bash
dotnet test
```

### Integration Tests

```bash
dotnet test --filter "Category=Integration"
```

### Playwright E2E Tests

```bash
# Install Playwright browsers
pwsh tests/ProfileGenie.Web.Tests/playwright.ps1 install

# Run Playwright tests
dotnet test --filter "Category=E2E"
```

## Docker

### Build Images

```bash
docker build -t profile-genie-api -f docker/Dockerfile.api .
docker build -t profile-genie-frontend -f docker/Dockerfile.frontend .
```

### Docker Compose

```bash
cd docker
docker-compose up -d
```

## CI/CD

This project uses GitHub Actions for continuous integration:
- Build and test on every push
- Security scanning with TruffleHog
- Code quality checks

## Contributing

1. Create a feature branch from `main`
2. Make your changes
3. Ensure all tests pass: `dotnet test`
4. Submit a pull request

## License

MIT License - See [LICENSE.md](../LICENSE.md) for details.
