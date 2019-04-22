FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore ./src/MarketCatalog.API/MarketCatalog.API.csproj

RUN dotnet publish ./src/MarketCatalog.API/MarketCatalog.API.csproj -c Release -o out

FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/src/MarketCatalog.API/out .
ENTRYPOINT ["dotnet", "MarketCatalog.API.dll"]
