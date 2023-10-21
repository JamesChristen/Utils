using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Validation
{
    public class ValidationException : AggregateException
    {
        public ValidationException()
            : base()
        {
        }

        public ValidationException(IEnumerable<string> errors)
            : this(errors.Select(x => new Exception(x)))
        {
        }

        public ValidationException(IEnumerable<Exception> exceptions)
            : this(string.Join(Environment.NewLine, exceptions.Select(x => x.Message)), exceptions)
        {
        }

        public ValidationException(string message, IEnumerable<Exception> exceptions)
            : base(message, exceptions)
        {
        }
    }
}
