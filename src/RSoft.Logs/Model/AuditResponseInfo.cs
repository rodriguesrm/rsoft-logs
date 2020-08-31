using System;
using System.Collections.Generic;

namespace RSoft.Logs.Model
{

    /// <summary>
    /// Audit response information data model
    /// </summary>
    internal class AuditResponseInfo
    {

        #region Constructors

        public AuditResponseInfo()
        {
            Date = DateTime.UtcNow;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Response id (TraceIdentifier)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Sesion id
        /// </summary>
        public string SessionId { get { return Id.Split(':')[0]; } }

        /// <summary>
        /// Session request number
        /// </summary>
        public string RequestNumber { get { return Id.Split(':')[1]; } }

        /// <summary>
        /// Event date/time
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Response header data
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Response status code1
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Response body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Exception data
        /// </summary>
        public LogExceptionInfo Exception { get; set; }

        #endregion

    }

}
