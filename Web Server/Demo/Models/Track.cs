namespace Demo.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    internal class Track : IdObject
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public decimal Price { get; set; }

        public string AlbumId { get; set; }

        [ForeignKey(nameof(AlbumId))]
        public virtual Album Album { get; set; }
    }
}