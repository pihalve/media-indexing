using System;

namespace Pihalve.MediaIndexer.MetaData
{
    public interface IExifTagReader
    {
        DateTime? GetDateTimeOriginal(string filePath);
    }
}
