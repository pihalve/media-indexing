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

            return _rootContainer;
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
