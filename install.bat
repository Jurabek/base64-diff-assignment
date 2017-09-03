@echo off
dotnet --info > nul 2>&1 || goto :nonetcore

echo.
echo Restoring NuGet packages...
dotnet restore || goto :error

echo.
echo.
echo Building solution...
dotnet build Base64Diff.sln --configuration Release || goto :error

echo.
echo.
echo Running tests...
dotnet vstest ^
	test/domain/bin/Release/netcoreapp2.0/Base64Diff.DomainTests.dll ^
	test/api/bin/Release/netcoreapp2.0/Base64Diff.ApiTests.dll ^
	test/integration/bin/Release/netcoreapp2.0/Base64Diff.IntegrationTests.dll || goto :error

echo.
echo.
echo Installation was successful. All tests passed.
exit /b 0

:nonetcore
echo.
echo This program requires .NET Core
echo Please find installation files at https://www.microsoft.com/net/core
exit /b 1

:error
echo.
echo Some error ocurred during installation.
echo Please check above messages.
exit /b 1