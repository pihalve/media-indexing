using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace Pihalve.MediaIndexer.MetaData
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
                return metadata != null ? metadata.Keywords : null;
            }
        }
    }
}
