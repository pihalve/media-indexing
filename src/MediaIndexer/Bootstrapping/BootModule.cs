using Autofac;

namespace Pihalve.MediaIndexer.Bootstrapping
{
    public abstract class BootModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Register(builder);
        }

        protected abstract void Register(ContainerBuilder builder);

        public virtual void Configure(IContainer rootContainer)
        {
        }
    }
}
