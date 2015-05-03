using System;
using System.IO;

namespace Pihalve.MediaIndexer.Application.Watchers
{
    public interface IWatcher : IDisposable
    {
        event FileSystemEventHandler Created;
        event FileSystemEventHandler Changed;
        event FileSystemEventHandler Renamed;
        event FileSystemEventHandler Deleted;
        void Start(string rootPath, string filter);
        void Stop();
    }
}
