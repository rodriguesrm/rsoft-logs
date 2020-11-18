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
* (CRITICAL BUG) => Responses is compromissed when match IgnoreActions

#### Version 1.0.0-rc1.6
* Fix critical bug responses is null wheb match IgnoreActions

#### Version 1.0.0-rc1.7
* Create flag to enable or disable Elastic Logging

#### Version 1.0.0
* Upgrade packages dependencies to released versions 5.0.0

##### Documentation

Online documentation can be see [here](https://github.com/rodriguesrm/rsoft-logs/blob/master/README.md)
