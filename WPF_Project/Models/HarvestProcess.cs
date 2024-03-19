using System;
using System.Collections.Generic;

namespace WPF_Project.Models
{
    public partial class HarvestProcess
    {
        public int HarvestProcessId { get; set; }
        public int? ProduceId { get; set; }
        public DateTime? Date { get; set; }
        public int? QuantityHarvested { get; set; }
        public string? Notes { get; set; }

        public virtual Produce? Produce { get; set; }
    }
}
