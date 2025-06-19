namespace DNUResourceBooker.Models
{
    public class Resource
    {
        public int ResourceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ResourceCategory Category { get; set; }
        public string Location { get; set; }
        public int? Capacity { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
    }
}