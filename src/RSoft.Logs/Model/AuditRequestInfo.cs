using System;
using System.Collections.Generic;

namespace RSoft.Logs.Model
{

    /// <summary>
    /// Request information data
    /// </summary>
    internal class AuditRequestInfo
    {

        #region Constructors

        /// <summary>
        /// Create a new AuditRequestInfo instance
        /// </summary>
        public AuditRequestInfo()
        {
            Date = DateTime.UtcNow;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Request id (TraceIdentifier)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Sesion id
        /// </summary>
        public string SessionId { get { return Id.Split(':')[0]; } }

        /// <summary>
        /// Sequential session number
        /// </summary>
        public string RequestNumber { get { return Id.Split(':')[1]; } }

        /// <summary>
        /// Event date/time
        /// </summary>
        public DateTime Date { get; protected set; }

        /// <summary>
        /// Connection scheme (http/https/etc)
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Request header information
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Request complete url
        /// </summary>
        public string RawUrl
        {
            get
            {
                string result = $"{Scheme}://{Host}{Path}{QueryString}";
                return result;
            }
        }

        /// <summary>
        /// Request path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Request httpverb (GET, POST, PUT, DELETE, HEAD, etc)
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Requestor's host name
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Query string expression
        /// </summary>
        public string QueryString { get; set; }

        /// <summary>
        /// Request body data
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Digital certificate
        /// </summary>
        public string ClientCertificate { get; set; }

        /// <summary>
        /// Local ip address
        /// </summary>
        public string LocalIpAddress { get; set; }

        /// <summary>
        /// Local port number
        /// </summary>
        public int LocalPort { get; set; }

        /// <summary>
        /// Remote IP address
        /// </summary>
        public string RemoteIpAddress { get; set; }

        /// <summary>
        /// Remote port number
        /// </summary>
        public int RemotePort { get; set; }

        #endregion

    }

}