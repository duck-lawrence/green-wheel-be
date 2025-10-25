# ============================
# STAGE 1: BUILD
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy file dự án (để tận dụng cache)
COPY NuGet.Config ./ 
COPY API/API.csproj ./API/
COPY Application/Application.csproj ./Application/
COPY Domain/Domain.csproj ./Domain/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/

# Restore dependencies
RUN dotnet restore "API/API.csproj" --verbosity minimal

# Copy toàn bộ source code
COPY . .

# Build & publish (Release mode)
WORKDIR /src/API
RUN dotnet publish "API.csproj" -c Release -o /app/publish

# ============================
# STAGE 2: RUNTIME
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy output publish từ stage build
COPY --from=build /app/publish .

# Copy file cấu hình JSON (rất quan trọng!)
COPY API/appsettings*.json ./

# Azure Container Apps sử dụng port 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Entry point
ENTRYPOINT ["dotnet", "API.dll"]
