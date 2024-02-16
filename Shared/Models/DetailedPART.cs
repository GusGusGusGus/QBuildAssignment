namespace Shared.Models

{
    public class DetailedPART
    { 
        public int Id { get; set; }
        public string Guid { get; set; }
        public string ParentName { get; set; }
        public string ComponentName { get; set; }
        public string? PartNumber { get; set; }
        public string? Title { get; set; }
        public int Quantity { get; set; }
        public string? Type { get; set; }
        public string? Item { get; set; }
        public string? Material { get; set; }
    }
}
