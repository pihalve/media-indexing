using System.Collections.Generic;
using Autofac;
using Autofac.Util;

namespace Pihalve.MediaIndexer.Bootstrapping
{
    public class BootStrapper : Disposable
    {
        private IContainer _rootContainer;

        public IContainer Boot()
        {
            var builder = new ContainerBuilder();

            builder.RegisterBootModules(typeof(BootStrapper).Assembly);

            _rootContainer = builder.Build();

            ConfigureModules();

            return _rootContainer;
        }

        private void ConfigureModules()
        {
            foreach (var module in _rootContainer.Resolve<IEnumerable<BootModule>>())
            {
                module.Configure(_rootContainer);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (_rootContainer != null)
            {
                _rootContainer.Dispose();
            }
        }
    }
}
