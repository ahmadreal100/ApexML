using System;
using Apex.Core.Enums;

namespace Apex.Core.Validations
{
    public class ValidationError
    {
        public ValidationError(string name, object attemptedValue, string message, ErrorKeys? key = null)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(message)) throw new ArgumentNullException(nameof(message));

            Key = key;
            Name = name;
            AttemptedValue = attemptedValue;
            Message = message;
        }

        public ValidationError(string name, object attemptedValue, Exception exception, ErrorKeys? key = null)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            Key = key;
            Name = name;
            AttemptedValue = attemptedValue;
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
            Message = exception.Message;
        }

        public ErrorKeys? Key { get; set; }
        public string Name { get; }
        public object AttemptedValue { get; }
        public string Message { get; }
        public Exception Exception { get; }
    }
}