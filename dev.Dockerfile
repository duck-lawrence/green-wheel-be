FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NuGet.Config ./
COPY API/API.csproj API/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/

RUN dotnet restore API/API.csproj --verbosity minimal

COPY . .
WORKDIR /src/API
RUN dotnet publish -c Release -o /app/publish

# ============================
# STAGE 2: RUNTIME
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# ASP.NET Core sẽ tự nhận appsettings.Development.json nếu ENV=Development
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5160
EXPOSE 5160

ENTRYPOINT ["dotnet", "API.dll"]


