@echo off
REM Citizens Portal - Development Setup Script (Windows)
REM This script sets up the development environment including ABP CLI

echo ==========================================
echo Citizens Portal - Development Setup
echo ==========================================
echo.

REM Check if .NET SDK is installed
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo X .NET SDK not found!
    echo.
    echo Please install .NET 8.0 SDK from:
    echo https://dotnet.microsoft.com/download/dotnet/8.0
    echo.
    pause
    exit /b 1
)

echo √ .NET SDK found
dotnet --version
echo.

REM Install ABP CLI globally
echo Installing ABP CLI globally...
dotnet tool install -g Volo.Abp.Cli
if %errorlevel% neq 0 (
    echo ABP CLI may already be installed, attempting update...
    dotnet tool update -g Volo.Abp.Cli
)

echo.
echo Installing local tools from manifest...
dotnet tool restore

echo.
echo Verifying ABP CLI installation...
abp --version

echo.
echo ==========================================
echo Setup Complete!
echo ==========================================
echo.
echo Available ABP CLI Commands:
echo   abp --help                    - Show all available commands
echo   abp new                       - Create new ABP solution
echo   abp add-module                - Add ABP module to solution
echo   abp add-package               - Add ABP package to project
echo   abp update                    - Update ABP packages
echo   abp clean                     - Clean ABP solution
echo   abp generate-proxy            - Generate client proxies
echo   abp bundle                    - Bundle CSS/JS files
echo.
echo Next Steps:
echo   1. Update connection string in src\CitizensPortal.HttpApi.Host\appsettings.json
echo   2. Run: cd src\CitizensPortal.EntityFrameworkCore
echo   3. Run: dotnet ef migrations add InitialCreate
echo   4. Run: dotnet ef database update
echo   5. Run: cd ..\CitizensPortal.HttpApi.Host ^&^& dotnet run
echo.
pause
