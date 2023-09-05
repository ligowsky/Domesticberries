FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Store/Dberries.Store.WebAPI/Dberries.Store.WebAPI.csproj", "src/Services/Store/Dberries.Store.WebAPI/"]
RUN dotnet restore "src/Services/Store/Dberries.Store.WebAPI/Dberries.Store.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Services/Store/Dberries.Store.WebAPI"
RUN dotnet build "Dberries.Store.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dberries.Store.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dberries.Store.WebAPI.dll"]
