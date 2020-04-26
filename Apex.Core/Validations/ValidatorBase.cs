namespace Apex.Core.Validations
{
    public abstract class ValidatorBase<T> : IValidator<T> where T : class
    {
        protected ValidationState State;
        protected ValidatorBase()
        {
            State = new ValidationState();
        }
        public abstract ValidationState Validate(T entity);
    }
}
