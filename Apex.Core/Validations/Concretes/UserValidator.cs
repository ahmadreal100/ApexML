using Apex.Core.Entities.UserE;

namespace Apex.Core.Validations.Concretes
{
    public class UserValidator : ValidatorBase<User>
    {

        public override ValidationState Validate(User entity)
        {
            if (string.IsNullOrWhiteSpace(entity.UserName))
                State.Errors.Add("", entity.UserName, "");

            return State;
        }
    }
}
