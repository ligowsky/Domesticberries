FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Warehouse/Dberries.Warehouse.WebAPI/Dberries.Warehouse.WebAPI.csproj", "src/Services/Warehouse/Dberries.Warehouse.WebAPI/"]
RUN dotnet restore "src/Services/Warehouse/Dberries.Warehouse.WebAPI/Dberries.Warehouse.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Services/Warehouse/Dberries.Warehouse.WebAPI"
RUN dotnet build "Dberries.Warehouse.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dberries.Warehouse.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dberries.Warehouse.WebAPI.dll"]
