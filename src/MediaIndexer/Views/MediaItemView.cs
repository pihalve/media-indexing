using Pihalve.MediaIndexer.Entities;
using RaptorDB;

namespace Pihalve.MediaIndexer.Views
{
    public class MediaItemViewRowSchema : RDBSchema
    {
        public string FilePath { get; set; }
    }

    [RegisterView]
    public class MediaItemView : View<MediaItem>
    {
        public MediaItemView()
        {
            Name = "MediaItem";
            Description = "A primary view for MediaItems";
            ConsistentSaveToThisView = true;
            isPrimaryList = true;
            isActive = true;
            BackgroundIndexing = true;
            Version = 1;

            Schema = typeof(MediaItemViewRowSchema);

            Mapper = (api, docid, doc) =>
            {
                api.EmitObject(docid, doc);
            };
        }
    }
}
