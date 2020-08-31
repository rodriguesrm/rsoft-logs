using System.Collections.Generic;

namespace RSoft.Logs.Model
{

    /// <summary>
    /// Log scope info data
    /// </summary>
    internal class LogScopeInfo
    {

        /// <summary>
        /// Scope text (name or identification)
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Scope properties dictionary
        /// </summary>
        public IDictionary<string, object> Properties { get; set; }

    }
}
