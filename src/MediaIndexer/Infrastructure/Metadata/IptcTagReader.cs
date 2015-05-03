using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Pihalve.MediaIndexer.Domain.Metadata;

namespace Pihalve.MediaIndexer.Infrastructure.Metadata
{
    public class IptcTagReader : IIptcTagReader
    {
        public IEnumerable<string> GetKeywords(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                //var image = System.Drawing.Image.FromStream(stream, false, false);
                var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
                var metadata = decoder.Frames[0].Metadata as BitmapMetadata;
                if (metadata != null && metadata.Keywords != null)
                {
                    return metadata.Keywords;
                }
                return Enumerable.Empty<string>();
            }
        }
    }
}
