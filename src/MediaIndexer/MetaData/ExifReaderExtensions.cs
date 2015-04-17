using ExifLib;

namespace Pihalve.MediaIndexer.MetaData
{
    internal static class ExifReaderExtensions
    {
        internal static T GetTagValue<T>(this ExifReader exifReader, ExifTags tag)
        {
            T value;
            return exifReader.GetTagValue(ExifTags.DateTimeOriginal, out value) ? value : default(T);
        }
    }
}
