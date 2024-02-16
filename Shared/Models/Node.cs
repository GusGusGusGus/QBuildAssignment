namespace Shared.Models
{
    public class Node
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public List<Node> Children { get; set; }
        public string? ParentName { get; set; }
        public string? PartNumber { get; set; }
        public string? Title { get; set; }
        public int? Quantity { get; set; }
        public string? Type { get; set; }
        public string? Item { get; set; }
        public string? Material { get; set; }
    }
}