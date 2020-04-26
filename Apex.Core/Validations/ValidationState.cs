namespace Apex.Core.Validations
{
    public class ValidationState
    {
        public ValidationState()
        {
            Errors = new ValidationErrorCollection();
        }

        public ValidationErrorCollection Errors { get; }

        public bool IsValid => Errors.Count == 0;
    }
}