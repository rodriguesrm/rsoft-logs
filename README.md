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
- Microsoft.Extensions.Http (>= 3.1.7)
- Microsoft.Extensions.Logging (>= 3.1.7)
- Microsoft.Extensions.Logging.Abstractions (>= 3.1.7)
- Microsoft.Extensions.Logging.Configuration (>= 3.1.7)
- System.Configuration.ConfigurationManager (>= 4.7.0)
- _System.Text.Json (>= 5.0.0-rc.1.20451.14)¹_
- 
**¹ WARNING:** RSoft.Logs uses release candidate version of the System.Text.Json package. This version is not yet stable and therefore the RSoft.Logs package will maintain the same condition until the final version of this version is released.

### Installaction

- PackageManage: ```Install-Package RSoft.Logs -Version 1.0.0-preview200904.1```
- .NET Cli: ```dotnet add package RSoft.Logs --version 1.0.0-preview200904.1```
- Package Reference: ```<PackageReference Include="RSoft.Logs" Version="1.0.0-preview200904.1" />```
- Paket Cli: ```paket add RSoft.Logs --version 1.0.0-preview200904.1```

### Configuration

The configuration of all the mechanisms of this package are done through the ```appsettings.json``` of the application, in the section Logging as complete complete below

```
"Logging": {
    "LogLevel": {
      [--- LogLevel default microsoft configuration ---]
    },
    "Elastic": {
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
      ]
    }
  }
```

##### ```Elastic``` Section configuration

- ```Uri``` => Elastic Service url running
- ```DefaultIndxName``` => Name of the document index to save the logs
- ```IgnoreCategoris``` => List of categories to ignore in the log record.

##### ```RequestResponseMiddleware``` Section configuration
- ```LogRequest``` => Enables or disables request logging (disabled by default)
- ```LogReponse``` => Enables or disables response logging (disabled by default)
- ```SecurityActions``` -> List of keys Method + Action that will have the request body hidden in the log recording. Use this section to indicate points where sensitive information, such as a password for example, is being trafficked.

### Implementation in your application

In the ```program.cs``` file, add the calls to the desired modules to the ```ConfigureLogging``` section, according to the model below::

```
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

In the ```Startup.cs``` file add the following codes: .

Import the namespaces:

```
using RSoft.Logs.Extensions;
using RSoft.Logs.Middleware;
```

In ```ConfigureServices``` method add the line:

```
services.AddMiddlewareLoggingOption(Configuration);
```

In ```Configure``` method add the line:

```
app.UseMiddleware<RequestResponseLogging<Startup>>();
```

That's all you need.

##### THANK'S
