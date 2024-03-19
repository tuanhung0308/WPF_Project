using System;
using System.Collections.Generic;

namespace WPF_Project.Models
{
    public partial class PreservationProcess
    {
        public int PreservationProcessId { get; set; }
        public int? ProduceId { get; set; }
        public string? Method { get; set; }
        public string? PreservationCondition { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Notes { get; set; }

        public virtual Produce? Produce { get; set; }
    }
}
