﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RSoft.Logs.Extensions;
using RSoft.Logs.Model;
using RSoft.Logs.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RSoft.Logs.Providers
{

    /// <summary>
    /// Log provider for Seq
    /// </summary>
    internal class SeqLoggerProvider : LoggerProvider
    {

        #region Local objects/variables

        private readonly HttpClient _client;
        private bool configIsOk;
        private readonly JsonSerializerOptions _serializerOptions;

        #endregion

        #region Construtors

        /// <summary>
        /// Create a new seq logger provider instance
        /// </summary>
        /// <param name="options">Options monitor to logger configuration</param>
        /// <param name="accessor">Http context acessor object</param>
        /// <param name="factory">Http client factory object</param>
        public SeqLoggerProvider(IOptionsMonitor<LoggerOptions> options, IHttpContextAccessor accessor, IHttpClientFactory factory) : this(options.CurrentValue, accessor, factory)
        {
            _settingsChangeToken = options.OnChange(opt =>
            {
                Settings = opt;
            });
        }

        /// <summary>
        /// Create a new seq logger provider instance
        /// </summary>
        /// <param name="settings">Logger options config settings</param>
        /// <param name="accessor">Http context acessor object</param>
        /// <param name="factory">Http client factory object</param>
        public SeqLoggerProvider(LoggerOptions settings, IHttpContextAccessor accessor, IHttpClientFactory factory) : base(accessor)
        {

            Settings = settings;

            configIsOk = settings.Elastic.Enable;

            if (configIsOk)
            {

                _client = factory.CreateClient();

                if (string.IsNullOrWhiteSpace(settings.Seq.Uri))
                {
                    configIsOk = false;
                    Terminal.Print(GetType().ToString(), LogLevel.Warning, $"Seq 'Uri' configuration not found or invalid.\n{Terminal.Margin}Logger not work");
                }
                if (string.IsNullOrWhiteSpace(settings.Seq.ApiKey))
                {
                    Terminal.Print(GetType().ToString(), LogLevel.Warning, $"Seq 'ApiKey' configuration not found or invalid.\n{Terminal.Margin}Logger maybe not work");
                }

                if (configIsOk)
                {
                    _client.BaseAddress = new Uri(Settings.Elastic.Uri);
                    if (!string.IsNullOrWhiteSpace(Settings.Seq.ApiKey))
                        _client.DefaultRequestHeaders.Add("X-Seq-ApiKey", Settings.Seq.ApiKey);
                    _serializerOptions = new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = false,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        ReferenceHandler = ReferenceHandler.Preserve
                    };
                }

            }

        }

        #endregion

        #region Properties

        /// <summary>
        /// Logger options configuration
        /// </summary>
        internal LoggerOptions Settings { get; private set; }

        #endregion

        #region Local methods

        /// <summary>
        /// Create Seq message request
        /// </summary>
        /// <param name="info">Log entry data object</param>
        private string CreateMessageRequest(LogEntry info)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();

            // @t  > Timestamp
            dic.Add("@t", info.Timestamp.ToString("u"));

            // @m  > Message (@mt not used)
            dic.Add("@m", info.Text);

            // @l  > Level
            dic.Add("@l", info.Level.ToString());

            // @x  > Exception
            if (info.Exception != null)
            {
                dic.Add("Exception_HResult", info.Exception.HResult.ToString());
                dic.Add("Exception_Source", info.Exception.Source);
                dic.Add("Exception_Type", info.Exception.GetType().FullName);
                dic.Add("@x", info.Exception.StackTrace.AsJson());
            }

            // @i  > EventId
            //dic.Add("@i", $"{info.EventId.Id}-{info.EventId.Name ?? "N/A"}");
            dic.Add("@i", info.EventId.Id.ToString());

            // @r  > Renderings

            dic.Add("SystemUser", info.SystemUser);
            
            if (info.ApplicationUser != null)
                dic.Add("ApplicationUser", $"{info.ApplicationUser.User}=>{info.ApplicationUser.Token}");
            
            dic.Add("HostName", info.HostName);
            dic.Add("Category", info.Category);


            if (info.Scopes?.Count > 0)
            {
                foreach (var scope in info.Scopes)
                {
                    dic.Add(scope.Key, scope.Value);
                }
            }

            string messageResult = "{" + string.Join(",", dic.Select(pair =>
            {
                string result = null;
                if (string.IsNullOrWhiteSpace(pair.Value) || pair.Value == "null")
                    result = $"\"{pair.Key}\" : null";
                else
                    result = $"\"{pair.Key}\" : \"{pair.Value}\"";
                return result;
            }).ToArray()) + "}";

            return messageResult;
        }

        ///<inheritdoc/>
        protected override void WriteLogAction(LogEntry info)
        {
            if (configIsOk)
            {

                if (!Settings.Elastic.IgnoreCategories.Contains(info.Category))
                {

                    try
                    {
                        //string message = JsonSerializer.Serialize(CreateMessageRequest(info), _serializerOptions);
                        string message = CreateMessageRequest(info);
                        StringContent content = new StringContent(message, Encoding.UTF8, "application/json");
                        HttpResponseMessage resp = _client.PostAsync($"{Settings.Seq.Uri}/api/events/raw?clef", content).GetAwaiter().GetResult();
                        if (!resp.IsSuccessStatusCode)
                        {
                            string responseBody = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                            Terminal.Print(GetType().ToString(), LogLevel.Error, responseBody);
                        }
                    }
                    catch (Exception ex)
                    {
                        Terminal.Print(GetType().ToString(), LogLevel.Error, ex.Message, ex);
                    }

                }

            }
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public override bool IsEnabled(LogLevel logLevel)
        {

            bool result =
                logLevel != LogLevel.None
                && Settings.LogLevel != LogLevel.None
                && Convert.ToInt32(logLevel) >= Convert.ToInt32(Settings.LogLevel);

            return result;

        }

        #endregion


    }

}
