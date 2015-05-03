using System;
using System.IO;
using System.Linq;

namespace Pihalve.MediaIndexer.Application.Watchers
{
    public class FileWatcher : IWatcher
    {
        private FileSystemWatcher _watcher;
        private string[] _watchFilter;

        public event FileSystemEventHandler Created;
        public event FileSystemEventHandler Changed;
        public event FileSystemEventHandler Renamed;
        public event FileSystemEventHandler Deleted;

        public FileWatcher()
        {
            _watcher = new FileSystemWatcher();
            _watcher.IncludeSubdirectories = true;

            _watcher.Created += OnCreated;
            _watcher.Changed += OnChanged;
            _watcher.Renamed += OnRenamed;
            _watcher.Deleted += OnDeleted;
        }

        public void Start(string rootPath, string filter)
        {
            _watchFilter = filter.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.TrimStart('*')).ToArray();
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

        protected virtual void OnFileCreated(FileSystemEventArgs e)
        {
            var handler = Created;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnFileChanged(FileSystemEventArgs e)
        {
            var handler = Changed;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnFileRenamed(FileSystemEventArgs e)
        {
            var handler = Renamed;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnFileDeleted(FileSystemEventArgs e)
        {
            var handler = Deleted;
            if (handler != null) handler(this, e);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (_watchFilter.Contains(Path.GetExtension(e.Name)))
            {
                OnFileCreated(e);
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (_watchFilter.Contains(Path.GetExtension(e.Name)))
            {
                OnFileChanged(e);
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            if (_watchFilter.Contains(Path.GetExtension(e.Name)))
            {
                OnFileRenamed(e);
            }
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            if (_watchFilter.Contains(Path.GetExtension(e.Name)))
            {
                OnFileDeleted(e);
            }
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
