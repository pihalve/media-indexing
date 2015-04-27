using System;
using Autofac;
using RaptorDB;
using RaptorDB.Common;

namespace Pihalve.MediaIndexer.Bootstrapping.Modules
{
    internal class RaptorDbModule : BootModule, IDisposable
    {
        private RaptorDBServer _raptorDbServer;
        private IRaptorDB _raptorDbClient;

        protected override void Register(ContainerBuilder builder)
        {
            var indexFolder = Configuration.GetAppSetting<string>("IndexFolder");
            var indexServerPort = Configuration.GetAppSetting<int>("IndexServerPort");

            //RaptorDB.Global.RequirePrimaryView = false;
            //var raptorDb = RaptorDB.RaptorDB.Open(indexFolder);
            //raptorDb.RegisterView(new MediaItemView());
            //_raptorDb = raptorDb;

            Global.RequirePrimaryView = true;
            Global.SaveAsBinaryJSON = true;

            _raptorDbServer = new RaptorDBServer(indexServerPort, indexFolder);

            _raptorDbClient = new RaptorDBClient("localhost", indexServerPort, "admin", "admin");
            builder.RegisterInstance(_raptorDbClient).As<IRaptorDB>().ExternallyOwned();
        }

        public void Dispose()
        {
            if (_raptorDbClient != null)
            {
                _raptorDbClient.Shutdown();
            }

            if (_raptorDbServer != null)
            {
                _raptorDbServer.Shutdown();
            }
        }
    }
}
