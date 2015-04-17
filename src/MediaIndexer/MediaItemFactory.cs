using System;
using System.IO;
using ExifLib;
using Pihalve.MediaIndexer.Entities;
using Pihalve.MediaIndexer.MetaData;

namespace Pihalve.MediaIndexer
{
    public class MediaItemFactory : IMediaItemFactory
    {
        private readonly IExifTagReader _exifReader;
        private readonly IIptcTagReader _iptcReader;

        public MediaItemFactory(IExifTagReader exifReader, IIptcTagReader iptcReader)
        {
            _exifReader = exifReader;
            _iptcReader = iptcReader;
        }

        public MediaItem Create(string filePath)
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
