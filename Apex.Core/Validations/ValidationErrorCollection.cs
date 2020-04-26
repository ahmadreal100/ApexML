using System;
using System.Collections.ObjectModel;
using Apex.Core.Enums;

namespace Apex.Core.Validations
{
    public class ValidationErrorCollection : Collection<ValidationError>
    {
        public void Add(string name, object attemptedValue, string message, ErrorKeys? key = null)
        {
            Add(new ValidationError(name, attemptedValue, message, key));
        }

        public void Add(string name, object attemptedValue, Exception exception, ErrorKeys? key = null)
        {
            Add(new ValidationError(name, attemptedValue, exception, key));
        }
    }
}