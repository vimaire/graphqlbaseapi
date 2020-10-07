FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.8-buster-slim AS base

EXPOSE 80
EXPOSE 443
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.402-buster AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/Migrations/*.csproj ./src/Migrations/
COPY src/Queries/*.csproj ./src/Queries/
COPY src/Commands/*.csproj ./src/Commands/
COPY src/Api/*.csproj ./src/Api/
RUN dotnet restore

# copy everything else and build app
COPY . ./
WORKDIR /source/
RUN dotnet publish -c release -o /app

# final stage/image
FROM base
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Api.dll"]