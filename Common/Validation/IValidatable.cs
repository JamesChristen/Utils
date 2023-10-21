using System;
using System.Linq;

namespace Common.Validation
{
    public interface IValidatable
    {
        /// <summary>
        /// Runs validation on the object properties. Only throws exception if specified
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        string GetErrors(bool throwErrors = true)
        {
            try
            {
                CheckValid();
                return string.Empty;
            }
            catch (ValidationException ex)
            {
                if (throwErrors)
                {
                    throw;
                }
                string message = string.Join(Environment.NewLine, ex.InnerExceptions.Select(x => x.Message));
                return $"Validation errors for {ValidationIdentifier}:{Environment.NewLine}{message}";
            }
        }

        void CheckValid();

        string ValidationIdentifier { get; }
    }
}
