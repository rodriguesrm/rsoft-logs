using System.Collections.Generic;

namespace RSoft.Logs.Options
{

    /// <summary>
    /// Elastic options configuration
    /// </summary>
    public class ElasticOptions
    {

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
