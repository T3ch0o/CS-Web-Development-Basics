namespace Demo.Models
{
    internal class Track : IdObject
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public decimal Price { get; set; }
    }
}