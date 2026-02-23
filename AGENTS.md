# Profile Genie — AGENTS.md

> AI coding agent instructions for the **Profile Genie** project.
> Profile Genie is a .NET 9 web application that automates filling out user-research screening surveys.
> It uses a "Scout Agent" protocol (disposable secondary account) to map questions before submitting
> the full application via the user's primary account.

---

## Project Overview

| Item | Value |
|------|-------|
| Language | C# / .NET 9.0 |
| Frontend | Blazor Server (`ProfileGenie.Web`) |
| Backend API | ASP.NET Core Web API (`ProfileGenie.ApiService`) |
| Data Access | PostgreSQL / EF Core (`ProfileGenie.Data`) |
| Browser Automation | Playwright (`ProfileGenie.PlaywrightService`) |
| Orchestration | .NET Aspire (`ProfileGenie.AppHost`) |
| Authentication | ASP.NET Core Identity + JWT |
| Real-time | SignalR (stretch goal: Interactive Fallback Mode) |
| Testing | xUnit, FluentAssertions, Moq, bUnit, Playwright |
| Container | Docker / Docker Compose |
| Security scanning | TruffleHog |

### Solution layout

```
ProfileGenie/
├── src/
│   ├── ProfileGenie.Core/              # Domain entities, interfaces, services
│   ├── ProfileGenie.Data/              # PostgreSQL / EF Core data access
│   ├── ProfileGenie.ApiService/        # REST API controllers + Swagger
│   ├── ProfileGenie.PlaywrightService/ # Browser automation (Scout Agent)
│   ├── ProfileGenie.Web/               # Blazor Server UI components
│   ├── ProfileGenie.Shared/            # Shared DTOs and contracts
│   ├── ProfileGenie.ServiceDefaults/   # Aspire service defaults (telemetry, health)
│   └── ProfileGenie.AppHost/           # .NET Aspire orchestration host
└── tests/
    ├── ProfileGenie.Core.Tests/        # xUnit unit tests for domain logic
    ├── ProfileGenie.ApiService.Tests/  # xUnit integration tests for API
    └── ProfileGenie.Web.Tests/         # bUnit + Playwright E2E tests
```

---

## Dev Environment Setup

### Prerequisites

- .NET 9.0 SDK (`global.json` pins to `9.0.x` with `latestFeature` roll-forward)
- PostgreSQL 15+ (or Docker — preferred for local dev)
- Docker & Docker Compose
- Optional: .NET Aspire workload for full orchestration

### Install / restore

```bash
cd ProfileGenie
dotnet restore
```

### Install Aspire workload (optional)

```bash
dotnet workload install aspire
```

---

## Build Commands

All commands run from `ProfileGenie/` unless noted.

```bash
# Build entire solution
dotnet build

# Build release
dotnet build -c Release

# Build a single project
dotnet build src/ProfileGenie.ApiService
```

---

## Run the Application

### Option A — Individual services (no Aspire)

```bash
# Terminal 1 — API
dotnet run --project src/ProfileGenie.ApiService

# Terminal 2 — Web
dotnet run --project src/ProfileGenie.Web
```

### Option B — .NET Aspire (recommended)

```bash
dotnet run --project src/ProfileGenie.AppHost
```

Aspire dashboard: `http://localhost:15000`
API Swagger UI: `http://localhost:5000/swagger`
API health: `http://localhost:5000/health`

### Option C — Docker Compose

```bash
cd ../docker
docker-compose up -d
```

---

## Testing Instructions

Run all tests before committing. The CI gate requires a passing test suite.

```bash
# Run all tests
cd ProfileGenie
dotnet test

# Run only unit tests (fast)
dotnet test --filter "Category!=Integration&Category!=E2E"

# Run integration tests
dotnet test --filter "Category=Integration"

# Run Playwright E2E tests
pwsh tests/ProfileGenie.Web.Tests/playwright.ps1 install
dotnet test --filter "Category=E2E"

# Run tests with coverage report
dotnet test --collect:"XPlat Code Coverage"
```

Test projects map 1-to-1 with source projects. Add or update tests for every code change;
do not merge code that reduces coverage for modified paths.

---

## Database Migrations

```bash
cd src/ProfileGenie.Data
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

Connection string env var: `ConnectionStrings__profilegenie-db`

---

## Code Style Guidelines

- Target framework: **net9.0** (set in `Directory.Build.props`)
- Nullable reference types: **enabled** — never suppress warnings with `!` without justification
- Implicit usings: **enabled**
- Namespaces follow folder structure (`ProfileGenie.<Project>.<Folder>`)
- Use **`string.Empty`** instead of `""`
- Repository pattern: interfaces in `ProfileGenie.Core/Interfaces/`, implementations in `ProfileGenie.Core/Services/`
- DTOs and shared contracts live in `ProfileGenie.Shared`; never reference project-internal types across service boundaries
- Sensitive data (third-party credentials) must use .NET Data Protection APIs — never store plain text
- Structured logging via Serilog; include correlation IDs on all automation jobs
- API endpoints follow REST conventions; document all public endpoints with XML `<summary>` tags

---

## Security Considerations

- **TruffleHog** runs in CI to scan for leaked secrets. Allowlisted patterns are in `security/trufflehog-allowlist.yml`.
  Never add real secrets there; use canary patterns only.
- Credentials for Primary and Scout accounts are encrypted at rest using .NET Data Protection APIs.
- JWT tokens are short-lived; refresh token rotation is required.
- Playwright automation must use human-like delays and realistic browser fingerprints to avoid detection —
  see the Scout Agent protocol in the architecture docs.
- On CAPTCHA detection, automation must fail gracefully, capture a screenshot, and notify the user.
  Never attempt to bypass CAPTCHAs programmatically.
- Never log credential values, JWT secrets, or database connection strings.

### Running TruffleHog locally

```bash
# PowerShell
./scripts/security/run-trufflehog.ps1

