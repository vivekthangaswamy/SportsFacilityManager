using Data;
using Data.Enums;
using Data.Models;
using NUnit.Framework;
using Services.Data;
using Services.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.Common;
using Tests.Data;
using Web.Models;
using Web.Models.Facilitys;
using Facilitys = Tests.Data.Facilitys;

namespace Tests.Service.Tests
{
    [NonParallelizable]
    public class FacilityserviceTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            MappingConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);
        }

        [Test]
        public async Task AddFacility_ShouldAddAFacility()
        {
            //Arange
            List<Facility> Facilitys = new() { Facilitys.Facility1 };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);

            //Act
            await Facilityservice.AddFacility(Facilitys.Facility2);

            //Assert
            Assert.AreEqual(Facilitys.Count + 1, context.Facilitys.Count());
        }

        [Test]
        public async Task GetFacility_ShouldGetAFacilityById()
        {
            //Arrange
            List<Facility> Facilitys = new() { Facilitys.Facility1 };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys);

            var Facilityservice = new Facilityservices(context);

            //Act
            var resultFacility = await Facilityservice.GetFacility<FacilityViewModel>(Facilitys.Facility1.Id);

            //Assert
            Assert.IsNotNull(resultFacility);
            Assert.AreEqual(Facilitys.Facility1.Id, resultFacility.Id);
            Assert.AreEqual(Facilitys.Facility1.Capacity, resultFacility.Capacity);
        }
        [Test]
        public async Task GetAll_ShouldGetAllFacilitys()
        {
            //Arrange
            List<Facility> Facilitys = new()
            {
                Facilitys.Facility1,
                Facilitys.Facility2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);

            //Act
            var resultList = await Facilityservice.GetAll<FacilityViewModel>();

            //
            Assert.IsNotNull(resultList);
            Assert.AreEqual(Facilitys.Count, resultList.Count());
        }

        [Test]
        public async Task DeleteFacility_ShouldRemoveAFacility()
        {
            //Arrange
            List<Facility> Facilitys = new()
            {
                Facilitys.Facility2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);

            //Act
            await Facilityservice.DeleteFacility(Facilitys.Facility2.Id);

            //Assert
            Assert.AreEqual(0, context.Facilitys.Count());
        }

        [Test]
        public async Task Update_ShouldUpdateAFacility()
        {
            //Arrange
            List<Facilitys> Facilitys = new()
            {
                Facilitys.Facility2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);

            //Act
            await Facilityservice.UpdateFacility(Facilitys.Facility2.Id, Facilitys.Facility2);

            //Assert      
            Assert.AreEqual(context.Facilitys.FirstOrDefault().IsTaken, Facilitys.Facility2.IsTaken);
            Assert.AreEqual(context.Facilitys.FirstOrDefault().Number, Facilitys.Facility2.Number);
            Assert.AreEqual(context.Facilitys.FirstOrDefault().AdultPrice, Facilitys.Facility2.AdultPrice);
            Assert.AreEqual(context.Facilitys.FirstOrDefault().ChildrenPrice, Facilitys.Facility2.ChildrenPrice);
            Assert.AreEqual(context.Facilitys.FirstOrDefault().Capacity, Facilitys.Facility2.Capacity);
            Assert.AreEqual(context.Facilitys.FirstOrDefault().Type, Facilitys.Facility2.Type);
        }

        [Test]
        public async Task CountAllFreeFacilitysAtPresent_ShouldCountAllFreeFacilitysAtPresent()
        {
            //Arrange
            List<Facilitys> Facilitys = new()
            {
                Facilitys.Facility1TakenAtPresent1ReservationUser4,
                Facilitys.Facility2TakenAtPresent,
                Facilitys.Facility1FreeAtPresent1ReservationUser3
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);

            //Act
            var count = await Facilityservice.CountFreeFacilitysAtPresent();

            //Assert
            Assert.AreEqual(1, count);
        }
        [Test]
        public async Task GetAllFreeAtPresentFacilitys_ShouldReturnAllFreeAtPresentFacilitys()
        {
            //Arrange
            List<Facilitys> Facilitys = new()
            {
                Facilitys.Facility1TakenAtPresent1ReservationUser4,
                Facilitys.Facility2TakenAtPresent,
                Facilitys.Facility1FreeAtPresent1ReservationUser3
            };
            List<ApplicationUser> users = new()
            {
                Users.User3NotEmployee
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(users)
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);

            //Act
            var result = await Facilityservice.GetAllFreeFacilitysAtPresent<FacilityViewModel>();

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
        }
        [Test]
        public async Task CountFreeFacilitysAtPresent_ShouldCountAllFreeFacilitys()
        {
            //Arrange
            List<Facilitys> Facilitys = new()
            {
                Facilitys.Facility1TakenAtPresent1ReservationUser4,
                Facilitys.Facility2TakenAtPresent,
                Facilitys.Facility1FreeAtPresent1ReservationUser3
            };

            List<ApplicationUser> users = new()
            {
                Users.User3NotEmployee
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(users)
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);

            //Act
            var result = await Facilityservice.CountFreeFacilitysAtPresent();

            //Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public async Task GetSearchResults_ShouldReturnAllFacilitysContainingTheCriteria()
        {
            List<Facilitys> Facilitys = new()
            {
                Facilitys.Facility1FreeAtPresent1ReservationUser3,
                Facilitys.Facility1TakenAtPresent1ReservationUser4
            };


            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);

            //Act
            var result1 = await Facilityservice.GetSearchResults<FacilityViewModel>(true, new FacilityType[] { FacilityType.Penthouse }, 2);
            var result2 = await Facilityservice.GetSearchResults<FacilityViewModel>(false, new FacilityType[] { FacilityType.Apartment }, 2);

            //Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.AreEqual(Facilitys.Facility1FreeAtPresent1ReservationUser3.Id, result1.FirstOrDefault().Id);
            Assert.AreEqual(Facilitys.Facility1FreeAtPresent1ReservationUser3.Number, result1.FirstOrDefault().Number);
            Assert.AreEqual(Facilitys.Facility1FreeAtPresent1ReservationUser3.Type, result1.FirstOrDefault().Type);
            Assert.AreEqual(Facilitys.Facility1TakenAtPresent1ReservationUser4.Id, result2.FirstOrDefault().Id);
            Assert.AreEqual(Facilitys.Facility1TakenAtPresent1ReservationUser4.Number, result2.FirstOrDefault().Number);
            Assert.AreEqual(Facilitys.Facility1TakenAtPresent1ReservationUser4.Type, result2.FirstOrDefault().Type);
        }

        [Test]
        public async Task GetAllByCapacity_ShouldGetAllFacilitysByCapacity()
        {
            //Arange
            List<Facilitys> Facilitys = new()
            {
                Facilitys.Facility1,
                Facilitys.Facility2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);
            //Act
            var result = await Facilityservice.GetAllByCapacity<FacilityViewModel>(4);
            //Arrange
            Assert.AreEqual(1, result.Count());
        }
        [Test]
        public async Task GetAllByType_ShouldGetAllFacilitysByType()
        {
            //Arange
            List<Facilitys> Facilitys = new()
            {
                Facilitys.Facility1,
                Facilitys.Facility2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);

            //Act
            var result1 = await Facilityservice.GetAllByType<FacilityViewModel>(Facilitys.Facility2.Type);
            var result2 = await Facilityservice.GetAllByType<FacilityViewModel>(Facilitys.Facility1.Type);

            //Arrange
            Assert.AreEqual(1, result1.Count());
            Assert.AreEqual(1, result2.Count());
        }

        [Test]
        public async Task CountAllFacilitys_ShouldCountAllFacilitysInDb()
        {
            //Arange
            List<Facilitys> Facilitys = new()
            {
                Facilitys.Facility1,
                Facilitys.Facility2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);

            //Act
            var count = Facilityservice.CountAllFacilitys();

            //Arrange
            Assert.AreEqual(Facilitys.Count(), count);
        }

        [Test]
        public async Task GetMinPrice_ShouldReturnTheLowestAdultPrice()
        {
            //Arange
            List<Facilitys> Facilitys = new()
            {
                Facilitys.Facility1,
                Facilitys.Facility2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);

            //Act
            var minPrice = Facilityservice.GetMinPrice();

            //Arrange
            Assert.AreEqual(Facilitys.First().AdultPrice, minPrice.Result);
        }

        [Test]
        public async Task GetMaxPrice_ShouldReturnTheHighestAdultPrice()
        {
            //Arange
            List<Facilitys> Facilitys = new()
            {
                Facilitys.Facility1,
                Facilitys.Facility2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);

            //Act
            var maxPrice = Facilityservice.GetMaxPrice();

            //Arrange
            Assert.AreEqual(Facilitys[1].AdultPrice, maxPrice.Result);
        }

        [Test]
        public async Task IsFacilityNumerFree_ShouldReturnIfTheFacilityNumberIsFree()
        {
            //Arange
            List<Facilitys> Facilitys = new()
            {
                Facilitys.Facility1,
                Facilitys.Facility2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);

            //Act
            var result1 = Facilityservice.IsFacilityNumberFree(2);
            var result2 = Facilityservice.IsFacilityNumberFree(3);

            //Arrange
            Assert.AreEqual(false, result1.Result);
            Assert.AreEqual(true, result2.Result);
        }

        [Test]
        public async Task GetMaxCapacity_ShouldReturnTheMaxCapacity()
        {
            //Arange
            List<Facilitys> Facilitys = new()
            {
                Facilitys.Facility1,
                Facilitys.Facility2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(Facilitys);
            var Facilityservice = new Facilityservices(context);

            //Act
            var maxCapacity = Facilityservice.GetMaxCapacity();

            //Arrange
            Assert.AreEqual(Facilitys.Facility2.Capacity, maxCapacity.Result);
        }
    }
}
