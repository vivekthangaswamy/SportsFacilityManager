using Data;
using Data.Models;
using NUnit.Framework;
using Services.Data;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.Common;
using Tests.Data;
using Web.Models;
using Web.Models.Reservations;

/// <summary>
/// Tests of the Controller layer project
/// </summary>
namespace Tests.Service.Tests
{
    class ReservationServiceTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            MappingConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);
        }

        [Test]
        public async Task AddReservation_ShouldAddReservation()
        {
            // Arange
            List<Setting> settings = new()
            {
                Settings.AllInclusive,
                Settings.Breakfast
            };

            List<Facility> Facilitys = new()
            {
                Facilitys.Facility1
            };

            List<ApplicationUser> users = new()
            {
                Users.User3NotEmployee
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(settings)
                                                                .SeedAsync(users)
                                                                .SeedAsync(Facilitys);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var reservation = await service.AddReservation(Reservations.Reservation1User3Facility1NoClient.Facility.Id,
                                         Reservations.Reservation1User3Facility1NoClient.AccommodationDate,
                                         Reservations.Reservation1User3Facility1NoClient.ReleaseDate,
                                         Reservations.AllInClusive1,
                                         Reservations.Breakfast1,
                                         Reservations.Reservation1User3Facility1NoClient.Clients,
                                         Reservations.Reservation1User3Facility1NoClient.User
                                         );

            // Assert
            Assert.NotNull(reservation);
            Assert.AreEqual(1, context.Reservations.Count());
        }

        [Test]
        public async Task AddReservation_ShouldAddReservationWithBreakfast()
        {
            // Arange
            List<Setting> settings = new()
            {
                Settings.AllInclusive,
                Settings.Breakfast
            };

            List<Facility> Facilitys = new()
            {
                Facilitys.Facility2
            };

            List<ApplicationUser> users = new()
            {
                Users.User3NotEmployee
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(settings)
                                                                .SeedAsync(users)
                                                                .SeedAsync(Facilitys);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var reservation = await service.AddReservation(Reservations.Reservation2User4Facility2NoClient.Facility.Id,
                                         Reservations.Reservation2User4Facility2NoClient.AccommodationDate,
                                         Reservations.Reservation2User4Facility2NoClient.ReleaseDate,
                                         Reservations.UpdateAllInClusive1,
                                         Reservations.Breakfast1,
                                         Reservations.Reservation2User4Facility2NoClient.Clients,
                                         Reservations.Reservation2User4Facility2NoClient.User
                                         );

            // Assert
            Assert.NotNull(reservation);
        }

        [Test]
        public async Task AddReservation_ShouldNotAddReservation()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation3User4Facility2NoClient
            };

            List<Facility> Facilitys = new()
            {
                Facilitys.Facility2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys)
                                                                .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var reservation = await service.AddReservation(Reservations.Reservation4User4Facility2NoClient.Facility.Id,
                                         Reservations.Reservation4User4Facility2NoClient.AccommodationDate,
                                         Reservations.Reservation4User4Facility2NoClient.ReleaseDate,
                                         Reservations.AllInClusive1,
                                         Reservations.Breakfast1,
                                         Reservations.Reservation4User4Facility2NoClient.Clients,
                                         Reservations.Reservation4User4Facility2NoClient.User);

            // Assert
            Assert.Null(reservation);
        }

        [Test]
        public async Task AddReservation_ShouldNotAddReservationForUnexistingFacility()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation3User4Facility2NoClient
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var reservation1 = await service.AddReservation("2",
                                         Reservations.Reservation4User4Facility2NoClient.AccommodationDate,
                                         Reservations.Reservation4User4Facility2NoClient.ReleaseDate,
                                         Reservations.AllInClusive1,
                                         Reservations.Breakfast1,
                                         Reservations.Reservation4User4Facility2NoClient.Clients,
                                         Reservations.Reservation4User4Facility2NoClient.User);

            var reservation2 = await service.AddReservation(Reservations.Reservation4User4Facility2NoClient.Facility.Id,
                                         Reservations.Reservation4User4Facility2NoClient.AccommodationDate,
                                         Reservations.Reservation4User4Facility2NoClient.ReleaseDate,
                                         Reservations.AllInClusive1,
                                         Reservations.Breakfast1,
                                         Reservations.Reservation4User4Facility2NoClient.Clients,
                                         Reservations.Reservation4User4Facility2NoClient.User);

            // Assert
            Assert.Null(reservation1);
            Assert.Null(reservation2);
        }

        [Test]
        public async Task AddReservation_ShouldNotAddReservationWithInvalidDate()
        {
            // Arange
            ApplicationDbContext context = InMemoryFactory.InitializeContext();

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var reservation1 = await service.AddReservation(Reservations.Reservation5User3Facility1NoClient.Facility.Id,
                                         Reservations.Reservation5User3Facility1NoClient.AccommodationDate,
                                         Reservations.Reservation5User3Facility1NoClient.ReleaseDate,
                                         Reservations.AllInClusive1,
                                         Reservations.Breakfast1,
                                         Reservations.Reservation5User3Facility1NoClient.Clients,
                                         Reservations.Reservation5User3Facility1NoClient.User);

            var reservation2 = await service.AddReservation(Reservations.Reservation6User3Facility1NoClient.Facility.Id,
                                         Reservations.Reservation6User3Facility1NoClient.AccommodationDate,
                                         Reservations.Reservation6User3Facility1NoClient.ReleaseDate,
                                         Reservations.AllInClusive1,
                                         Reservations.Breakfast1,
                                         Reservations.Reservation6User3Facility1NoClient.Clients,
                                         Reservations.Reservation6User3Facility1NoClient.User);
            // Assert
            Assert.Null(reservation1);
            Assert.Null(reservation2);
        }

        [Test]
        public async Task UpdateReservation_ShouldNotUpdateReservation()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation1User3Facility1NoClient
            };

            List<Setting> settings = new()
            {
                Settings.AllInclusive,
                Settings.Breakfast
            };

            List<ApplicationUser> users = new()
            {
                Users.User3NotEmployee
            };

            List<ClientData> clients = new()
            {
                new ClientData
                {
                    Id = null,
                    Email = "email",
                    FullName = "name"
                }
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(settings)
                                                                .SeedAsync(users);

            context.Reservations.RemoveRange(context.Reservations.ToList());
            context.AddRange(reservationsData);
            context.SaveChanges();

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var result = await service.UpdateReservation(Reservations.Reservation1User3Facility1NoClient.Id,
                                            DateTime.Today.AddDays(7),
                                            DateTime.Today.AddDays(9),
                                            Reservations.UpdateAllInClusive1,
                                            Reservations.UpdateBreakfast1,
                                            clients,
                                            Users.User1Employee);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteReservation_ShouldDeleteReservation()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation1User3Facility1NoClient
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            bool result1 = await service.DeleteReservation(Reservations.Reservation1User3Facility1NoClient.Id);
            bool result2 = await service.DeleteReservation("2");

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        [Test]
        public async Task GetReservation_ShouldReturnReservation()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation1User3Facility1NoClient,
                Reservations.Reservation2User4Facility2NoClient
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var reservation = await service.GetReservation<ReservationViewModel>(
                                                           Reservations.Reservation2User4Facility2NoClient.Id);

            // Assert
            Assert.AreEqual(Reservations.Reservation2User4Facility2NoClient.Id, reservation.Id);
            Assert.AreEqual(Reservations.Reservation2User4Facility2NoClient.ReleaseDate, reservation.ReleaseDate);
            Assert.AreEqual(Reservations.Reservation2User4Facility2NoClient.Price, reservation.Price);
            Assert.AreEqual(Reservations.Reservation2User4Facility2NoClient.AllInclusive, reservation.AllInclusive);
            Assert.AreEqual(Reservations.Reservation2User4Facility2NoClient.Breakfast, reservation.Breakfast);
        }

        [Test]
        public async Task GetReservationsForUser_ReturnsUsersReservations()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation1User3Facility1NoClient,
                Reservations.Reservation2User4Facility2NoClient,
                Reservations.Reservation3User4Facility2NoClient
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var userReservations = await service.GetReservationsForUser<ReservationViewModel>(
                                                                        Users.User4NotEmployee.Id);

            // Assert
            Assert.AreEqual(context.Reservations.Count(x => x.User.Id == Users.User4NotEmployee.Id), userReservations.Count());
        }

        [Test]
        public async Task GetForUserOnPage_ShouldReturnReservationsForUserOnPage()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation1User3Facility1NoClient,
                Reservations.Reservation2User4Facility2NoClient,
                Reservations.Reservation3User4Facility2NoClient
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var userReservations = await service.GetForUserOnPage<ReservationViewModel>(
                                                                  Users.User4NotEmployee.Id, 1, 2);

            // Assert
            Assert.AreEqual(2, userReservations.Count());
        }

        [Test]
        public async Task GetAll_ShouldReturnAllReservations()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation1User3Facility1NoClient,
                Reservations.Reservation2User4Facility2NoClient,
                Reservations.Reservation3User4Facility2NoClient
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var allReservations = await service.GetAll<ReservationViewModel>();

            // Assert
            Assert.AreEqual(context.Reservations.Count(), allReservations.Count());
        }

        [Test]
        public async Task CountAll_ShouldReturnTheCountOfAllReservations()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation1User3Facility1NoClient,
                Reservations.Reservation2User4Facility2NoClient,
                Reservations.Reservation3User4Facility2NoClient
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var allReservationsCount = await service.CountAllReservations();

            // Assert
            Assert.AreEqual(context.Reservations.Count(), allReservationsCount);
        }


        [Test]
        public async Task UpdateClientsForReservation_ShouldUpdateClientsForReservation()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation7User3Facility1NoClient
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var allClients = await service.UpdateClientsForReservation(
                                                Reservations.Reservation7User3Facility1NoClient.Id,
                                                new List<ClientData>
                                                {
                                                    Users.Client2User,
                                                    Users.Client3User,
                                                    Users.Client4User
                                                });

            // Assert
            Assert.AreEqual(3, allClients.Count());
        }
    }
}
