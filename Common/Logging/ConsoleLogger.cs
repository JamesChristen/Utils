using System;

namespace Common.Logging
{
    public class ConsoleLogger : BaseLogger
    {
        public override void Debug(string msg) => Log(Format(null, msg));
        public override void Debug(object sender, string msg) => Log(Format(sender, msg));

        public override void Info(string msg) => Log(Format(null, msg));
        public override void Info(object sender, string msg) => Log(Format(sender, msg));

        public override void Warn(string msg) => Log(Format(null, msg));
        public override void Warn(object sender, string msg) => Log(Format(sender, msg));

        public override void Error(string msg) => Log(Format(null, msg));
        public override void Error(string msg, Exception ex) => Log(Format(null, $"{msg}\n{ex.Message}\n{ex.StackTrace}"));
        public override void Error(Exception ex) => Log(Format(null, $"{ex.Message}\n{ex.StackTrace}"));

        public override void Error(object sender, string msg) => Log(Format(sender, msg));
        public override void Error(object sender, string msg, Exception ex) => Log(Format(sender, $"{msg}\n{ex.Message}\n{ex.StackTrace}"));
        public override void Error(object sender, Exception ex) => Log(Format(sender, $"{ex.Message}\n{ex.StackTrace}"));

        private void Log(string msg) => Console.WriteLine(msg);
    }
}
