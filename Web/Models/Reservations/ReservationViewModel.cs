﻿using Data.Enums;
using System;
using System.Collections.Generic;
using Web.Models.Clients;

namespace Web.Models.Reservations
{
    public class ReservationViewModel
    {
        public string Id { get; set; }
        public IList<ClientViewModel> Clients { get; set; }
        public DateTime AccommodationDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool Breakfast { get; set; }
        public bool AllInclusive { get; set; }
        public double Price { get; set; }
        public string FacilityId { get; set; }
        public string UserId { get; set; }
        public FacilityType FacilityType { get; set; }
        public string FacilityNumber { get; set; }
        public string FacilityImageUrl { get; set; }
        public string UserFullName => $"{UserFirstName} {UserLastName}";
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public bool UserIsAdult { get; set; }
    }
}
