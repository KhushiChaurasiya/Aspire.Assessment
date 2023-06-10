using Assignment.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Assignment.API
{
    public class CustomLoggingProvider : ILoggerProvider
    {
        public readonly DbLoggerOptions Options;
        public CustomLoggingProvider(IOptions<DbLoggerOptions> _options)
        {
            Options = _options.Value;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger(Options);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
