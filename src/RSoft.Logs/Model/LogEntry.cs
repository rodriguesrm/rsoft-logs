﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;

namespace RSoft.Logs.Model
{

    /// <summary>
    /// Log entry data model
    /// </summary>
    internal class LogEntry
    {

        /// <summary>
        /// Utc timestamp
        /// </summary>
        public DateTime Timestamp => DateTime.UtcNow;

        /// <summary>
        /// Operating system user running the application or service
        /// </summary>
        public string SystemUser { get; private set; }

        /// <summary>
        /// Authenticated user information on the system
        /// </summary>
        public ApplicationUserInfo ApplicationUser { get; set; }

        /// <summary>
        /// Host machine name
        /// </summary>
        public string HostName => Dns.GetHostName();
        
        /// <summary>
        /// Category name
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Log level
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Log text message
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// Exception information data
        /// </summary>
        public LogExceptionInfo Exception { get; set; }
        
        /// <summary>
        /// Log event id
        /// </summary>
        public EventId EventId { get; set; }
        
        /// <summary>
        /// Log state object
        /// </summary>
        public object State { get; set; }
        
        /// <summary>
        /// Log state text
        /// </summary>
        public string StateText { get; set; }
        
        /// <summary>
        /// Log statate dictionary
        /// </summary>
        public IDictionary<string, object> StateProperties { get; set; }

        /// <summary>
        /// Log scope information list
        /// </summary>
        public IList<LogScopeInfo> Scopes { get; set; } = new List<LogScopeInfo>();

    }
}
