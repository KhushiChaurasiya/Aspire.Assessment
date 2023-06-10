using Assignment.Migrations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Threading;
using Assignment.Contracts.Data.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Data.Sqlite;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Assignment.API
{
    public class DatabaseLogger : ILogger
    {
        private readonly CustomLoggingProvider _dbLoggerProvider;
        private DbLoggerOptions options;

        /// <summary>  
        /// Creates a new instance of <see cref="FileLogger" />.  
        /// </summary>  
        /// <param name="fileLoggerProvider">Instance of <see cref="FileLoggerProvider" />.</param>  
        public DatabaseLogger([NotNull] CustomLoggingProvider dbLoggerProvider)
        {
            _dbLoggerProvider = dbLoggerProvider;
        }

        public DatabaseLogger(DbLoggerOptions options)
        {
            this.options = options;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                // Don't log the entry if it's not enabled.  
                return;
            }

            var values = new JObject();
            if (_dbLoggerProvider?.Options?.LogFields?.Any() ?? false)
            {
                foreach (var logField in _dbLoggerProvider.Options.LogFields)
                {
                    switch (logField)
                    {
                        case "LogLevel":
                            if (!string.IsNullOrWhiteSpace(logLevel.ToString()))
                            {
                                values["LogLevel"] = logLevel.ToString();
                            }
                            break;
                        case "EventId":
                            values["EventId"] = eventId.Id;
                            break;
                        case "EventName":
                            if (!string.IsNullOrWhiteSpace(eventId.Name))
                            {
                                values["EventName"] = eventId.Name;
                            }
                            break;
                        case "Message":
                            if (!string.IsNullOrWhiteSpace(formatter(state, exception)))
                            {
                                values["Message"] = formatter(state, exception);
                            }
                            break;
                        case "ExceptionMessage":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.Message))
                            {
                                values["ExceptionMessage"] = exception?.Message;
                            }
                            break;
                        case "ExceptionStackTrace":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.StackTrace))
                            {
                                values["ExceptionStackTrace"] = exception?.StackTrace;
                            }
                            break;
                        case "ExceptionSource":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.Source))
                            {
                                values["ExceptionSource"] = exception?.Source;
                            }
                            break;
                    }
                }
            }

            var connection = _dbLoggerProvider.Options.ConnectionString;


            var con = new SqliteConnection(connection);
            con.Open();

            using var cmd = new SqliteCommand(con.ToString());
            cmd.CommandText = string.Format("INSERT INTO {0} ([Values], [Created]) VALUES (@Values, @Created)", _dbLoggerProvider.Options.LogTable);

            cmd.Parameters.AddWithValue("@Values", JsonConvert.SerializeObject(values, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                Formatting = Formatting.None
            }).ToString());
            cmd.Parameters.AddWithValue("@Created", DateTimeOffset.Now);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }
    }
}
