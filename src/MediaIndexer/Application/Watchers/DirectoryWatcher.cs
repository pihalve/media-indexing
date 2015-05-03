using System.IO;

namespace Pihalve.MediaIndexer.Application.Watchers
{
    public class DirectoryWatcher : IWatcher
    {
        private FileSystemWatcher _watcher;
        public event FileSystemEventHandler Created;
        public event FileSystemEventHandler Changed;
        public event FileSystemEventHandler Renamed;
        public event FileSystemEventHandler Deleted;

        public DirectoryWatcher()
        {
            _watcher = new FileSystemWatcher();
            _watcher.IncludeSubdirectories = true;
            _watcher.NotifyFilter = NotifyFilters.DirectoryName;

            _watcher.Created += OnCreated;
            _watcher.Changed += OnChanged;
            _watcher.Renamed += OnRenamed;
            _watcher.Deleted += OnDeleted;
        }

        public void Start(string rootPath, string filter)
        {
            _watcher.Path = rootPath;
            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            if (_watcher != null)
            {
                _watcher.Dispose();
                _watcher = null;
            }
        }

        protected virtual void OnDirectoryCreated(FileSystemEventArgs e)
        {
            var handler = Created;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnDirectoryChanged(FileSystemEventArgs e)
        {
            var handler = Changed;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnDirectoryRenamed(FileSystemEventArgs e)
        {
            var handler = Renamed;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnDirectoryDeleted(FileSystemEventArgs e)
        {
            var handler = Deleted;
            if (handler != null) handler(this, e);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            OnDirectoryCreated(e);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            OnDirectoryChanged(e);
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            OnDirectoryRenamed(e);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            OnDirectoryDeleted(e);
        }

        public void Dispose()
        {
            if (_watcher != null)
            {
                _watcher.Dispose();
            }
        }
    }
}
