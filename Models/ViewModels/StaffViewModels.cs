using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Models.ViewModels
{
    internal class StaffDatagridViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public StaffDatagridViewModel(Staff staff)
        {
            Id = staff.Id;
            Name = staff.Name;
            FirstName = staff.FirstName;
            LastName = staff.LastName;
        }
    }
}
