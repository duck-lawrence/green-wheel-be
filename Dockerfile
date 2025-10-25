# ============================
# STAGE 1: BUILD
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files (để tối ưu cache)
COPY NuGet.Config ./
COPY API/API.csproj ./API/
COPY Application/Application.csproj ./Application/
COPY Domain/Domain.csproj ./Domain/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/

# Restore dependencies
RUN dotnet restore API/API.csproj --verbosity minimal

# Copy toàn bộ source code
COPY . .

# Build & publish API project (Release mode)
WORKDIR /src/API
RUN dotnet publish -c Release -o /app/publish

# ============================
# STAGE 2: RUNTIME
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy kết quả publish từ stage build
COPY --from=build /app/publish .

# Azure App Service yêu cầu port 8080
ENV ASPNETCORE_URLS=http://+:8080

# Không hard-code môi trường ở đây (Azure sẽ inject)
# Nếu cần test local, bạn có thể override bằng `-e ASPNETCORE_ENVIRONMENT=Development`
EXPOSE 8080

# Khởi chạy ứng dụng
ENTRYPOINT ["dotnet", "API.dll"]
