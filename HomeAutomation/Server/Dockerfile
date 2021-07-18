#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["HomeAutomation/Server/HomeAutomation.Server.csproj", "HomeAutomation/Server/"]
COPY ["HomeAutomation/Shared/HomeAutomation.Shared.csproj", "HomeAutomation/Shared/"]
COPY ["HomeAutomation/Client/HomeAutomation.Client.csproj", "HomeAutomation/Client/"]
RUN dotnet restore "HomeAutomation/Server/HomeAutomation.Server.csproj"
COPY . .
WORKDIR "/src/HomeAutomation/Server"
RUN dotnet build "HomeAutomation.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeAutomation.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeAutomation.Server.dll"]