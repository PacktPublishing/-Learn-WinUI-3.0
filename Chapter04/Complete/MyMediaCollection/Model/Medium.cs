using MyMediaCollection.Enums;

namespace MyMediaCollection.Model
{
    public class Medium
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ItemType MediaType { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
