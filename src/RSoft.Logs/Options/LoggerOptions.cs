﻿using Microsoft.Extensions.Logging;

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

        /// <summary>
        /// Elastic options parameters
        /// </summary>
        public ElasticOptions Elastic { get; set; } = new ElasticOptions();

        /// <summary>
        /// Seq options parameters
        /// </summary>
        public SeqOptions Seq { get; set; } = new SeqOptions();

    }

}
