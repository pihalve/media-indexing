using Autofac;
using Pihalve.MediaIndexer.ServiceApi;

namespace Pihalve.MediaIndexer.Bootstrapping.Modules
{
    public class ServiceApiModule : BootModule
    {
        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterType<MediaIndexerController>().InstancePerLifetimeScope();
        }
    }
}
