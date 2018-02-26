# Aktuel Listesi API

This project is API of Aktuel Listesi Mobile App. Developed by ASP.NET Core Web API, EF Core 2.0, Azure Storage, Cognitive Services and Azure Functions

[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/?repository=https://github.com/peacecwz/aktuel-listesi)


![Build status](https://peacecwz.visualstudio.com/_apis/public/build/definitions/8c44d338-1b68-4c2f-b689-2d4e64ea03f4/9/badge)



## Getting Started

First of all, you need to clone the project to your local machine

```
git clone https://github.com/sinemhasircioglu/MusicDatabase-API.git
cd MusicDatabase-API
```

### Building

A step by step series of building that project

1. Restore the project :hammer:

```
dotnet restore
```

2. Create appsettings.Development.json into AktuelListesi.API Project (Copy appsettings.json to appsettings.Development.json for Development Stage)

2. Change connection string of Database (File: appsettings.Development.json, Line: 13)

3. If you want to use change Database Provider to MS SQL, MySQL etc... You can change on Startup.cs File (Line: 58)

```
    //For Microsoft SQL Server
    services.AddDbContext<AktuelDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AktuelDbConnection"), opt => opt.MigrationsAssembly("AktuelListesi.API")), contextLifetime: ServiceLifetime.Singleton, optionsLifetime: ServiceLifetime.Singleton);
```

7. Run EF Core Migrations

```
dotnet ef database update
```

6. Fill Azure Cognitive Services Url and keys in appsettings.Development.json

7. Fill Azure Storage Account name and Key in appsettings.Development.json

8. Run the project and Enjoy! :bomb:

```
dotnet run
```
## Demo

You can try it on [Swagger UI.](https://aktuellistesi.azurewebsites.net/swagger/) :gun:

[Find source code of Aktuel Listesi mobile ppp](https://github.com/peacecwz/aktuel-listesi-app)


## Built With

* [.NET Core 2.0](https://www.microsoft.com/net/) 
* [Entitiy Framework Core](https://docs.microsoft.com/en-us/ef/core/) - .NET ORM Tool
* [NpgSQL for EF Core](http://www.npgsql.org/efcore/) - PostgreSQL extension for EF 
* [Swagger](https://swagger.io/) - API developer tools for testing and documention
* [Azure Storage](https://azure.microsoft.com/en-us/services/storage/)
* [Azure Cognitive Services](https://azure.microsoft.com/en-us/services/cognitive-services/)
* [Azure Functions](https://azure.microsoft.com/en-us/services/functions/)

## Contributing

* If you want to contribute to codes, create pull request
* If you find any bugs or error, create an issue

