# Use the ASP.NET Core runtime image for .NET 10.0
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the .NET 10.0 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copy csproj files and restore dependencies
COPY ["BookFlix.Web/BookFlix.Web.csproj", "BookFlix.Web/"]
COPY ["BookFlix.Core/BookFlix.Core.csproj", "BookFlix.Core/"]
COPY ["BookFlix.Infrastructure/BookFlix.Infrastructure.csproj", "BookFlix.Infrastructure/"]
RUN dotnet restore "BookFlix.Web/BookFlix.Web.csproj"

# Copy the rest of the source code and build
COPY . .
WORKDIR "/src/BookFlix.Web"
RUN dotnet build "BookFlix.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BookFlix.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
# Final stage: copy published output and set entry point
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookFlix.Web.dll"]
