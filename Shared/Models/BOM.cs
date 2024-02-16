namespace Shared.Models

{
    public class BOM
    {
        public int Id { get; set; }
        public string ParentName { get; set; }
        public int Quantity { get; set; }
        public string ComponentName { get; set; }
        public int PartFkId { get; set; }
    }
}
