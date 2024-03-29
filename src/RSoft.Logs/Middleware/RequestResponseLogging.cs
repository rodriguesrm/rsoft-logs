﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RSoft.Logs.Model;
using RSoft.Logs.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RSoft.Logs.Middleware
{

    /// <summary>
    /// Middleware to capture and log requests and responses in ASP.Net applications
    /// </summary>
    public class RequestResponseLogging<TCategory>
    {

        #region Local objects/variables

        private readonly RequestDelegate _next;
        private readonly RequestResponseMiddlewareOptions _options;
        private readonly ILogger<TCategory> _logger;
        private readonly EventId _eventId = new EventId(1001, "RSoft.Logs.Middleware");

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new RequestResponseLogging instance
        /// </summary>
        /// <param name="next">Request delegate</param>
        /// <param name="logger">Logger object</param>
        /// <param name="options">Options configuration</param>
        public RequestResponseLogging(RequestDelegate next, ILogger<TCategory> logger, IOptions<RequestResponseMiddlewareOptions> options)
        {
            _next = next;
            _logger = logger;
            _options = options?.Value;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Get request body information
        /// </summary>
        /// <param name="request">Request data object</param>
        private async Task<string> GetBodyRequest(HttpRequest request)
        {

            if ((request.ContentLength ?? 0) == 0)
                return null;

            if (request.ContentType.ToLower().StartsWith("multipart/form-data"))
                return "*** MULTIPART/FORM-DATA ***";

            Stream body = request.Body;
            byte[] buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await body.ReadAsync(buffer, 0, buffer.Length);

            string result = Encoding.UTF8.GetString(buffer);

            return result;

        }

        /// <summary>
        /// Get response body information
        /// </summary>
        /// <param name="response">Response data object</param>
        private async Task<string> GetBodyResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string textResp = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return textResp;
        }

        /// <summary>
        /// Log request data
        /// </summary>
        /// <param name="context">Http context object</param>
        /// <param name="body">Request body expression</param>
        private void LogRequest(HttpContext context, string body)
        {
            if (_options.LogRequest)
            {

                body = Helpers.SecurityApplyBody(_options, context.Request.Method, context.Request.Path, body);

                AuditRequestInfo requestInfo = new AuditRequestInfo()
                {
                    Id = context.TraceIdentifier,
                    Scheme = context.Request.Scheme,
                    Headers = context.Request.Headers.ToDictionary(k => k.Key, v => v.Value.ToString()),
                    Path = context.Request.Path,
                    Method = context.Request.Method.ToUpper(),
                    Host = context.Request.Host.ToString(),
                    QueryString = context.Request.QueryString.Value,
                    Body = body,
                    ClientCertificate = context.Connection.ClientCertificate?.SerialNumber,
                    LocalIpAddress = context.Connection.LocalIpAddress.ToString(),
                    LocalPort = context.Connection.LocalPort,
                    RemoteIpAddress = context.Connection.RemoteIpAddress.ToString(),
                    RemotePort = context.Connection.RemotePort
                };

                _logger.Log(LogLevel.Information, _eventId, requestInfo, null, (i, e) => { return $"REQUEST: {requestInfo.Method} {requestInfo.RawUrl}"; });
            }
        }

        /// <summary>
        /// Log response data
        /// </summary>
        /// <param name="context">Http context object</param>
        /// <param name="body"></param>
        /// <param name="ex">Exception data object</param>
        private void LogResponse(HttpContext context, string body, Exception ex)
        {
            if (_options.LogResponse)
            {

                AuditResponseInfo respInfo = new AuditResponseInfo()
                {
                    Id = context.TraceIdentifier,
                    Headers = context.Response.Headers.ToDictionary(k => k.Key, v => v.Value.ToString()),
                    StatusCode = context.Response.StatusCode,
                    Body = body,
                    Exception = ex
                };

                _logger.Log(ex == null ? LogLevel.Information : LogLevel.Error, _eventId, respInfo, ex, (i, e) => { return $"RESPONSE: {respInfo.StatusCode}-{(HttpStatusCode)respInfo.StatusCode}"; });

            }
        }


        #endregion

        #region Public methods


        /// <summary>
        /// Perform capture and log requests and responses
        /// </summary>
        /// <param name="ctx">Http context object instance</param>
        public async Task Invoke(HttpContext ctx)
        {

            bool logAction = !Helpers.IsIgnoreAction(_options, ctx.Request.Method, ctx.Request.Path);

            Stream streamBody = ctx.Response.Body;
            ctx.Request.EnableBuffering();

            // Catch request
            string body = await GetBodyRequest(ctx.Request);
            ctx.Request.Body.Position = 0;

            // Log request
            if (logAction)
                LogRequest(ctx, body);

            try
            {

                ctx.Response.Body = new MemoryStream();

                await _next.Invoke(ctx);

                string responseText = null;
                if (ctx.Response.StatusCode != StatusCodes.Status204NoContent)
                {
                    if (ctx.Response.ContentType == null)
                    {
                        ctx.Response.Body = new MemoryStream();
                    }
                    else
                    {
                        if (ctx.Response.ContentType.StartsWith("application/json") || ctx.Response.ContentType.StartsWith("text/plain"))
                        {
                            responseText = await GetBodyResponse(ctx.Response);
                            if (!string.IsNullOrWhiteSpace(responseText))
                            {
                                byte[] buffer = Encoding.UTF8.GetBytes(responseText);
                                await streamBody.WriteAsync(buffer, 0, buffer.Length);
                            }
                        }
                        else
                        {
                            ctx.Response.Body.Position = 0;
                            await ctx.Response.Body.CopyToAsync(streamBody);
                            responseText = "*** BINARY CONTENT ***";
                        }
                    }

                }

                if (logAction)
                    LogResponse(ctx, responseText, null);

            }
            catch (Exception ex)
            {
                GerericExceptionResponse exResp = new GerericExceptionResponse("500", ex.Message, ctx.TraceIdentifier);
                string response = JsonSerializer.Serialize(exResp);
                byte[] buffer = Encoding.UTF8.GetBytes(response);

                ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
                ctx.Response.ContentType = "application/json";

                await streamBody.WriteAsync(buffer, 0, buffer.Length);

                LogResponse(ctx, response, ex);

            }
            finally
            {
                ctx.Response.Body = streamBody;
            }

        }

        #endregion

    }

}
