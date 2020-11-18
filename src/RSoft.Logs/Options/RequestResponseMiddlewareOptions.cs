using System.Collections.Generic;

namespace RSoft.Logs.Options
{

    /// <summary>
    /// Request and response log middleware options
    /// </summary>
    public class RequestResponseMiddlewareOptions
    {

        /// <summary>
        /// Enables or disables the request log
        /// </summary>
        public bool LogRequest { get; set; }

        /// <summary>
        /// Enables or disables the response log
        /// </summary>
        public bool LogResponse { get; set; }

        /// <summary>
        /// Sensitive information actions list (verbs and routes path)
        /// </summary>
        public IEnumerable<RequestResponseActionOptions> SecurityActions { get; set; }

        /// <summary>
        /// Ignore action list (verbs and routes path)
        /// </summary>
        public IEnumerable<RequestResponseActionOptions> IgnoreActions { get; set; }

    }

}