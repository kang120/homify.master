# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

ARG PACKAGE_TOKEN
ARG PACKAGE_SOURCE=https://nuget.pkg.github.com/kang120/index.json

RUN wget -qO- https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh | bash

ENV NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED true
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS "{\"endpointCredentials\": [{\"endpoint\":\"${PACKAGE_SOURCE}\", \"username\":\"kang120\", \"password\":\"${PACKAGE_TOKEN}\"}]}"

# Copy solution and project files first (for layer caching)
COPY Master.slnx .
COPY MasterApi/MasterApi.csproj MasterApi/
COPY MasterService/MasterService.csproj MasterService/
COPY MasterDatabase/MasterDatabase.csproj MasterDatabase/

# Copy everything else
COPY . .
RUN dotnet publish "MasterApi/MasterApi.csproj" --source "${PACKAGE_SOURCE};https://api.nuget.org/v3/index.json" -c Release -o /app/publish

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "MasterApi.dll"]
