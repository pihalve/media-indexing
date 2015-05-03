using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;

namespace Pihalve.MediaIndexer.Infrastructure.Bootstrapping
{
    public static class RegistrationExtensions
    {
        public static void RegisterBootModules(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            var type = typeof(BootModule);
            var moduleBuilder = new ContainerBuilder();

            moduleBuilder.RegisterAssemblyTypes(assemblies).Where(type.IsAssignableFrom).As<IModule>();

            using (var moduleContainer = moduleBuilder.Build())
            {
                foreach (var module in moduleContainer.Resolve<IEnumerable<IModule>>())
                {
                    builder.RegisterInstance(module).As<BootModule>();
                    builder.RegisterModule(module);
                }
            }
        }
    }
}
