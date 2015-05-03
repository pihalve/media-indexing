using System;
using System.Collections.Generic;

namespace Pihalve.MediaIndexer.Entities
{
    public class MediaItem
    {
        private readonly IList<string> _keywords = new List<string>();

        public string Id { get; set; }

        public string FilePath { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime? DateTimeOriginal { get; set; }
        
        public IList<string> Keywords
        {
            get
            {
                return _keywords;
            }
        }
    }
}
