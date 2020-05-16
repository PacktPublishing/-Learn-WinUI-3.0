using MyMediaCollection.Enums;

namespace MyMediaCollection.Model
{
    public class MediaItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ItemType MediaType { get; set; }
        public Medium MediumInfo { get; set; }
        public LocationType Location { get; set; }
    }
}
