using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Logging
{
    public class LogContainer : ILog
    {
        private readonly IList<ILog> _loggers;

        public LogContainer(params ILog[] loggers)
        {
            _loggers = loggers.ToList();
        }

        public void AddLogger(ILog logger)
        {
            if (!_loggers.Contains(logger))
            {
                _loggers.Add(logger);
            }
        }

        public ILog CreateSubLogger(string prefix) => new SubLogger(prefix, this);

        public void Debug(string msg) => Debug(null, msg);
        public void Info(string msg) => Info(null, msg);
        public void Warn(string msg) => Warn(null, msg);
        public void Error(string msg) => Error(null, msg);
        public void Error(string msg, Exception ex) => Error(null, msg, ex);
        public void Error(Exception ex) => Error(null, ex);

        public void Debug(object sender, string msg) => _loggers.ForEach(x => x.Debug(sender, msg));
        public void Info (object sender, string msg) => _loggers.ForEach(x => x.Info(sender, msg));
        public void Warn (object sender, string msg) => _loggers.ForEach(x => x.Warn(sender, msg));
        public void Error(object sender, string msg) => _loggers.ForEach(x => x.Error(sender, msg));
        public void Error(object sender, string msg, Exception ex) => _loggers.ForEach(x => x.Error(sender, msg, ex));
        public void Error(object sender, Exception ex) => _loggers.ForEach(x => x.Error(sender, ex));
    }
}
