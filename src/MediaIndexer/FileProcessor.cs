using System;
using System.IO;
using ExifLib;
using Pihalve.MediaIndexer.Entities;
using Pihalve.MediaIndexer.MetaData;

namespace Pihalve.MediaIndexer
{
    public class FileProcessor : IFileProcessor
    {
        private readonly IIndexer _indexer;
        private readonly IExifTagReader _exifReader;
        private readonly IIptcTagReader _iptcReader;

        public FileProcessor(IIndexer indexer, IExifTagReader exifReader, IIptcTagReader iptcReader)
        {
            _indexer = indexer;
            _exifReader = exifReader;
            _iptcReader = iptcReader;
        }

        public void Process(string filePath)
        {
            var item = CreateMediaItem(filePath);
            _indexer.Save(item);
        }

        private MediaItem CreateMediaItem(string filePath)
        {
            var mediaFile = new FileInfo(filePath);
            if (!mediaFile.Exists)
            {
                return null;
            }

            var item = new MediaItem(filePath);
            item.Created = mediaFile.CreationTime;

            if (mediaFile.Extension.Equals(".jpg"))
            {
                item.DateTimeOriginal = _exifReader.GetTagValue<DateTime>(filePath, ExifTags.DateTimeOriginal);
                var keywords = _iptcReader.GetKeywords(filePath);
                foreach (var keyword in keywords)
                {
                    item.Keywords.Add(keyword);
                }
            }

            return item;
        }
    }
}
