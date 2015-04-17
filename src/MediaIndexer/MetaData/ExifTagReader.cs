using System;
using ExifLib;

namespace Pihalve.MediaIndexer.MetaData
{
    public class ExifTagReader : IExifTagReader
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

        public DateTime? GetDateTimeOriginal(string filePath)
        {
            ExifReader reader = null;
            DateTime? dateTimeOriginal;

            try
            {
                reader = _exifReader ?? new ExifReader(filePath);
                dateTimeOriginal = GetDateTime(reader, ExifTags.DateTimeOriginal);
            }
            finally
            {
                if (_exifReader == null && reader != null)
                {
                    reader.Dispose();
                }
            }

            return dateTimeOriginal;
        }

        private static DateTime? GetDateTime(ExifReader exifReader, ExifTags tag)
        {
            DateTime dateTimeOriginal;
            if (exifReader.GetTagValue(ExifTags.DateTimeOriginal, out dateTimeOriginal))
            {
                return dateTimeOriginal;
            }

            return null;
        }
    }
}
