# RSoft.Logs
Logging mechanism for elastic (direct mode) and terminal console

RSoft.Logs is a provider of logging mechanisms based on the standard .Net Core ILogger interface. The available resources are:

  - Seq logger provider
  - Elastic logger provider
  - Console logger provider
  - Middlware API Request/Response logger
  - gRPC Request/Response interceptor

### Release Notes

#### Version 1.0.0-rc1.1
* Fix field name 'TradeId' => 'TraceId' in GenericExceptionResponse.

#### Version 1.0.0-rc1.2
* Fix access modifier ``RequestResponseLogging.LogResponse`` method from ``public`` to ``private``.

#### Version 1.0.0-rc1.3
* Fix print date/time in console provider to use 24h format.

#### Version 1.0.0-rc1.4
* Fix methods xml documentation.

#### Version 1.0.0-rc1.5
* Added IgnoreActions for not log in request-response middleware logger.
* Upgrade nuget packages
* (CRITICAL BUG) => Responses is compromissed when match IgnoreActions

#### Version 1.0.0-rc1.6
* Fix critical bug responses is null wheb match IgnoreActions

#### Version 1.0.0-rc1.7
* Create flag to enable or disable Elastic Logging

#### Version 1.0.0
* Upgrade packages dependencies to released versions 5.0.0

#### Version 1.1.0-rc1.0
* Added Seq logger provider

#### Version 1.1.0-rc1.1
* Update packages

#### Version 1.1.0-rc1.2
* Fix request headers capture when their content contained quotation marks

#### Version 1.1.0-rc1.3
* Fix capture ApplicationName, ApplicationVersion and Environment

#### Version 1.1.0-rc1.4
* Fix scape bar " \ " in seq log payload api

#### Version 1.1.0-rc1.5
* Show details in log message on seq error result

#### Version 1.1.0-rc1.6
* Manage string to scaped characters for Seq Logger

#### Version 1.1.0-rc1.7
* Manage MethodInfo state data

#### Version 1.1.0
* Launch/Release final 1.1.0 version

#### Version 1.2.0
* Created gRPC log request/response interceptor

#### Version 1.2.1
* Fix interceptor when cat RPC Exception, throw the same exception

#### Version 1.2.2
* Fix interceptor on get RPC Exception body and StatusCode

#### Version 1.2.3
* Fix bug top processor when application container run in docker

#### Version 1.2.4
* Fiz loop log time

##### Documentation

Online documentation can be see [here](https://github.com/rodriguesrm/rsoft-logs/blob/master/README.md)
