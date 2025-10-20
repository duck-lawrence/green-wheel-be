# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY NuGet.Config ./
COPY API/API.csproj ./API/
COPY Application/Application.csproj ./Application/
COPY Domain/Domain.csproj ./Domain/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
RUN dotnet restore API/API.csproj --packages /src/.nuget/packages --verbosity minimal

# Copy source code
COPY . .

# Build and publish API project
WORKDIR /src/API
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy publish result from stage build
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development \
    ASPNETCORE_URLS=http://+:5160

# Expose port
EXPOSE 5160

# Start app
ENTRYPOINT ["dotnet", "API.dll"]