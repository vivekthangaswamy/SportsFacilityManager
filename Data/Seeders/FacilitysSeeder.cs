using Data.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeders
{
    /// <summary>
    /// Facilitys table seeder
    /// </summary>
    public class FacilitysSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, ILogger logger)
        {
            if (dbContext.Facilitys.Any())
            {
                return;
            }

            await dbContext.Facilitys.AddRangeAsync(new Facility[]
           {
                new Facility
                {
                    Id="ExampleFacility1",
                    AdultPrice=50,
                    ChildrenPrice=30,
                    Capacity=5,
                    Number=105,
                    Type=Enums.FacilityType.YogaStudio,
                    ImageUrl="https://cf.bstatic.com/images/hotel/max1024x768/197/197179243.jpg",
                },
                new Facility
                {
                    Id="ExampleFacility2",
                    AdultPrice=30,
                    ChildrenPrice=10,
                    Capacity=3,
                    Number=205,
                    Type=Enums.FacilityType.SwimmingLane,
                    ImageUrl="https://pix10.agoda.net/hotelImages/5668227/0/7542736b26b0676a0e9e3c4aab831241.jpg?s=1024x768",
                }
           });

            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Finished executing {nameof(FacilitysSeeder)}");
        }
    }
}
