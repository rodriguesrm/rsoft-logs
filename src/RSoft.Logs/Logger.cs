using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RSoft.Logs.Model;
using RSoft.Logs.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RSoft.Logs
{
    internal class Logger : ILogger
    {

        #region Local objects/variables

        private readonly LoggerProvider _provider;
        private readonly string _category;
        private readonly IHttpContextAccessor _accessor;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of Logger
        /// </summary>
        /// <param name="provider">Log provider object instance</param>
        /// <param name="category">Log category name/identification</param>
        /// <param name="accessor">Http context accessor object</param>
        public Logger(LoggerProvider provider, string category, IHttpContextAccessor accessor)
        {
            _provider = provider;
            _category = category;
            _accessor = accessor;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Get signed user information data (login and token)
        /// </summary>
        private ApplicationUserInfo GetSignedUserInformation()
        {

            ApplicationUserInfo result = null;

            try
            {
                string user = _accessor?.HttpContext?.User?.Claims?.FirstOrDefault(f => f.Type.EndsWith("nameidentifier"))?.Value;
                string token = _accessor?.HttpContext?.Request?.Headers?.FirstOrDefault(f => f.Key == "Authorization").Value.FirstOrDefault(x => x.ToLower().StartsWith("bearer"));

                if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(token))
                    result =  new ApplicationUserInfo(user, token);

            }
            catch { /* Not to do. Fire and forget */ }

            return result;

        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public IDisposable BeginScope<TState>(TState state)
            => _provider.ScopeProvider.Push(state);

        ///<inheritdoc/>
        public bool IsEnabled(LogLevel logLevel)
            => _provider.IsEnabled(logLevel);

        ///<inheritdoc/>
        public void Log<TState>
        (
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {

            if ((this as ILogger).IsEnabled(logLevel))
            {

                LogEntry info = new LogEntry()
                {
                    Category = _category,
                    Level = logLevel,
                    Text = exception?.Message ?? state.ToString(),
                    Exception = exception != null ? new LogExceptionInfo(exception) : null,
                    EventId = eventId,
                    State = state,
                    ApplicationUser = GetSignedUserInformation()
                };

                if (state is string)
                {
                    info.StateText = state.ToString();
                }
                else if (state is AuditRequestInfo auditRequest)
                {

                    info.Text = formatter(state, exception);
                    if (auditRequest.Body != null && info.Text.Contains(auditRequest.Body))
                        auditRequest.Body = null;
                    LogScopeInfo scope = new LogScopeInfo
                    {
                        Text = $"{auditRequest.Id} | {auditRequest.Method} {auditRequest.RawUrl}"
                    };

                    IDictionary<string, object> dicScope = new Dictionary<string, object>
                    {
                        { "Id", auditRequest.Id },
                        { "Date", auditRequest.Date },
                        { "Scheme", auditRequest.Scheme },
                        { "Headers", auditRequest.Headers },
                        { "Path", auditRequest.Path },
                        { "Method", auditRequest.Method },
                        { "Host", auditRequest.Host },
                        { "QueryString", auditRequest.QueryString },
                        { "ClientCertificate", auditRequest.ClientCertificate },
                        { "LocalIpAddress", auditRequest.LocalIpAddress },
                        { "LocalPort", auditRequest.LocalPort },
                        { "RemoteIpAddress", auditRequest.RemoteIpAddress },
                        { "RemotePort", auditRequest.RemotePort },
                        { "RawUrl", auditRequest.RawUrl },
                        { "RequestNumber", auditRequest.RequestNumber },
                        { "SessionId", auditRequest.SessionId }
                    };

                    info.Scopes.Add(scope);

                }
                else if (state is AuditResponseInfo auditResponse)
                {
                    
                    info.Text = formatter(state, exception);
                    if (auditResponse.Body != null && info.Text.Contains(auditResponse.Body))
                        auditResponse.Body = null;
                    LogScopeInfo scope = new LogScopeInfo
                    {
                        Text = $"{auditResponse.Id} | {auditResponse.StatusCode}-{(HttpStatusCode)auditResponse.StatusCode}"
                    };

                    IDictionary<string, object> dicScope = new Dictionary<string, object>
                    {
                        { "Id", auditResponse.Id },
                        { "Date", auditResponse.Date },
                        { "Headers", auditResponse.Headers },
                        { "StatusCode", auditResponse.StatusCode },
                        { "Exception", auditResponse.Exception },
                        { "RequestNumber", auditResponse.RequestNumber },
                        { "SessionId", auditResponse.SessionId }
                    };

                    info.Scopes.Add(scope);

                }
                else if (state is IEnumerable<KeyValuePair<string, object>> properties)
                {

                    info.StateProperties = new Dictionary<string, object>();
                    foreach (KeyValuePair<string, object> item in properties)
                    {
                        if (item.Key != "{OriginalFormat}")
                            info.StateProperties[item.Key] = item.Value;
                    }
                }

                if (_provider.ScopeProvider != null)
                {

                    _provider.ScopeProvider.ForEachScope((value, loggingProps) =>
                    {

                        LogScopeInfo scope = new LogScopeInfo();

                        if (value is string)
                        {
                            scope.Text = value.ToString();
                        }
                        else if (value is IEnumerable<KeyValuePair<string, object>> props)
                        {
                            if (scope.Properties == null)
                                scope.Properties = new Dictionary<string, object>();
                            foreach (KeyValuePair<string, object> pair in props)
                            {
                                scope.Properties[pair.Key] = pair.Value;
                            }
                        }

                        info.Scopes.Add(scope);


                    }, state);

                }

                _provider.WriteLog(info);

            }

        }

        #endregion

    }
}
