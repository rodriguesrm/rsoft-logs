using Microsoft.Extensions.Logging;
using RSoft.Logs.Model;
using System;

namespace RSoft.Logs
{

    /// <summary>
    /// Provides methods for displaying formatted messages on the console
    /// </summary>
    public static class Terminal
    {

        #region Local objects/variables

        private readonly static string[] levelNames = new string[]
        {
            "TRC",
            "DBG",
            "INF",
            "WRN",
            "ERR",
            "CRITICAL"
        };

        #endregion

        #region Constrants

        /// <summary>
        /// Margin space constant
        /// </summary>
        public const string Margin = "     ";

        #endregion

        #region Local methods

        /// <summary>
        /// Write message in terminal with tab space
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="foregroundColor">Foreground color code</param>
        /// <param name="backgroundColor">Background color code</param>
        private static void WriteMessageWithTab(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Console.ResetColor();
            Console.Write(Margin);
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(message);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Print message in console terminal
        /// </summary>
        /// <param name="category">Category logger</param>
        /// <param name="level">Entry will be written on this level.</param>
        /// <param name="message">Entry message</param>
        public static void Print(string category, LogLevel level, string message)
            => Print(category, level, default, message, true, logException: null);

        /// <summary>
        /// Print message in console terminal
        /// </summary>
        /// <param name="category">Category logger</param>
        /// <param name="level">Entry will be written on this level.</param>
        /// <param name="eventId">Identifies a logging event</param>
        /// <param name="message">Entry message</param>
        public static void Print(string category, LogLevel level, EventId eventId, string message)
            => Print(category, level, eventId, message, true, logException: null);

        /// <summary>
        /// Print message in console terminal
        /// </summary>
        /// <param name="category">Category logger</param>
        /// <param name="level">Entry will be written on this level.</param>
        /// <param name="message">Entry message</param>
        /// <param name="printDate">Indicate print date/time flag</param>
        public static void Print(string category, LogLevel level, string message, bool printDate)
            => Print(category, level, default, message, printDate, logException: null);

        /// <summary>
        /// Print message in console terminal
        /// </summary>
        /// <param name="category">Category logger</param>
        /// <param name="level">Entry will be written on this level.</param>
        /// <param name="eventId">Identifies a logging event</param>
        /// <param name="message">Entry message</param>
        /// <param name="printDate">Indicate print date/time flag</param>
        public static void Print(string category, LogLevel level, EventId eventId, string message, bool printDate)
            => Print(category, level, eventId, message, printDate, logException: null);

        /// <summary>
        /// Print message in console terminal
        /// </summary>
        /// <param name="category">Category logger</param>
        /// <param name="level">Entry will be written on this level.</param>
        /// <param name="message">Entry message</param>
        /// <param name="exception">The exception related to this entry.</param>
        public static void Print(string category, LogLevel level, string message, Exception exception)
            => Print(category, level, default, message, true, new LogExceptionInfo(exception));

        /// <summary>
        /// Print message in console terminal
        /// </summary>
        /// <param name="category">Category logger</param>
        /// <param name="level">Entry will be written on this level.</param>
        /// <param name="eventId">Identifies a logging event</param>
        /// <param name="message">Entry message</param>
        /// <param name="exception">The exception related to this entry.</param>
        public static void Print(string category, LogLevel level, EventId eventId, string message, Exception exception)
            => Print(category, level, eventId, message, true, new LogExceptionInfo(exception));

        /// <summary>
        /// Print message in console terminal
        /// </summary>
        /// <param name="category">Category logger</param>
        /// <param name="level">Entry will be written on this level.</param>
        /// <param name="message">Entry message</param>
        /// <param name="logException">The exception related to this entry.</param>
        internal static void Print(string category, LogLevel level, string message, LogExceptionInfo logException)
            => Print(category, level, default, message, true, logException);

        /// <summary>
        /// Print message in console terminal
        /// </summary>
        /// <param name="category">Category logger</param>
        /// <param name="level">Entry will be written on this level.</param>
        /// <param name="message">Entry message</param>
        /// <param name="logException">The exception related to this entry.</param>
        internal static void Print(string category, LogLevel level, string message, bool printDate, LogExceptionInfo logException)
            => Print(category, level, default, message, printDate, logException);

        /// <summary>
        /// Print message in console terminal
        /// </summary>
        /// <param name="category">Category logger</param>
        /// <param name="level">Entry will be written on this level.</param>
        /// <param name="eventId">Identifies a logging event</param>
        /// <param name="message">Entry message</param>
        /// <param name="logException">The exception related to this entry.</param>
        internal static void Print(string category, LogLevel level, EventId eventId, string message, LogExceptionInfo logException)
            => Print(category, level, eventId, message, true, logException);

        /// <summary>
        /// Print message in console terminal
        /// </summary>
        /// <param name="category">Category logger</param>
        /// <param name="level">Entry will be written on this level.</param>
        /// <param name="message">Entry message</param>
        /// <param name="printDate">Indicate print date/time flag</param>
        /// <param name="exception">The exception related to this entry.</param>
        public static void Print(string category, LogLevel level, string message, bool printDate, Exception exception)
            => Print(category, level, default, message, printDate, new LogExceptionInfo(exception));

        /// <summary>
        /// Print message in console terminal
        /// </summary>
        /// <param name="category">Category logger</param>
        /// <param name="level">Entry will be written on this level.</param>
        /// <param name="eventId">Identifies a logging event</param>
        /// <param name="message">Entry message</param>
        /// <param name="printDate">Indicate print date/time flag</param>
        /// <param name="exception">The exception related to this entry.</param>
        public static void Print(string category, LogLevel level, EventId eventId, string message, bool printDate, Exception exception)
            => Print(category, level, eventId, message, printDate, new LogExceptionInfo(exception));

        /// <summary>
        /// Print message in console terminal
        /// </summary>
        /// <param name="category">Category logger</param>
        /// <param name="level">Entry will be written on this level.</param>
        /// <param name="eventId">Identifies a logging event</param>
        /// <param name="message">Entry message</param>
        /// <param name="printDate">Indicate print date/time flag</param>
        /// <param name="logException">The exception related to this entry.</param>
        internal static void Print(string category, LogLevel level, EventId eventId, string message, bool printDate, LogExceptionInfo logException)
        {

            if (level == LogLevel.None)
                return;

            // Set defaults
            Console.ResetColor();
            ConsoleColor levelForegroundColor = Console.ForegroundColor;
            ConsoleColor levelBackgroundColor = Console.BackgroundColor;
            ConsoleColor messageForegroundColor = Console.ForegroundColor;
            ConsoleColor messageBackgroundColor = Console.BackgroundColor;
            ConsoleColor errorForegroundColor = Console.ForegroundColor;
            ConsoleColor errorBackgroundColor = Console.BackgroundColor;

            // Level
            switch (level)
            {

                case LogLevel.Trace:
                    levelForegroundColor = ConsoleColor.DarkCyan;
                    break;

                case LogLevel.Debug:
                    levelForegroundColor = ConsoleColor.Gray;
                    messageForegroundColor = ConsoleColor.DarkGray;
                    break;

                case LogLevel.Information:
                    if (eventId.Id == 1001 && eventId.Name == "RSoft.Logs.Middleware")
                        messageForegroundColor = ConsoleColor.White;
                    levelForegroundColor = ConsoleColor.Green;
                    break;

                case LogLevel.Warning:
                    levelForegroundColor = ConsoleColor.DarkYellow;
                    messageForegroundColor = ConsoleColor.White;
                    break;

                case LogLevel.Error:
                    levelForegroundColor = ConsoleColor.DarkRed;
                    messageForegroundColor = ConsoleColor.Yellow;
                    errorForegroundColor = ConsoleColor.Yellow;
                    break;

                case LogLevel.Critical:
                    levelForegroundColor = ConsoleColor.Yellow;
                    levelBackgroundColor = ConsoleColor.DarkRed;
                    messageForegroundColor = ConsoleColor.DarkRed;
                    messageBackgroundColor = ConsoleColor.Yellow;
                    errorForegroundColor = ConsoleColor.DarkRed;
                    break;

            }

            Console.ForegroundColor = levelForegroundColor;
            Console.BackgroundColor = levelBackgroundColor;
            Console.Write($"{levelNames[(int)level]}");
            Console.ResetColor();
            Console.Write(": ");

            // Category / Date
            Console.ResetColor();
            if (level == LogLevel.Error || level == LogLevel.Critical)
            {
                Console.ForegroundColor = messageForegroundColor;
                Console.BackgroundColor = messageBackgroundColor;
            }
            string date = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} | ";
            if (!printDate)
                date = string.Empty;
            Console.WriteLine($"{date}{category}[{eventId.Id}]");

            // Message
            Console.ForegroundColor = messageForegroundColor;
            Console.BackgroundColor = messageBackgroundColor;
            Console.WriteLine($"{Margin}{message}");
            Console.ResetColor();

            // Exception
            if (logException != null)
            {
                WriteMessageWithTab($"ErrorMessage: {logException.Message}", errorForegroundColor, errorBackgroundColor);
                WriteMessageWithTab($"Type: {logException.Type}", errorForegroundColor, errorBackgroundColor);
                WriteMessageWithTab($"Source: {logException.Source}", errorForegroundColor, errorBackgroundColor);
                WriteMessageWithTab($"StackTrace:", errorForegroundColor, errorBackgroundColor);
                WriteMessageWithTab($" {logException.StackTrace?.Trim()}", errorForegroundColor, errorBackgroundColor);
            }

            Console.ResetColor();

        }

        #endregion

    }
}
