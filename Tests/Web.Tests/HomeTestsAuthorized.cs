using System.Net;
using System.Net.Http;
using Tests.Common;
using Xunit;

/// <summary>
/// Tests of the View layer project
/// </summary>
[assembly: CollectionBehavior(DisableTestParallelization = true, MaxParallelThreads = 1)]
namespace Tests.Web.Tests
{
    [CollectionDefinition("HomeTestsAuthorized", DisableParallelization = true)]
    public class HomeTestsAuthorized : IClassFixture<CustomAppFactory>, IClassFixture<DisposableClassFixture>
    {
        private readonly CustomAppFactory factory;
        private readonly HttpClient client;

        public HomeTestsAuthorized(CustomAppFactory factory)
        {
            this.factory = factory;
            this.client = this.factory.WithWebHostBuilder(builder =>
            {
                ///
            }).CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Theory]
        [InlineData("/Facilitys/create")]
        [InlineData("/Facilitys/")]
        [InlineData("/Facilitys/update/ExampleFacility1")]
        [InlineData("/Facilitys/details/ExampleFacility1")]
        [InlineData("/reservations")]
        [InlineData("/reservations/create/ExampleFacility1")]
        [InlineData("/reservations/details/ExampleReservation1")]
        [InlineData("/reservations/update/ExampleReservation1")]
        [InlineData("/reservations/all")]
        [InlineData("/reservations/all/-2&pageSize=-20")]
        [InlineData("/reservations/all/500&pageSize=1")]
        [InlineData("/users/add")]
        [InlineData("/users/all")]
        [InlineData("/identity/account/manage")]
        [InlineData("/identity/account/login")]
        [InlineData("/identity/account/manage/personaldata")]
        [InlineData("/identity/account/manage/setpassword")]
        [InlineData("/identity/account/ForgotPasswordConfirmation")]
        [InlineData("/identity/account/lockout")]
        [InlineData("/identity/account/manage/deletepersonaldata")]
        [InlineData("/users/promote/Admin")]
        [InlineData("/users/index?search=Admin")]
        [InlineData("/users/index")]
        [InlineData("/users/all?search=Admin")]
        [InlineData("/settings")]
        public async void Get_ShouldReturnPage(string url)
        {
            var res = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }

        [Theory]
        [InlineData("/Facilitys/details/notexistingFacility")]
        [InlineData("/Facilitys/update")]
        [InlineData("/reservations/create")]
        [InlineData("/reservations/details")]
        [InlineData("/reservations/update")]
        [InlineData("/users/update/notexistinguser")]
        [InlineData("/Facilitys/update/notexisting1")]
        [InlineData("/users/promote/notexistinguser")]
        public async void Get_ShouldReturnNotFound(string url)
        {
            var res = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
        }

        [Theory]
        [InlineData("/Facilitys/details")]
        [InlineData("/reservations/create")]
        [InlineData("/reservations/details")]
        [InlineData("/reservations/update")]
        [InlineData("/users/update/NotExistingUser")]
        [InlineData("/users/promote/NotExistingUser")]
        [InlineData("/users/update")]
        [InlineData("/users/promote")]
        [InlineData("/Facilitys/update/notexisting1")]
        [InlineData("/Facilitys/delete/notexisitng")]
        public async void Post_ShouldReturnNotFound(string url)
        {
            var res = await client.PostAsync(url, null);

            Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
        }


        [Theory]
        [InlineData("/users/all")]
        public async void Post_ShouldReturnOk(string url)
        {
            var res = await client.PostAsync(url, null);

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }


        [Theory]
        [InlineData("/Facilitys/delete/ExampleFacility2")]
        public async void Post_ShouldReturnRedirect(string url)
        {
            var res = await client.PostAsync(url, null);

            Assert.Equal(HttpStatusCode.Redirect, res.StatusCode);
        }
    }
}
