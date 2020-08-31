using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace RSoft.Logs.Options
{

    internal class LoggerOptionsSetup : ConfigureFromConfigurationOptions<LoggerOptions>
    {

        private readonly IConfiguration _configuration;

        public LoggerOptionsSetup(ILoggerProviderConfiguration<ILoggerProvider> providerConfiguration, IConfiguration configuration) : base(providerConfiguration.Configuration)
        {
            _configuration = configuration;
        }

        public override void Configure(LoggerOptions options)
        {
            base.Configure(options);
            
            options.Elastic = new ElasticOptions();
            _configuration.GetSection("Logging:Elastic").Bind(options.Elastic);

        }

    }

}
