# Use official .NET 8.0 SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files
COPY *.sln .
COPY MagasinCentral/*.csproj ./MagasinCentral/
COPY MagasinCentral.Tests/*.csproj ./MagasinCentral.Tests/

# Restore dependencies
RUN dotnet restore

# Copy everything and build
COPY . .
WORKDIR /app/MagasinCentral
RUN dotnet publish -c Release -o /out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["dotnet", "MagasinCentral.dll"]
