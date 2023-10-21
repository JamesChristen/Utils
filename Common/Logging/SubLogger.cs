using System;

namespace Common.Logging
{
    internal class SubLogger : ILog
    {
        private readonly string _prefix;
        private readonly ILog _logger;

        public SubLogger(string prefix, ILog logger)
        {
            _prefix = prefix;
            _logger = logger;
        }

        public ILog CreateSubLogger(string prefix) => new SubLogger(prefix, this);

        public void Debug(string msg) => _logger.Debug(Format(msg));
        public void Debug(object sender, string msg) => _logger.Debug(sender, Format(msg));

        public void Info(string msg) => _logger.Info(Format(msg));
        public void Info(object sender, string msg) => _logger.Info(sender, Format(msg));

        public void Warn(string msg) => _logger.Warn(Format(msg));
        public void Warn(object sender, string msg) => _logger.Warn(sender, Format(msg));

        public void Error(string msg) => _logger.Error(Format(msg));
        public void Error(string msg, Exception ex) => _logger.Error(Format(msg), ex);
        public void Error(Exception ex) => _logger.Error(ex);

        public void Error(object sender, string msg) => _logger.Error(sender, Format(msg));
        public void Error(object sender, string msg, Exception ex) => _logger.Error(sender, Format(msg), ex);
        public void Error(object sender, Exception ex) => _logger.Error(sender, ex);

        private string Format(string msg) => _prefix + msg;
    }
}
