# Training Plan

This repo contains the sample code of a Web API and a Web client combinations dedicated to managing training plans for runners.

It uses .NET Core 2.x and is inspired by the following reference architectures:

- [eShopOnWeb](https://github.com/dotnet-architecture/eShopOnWeb)
- [eShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainerseShopOnWeb)

as well as the following code samples:

- [Design Patterns: Asp.Net Core Web API, services, and repositories](http://www.forevolve.com/en/articles/2017/08/11/design-patterns-web-api-service-and-repository-part-1/) blog series by ForEvolve

As suggested by Clean architecture advocates, the Web API code is organized in three main projects:

- **TrainingPlan.ApplicationCore**: POCO entities, application exceptions, interfaces, business services...
- **TrainingPlan.Infrastructure**: only the EF-based data access layer currently but other infrastructure services eventually
- **TrainingPlan.WebApi**: MVC-based RESTful Web API for basic CRUD operations; this Web application is the sole access point for the time being

The code also includes a fourth project:

- **TrainingPlan.WebMvc**: MVC-based Web application solely meant for providing a first client for the Web API; it will eventually be completed or replaced by a proper user-facing Web application.

> ### DISCLAIMER
> **IMPORTANT:** The current state of this application is 0.0. It is open to community feedback and contributions. **Feedback with improvements and pull requests from the community are highly appreciated and will be accepted if possible.**

## Running the sample

After cloning or downloading the sample you should be able to run it immediately using an In Memory database.

`Startup.cs` also enables the use of a persistent database ([SQLite](https://www.sqlite.org)) through the `ASPNETCORE_ENVIRONMENT` environment variable, as explained below.

1. Update the `ASPNETCORE_ENVIRONMENT` environment variable of the TrainingPlan.WebApi project and set it to `Development`; optionally, adjust the `DataConnection` connection string in the `appsettings.Development.json` file of the TrainingPlan.WebApi project.

2. Open a command prompt in the TrainingPlan.WebApi folder and execute the following commands:

```
dotnet restore
dotnet ef database update -c TrainingPlanContext -s TrainingPlan.WebApi.csproj -p ..\TrainingPlan.Infrastructure\TrainingPlan.Infrastructure.csproj
```

   The first command makes sure all dependencies and tools of the TrainingPlan.WebApi project are restored while the second will create the sole database for the sample.

3. Run the TrainingPlan.WebApi sample; the first the first time you run it, it will seed the database with demo data.

4. Test it with typical API development tools (e.g. [Postman](https://www.getpostman.com/)); optionally, test it by also running the TrainingPlan.WebMvc sample.