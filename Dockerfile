# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY *.sln .
COPY Bootstrapper/Api/*.csproj ./Bootstrapper/Api/
COPY Modules/*/*.csproj ./Modules/
COPY Shared/*/*.csproj ./Shared/

# Restore dependencies
RUN dotnet restore

# Copy source code
COPY . .

# Build the application
RUN dotnet build -c Release --no-restore

# Publish stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS publish
WORKDIR /app

# Copy published application
COPY --from=build /src/Bootstrapper/Api/bin/Release/net9.0/publish .

# Expose port
EXPOSE 7001

# Set entry point
ENTRYPOINT ["dotnet", "Api.dll"] 