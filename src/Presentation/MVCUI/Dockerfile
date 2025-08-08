# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/Presentation/MVCUI/MVCUI.csproj", "src/Presentation/MVCUI/"]
COPY ["src/Core/Application/Application.csproj", "src/Core/Application/"]
COPY ["src/Core/Domain/Domain.csproj", "src/Core/Domain/"]
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]
COPY ["src/Infrastructure/Infrastructure/Infrastructure.csproj", "src/Infrastructure/Infrastructure/"]
COPY ["src/Infrastructure/Persistence/Persistence.csproj", "src/Infrastructure/Persistence/"]
RUN dotnet restore "./src/Presentation/MVCUI/MVCUI.csproj"
COPY . .
WORKDIR "/src/src/Presentation/MVCUI"
RUN dotnet build "./MVCUI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MVCUI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MVCUI.dll"]