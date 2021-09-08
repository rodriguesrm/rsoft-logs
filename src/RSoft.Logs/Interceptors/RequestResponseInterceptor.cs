using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using RSoft.Logs.Options;
using System;
using System.Threading.Tasks;
using RSoft.Logs.Model;
using Microsoft.Extensions.Options;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace RSoft.Logs.Interceptors
{

    /// <summary>
    /// Request logger gRPC interceptor
    /// </summary>
    public class RequestResponseInterceptor<TCategory> : Interceptor
    {

        #region Local objects/variables

        private readonly RequestResponseMiddlewareOptions _options;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<TCategory> _logger;
        private readonly EventId _eventId = new EventId(1001, "RSoft.Logs.Interceptor");
        private readonly string _traceId;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new RequestResponseInterceptor instance
        /// </summary>
        /// <param name="options">Options configuration</param>
        /// <param name="logger">Logger object</param>
        /// <param name="accessor">Http context accessor</param>
        public RequestResponseInterceptor(IOptions<RequestResponseMiddlewareOptions> options, ILogger<TCategory> logger, IHttpContextAccessor accessor) : base()
        {
            _options = options?.Value;
            _logger = logger;
            _accessor = accessor;
            _traceId = _accessor.HttpContext.TraceIdentifier;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Log request data
        /// </summary>
        /// <param name="context">Server call context</param>
        /// <param name="body">Request body expression</param>
        private void LogRequest(ServerCallContext context, string body)
        {
            if (_options.LogRequest)
            {

                body = Helpers.SecurityApplyBody(_options, "RPC", context.Method, body);

                AuditRequestInfo requestInfo = new AuditRequestInfo()
                {
                    Id = _traceId,
                    Scheme = _accessor.HttpContext.Request.Scheme,
                    Headers = context.RequestHeaders.ToDictionary(k => k.Key, v => v.Value),
                    Path = context.Method,
                    Method = "RPC",
                    Host = context.Host,
                    QueryString = _accessor.HttpContext.Request.QueryString.Value,
                    Body = body,
                    ClientCertificate = _accessor.HttpContext.Connection.ClientCertificate?.SerialNumber,
                    LocalIpAddress = _accessor.HttpContext.Connection.LocalIpAddress.ToString(),
                    LocalPort = _accessor.HttpContext.Connection.LocalPort,
                    RemoteIpAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                    RemotePort = _accessor.HttpContext.Connection.RemotePort
                };

                _logger.Log(LogLevel.Information, _eventId, requestInfo, null, (i, e) => { return $"REQUEST: {requestInfo.Method} {requestInfo.RawUrl}"; });
            }
        }

        /// <summary>
        /// Log response data
        /// </summary>
        /// <param name="context">Server call context</param>
        /// <param name="body">Resposne body expression</param>
        /// <param name="ex">Exception data object</param>
        private void LogResponse(ServerCallContext context, string body, Exception ex)
        {
            if (_options.LogResponse)
            {

                AuditGrpcResponseInfo respInfo = new AuditGrpcResponseInfo()
                {
                    Id = _traceId,
                    Headers = context.ResponseTrailers.ToDictionary(k => k.Key, v => v.Value.ToString()),
                    StatusCode = ex == null ? (int)context.Status.StatusCode : (int)StatusCode.Internal,
                    Body = body,
                    Exception = ex
                };

                _logger.Log(ex == null ? LogLevel.Information : LogLevel.Error, _eventId, respInfo, ex, (i, e) =>
                    {
                        if (ex == null)
                            return $"RESPONSE: {respInfo.StatusCode}:{(StatusCode)respInfo.StatusCode}";
                        else
                            return $"RESPONSE: {respInfo.StatusCode}:{(StatusCode)respInfo.StatusCode} - {ex.Message}";
                    });

            }
        }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {

            bool logAction = !Helpers.IsIgnoreAction(_options, "RPC", context.Method);

            //Catch request
            string body = request.ToString();

            // Log request
            if (logAction)
                LogRequest(context, body);


            string responseText = null;
            try
            {
                TResponse response = await base.UnaryServerHandler(request, context, continuation);
                if (response != null)
                    responseText = response.ToString();
                if (logAction)
                    LogResponse(context, responseText, null);
                return response;
            }
            catch (Exception ex)
            {
                LogResponse(context, responseText, ex.GetBaseException());
                throw new RpcException(new Status(StatusCode.Internal, ex.GetBaseException().Message), ex.GetBaseException().Message);
            }
        }

        #endregion

    }

}
