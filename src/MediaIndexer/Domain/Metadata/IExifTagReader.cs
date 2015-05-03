using System;

namespace Pihalve.MediaIndexer.Domain.Metadata
{
    public interface IExifTagReader
    {
        bool BeginRead(string filePath);
        void EndRead();
        DateTime? GetDateTimeOriginal();
        DateTime? GetDateTimeOriginal(string filePath);
    }
}
