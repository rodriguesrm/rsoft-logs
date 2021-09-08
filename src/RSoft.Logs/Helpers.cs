using RSoft.Logs.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RSoft.Logs
{

    /// <summary>
    /// Provides helpers methods
    /// </summary>
    internal static class Helpers
    {

        /// <summary>
        /// Checks whether the action is in the list of actions to be ignored
        /// </summary>
        /// <param name="options">Options configuration</param>
        /// <param name="httpVerb">Http verb action (method)</param>
        /// <param name="path">Route path</param>
        internal static bool IsIgnoreAction(RequestResponseMiddlewareOptions options, string httpVerb, string path)
        {
            IEnumerable<string> ignoredList = new List<string>();

            if (options?.IgnoreActions != null)
                ignoredList = options?
                    .IgnoreActions
                    .Select(s => $"{s.Method.ToUpper()}:{s.Path.ToLower()}")
                    .ToList();

            string action = $"{httpVerb.ToUpper()}:{path.ToLower()}";
            if (ignoredList.Contains(action))
                return true;

            return false;
        }

        /// <summary>
        /// Apply security analysis to preserve sensitive information
        /// </summary>
        /// <param name="options">Options configuration</param>
        /// <param name="httpVerb">Http verb action (method)</param>
        /// <param name="path">Route path</param>
        /// <param name="body">Request body</param>
        internal static string SecurityApplyBody(RequestResponseMiddlewareOptions options, string httpVerb, string path, string body)
        {

            IEnumerable<string> secList = new List<string>();

            if (options?.SecurityActions != null)
                secList = options?
                    .SecurityActions
                    .Select(s => $"{s.Method.ToUpper()}:{s.Path.ToLower()}")
                    .ToList();

            string action = $"{httpVerb.ToUpper()}:{path.ToLower()}";
            if (secList.Contains(action))
                body = "*** OMITTED FOR SECURITY ***";

            return body;
        }

    }
}
