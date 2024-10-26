using System;
using System.Collections.Generic;
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



        public Staff Staff { get; set; }
    }
}
