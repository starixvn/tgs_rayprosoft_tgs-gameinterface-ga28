#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameInterface.Ga28/Tgs.GameInterface.Ga28.csproj", "Tgs.GameInterface.Ga28/"]
COPY ["Tgs.GameInterface.Ga28.Core/Tgs.GameInterface.Ga28.Core.csproj", "Tgs.GameInterface.Ga28.Core/"]
COPY ["Tgs.GameInterface.Ga28.Protos/Tgs.GameInterface.Ga28.Protos.csproj", "Tgs.GameInterface.Ga28.Protos/"]
RUN dotnet restore "./Tgs.GameInterface.Ga28/Tgs.GameInterface.Ga28.csproj"
COPY . .
WORKDIR "/src/Tgs.GameInterface.Ga28"
RUN dotnet build "./Tgs.GameInterface.Ga28.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameInterface.Ga28.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameInterface.Ga28.dll"]