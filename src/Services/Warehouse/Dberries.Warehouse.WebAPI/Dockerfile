FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Warehouse/Dberries.Warehouse.WebAPI/Dberries.Warehouse.WebAPI.csproj", "src/Services/Warehouse/Dberries.Warehouse.WebAPI/"]
COPY ["src/Services/Warehouse/Dberries.Warehouse.Persistence/Dberries.Warehouse.Persistence.csproj", "src/Services/Warehouse/Dberries.Warehouse.Persistence/"]
COPY ["src/Services/Warehouse/Dberries.Warehouse.Core/Dberries.Warehouse.Core.csproj", "src/Services/Warehouse/Dberries.Warehouse.Core/"]
COPY ["src/Shared/Dberries.Shared.Contracts/Dberries.Shared.Contracts.csproj", "src/Shared/Dberries.Shared.Contracts/"]
COPY ["src/Shared/Dberries.Shared.Validation/Dberries.Shared.Validation.csproj", "src/Shared/Dberries.Shared.Validation/"]
COPY ["src/Services/Warehouse/Dberries.Warehouse.Presentation/Dberries.Warehouse.Presentation.csproj", "src/Services/Warehouse/Dberries.Warehouse.Presentation/"]
COPY ["src/Services/Warehouse/Dberries.Warehouse.Infrastructure/Dberries.Warehouse.Infrastructure.csproj", "src/Services/Warehouse/Dberries.Warehouse.Infrastructure/"]
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
