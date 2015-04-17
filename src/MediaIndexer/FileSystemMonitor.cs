using System;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace Pihalve.MediaIndexer
{
    public class FileSystemMonitor : IFileSystemMonitor, IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string[] _watchFilter;
        private readonly IFileProcessor _processor;
        private readonly FileSystemWatcher _watcher;

        public FileSystemMonitor(string watchFolder, string watchFilter, IFileProcessor processor)
        {
            _watchFilter = watchFilter.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.TrimStart('*')).ToArray();
            _processor = processor;

            _watcher = new FileSystemWatcher(watchFolder);
            _watcher.IncludeSubdirectories = true;
            _watcher.InternalBufferSize = 16384; // Each event can take up to 16 bytes. For buffer of 16384 it means at least 1024 events.
            _watcher.Created += OnCreated;
            _watcher.Changed += OnChanged;
            //_watcher.Renamed += OnRenamed;
            //_watcher.Deleted += OnDeleted;
        }

        public void Start()
        {
            _watcher.EnableRaisingEvents = true;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (_watchFilter.Contains(Path.GetExtension(e.Name)))
            {
                try
                {
                    if (Log.IsDebugEnabled) Log.Debug(string.Format("Processing created file: {0}", e.FullPath));

                    //TODO: handle created media file
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("Error processing created file: {0}", e.FullPath), ex);
                }
            }
            else
            {
                var directory = new DirectoryInfo(e.FullPath);
                if (directory.Exists)
                {
                    try
                    {
                        if (Log.IsDebugEnabled) Log.Debug(string.Format("Processing created directory: {0}", e.FullPath));

                        //TODO: handle created directory - the whole hierarchy since only event for root of new hierarchy is triggered
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Format("Error processing created directory: {0}", e.FullPath), ex);
                    }
                }
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (_watchFilter.Contains(Path.GetExtension(e.Name)))
            {
                try
                {
                    if (Log.IsDebugEnabled) Log.Debug(string.Format("Processing changed file: {0}", e.FullPath));

                    //TODO: handle changed media file
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("Error processing changed file: {0}", e.FullPath), ex);
                }
            }
            else
            {
                var directory = new DirectoryInfo(e.FullPath);
                if (directory.Exists)
                {
                    try
                    {
                        if (Log.IsDebugEnabled) Log.Debug(string.Format("Processing changed directory: {0}", e.FullPath));

                        //TODO: handle changed directory
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Format("Error processing changed directory: {0}", e.FullPath), ex);
                    }
                }
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