# Bash
./scripts/security/run-trufflehog.sh
```

---

## CI/CD

GitHub Actions workflows live in `.github/workflows/`. Every push triggers:

1. `dotnet build` + `dotnet test`
2. TruffleHog secret scan
3. Code quality checks

Branch protection requires green CI before merge to `main`.

---

## PR Guidelines

- Branch from `main`; use descriptive branch names (`feature/`, `fix/`, `chore/`)
- Commit message format: `<type>: <short description>` (e.g., `feat: add scout agent workflow`)
- Ensure `dotnet build` and `dotnet test` pass locally before opening a PR
- One logical change per PR; keep diffs reviewable
- Reference the relevant GitHub issue in the PR description

---

## Key Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `ConnectionStrings__profilegenie-db` | PostgreSQL connection string | `localhost` |
| `ASPNETCORE_ENVIRONMENT` | `Development` or `Production` | `Development` |

---

## Agent Orchestration Bootstrap

The section below is the project-agnostic entry point for AI agent instruction modules.
It is **not** project-specific and must be preserved as-is.

---
description: Entry point for AGENTS custom instructions
scope: global
role: System Orchestrator
---

<instructions>
  <overview>
    This file serves as the bootstrap entry point for the AI agent's instruction set.
    It defines the location of core modules, the protocol for loading remote instructions, and the single source of truth policy.
  </overview>

  <configuration>
    <!-- BRANCH PARAMETER: Change this value to load instructions from a different branch -->
    <!-- Valid values: main, optimization, feature/*, or any valid branch name -->
    <branch>optimization</branch>
  </configuration>

  <instruction_source>
    <repository>
      <name>nam20485/agent-instructions</name>
      <url>https://github.com/nam20485/agent-instructions/tree/{branch}</url>
      <branch>{branch}</branch>
    </repository>
    <guidance>
      Start with the Core Instructions linked below. Follow links to other modules as required by the user's request.
      All remote URLs use the branch specified in the configuration section above.
    </guidance>
  </instruction_source>

  <module_registry>
    <module type="core" required="true">
      <name>Core Instructions</name>
      <link>https://github.com/nam20485/agent-instructions/blob/{branch}/ai_instruction_modules/ai-core-instructions.md</link>
      <description>The foundational behaviors and rules for the agent.</description>
    </module>

    <module type="local" required="true">
      <name>Local AI Instructions</name>
      <path>../local_ai_instruction_modules</path>
      <description>Context-specific instructions located in the local workspace.</description>
    </module>

    <module type="dynamic workflow" required="true">
      <name>Dynamic Workflow Orchestration</name>
      <path>../local_ai_instruction_modules/ai-dynamic-workflows.md</path>
      <description>Protocol for resolving workflows from the remote canonical repository.</description>
    </module>

    <module type="workflow assignment" required="true">
      <name>Workflow Assignments</name>
      <path>../local_ai_instruction_modules/ai-workflow-assignments.md</path>
      <description>Index of active workflow assignments by shortId.</description>
    </module>

    <module type="optional">
      <name>Terminal Commands</name>
      <path>../local_ai_instruction_modules/ai-terminal-commands.md</path>
      <description>Reference for terminal operations and GitHub CLI usage.</description>
    </module>
  </module_registry>

  <loading_protocol>
    <rule id="branch_resolution">
      <description>Resolving the active branch</description>
      <instruction>
        Read the branch value from the configuration section at the top of this file.
        Replace all `{branch}` placeholders in URLs with this value.
        Default: use the configured `<branch>` value; if missing, use the repository default branch.
      </instruction>
    </rule>

    <rule id="remote_access">
      <description>Accessing files in the remote repository</description>
      <instruction>
        Always use the RAW URL to read file contents. Do not use the GitHub UI URL.
      </instruction>
    </rule>

    <algorithm name="url_translation">
      <step>Read the configured branch from `<configuration><branch>`.</step>
      <step>Identify the GitHub UI URL (e.g., `https://github.com/.../blob/{branch}/...`).</step>
      <step>Replace `https://github.com/` with `https://raw.githubusercontent.com/`.</step>
      <step>Remove `blob/` from the path.</step>
      <step>Substitute `{branch}` with the configured branch value.</step>
      <step>Result: `https://raw.githubusercontent.com/.../{branch}/...`</step>
    </algorithm>

    <examples>
      <example title="Default (configured branch)">
        <config_branch>{branch}</config_branch>
        <input>https://github.com/nam20485/agent-instructions/blob/{branch}/ai_instruction_modules/ai-core-instructions.md</input>
        <output>https://raw.githubusercontent.com/nam20485/agent-instructions/{branch}/ai_instruction_modules/ai-core-instructions.md</output>
      </example>
      <example title="Optimization branch">
        <config_branch>optimization</config_branch>
        <input>https://github.com/nam20485/agent-instructions/blob/{branch}/ai_instruction_modules/ai-core-instructions.md</input>
        <output>https://raw.githubusercontent.com/nam20485/agent-instructions/optimization/ai_instruction_modules/ai-core-instructions.md</output>
      </example>
    </examples>
  </loading_protocol>

  <policy name="single_source_of_truth">
    <statement>
      The remote canonical repository is the ONLY authoritative source for dynamic workflows and workflow assignments.
    </statement>
    <rules>
      <rule>Do not use local mirrors or cached plans to derive steps.</rule>
      <rule>Fetch and execute directly from the remote canonical URLs.</rule>
      <rule>Changes in the remote repo take effect immediately.</rule>
    </rules>
  </policy>
</instructions>
