using System;

namespace Common.Logging
{
    public class DummyLogger : ILog
    {
        public ILog CreateSubLogger(string prefix) => this;

        public void Debug(string msg) { }
        public void Debug(object sender, string msg) { }

        public void Info(string msg) { }
        public void Info(object sender, string msg) { }

        public void Warn(string msg) { }
        public void Warn(object sender, string msg) { }

        public void Error(string msg) { }
        public void Error(string msg, Exception ex) { }
        public void Error(Exception ex) { }
        public void Error(object sender, string msg) { }
        public void Error(object sender, string msg, Exception ex) { }
        public void Error(object sender, Exception ex) { }
    }
}
