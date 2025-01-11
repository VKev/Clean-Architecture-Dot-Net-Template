# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /App

# Copy the solution file
COPY Web.sln ./

# Copy all project files while maintaining the directory structure
COPY src/WebApi/WebApi.csproj ./src/WebApi/
COPY src/Application/Application.csproj ./src/Application/
COPY src/Infrastructure/Infrastructure.csproj ./src/Infrastructure/
COPY src/Domain/Domain.csproj ./src/Domain/
COPY test/test.csproj ./test/

# Restore dependencies
RUN dotnet restore

# Copy the entire source code
COPY src/ ./src/

# Build and publish the WebApi project
WORKDIR /App/src/WebApi
RUN dotnet publish -c Release -o /App/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /App

# Set environment to Production
ENV ASPNETCORE_ENVIRONMENT=Production

# Copy the published output from the build stage
COPY --from=build /App/out .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "WebApi.dll"]
