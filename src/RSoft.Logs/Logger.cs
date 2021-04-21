using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RSoft.Logs.Extensions;
using RSoft.Logs.Model;
using RSoft.Logs.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

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
                    ApplicationUser = GetSignedUserInformation()
                };

                if (state is string)
                {
                    info.Scopes.Add("Text", state.ToString());
                }
                else if (state is AuditRequestInfo auditRequest)
                {

                    info.Text = formatter(state, exception);
                    if (auditRequest.Body != null && info.Text.Contains(auditRequest.Body))
                        auditRequest.Body = null;

                    info.Scopes.Add("Date", auditRequest.Date.ToString("u"));
                    info.Scopes.Add("Scheme", auditRequest.Scheme);
                    
                    if (auditRequest.Headers?.Count > 0)
                    {
                        foreach (var header in auditRequest.Headers)
                        {
                            info.Scopes.Add($"Headers.{header.Key}", header.Value.ToEscapedString());
                        }
                    }

                    info.Scopes.Add("Method", auditRequest.Method);
                    info.Scopes.Add("Host", auditRequest.Host);
                    if (!string.IsNullOrWhiteSpace(auditRequest.QueryString))
                        info.Scopes.Add("QueryString", auditRequest.QueryString);
                    info.Scopes.Add("ClientCertificate", auditRequest.ClientCertificate);
                    info.Scopes.Add("LocalIpAddress", auditRequest.LocalIpAddress);
                    info.Scopes.Add("LocalPort", auditRequest.LocalPort.ToString());
                    info.Scopes.Add("RemoteIpAddress", auditRequest.RemoteIpAddress);
                    info.Scopes.Add("RemotePort", auditRequest.RemotePort.ToString());
                    info.Scopes.Add("RawUrl", auditRequest.RawUrl);
                    
                    if (!string.IsNullOrWhiteSpace(auditRequest.Body))
                        info.Scopes.Add("Body", auditRequest.Body.AsJson());

                }
                else if (state is AuditResponseInfo auditResponse)
                {

                    info.Text = formatter(state, exception);
                    if (auditResponse.Body != null && info.Text.Contains(auditResponse.Body))
                        auditResponse.Body = null;

                    info.Scopes.Add("StatusCode", auditResponse.StatusCode.ToString());
                    info.Scopes.Add("Date", auditResponse.Date.ToString("u"));

                    if (auditResponse.Headers?.Count > 0)
                    {
                        foreach (var header in auditResponse.Headers)
                        {
                            info.Scopes.Add($"Headers.{header.Key}", header.Value.ToEscapedString());
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(auditResponse.Body))
                        info.Scopes.Add("Body", auditResponse.Body.AsJson());

            }
                else if (state is IEnumerable<KeyValuePair<string, object>> properties)
                {

                    foreach (KeyValuePair<string, object> item in properties)
                    {
                        if (item.Key != "{OriginalFormat}")
                        {
                            if (item.Value is string)
                                info.Scopes[item.Key] = item.Value.ToString();
                            else
                                info.Scopes[item.Key] = item.Value.AsJson();
                        }
                    }
                }

                if (_provider.ScopeProvider != null)
                {

                    _provider.ScopeProvider.ForEachScope((value, loggingProps) =>
                    {

                        if (value is string)
                        {
                            info.Scopes["Text"] = value.ToString();

                        }
                        else if (value is IEnumerable<KeyValuePair<string, object>> props)
                        {
                            foreach (KeyValuePair<string, object> pair in props)
                            {
                                string pairValue = null;
                                if (pair.Value is string)
                                    pairValue = pair.Value.ToString();
                                else
                                    pairValue = pair.Value.AsJson();

                                if (!string.IsNullOrWhiteSpace(pairValue))
                                    info.Scopes[pair.Key] = pairValue;
                            }
                        }

                        info.Scopes["ApplicationName"] = Assembly.GetEntryAssembly().GetName().Name;
                        info.Scopes["ApplicationVersion"] = Assembly.GetEntryAssembly().GetName().Version.ToString();
                        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT ");
                        info.Scopes["Environment"] = environment;


                    }, state);

                }

                _provider.WriteLog(info);

            }

        }

        #endregion

    }
}
