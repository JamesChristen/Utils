using System;

namespace Common.Logging
{
    public interface ILog
    {
        void Debug(string msg);
        void Info(string msg);
        void Warn(string msg);
        void Error(string msg);
        void Error(string msg, Exception ex);
        void Error(Exception ex);

        void Debug(object sender, string msg);
        void Info(object sender, string msg);
        void Warn(object sender, string msg);
        void Error(object sender, string msg);
        void Error(object sender, string msg, Exception ex);
        void Error(object sender, Exception ex);

        ILog CreateSubLogger(string prefix);
    }
}
