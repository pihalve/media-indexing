using System.Collections.Generic;

namespace Pihalve.MediaIndexer.Domain.Metadata
{
    public interface IIptcTagReader
    {
        IEnumerable<string> GetKeywords(string filePath);
    }
}
