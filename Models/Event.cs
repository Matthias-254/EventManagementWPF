using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.MaxValue;
        public DateTime Deleted { get; set; } = DateTime.MaxValue;

        [ForeignKey("Location")]
        public int LocationId { get; set; }

        public Location Location { get; set; }
        public List<StaffEvent> StaffEvents { get; set; }
    }
}
