﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Auth/Dberries.Auth.WebAPI/Dberries.Auth.WebAPI.csproj", "src/Services/Auth/Dberries.Auth.WebAPI/"]
RUN dotnet restore "src/Services/Auth/Dberries.Auth.WebAPI/Dberries.Auth.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Services/Auth/Dberries.Auth.WebAPI"
RUN dotnet build "Dberries.Auth.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dberries.Auth.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dberries.Auth.WebAPI.dll"]
