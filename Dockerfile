# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files first (for layer caching)
COPY Master.slnx .
COPY MasterApi/MasterApi.csproj MasterApi/
COPY MasterService/MasterService.csproj MasterService/
COPY MasterDatabase/MasterDatabase.csproj MasterDatabase/

# Restore dependencies
RUN dotnet restore MasterApi/MasterApi.csproj

# Copy everything else
COPY . .

# Build & publish
WORKDIR /src/MasterApi
RUN dotnet publish -c Release -o /app/publish

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "MasterApi.dll"]
