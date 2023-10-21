namespace Common.Logging
{
    public class Log4NetLogger : ILog
    {
        private readonly log4net.ILog _logger;

        public Log4NetLogger(log4net.ILog logger)
        {
            _logger = logger;
        }

        public void Debug(string msg) => Debug(new System.Diagnostics.StackFrame(1).GetMethod(), msg);
        public void Info(string msg) => Info(new System.Diagnostics.StackFrame(1).GetMethod(), msg);
        public void Warn(string msg) => Warn(new System.Diagnostics.StackFrame(1).GetMethod(), msg);
        public void Error(string msg) => Error(new System.Diagnostics.StackFrame(1).GetMethod(), msg);
        public void Error(string msg, Exception ex) => Error(new System.Diagnostics.StackFrame(1).GetMethod(), msg, ex);
        public void Error(Exception ex) => Error(new System.Diagnostics.StackFrame(1).GetMethod(), ex);

        public void Debug(object sender, string msg) => _logger.Debug(Format(sender, msg));
        public void Info(object sender, string msg) => _logger.Info(Format(sender, msg));
        public void Warn(object sender, string msg) => _logger.Warn(Format(sender, msg));
        public void Error(object sender, string msg) => _logger.Error(Format(sender, msg));
        public void Error(object sender, string msg, Exception ex) => _logger.Error(Format(sender, msg), ex);
        public void Error(object sender, Exception ex) => _logger.Error(ex);

        private string Format(object sender, string msg)
        {
            string result = string.Empty;
            if (sender != null)
            {
                result = sender.ToString() + " :: ";
            }
            result += msg;
            return result;
        }

        public ILog CreateSubLogger(string prefix) => new SubLogger(prefix, this);
    }
}
