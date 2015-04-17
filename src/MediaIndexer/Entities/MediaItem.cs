using System;
using System.Collections.Generic;

namespace Pihalve.MediaIndexer.Entities
{
    public class MediaItem
    {
        private readonly IList<string> _keywords;

        public MediaItem(string filePath)
        {
            Id = Guid.NewGuid();
            FilePath = filePath;
            _keywords = new List<string>();
        }

        public Guid Id { get; private set; }

        public string FilePath { get; private set; }
        
        public DateTime Created { get; set; }
        
        public DateTime? DateTimeOriginal { get; set; }
        
        public IList<string> Keywords
        {
            get
            {
                return _keywords;
            }
        }

        public void ChangeIdentity(Guid id)
        {
            Id = id;
        }
    }
}
