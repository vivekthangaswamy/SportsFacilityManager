using Data.Enums;
using Data.Models;
using System;
using System.Collections.Generic;

namespace Tests.Data
{
    /// <summary>
    /// Facilitys test data
    /// </summary>
    public static class Facilitys
    {
        public static readonly global::Data.Models.Facility Facility1 = new global::Data.Models.Facility
        {
            AdultPrice = AdultPrice1,
            ChildrenPrice = ChildrenPrice1,
            Capacity = Capacity1,
            Number = 1,
            Type = FacilityType.YogaStudio,
            IsTaken = IsTaken1,
            Reservations = new List<Reservation>()
        };
        public static readonly global::Data.Models.Facility Facility2 = new global::Data.Models.Facility
        {
            AdultPrice = AdultPrice2,
            ChildrenPrice = ChildrenPrice2,
            Capacity = Capacity2,
            Number = 2,
            Type = FacilityType.BatmitonCourt,
            IsTaken = IsTaken2,
        };
        public static readonly global::Data.Models.Facility Facility1FreeAtPresent1ReservationUser3 = new global::Data.Models.Facility
        {
            AdultPrice = AdultPrice2,
            ChildrenPrice = ChildrenPrice2,
            Capacity = Capacity2,
            Number = 2,
            Type = FacilityType.BatmitonCourt,
            IsTaken = IsTaken1,
            Reservations = new List<Reservation>()
            {
                new Reservation
                {
                    AccommodationDate = DateTime.Today.AddDays(-3),
                    ReleaseDate = DateTime.Today.AddDays(-1),
                    AllInclusive=true,
                    Breakfast=true,
                    Clients = new List<ClientData>(),
                    Price = 5,
                    User = Users.User3NotEmployee
                }
            }
        };
        public static readonly global::Data.Models.Facility Facility1TakenAtPresent1ReservationUser4 = new global::Data.Models.Facility
        {
            AdultPrice = AdultPrice2,
            ChildrenPrice = ChildrenPrice2,
            Capacity = Capacity2,
            Number = 2,
            Type = FacilityType.YogaStudio,
            IsTaken = IsTaken2,
            Reservations = new List<Reservation>()
            {
                new Reservation
                {
                    AccommodationDate = DateTime.Today,
                    ReleaseDate = DateTime.Today.AddDays(3),
                    AllInclusive=true,
                    Breakfast=true,
                    Clients = new List<ClientData>(),
                    Price = 5,
                    User = Users.User4NotEmployee
                }
            }
        };
        public static readonly global::Data.Models.Facility Facility2TakenAtPresent = new global::Data.Models.Facility
        {
            AdultPrice = AdultPrice2,
            ChildrenPrice = ChildrenPrice2,
            Capacity = Capacity2,
            Number = 2,
            Type = FacilityType.BatmitonCourt,
            IsTaken = IsTaken2,
            Reservations = new List<Reservation>()
            {
                new Reservation
                {
                    AccommodationDate = DateTime.Today.AddDays(-2),
                    ReleaseDate = DateTime.Today.AddDays(5)
                }
            }
        };

        private const double AdultPrice1 = 20;
        private const double ChildrenPrice1 = 20;
        private const int Capacity1 = 2;
        private const double AdultPrice2 = 30;
        private const double ChildrenPrice2 = 30;
        private const int Capacity2 = 4;
        private const bool IsTaken1 = false;
        private const bool IsTaken2 = true;
    }
}
