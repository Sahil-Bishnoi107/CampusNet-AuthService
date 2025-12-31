# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files first (better caching)
COPY AuthService.API/AuthService.API.csproj AuthService.API/
COPY Application/AuthService.Application.csproj Application/
COPY Domain/AuthService.Domain.csproj Domain/
COPY AuthService.Infrastructure/AuthService.Infrastructure.csproj AuthService.Infrastructure/

RUN dotnet restore AuthService.API/AuthService.API.csproj

# Copy the rest of the source
COPY . .

WORKDIR /src/AuthService.API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false


# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AuthService.API.dll"]
