namespace RSoft.Logs.Options
{

    /// <summary>
    /// Seq options configuration
    /// </summary>
    public class SeqOptions
    {

        /// <summary>
        /// Indicates whether the log provider is enabled or disabled (Default=true)
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// Uri seq server
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// API-Key credential access
        /// </summary>
        public string ApiKey { get; set; }

    }
}
