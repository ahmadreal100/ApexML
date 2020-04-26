using System;
using Apex.Core;
using Apex.Core.Validations;

namespace Apex.Service
{
    public class ServiceResult<T>
    {
        public bool NotFound { get; set; }
        public Type NotFoundType { get; set; }
        public ValidationState State { get; set; } = new ValidationState();
        public T Result { get; set; }
        public bool Succeeded => State.IsValid && !NotFound;

        public void ServerError()
        {
            State.Errors.Add("500", "", "error");
        }

        public void DocumentPermanentError(DocumentPermanentException ex)
        {
            State.Errors.Add(ex.Code ?? "1000", "", "_");
        }

        public void NotAuthorize()
        {
            State.Errors.Add("401", "", "not allowed");
        }
    }
}
