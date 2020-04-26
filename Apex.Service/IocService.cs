using Apex.DAL.Abstracts;
using Apex.DAL.Concretes;
using Apex.Service.Abstracts;
using Apex.Service.Concretes;
using Apex.Service.Services;
using Autofac;
using Apex.Service.Services.Content;
using Apex.Service.Services.Shop;

namespace Apex.Service
{
    public class IocService : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>));
            builder.RegisterType(typeof(UserService)).As(typeof(IUserService));
            builder.RegisterType<UserService>().AsSelf();

            builder.RegisterType<ProductService>().AsSelf();
            builder.RegisterType<CategoryService>().AsSelf();
            builder.RegisterType<SliderService>().AsSelf();

            builder.RegisterGeneric(typeof(GenericService<,>)).AsSelf();

            builder.RegisterType<MessageService>().AsSelf();
        }
    }

}
