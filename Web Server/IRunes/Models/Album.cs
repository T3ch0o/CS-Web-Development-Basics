namespace Demo.Models
{
    using System.Collections.Generic;
    using System.Linq;

    internal class Album : IdObject
    {
        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal Price => Tracks.Sum(track => track.Price);

        public virtual ICollection<Track> Tracks { get; } = new HashSet<Track>();
    }
}