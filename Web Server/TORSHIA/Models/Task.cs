namespace Torshia.Models
{
    using System;
    using System.Collections.Generic;

    internal class Task : IdObject
    {
        public string Title { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsReported { get; set; }

        public string Description { get; set; }

        public virtual ICollection<string> Participants { get; } = new HashSet<string>();

        public virtual ICollection<Sector> AffectedSectors { get; } = new HashSet<Sector>();
    }
}