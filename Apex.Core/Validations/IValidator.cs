
namespace Apex.Core.Validations
{
    public interface IValidator<in T> where T : class
    {
        ValidationState Validate(T entity);
    }
}
