namespace RSoft.Logs.Options
{

    /// <summary>
    /// Sensitive information actions list (verbs and routes path) options model
    /// </summary>
    public class RequestResponseSecurityActionOptions
    {

        /// <summary>
        /// Http verb method
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Route path
        /// </summary>
        public string Path { get; set; }

    }
}
