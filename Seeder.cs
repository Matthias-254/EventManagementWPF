using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement
{
    internal class Seeder
    {
        public Seeder(EM_Context context)
        {
            if (!context.Locations.Any())
            {
                context.Locations.AddRange(
                    new Location { Name = "?", Address = "?", Deleted = DateTime.Now },
                    new Location { Name = "Kasteel Van Huizingen", Address = "Henry Torleylaan 100, 1654 Huizingen" },
                    new Location { Name = "Le Grand Salon", Address = "Herman Teirlinckplein 4, 1650 Beersel" },
                    new Location { Name = "Feestzaal Ter Heyde", Address = "Kesterweg 35, 1755 Gooik" }
                );
                context.SaveChanges();
            }

            if (!context.Events.Any())
            {
                context.Events.AddRange(
                    new Event { Name = "?", Description = "?", Deleted = DateTime.Now, LocationId = 1},
                    new Event { Name = "Wijnproeverij Avond", Description = "New local wine tasting time.", StartDate = DateTime.Now, LocationId = 1},
                    new Event { Name = "Kunsttentoonstelling", Description = "Art exhibition of all kinds of artists from the region", StartDate = DateTime.Now, LocationId = 2},
                    new Event { Name = "Boekenbeurs", Description = "Book fair of various books made by authors from the region", StartDate = DateTime.Now, LocationId = 3}
                );
                context.SaveChanges();
            }

            if (!context.Staffs.Any())
            {
                context.Staffs.AddRange(
                    new Staff { Name = "?", FirstName = "?", LastName = "?", Deleted = DateTime.Now },
                    new Staff { Name = "Johny", FirstName = "John", LastName = "Doe" },
                    new Staff { Name = "Jany", FirstName = "Jane", LastName = "Doe" },
                    new Staff { Name = "Joey", FirstName = "Joe", LastName = "Bloggs" }
                );
                context.SaveChanges();
            }

            if (!context.StaffsEvents.Any())
            {
                int eventId = context.Events.First(p => p.Name == "Wijnproeverij Avond").Id;
                context.StaffsEvents.AddRange(
                    new StaffEvent { StaffId = context.Staffs.FirstOrDefault(staff => staff.Name == "Johny").Id, EventId = eventId},
                    new StaffEvent { StaffId = context.Staffs.FirstOrDefault(staff => staff.Name == "Jany").Id, EventId = eventId }
                );
                context.SaveChanges();
            }
        }
    }
}
