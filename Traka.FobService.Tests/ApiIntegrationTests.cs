using Microsoft.AspNetCore.TestHost;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using Traka.FobService.Model;
using System.Linq;

namespace Traka.FobService.IntegrationTests
{
    public class UserTests
    {
        private HttpClient _client;
        private TestServer _server;

        [SetUp]
        public void Setup()
        {
            _server = new TestServer(new WebHostBuilder()
                                         .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Test]
        public async Task Get_Returns_All_Users()
        {
            var response = await _client.GetAsync("/user");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var users = await response.Content.ReadFromJsonAsync<IEnumerable<User>>();
            Assert.That(users, Has.Count.EqualTo(3));
        }

        [TestCase]
        public async Task GetById_Returns_Correct_User()
        {
            var response = await _client.GetAsync("/user/1");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var user = await response.Content.ReadFromJsonAsync<User>();
            Assert.That(user.Username, Is.EqualTo("Sneezy"));
        }

        [TestCase]
        public async Task GetById_UserDoesNotExist_Return_Error()
        {
            var response = await _client.GetAsync("/user/8");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase]
        public async Task Post_Adds_New_User()
        {
            var response = await _client.PostAsync("/user",
                                                   JsonContent.Create(new User
                                                   {
                                                       Id = 4,
                                                       Username = "Dopey"
                                                   }));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = await _client.GetAsync("/user/4");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var user = await response.Content.ReadFromJsonAsync<User>();
            Assert.That(user.Username, Is.EqualTo("Dopey"));
        }

        [TestCase]
        public async Task Put_Updates_Existing_User()
        {
            var response = await _client.PutAsync("/user/3", JsonContent.Create(new User {Username = "Happy"}));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = await _client.GetAsync("/user/3");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var user = await response.Content.ReadFromJsonAsync<User>();
            Assert.That(user.Username, Is.EqualTo("Happy"));
        }

        [TestCase]
        public async Task Delete_Deletes_User()
        {
            var response = await _client.DeleteAsync("/user/3");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = await _client.GetAsync("/user/3");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase]
        public async Task GetHeldFobs_Returns_Fobs_For_User()
        {
            var response = await _client.GetAsync("/user/1/fobs");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var fobs = await response.Content.ReadFromJsonAsync<IEnumerable<Fob>>();
            Assert.That(fobs, Has.Count.EqualTo(2));
            Assert.That(fobs, Has.Exactly(1).Matches<Fob>(fob => fob.SerialNumber == "A6E77B080000"));
            Assert.That(fobs, Has.Exactly(1).Matches<Fob>(fob => fob.SerialNumber == "BC2433AF8299"));
        }

        [TestCase]
        public async Task Put_Updates_FobUser()
        {
            var response = await _client.PutAsync("/system/1/assignfob/5/3", null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = await _client.GetAsync("/user/3/fobs");
            var fobs = await response.Content.ReadFromJsonAsync<List<Fob>>();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(fobs.FirstOrDefault().Position == 5);
        }
    }
}
