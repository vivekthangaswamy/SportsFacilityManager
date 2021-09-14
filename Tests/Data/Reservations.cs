﻿using Data.Models;
using System;
using System.Collections.Generic;

/// <summary>
/// Test project space related to dummy data definition for usage in tests
/// </summary>
namespace Tests.Data
{
    /// <summary>
    /// Reservations test data
    /// </summary>
    public static class Reservations
    {
        public static readonly Reservation Reservation1User3Facility1NoClient = new Reservation
        {
            AccommodationDate = DateTime.Today.AddDays(2),
            ReleaseDate = DateTime.Today.AddDays(5),
            AllInclusive = AllInClusive1,
            Breakfast = Breakfast1,
            Clients = new List<ClientData>(),
            Facility = Facilitys.Facility1,
            User = Users.User3NotEmployee
        };

        public static readonly Reservation Reservation2User4Facility2NoClient = new Reservation
        {
            AccommodationDate = DateTime.Today.AddDays(8),
            ReleaseDate = DateTime.Today.AddDays(10),
            AllInclusive = AllInClusive1,
            Breakfast = Breakfast1,
            Clients = new List<ClientData>(),
            Facility = Facilitys.Facility2,
            User = Users.User4NotEmployee
        };

        public static readonly Reservation Reservation3User4Facility2NoClient = new Reservation
        {
            AccommodationDate = DateTime.Today.AddDays(11),
            ReleaseDate = DateTime.Today.AddDays(15),
            AllInclusive = AllInClusive1,
            Breakfast = Breakfast1,
            Clients = new List<ClientData>(),
            Facility = Facilitys.Facility2,
            User = Users.User4NotEmployee
        };

        public static readonly Reservation Reservation4User4Facility2NoClient = new Reservation
        {
            AccommodationDate = DateTime.Today.AddDays(12),
            ReleaseDate = DateTime.Today.AddDays(14),
            AllInclusive = AllInClusive1,
            Breakfast = Breakfast1,
            Clients = new List<ClientData>
            {
                new ClientData(),
                new ClientData(),
                new ClientData(),
                new ClientData(),
                new ClientData(),
            },
            Facility = Facilitys.Facility2,
            User = Users.User4NotEmployee
        };

        public static readonly Reservation Reservation5User3Facility1NoClient = new Reservation
        {
            AccommodationDate = DateTime.Today.AddDays(8),
            ReleaseDate = DateTime.Today.AddDays(5),
            AllInclusive = AllInClusive1,
            Breakfast = Breakfast1,
            Clients = new List<ClientData>(),
            Facility = Facilitys.Facility1,
            User = Users.User3NotEmployee
        };

        public static readonly Reservation Reservation6User3Facility1NoClient = new Reservation
        {
            AccommodationDate = DateTime.Today.AddDays(-2),
            ReleaseDate = DateTime.Today.AddDays(5),
            AllInclusive = AllInClusive1,
            Breakfast = Breakfast1,
            Clients = new List<ClientData>(),
            Facility = Facilitys.Facility1,
            User = Users.User3NotEmployee
        };

        public static readonly Reservation Reservation7User3Facility1NoClient = new Reservation
        {
            AccommodationDate = DateTime.Today.AddDays(-2),
            ReleaseDate = DateTime.Today.AddDays(5),
            AllInclusive = AllInClusive1,
            Breakfast = Breakfast1,
            Clients = new List<ClientData>
            {
                Users.Client1User,
                Users.Client2User,
                Users.Client3User
            },
            Facility = Facilitys.Facility2,
            User = Users.User3NotEmployee
        };

        public const bool AllInClusive1 = true;
        public const bool Breakfast1 = true;

        public const bool UpdateAllInClusive1 = false;
        public const bool UpdateBreakfast1 = false;
    }
}
