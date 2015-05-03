using Autofac;
using Pihalve.MediaIndexer.Interfaces.Api;

namespace Pihalve.MediaIndexer.Infrastructure.Bootstrapping.Modules
{
    public class ServiceApiModule : BootModule
    {
        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterType<MediaIndexerController>().InstancePerLifetimeScope();
        }
    }
}
