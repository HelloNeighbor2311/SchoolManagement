using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace SchoolManagement.Infrastructure.Logging
{
    public class OperationTimer<T> : IDisposable
    {
        private readonly ILogger<T> _logger;
        private readonly string _operationName;
        private readonly Stopwatch _stopwatch;

        public OperationTimer(ILogger<T> logger, string operationName)
        {
            _logger = logger;
            _operationName = operationName;
            _stopwatch = Stopwatch.StartNew();
            _logger.LogDebug("Starting {Operation}", operationName);
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _logger.LogOperationComplete(_operationName, _stopwatch.ElapsedMilliseconds);
        }
    }
}
