using System;
using System.Collections.Generic;

namespace WPF_Project.Models
{
    public partial class Produce
    {
        public Produce()
        {
            CareProcesses = new HashSet<CareProcess>();
            HarvestProcesses = new HashSet<HarvestProcess>();
            PreservationProcesses = new HashSet<PreservationProcess>();
        }

        public int ProduceId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? Season { get; set; }
        public int? Quantity { get; set; }

        public virtual ICollection<CareProcess> CareProcesses { get; set; }
        public virtual ICollection<HarvestProcess> HarvestProcesses { get; set; }
        public virtual ICollection<PreservationProcess> PreservationProcesses { get; set; }
    }
}
