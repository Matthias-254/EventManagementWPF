using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string Name { get; set; } // NickName
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Deleted { get; set; } = DateTime.MaxValue;
    }

    public class StaffEvent
    {
        public int Id { get; set; }

        [ForeignKey("Staff")]
        public int StaffId { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public DateTime Added { get; set; } = DateTime.Now;
        public DateTime Deleted { get; set; } = DateTime.MaxValue;

        public Staff Staff { get; set; }
    }
}
