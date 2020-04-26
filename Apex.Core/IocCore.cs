using Apex.Core.Entities.UserE;
using Apex.Core.Validations;
using Apex.Core.Validations.Concretes;
using Autofac;

namespace Apex.Core
{
    public class IocCore : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<UserValidator>().As<IValidator<User>>();
        }
    }
}
