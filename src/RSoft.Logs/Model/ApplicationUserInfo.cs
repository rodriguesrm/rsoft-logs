namespace RSoft.Logs.Model
{

    /// <summary>
    /// Application logged user information
    /// </summary>
    internal class ApplicationUserInfo
    {

        /// <summary>
        /// Create a new model instance
        /// </summary>
        /// <param name="user">User login</param>
        /// <param name="token">Authorization token</param>
        public ApplicationUserInfo(string user, string token)
        {
            User = user;
            Token = token;
        }

        /// <summary>
        /// User login or name
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Authorization token
        /// </summary>
        public string Token { get; set; }

    }
}
