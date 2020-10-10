using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace RSoft.Logs.Options
{

    /// <summary>
    /// Logger options setup
    /// </summary>
    internal class LoggerOptionsSetup : ConfigureFromConfigurationOptions<LoggerOptions>
    {

        #region Local objects/variables

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new logger options setup instance
        /// </summary>
        /// <param name="providerConfiguration">Provider configuration object</param>
        /// <param name="configuration">Configuration object</param>
        public LoggerOptionsSetup(ILoggerProviderConfiguration<ILoggerProvider> providerConfiguration, IConfiguration configuration) : base(providerConfiguration.Configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        public override void Configure(LoggerOptions options)
        {
            base.Configure(options);
            
            options.Elastic = new ElasticOptions();
            _configuration.GetSection("Logging:Elastic").Bind(options.Elastic);

        }

        #endregion

    }

}
