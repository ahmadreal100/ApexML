using Apex.DAL.Abstracts;
using Apex.DAL.Concretes;
using Apex.DAL.EF;
using Autofac;

namespace Apex.DAL
{
    public class IocDal : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<ApexContext>().AsSelf().InstancePerRequest();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<LanguageRepository>().As<ILanguageRepository>();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<SliderRepository>().As<ISliderRepository>();

            builder.RegisterType<LanguageRepository>().As<ILanguageRepository>();
        }
    }
}
