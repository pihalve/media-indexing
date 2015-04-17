using System.Collections.Generic;

namespace Pihalve.MediaIndexer.MetaData
{
    public interface IIptcTagReader
    {
        IEnumerable<string> GetKeywords(string filePath);
    }
}
