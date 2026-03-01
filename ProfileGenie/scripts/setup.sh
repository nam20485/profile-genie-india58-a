#!/bin/bash
# Profile Genie Setup Script
# Run this script to set up the development environment

set -e

echo "===================================="
echo "Profile Genie - Setup Script"
echo "===================================="

# Check for .NET SDK
echo ""
echo "Checking prerequisites..."

if ! command -v dotnet &> /dev/null; then
    echo "ERROR: .NET SDK is not installed."
    echo "Please install .NET 9.0 SDK from: https://dotnet.microsoft.com/download/dotnet/9.0"
    exit 1
fi

DOTNET_VERSION=$(dotnet --version)
echo "Found .NET SDK version: $DOTNET_VERSION"

if [[ ! $DOTNET_VERSION == 9.* ]]; then
    echo "WARNING: .NET 9.0 SDK is recommended. Current version: $DOTNET_VERSION"
fi

# Restore dependencies
echo ""
echo "Restoring NuGet packages..."
dotnet restore

# Build the solution
echo ""
echo "Building the solution..."
dotnet build --configuration Debug

# Run tests
echo ""
echo "Running tests..."
dotnet test --no-build --verbosity minimal

echo ""
echo "===================================="
echo "Setup complete!"
echo "===================================="
echo ""
echo "To run the API:"
echo "  dotnet run --project src/ProfileGenie.Api"
echo ""
echo "To run the Frontend:"
echo "  dotnet run --project src/ProfileGenie.Frontend"
echo ""
echo "To run with Docker Compose:"
echo "  docker-compose up -d"
echo ""
