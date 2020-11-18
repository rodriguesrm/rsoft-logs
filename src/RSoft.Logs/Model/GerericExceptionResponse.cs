namespace RSoft.Logs.Model
{

    /// <summary>
    /// Exception response generic data object
    /// </summary>
    public class GerericExceptionResponse
    {

        /// <summary>
        /// Create a new GerericExceptionResponse instance
        /// </summary>
        /// <param name="code">Exception code</param>
        /// <param name="message">Exception message</param>
        /// <param name="traceId">Operation trace id</param>
        public GerericExceptionResponse(string code, string message, string traceId)
        {
            Code = code;
            Message = message;
            TraceId = traceId;
        }

        /// <summary>
        /// Exception code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Exception message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Operation trace id
        /// </summary>
        public string TraceId { get; set; }

    }

}
