using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using RSoft.Logs.Options;
using RSoft.Logs.Providers;
using System;
using System.Net.Http;
using System.Text.Json;

namespace RSoft.Logs.Extensions
{

    /// <summary>
    /// Provide extensino methods for RSoft.Logger
    /// </summary>
    public static class LoggerExtensions
    {

        /// <summary>
        /// Add middleware loggin options parameter
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddMiddlewareLoggingOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RequestResponseMiddlewareOptions>(options => configuration.GetSection("Logging:RequestResponseMiddleware").Bind(options));
            return services;
        }

        /// <summary>
        /// Add RSoft Console Logger
        /// </summary>
        /// <param name="builder">Logging builder object</param>
        public static ILoggingBuilder AddConsoleLogger(this ILoggingBuilder builder)
        {

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<LoggerOptions>, LoggerOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<LoggerOptions>, LoggerProviderOptionsChangeTokenSource<LoggerOptions, ConsoleLoggerProvider>>());
            builder.Services.AddHttpContextAccessor();

            return builder;

        }

        /// <summary>
        /// Add RSoft Console Logger
        /// </summary>
        /// <param name="builder">Logging builder object</param>
        /// <param name="configure">Action configure option</param>
        public static ILoggingBuilder AddConsoleLogger(this ILoggingBuilder builder, Action<LoggerOptions> configure)
        {
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            builder.AddConsoleLogger();
            builder.Services.Configure(configure);

            return builder;

        }

        /// <summary>
        /// Add RSoft Elastic Logger
        /// </summary>
        /// <param name="builder">Logging builder object</param>
        public static ILoggingBuilder AddElasticLogger(this ILoggingBuilder builder)
        {

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactory, HttpClientFactory>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ElasticLoggerProvider>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<LoggerOptions>, LoggerOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<LoggerOptions>, LoggerProviderOptionsChangeTokenSource<LoggerOptions, ElasticLoggerProvider>>());
            builder.Services.AddHttpContextAccessor();

            return builder;

        }

        /// <summary>
        /// Add RSoft Elastic Logger
        /// </summary>
        /// <param name="builder">Logging builder object</param>
        /// <param name="configure">Action configure option</param>
        public static ILoggingBuilder AddElasticLogger(this ILoggingBuilder builder, Action<LoggerOptions> configure)
        {
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            builder.AddElasticLogger();
            builder.Services.Configure(configure);

            return builder;

        }

        /// <summary>
        /// Add RSoft Seq Logger
        /// </summary>
        /// <param name="builder">Logging builder object</param>
        public static ILoggingBuilder AddSeqLogger(this ILoggingBuilder builder)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactory, HttpClientFactory>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, SeqLoggerProvider>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<LoggerOptions>, LoggerOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<LoggerOptions>, LoggerProviderOptionsChangeTokenSource<LoggerOptions, SeqLoggerProvider>>());
            builder.Services.AddHttpContextAccessor();
            return builder;
        }

        /// <summary>
        /// Add RSoft Seq Logger
        /// </summary>
        /// <param name="builder">Logging builder object</param>
        /// <param name="configure">Action configure option</param>
        public static ILoggingBuilder AddSeqLogger(this ILoggingBuilder builder, Action<LoggerOptions> configure)
        {
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            builder.AddSeqLogger();
            builder.Services.Configure(configure);

            return builder;

        }

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a JSON string.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="escapeQuoteMark">Escape mark quotation</param>
        public static string AsJson(this object value, bool escapeQuoteMark = true)
        {
            if (value == null)
                return string.Empty;

            string result = JsonSerializer.Serialize(value);

            if (escapeQuoteMark)
                result = result.Replace("\"", "\\\"");

            return result;

        }

    }

}
