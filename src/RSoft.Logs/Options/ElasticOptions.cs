using System.Collections.Generic;

namespace RSoft.Logs.Options
{

    /// <summary>
    /// Elastic options configuration
    /// </summary>
    public class ElasticOptions
    {

        /// <summary>
        /// Indicates whether the log provider is enabled or disabled (Default=true)
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// Uri elastic server
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Default index name
        /// </summary>
        public string DefaultIndexName { get; set; }

        /// <summary>
        /// Categories to ignore in the log
        /// </summary>
        public IEnumerable<string> IgnoreCategories { get; set; }

    }
}
