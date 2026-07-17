<#
.SYNOPSIS
    Scaffolds C# entity models from the BookFlix database using EF Core.

.DESCRIPTION
    This script reverse-engineers the BookFlix database into C# entity models
    and a DbContext class under BookFlix.Infrastructure. Run this script after
    publishing the SQL Database Project to keep C# models in sync with the schema.

.NOTES
    Prerequisites:
    - dotnet-ef tool installed: dotnet tool install --global dotnet-ef
    - A running SQL Server instance with the BookFlix database published.
#>

param(
    [string]$ConnectionString = "Server=.;Database=BookFlix;Trusted_Connection=True;TrustServerCertificate=True;"
)

$ErrorActionPreference = "Stop"

Write-Host "============================================" -ForegroundColor Cyan
Write-Host " BookFlix - EF Core Database Scaffolding"     -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Connection: $ConnectionString" -ForegroundColor DarkGray
Write-Host ""

dotnet ef dbcontext scaffold $ConnectionString Microsoft.EntityFrameworkCore.SqlServer `
    --output-dir Models `
    --context AppDbContext `
    --context-dir Data `
    --project "$PSScriptRoot\..\BookFlix.Infrastructure" `
    --startup-project "$PSScriptRoot\..\BookFlix.Web" `
    --force `
    --no-onconfiguring

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "Scaffolding completed successfully!" -ForegroundColor Green
    Write-Host "Generated models are in: BookFlix.Infrastructure\Models" -ForegroundColor DarkGray
    Write-Host "Generated DbContext is in: BookFlix.Infrastructure\Data" -ForegroundColor DarkGray
} else {
    Write-Host ""
    Write-Host "Scaffolding failed. See errors above." -ForegroundColor Red
    exit 1
}
