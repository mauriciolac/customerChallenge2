using System.Net.Http;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using customerChallenge;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace CustomerChallengeTests
{
    public class TestCustomerRequests
    {

        private readonly TestServer server;
        private readonly HttpClient client;

        public TestCustomerRequests()
        {
            server = new TestServer(new WebHostBuilder()
            .UseStartup<Startup>());
            client = server.CreateClient();
        }

        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }

        [Fact]
        public async void TestCreate1()
        {
            
            var customer = new Customer
            {
                Name = "John Frusciante",
                Email = "fruscieante@email.com"
            };

            StringContent jsonStringContent = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");

            var postResponse1 = await client.PostAsync("/api/customers", jsonStringContent);
            Assert.Equal(HttpStatusCode.Created, postResponse1.StatusCode);
            var postResponse2 = await client.PostAsJsonAsync("/api/customers", customer);
            Assert.Equal(HttpStatusCode.Created, postResponse2.StatusCode);

        }

        [Fact]
        public async void TestCreate2()
        {

            var customer = new Customer
            {
                Name = "Ronaldo Nazário",
                Email = "r9@email.com"
            };

            var postResponse = await client.PostAsJsonAsync("/api/customers", customer);
            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        }

        [Fact]
        public async void TestCreateDifferentCustomers()
        {

            var customer1 = new Customer
            {
                Name = "John Mayer",
                Email = "mayer@email.com"
            };

            var customer2 = new Customer
            {
                Name = "Robert Plant",
                Email = "plant@email.com"
            };

            var postResponse1 = await client.PostAsJsonAsync("/api/customers", customer1);
            Assert.Equal(HttpStatusCode.Created, postResponse1.StatusCode);
            var created1 = await postResponse1.Content.ReadAsAsync<Customer>();

            var postResponse2 = await client.PostAsJsonAsync("/api/customers", customer2);
            Assert.Equal(HttpStatusCode.Created, postResponse2.StatusCode);
            var created2 = await postResponse2.Content.ReadAsAsync<Customer>();

            Assert.NotEqual(created1.Id, created2.Id);

        }

        [Fact]
        public async void TestUpdateCustomersSameEmail()
        {

            var customer1 = new Customer
            {
                Name = "Paul Gonzales",
                Email = "gonzales@email.com"
            };

            var customer2 = new Customer
            {
                Name = "Raul Gonzales",
                Email = "gonzales@email.com"
            };

            var postResponse1 = await client.PostAsJsonAsync("/api/customers", customer1);
            Assert.Equal(HttpStatusCode.Created, postResponse1.StatusCode);
            var created1 = await postResponse1.Content.ReadAsAsync<Customer>();

            var postResponse2 = await client.PostAsJsonAsync("/api/customers", customer2);
            Assert.Equal(HttpStatusCode.Created, postResponse1.StatusCode);
            var created2 = await postResponse2.Content.ReadAsAsync<Customer>();
            Assert.Equal(created1.Id, created2.Id);
            Assert.Equal(created2.Name, customer2.Name);
            Assert.NotEqual(created1.Name, created2.Name);
                       
        }
        
        [Fact]
        public async void TestEmptyName()
        {

            var customer1 = new Customer
            {
                Name = "",
                Email = "gonzales@email.com"
            };

            var customer2 = new Customer
            {
                Name = "Raul Gonzales",
                Email = ""
            };

            var postResponse1 = await client.PostAsJsonAsync("/api/customers", customer1);
            Assert.Equal(HttpStatusCode.BadRequest, postResponse1.StatusCode);

        }

        [Fact]
        public async void TestEmptyEmail()
        {            
            var customer1 = new Customer
            {
                Name = "Raul Gonzales",
                Email = ""
            };

            var postResponse1 = await client.PostAsJsonAsync("/api/customers", customer1);
            Assert.Equal(HttpStatusCode.BadRequest, postResponse1.StatusCode);
        }

        [Fact]
        public async void TestEmailFormat()
        {
            var customer1 = new Customer
            {
                Name = "Michael Gonzales",
                Email = "wrongemailformat"
            };

            var postResponse1 = await client.PostAsJsonAsync("/api/customers", customer1);
            Assert.Equal(HttpStatusCode.BadRequest, postResponse1.StatusCode);

        }

    }

}
