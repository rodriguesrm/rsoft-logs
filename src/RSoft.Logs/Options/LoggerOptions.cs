using Microsoft.Extensions.Logging;

namespace RSoft.Logs.Options
{

    /// <summary>
    /// Logger option model
    /// </summary>
    public class LoggerOptions
    {

        /// <summary>
        /// Create a new logger options instance
        /// </summary>
        public LoggerOptions() { }

        /// <summary>
        /// Logging severity levels
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        public ElasticOptions Elastic { get; set; }

    }

}
