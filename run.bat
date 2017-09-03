@echo off
dotnet --info > nul 2>&1 || goto :nonetcore

set ASPNETCORE_ENVIRONMENT=Development && start /I /D src\api dotnet run
exit /b 0

:nonetcore
echo.
echo This program requires .NET Core
echo Please find installation files at https://www.microsoft.com/net/core
exit /b 1

:error
exit /b 1

:end