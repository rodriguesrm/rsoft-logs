# RSoft.Logs
Logging mechanism for elastic (direct mode) and terminal console

RSoft.Logs is a provider of logging mechanisms based on the standard .Net Core ILogger interface. The available resources are:

  - Elastic logger provider
  - Console logger provider
  - Middlware API Request/Response logger

### Dependencies

RSoft.Logs has the following dependencies.

- Microsoft.AspNetCore.Hosting (>= 2.2.7)
- Microsoft.AspNetCore.Http (>= 2.2.2)
- Microsoft.Extensions.Http (>= 5.0.0)
- Microsoft.Extensions.Logging (>= 5.0.0)
- Microsoft.Extensions.Logging.Abstractions (>= 5.0.0)
- Microsoft.Extensions.Logging.Configuration (>= 5.0.0)
- System.Configuration.ConfigurationManager (>= 5.0.0)
- _System.Text.Json (>= 5.0.0)

### Installaction

- PackageManage: `Install-Package RSoft.Logs -Version x.x.x`
- .NET Cli: `dotnet add package RSoft.Logs --version x.x.x`
- Package Reference: `<PackageReference Include="RSoft.Logs" Version="x.x.x" />`
- Paket Cli: `paket add RSoft.Logs --version x.x.x`

### Configuration

The configuration of all the mechanisms of this package are done through the `appsettings.json` of the application, in the section Logging as complete complete below

```json
"Logging": {
    "LogLevel": {
      [--- LogLevel default microsoft configuration ---]
    },
    "Elastic": {
      "Enable": true,
      "Uri": "http://localhost:9200",
      "DefaultIndexName": "my-index-name",
      "IgnoreCategories": [
        "Microsoft.Hosting.Lifetime"
      ]
    },
    "RequestResponseMiddleware": {
      "LogRequest": true,
      "LogResponse": true,
      "SecurityActions": [
        {
          "Method": "POST",
          "Path": "/api/auth"
        }
      ],
      "IgnoreActions": [
        {
          "Method": "GET",
          "Path": "/healthcheck"
        },
        {
          "Method": "GET",
          "Path": "/swagger"
        }
      ]
    }
  }
```

##### `Elastic` Section configuration

- `Enable` => Indicates whether the log provider is enabled or disabled (Default=true)
- `Uri` => Elastic Service url running
- `DefaultIndxName` => Name of the document index to save the logs
- `IgnoreCategories` => List of categories to ignore in the log record.

##### `RequestResponseMiddleware` Section configuration
- `LogRequest` => Enables or disables request logging (disabled by default)
- `LogReponse` => Enables or disables response logging (disabled by default)
- `SecurityActions` -> List of keys Method + Action that will have the request body hidden in the log recording. Use this section to indicate points where sensitive information, such as a password for example, is being trafficked.
- `IgnoreActions`` -> Key list Method + Action that will be ignored by logging middleware. Use this section to indicate points that should not be logged due to the low relevance of the information, for example: healthcheckes, swagger, favicon, etc.

### Implementation in your application

In the `program.cs` file, add the calls to the desired modules to the `ConfigureLogging` section, according to the model below::

```cs
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RSoft.Logs.Extensions;

namespace MyApplication.Web.Api
{
	public class Program
    {
		public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
		
		public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsoleLogger();
                    logging.AddElasticLogger();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
```

In the `Startup.cs` file add the following codes: .

Import the namespaces:

```cs
using RSoft.Logs.Extensions;
using RSoft.Logs.Middleware;
```

In `ConfigureServices` method add the line:

```cs
services.AddMiddlewareLoggingOption(Configuration);
```

In `Configure` method add the line:

```cs
app.UseMiddleware<RequestResponseLogging<Startup>>();
```

That's all you need.

##### THANK'S
