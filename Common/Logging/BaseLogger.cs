using System;
using System.Diagnostics;
using System.Reflection;

namespace Common.Logging
{
    public abstract class BaseLogger : ILog
    {
        public virtual ILog CreateSubLogger(string prefix) => new SubLogger(prefix, this);

        public abstract void Debug(string msg);
        public abstract void Info(string msg);
        public abstract void Warn(string msg);
        public abstract void Error(string msg);
        public abstract void Error(string msg, Exception ex);
        public abstract void Error(Exception ex);

        public abstract void Debug(object sender, string msg);
        public abstract void Info(object sender, string msg);
        public abstract void Warn(object sender, string msg);
        public abstract void Error(object sender, string msg);
        public abstract void Error(object sender, string msg, Exception ex);
        public abstract void Error(object sender, Exception ex);

        protected virtual string Format(object sender, string msg)
        {
            if (sender == null)
            {
                MethodBase m = GetSender();
                sender = m == null ? string.Empty : $"{m.DeclaringType}.{m.Name} :: ";
            }
            else
            {
                sender = sender.ToString() + " :: ";
            }

            return (sender?.ToString() ?? string.Empty) + msg;
        }

        private MethodBase GetSender()
        {
            StackTrace st = new StackTrace();
            int i = 2;
            while (i < st.FrameCount)
            {
                StackFrame sf = st.GetFrame(i++);
                MethodBase m = sf.GetMethod();
                if (!typeof(ILog).IsAssignableFrom(m.DeclaringType) 
                    && !m.DeclaringType.Name.Contains("<") 
                    && !m.DeclaringType.Name.Contains("AnonymousType")
                    && m.DeclaringType != typeof(IEnumerableExtensions))
                {
                    return m;
                }
            }
            return null;
        }
    }
}
