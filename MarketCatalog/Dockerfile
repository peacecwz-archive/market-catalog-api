FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["MarketCatalog/MarketCatalog.csproj", "MarketCatalog/"]
RUN dotnet restore "MarketCatalog/MarketCatalog.csproj"
COPY . .
WORKDIR "/src/MarketCatalog"
RUN dotnet build "MarketCatalog.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MarketCatalog.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MarketCatalog.dll"]