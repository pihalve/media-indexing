using ExifLib;

namespace Pihalve.MediaIndexer.MetaData
{
    public interface IExifTagReader
    {
        void BeginRead(string filePath);
        void EndRead();
        T GetTagValue<T>(ExifTags exifTag);
        T GetTagValue<T>(string filePath, ExifTags exifTag);
    }
}
