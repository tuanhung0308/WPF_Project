using System;
using System.Collections.Generic;

namespace WPF_Project.Models
{
    public partial class CareProcess
    {
        public int CareProcessId { get; set; }
        public int? ProductId { get; set; }
        public string? Activity { get; set; }
        public DateTime? Date { get; set; }
        public string? Notes { get; set; }

        public virtual Product? Product { get; set; }
    }
}
