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
        /// <param name="tradeId">Operation trade id</param>
        public GerericExceptionResponse(string code, string message, string tradeId)
        {
            Code = code;
            Message = message;
            TradeId = tradeId;
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
        /// Operation trade id
        /// </summary>
        public string TradeId { get; set; }

    }

}
