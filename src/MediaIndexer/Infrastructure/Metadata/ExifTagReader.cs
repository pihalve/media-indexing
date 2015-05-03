using System;
using ExifLib;
using Pihalve.MediaIndexer.Domain.Metadata;

namespace Pihalve.MediaIndexer.Infrastructure.Metadata
{
    public class ExifTagReader : IExifTagReader, IDisposable
    {
        private ExifReader _exifReader;

        public bool BeginRead(string filePath)
        {
            if (_exifReader != null)
            {
                throw new Exception("Existing read is in progress. Please call EndRead first.");
            }

            try
            {
                _exifReader = new ExifReader(filePath);
                return true;
            }
            catch (ExifLibException)
            {
                return false;
            }
        }

        public void EndRead()
        {
            if (_exifReader != null)
            {
                _exifReader.Dispose();
                _exifReader = null;
            }
        }

        public DateTime? GetDateTimeOriginal()
        {
            if (_exifReader == null)
            {
                throw new Exception("BeginRead must be called first");
            }

            DateTime result;
            if (_exifReader.GetTagValue(ExifTags.DateTimeOriginal, out result))
            {
                return result;
            }

            return null;
        }

        public DateTime? GetDateTimeOriginal(string filePath)
        {
            try
            {
                using (var exifReader = new ExifReader(filePath))
                {
                    DateTime result;
                    if (exifReader.GetTagValue(ExifTags.DateTimeOriginal, out result))
                    {
                        return result;
                    }
                }
            }
            catch (ExifLibException)
            {
                // unable to get exif tag - eat exception so we that can return null
            }

            return null;
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
