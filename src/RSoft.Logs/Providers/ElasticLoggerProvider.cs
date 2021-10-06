using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RSoft.Logs.Model;
using RSoft.Logs.Options;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RSoft.Logs.Providers
{

    /// <summary>
    /// Log provider for elastic
    /// </summary>
    internal class ElasticLoggerProvider : LoggerProvider
    {

        #region Local objects/variables

        private readonly HttpClient _client;
        private readonly bool _configIsOk;
        private string _indexName;
        private readonly JsonSerializerOptions _serializerOptions;

        #endregion

        #region Construtors

        /// <summary>
        /// Create a new elastic logger provider instance
        /// </summary>
        /// <param name="options">Options monitor to logger configuration</param>
        /// <param name="accessor">Http context acessor object</param>
        /// <param name="factory">Http client factory object</param>
        public ElasticLoggerProvider(IOptionsMonitor<LoggerOptions> options, IHttpContextAccessor accessor, IHttpClientFactory factory) : this(options.CurrentValue, accessor, factory)
        {
            _settingsChangeToken = options.OnChange(opt =>
            {
                Settings = opt;
            });
        }

        /// <summary>
        /// Create a new elastic logger provider instance
        /// </summary>
        /// <param name="settings">Logger options config settings</param>
        /// <param name="accessor">Http context acessor object</param>
        /// <param name="factory">Http client factory object</param>
        public ElasticLoggerProvider(LoggerOptions settings, IHttpContextAccessor accessor, IHttpClientFactory factory) : base(accessor)
        {

            Settings = settings;

            _configIsOk = settings.Elastic.Enable;

            if (_configIsOk)
            {

                _client = factory.CreateClient();

                if (string.IsNullOrWhiteSpace(settings.Elastic.Uri))
                {
                    _configIsOk = false;
                    Terminal.Print(GetType().ToString(), LogLevel.Warning, $"Elastic 'Uri' configuration not found or invalid.\n{Terminal.Margin}Logger not work");
                }
                if (string.IsNullOrWhiteSpace(settings.Elastic.DefaultIndexName))
                {
                    _configIsOk = false;
                    Terminal.Print(GetType().ToString(), LogLevel.Warning, $"Elastic 'DefaultIndexName' configuration not found or invalid.\n{Terminal.Margin}Logger not work");
                }

                if (_configIsOk)
                {
                    _client.BaseAddress = new Uri(Settings.Elastic.Uri);
                    _indexName = Settings.Elastic.DefaultIndexName;
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

        ///<inheritdoc/>
        protected override void WriteLogAction(LogEntry info)
        {
            if (_configIsOk)
            {
                if (!Settings.Elastic.IgnoreCategories.Contains(info.Category))
                {

                    try
                    {

                        string message = JsonSerializer.Serialize(info, _serializerOptions);
                        StringContent content = new StringContent(message, Encoding.UTF8, "application/json");
                        HttpResponseMessage resp = _client.PostAsync($"{_indexName}/_doc", content).GetAwaiter().GetResult();
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
