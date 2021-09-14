using System;
using System.Collections.Generic;
using Data.Enums;

namespace Data.Models
{
    /// <summary>
    /// Facility data scheme
    /// </summary>
    public class Facility
    {
        public Facility()
        {
            this.Id = Guid.NewGuid().ToString();         
        }

        public string Id { get; set; }
        public int Capacity { get; set; }
        public FacilityType Type { get; set; }
        public bool IsTaken { get; set; }
        public double AdultPrice { get; set; }
        public double ChildrenPrice { get; set; }
        public int Number { get; set; }
        public string ImageUrl { get; set; }

        public virtual IEnumerable<Reservation> Reservations { get; set; }
    }
}
