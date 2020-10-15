# RSoft.Logs
Logging mechanism for elastic (direct mode) and terminal console

RSoft.Logs is a provider of logging mechanisms based on the standard .Net Core ILogger interface. The available resources are:

  - Elastic logger provider
  - Console logger provider
  - Middlware API Request/Response logger

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

##### Documentation

Online documentation can be see [here](https://github.com/rodriguesrm/rsoft-logs/blob/release/1.0.0-rc/README.md)
