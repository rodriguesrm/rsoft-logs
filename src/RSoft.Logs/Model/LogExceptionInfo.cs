using System;

namespace RSoft.Logs.Model
{

    /// <summary>
    /// Exception info data
    /// </summary>
    internal class LogExceptionInfo
    {

        /// <summary>
        /// Create a new object instance
        /// </summary>
        /// <param name="exception">Exception object instance</param>
        public LogExceptionInfo(Exception exception)
        {
            HResult = exception.HResult;
            Message = exception.Message;
            StackTrace = exception.StackTrace;
            Source = exception.Source;
            Type = exception.GetType().ToString();

            if (exception.InnerException != null)
                InnerException = new LogExceptionInfo(exception.InnerException);
        }

        /// <summary>
        /// Gets HRESULT, a coded numerical value that is assigned to a specific exception.
        /// </summary>
        public int HResult { get; private set; }

        /// <summary>
        /// Exception type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public virtual string Message { get; private set; }

        /// <summary>
        /// Gets a string representation of the immediate frames on the call stack.
        /// </summary>
        public virtual string StackTrace { get; private set; }

        /// <summary>
        /// Gets the name of the application or the object that causes the error.
        /// </summary>
        public virtual string Source { get; private set; }

        /// <summary>
        /// Gets the System.Exception instance that caused the current exception.
        /// </summary>
        public LogExceptionInfo InnerException { get; private set; }

    }
}
