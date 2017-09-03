.PHONY: test

build:
	@dotnet build Base64Diff.sln

test: build
	@dotnet vstest \
		test/domain/bin/Debug/netcoreapp2.0/Base64Diff.DomainTests.dll \
		test/api/bin/Debug/netcoreapp2.0/Base64Diff.ApiTests.dll \
		test/integration/bin/Debug/netcoreapp2.0/Base64Diff.IntegrationTests.dll

run:
	@cd src/api && \
	ASPNETCORE_ENVIRONMENT=Development dotnet run
