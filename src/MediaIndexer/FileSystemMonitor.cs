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
        private readonly IMediaItemFactory _mediaItemFactory;
        private readonly IMediaItemIndexService _indexService;
        private readonly FileSystemWatcher _watcher;

        public FileSystemMonitor(string watchFolder, string watchFilter, IMediaItemFactory mediaItemFactory, IMediaItemIndexService indexService)
        {
            _watchFilter = watchFilter.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.TrimStart('*')).ToArray();
            _mediaItemFactory = mediaItemFactory;
            _indexService = indexService;

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

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (_watchFilter.Contains(Path.GetExtension(e.Name)))
            {
                try
                {
                    if (Log.IsDebugEnabled) Log.DebugFormat("Processing created file: {0}", e.FullPath);

                    var mediaItem = _mediaItemFactory.Create(e.FullPath);
                    _indexService.Save(mediaItem);
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
                        if (Log.IsDebugEnabled) Log.DebugFormat("Processing created directory: {0}", e.FullPath);

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
                    if (Log.IsDebugEnabled) Log.DebugFormat("Processing changed file: {0}", e.FullPath);

                    //TODO: handle changed media file
                    var mediaItem = _mediaItemFactory.Create(e.FullPath);
                    var existingMediaItem = _indexService.Query(e.FullPath).FirstOrDefault();
                    if (existingMediaItem != null)
                    {
                        mediaItem.ChangeIdentity(existingMediaItem.docid);
                    }
                    else
                    {
                        Log.WarnFormat("Expected to find file in index, but it wasn't there. Adding file: {0}", e.FullPath);
                    }

                    _indexService.Save(mediaItem);
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
                        if (Log.IsDebugEnabled) Log.DebugFormat("Processing changed directory: {0}", e.FullPath);

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
