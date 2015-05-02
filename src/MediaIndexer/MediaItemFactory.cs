using System.IO;
using System.Linq;
using Pihalve.MediaIndexer.Entities;
using Pihalve.MediaIndexer.MetaData;

namespace Pihalve.MediaIndexer
{
    public class MediaItemFactory : IMediaItemFactory
    {
        private static readonly string[] JpgExtensions = {".jpg", ".jpeg"};
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

            var item = new MediaItem
            {
                FilePath = filePath,
                Created = mediaFile.CreationTime
            };

            if (JpgExtensions.Contains(mediaFile.Extension.ToLower()))
            {
                item.DateTimeOriginal = _exifReader.GetDateTimeOriginal(filePath);

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
