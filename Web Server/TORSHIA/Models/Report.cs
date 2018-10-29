namespace Torshia.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    internal class Report : IdObject
    {
        public Status Status { get; set; }

        public DateTime ReportedOn { get; set; }

        public string TaskId { get; set; }

        [ForeignKey(nameof(TaskId))]
        public virtual Task Task { get; set; }

        public string ReporterId { get; set; }

        [ForeignKey(nameof(ReporterId))]
        public virtual User Reporter { get; set; }
    }
}