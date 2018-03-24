using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CustomerChallengeTests
{
    public class TestCustomerRequests
    {

        //private string baseUrl = "https://fast-badlands-14317.herokuapp.com/api/customers";
        private string baseUrl = "http://localhost:24649/";
        private HttpClient client;

        public TestCustomerRequests()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public class Customer
        {
            public int Id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
        }

        [Fact]
        public async void testCreate1()
        {

            var customer = new Customer
            {
                name = "John Frusciante",
                email = "fruscieante@email.com"
            };

            var postResponse = await client.PostAsJsonAsync("/api/customers", customer);
            var created = await postResponse.Content.ReadAsAsync<Customer>();
            Assert.True(created.Id != 0, "id:" + created.Id.ToString());
            //Assert.True(postResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async void testCreate2()
        {

            var customer = new Customer
            {
                name = "Ronaldo Nazário",
                email = "r9@email.com"
            };

            var postResponse = await client.PostAsJsonAsync("/api/customers", customer);
            var created = await postResponse.Content.ReadAsAsync<Customer>();
            Assert.True(created.Id != 0, "id:" + created.Id.ToString());
            //Assert.True(postResponse.IsSuccessStatusCode);


        }

        [Fact]
        public async void testCreateDifferentCustomers()
        {

            var customer1 = new Customer
            {
                name = "John Mayer",
                email = "mayer@email.com"
            };

            var customer2 = new Customer
            {
                name = "Robert Plant",
                email = "plant@email.com"
            };

            var postResponse1 = await client.PostAsJsonAsync("/api/customers", customer1);
            var created1 = await postResponse1.Content.ReadAsAsync<Customer>();

            var postResponse2 = await client.PostAsJsonAsync("/api/customers", customer2);
            var created2 = await postResponse2.Content.ReadAsAsync<Customer>();


            Assert.True(created1.Id != created2.Id, "id1:" + created1.Id.ToString() + " id2:" + created2.Id);

        }

        [Fact]
        public async void testUpdateCustomersSameEmail()
        {

            var customer1 = new Customer
            {
                name = "Paul Gonzales",
                email = "gonzales@email.com"
            };

            var customer2 = new Customer
            {
                name = "Raul Gonzales",
                email = "gonzales@email.com"
            };

            var postResponse1 = await client.PostAsJsonAsync("/api/customers", customer1);
            var created1 = await postResponse1.Content.ReadAsAsync<Customer>();

            var postResponse2 = await client.PostAsJsonAsync("/api/customers", customer2);
            var created2 = await postResponse2.Content.ReadAsAsync<Customer>();

            if (created1.Id == created2.Id && created1.name != created2.name && created2.name == customer2.name)
            {
                Assert.True(true);
            }
            else
            {
                Assert.True(false, "id1:" + created1.Id + "id2:" + created2.Id);
            }
        }

        [Fact]
        public async void testEmptyName()
        {

            var customer1 = new Customer
            {
                name = "",
                email = "gonzales@email.com"
            };

            var customer2 = new Customer
            {
                name = "Raul Gonzales",
                email = ""
            };

            var postResponse1 = await client.PostAsJsonAsync("/api/customers", customer1);
            Assert.False(postResponse1.IsSuccessStatusCode);

        }

        [Fact]
        public async void testEmptyEmail()
        {            
            var customer1 = new Customer
            {
                name = "Raul Gonzales",
                email = ""
            };

            var postResponse1 = await client.PostAsJsonAsync("/api/customers", customer1);
            Assert.False(postResponse1.IsSuccessStatusCode);
        }

        [Fact]
        public async void testEmailFormat()
        {
            var customer1 = new Customer
            {
                name = "Michael Gonzales",
                email = "wrongemailformat"
            };

            var postResponse1 = await client.PostAsJsonAsync("/api/customers", customer1);
            Assert.False(postResponse1.IsSuccessStatusCode);
        }

    }

}
