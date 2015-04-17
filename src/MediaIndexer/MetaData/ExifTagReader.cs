using System;
using ExifLib;

namespace Pihalve.MediaIndexer.MetaData
{
    public class ExifTagReader : IExifTagReader, IDisposable
    {
        private ExifReader _exifReader;

        public void BeginRead(string filePath)
        {
            _exifReader = new ExifReader(filePath);
        }

        public void EndRead()
        {
            if (_exifReader != null)
            {
                _exifReader.Dispose();
                _exifReader = null;
            }
        }

        public T GetTagValue<T>(ExifTags exifTag)
        {
            if (_exifReader == null)
            {
                throw new Exception("BeginRead must be called first");
            }

            return _exifReader.GetTagValue<T>(exifTag);
        }

        public T GetTagValue<T>(string filePath, ExifTags exifTag)
        {
            using (var exifReader = new ExifReader(filePath))
            {
                return exifReader.GetTagValue<T>(exifTag);
            }
        }

        public void Dispose()
        {
            if (_exifReader != null)
            {
                _exifReader.Dispose();
            }
        }
    }
}
