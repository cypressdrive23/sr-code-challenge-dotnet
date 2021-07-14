using System;
using System.Net;
using System.Net.Http;
using System.Text;
using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using code_challenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void RunTests()
        {
            CreateCompensation_Returns_Created();
            GetCompensationByEmployeeId_Returns_Ok();
        }

        private void CreateCompensation_Returns_Created()
        {
            // Arrange
            var compensation = new Compensation
            {
                salary = 50000.50M, 
                effectiveDate = new DateTime(2021, 7, 14),
                employee = "16a596ae-edd3-4847-99fe-c4518e82c86f"
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
                new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(compensation.employee);
            Assert.AreEqual(compensation.employee, newCompensation.employee);
            Assert.AreEqual(compensation.salary, newCompensation.salary);
            Assert.AreEqual(compensation.effectiveDate, newCompensation.effectiveDate);
        }
        
        private void GetCompensationByEmployeeId_Returns_Ok()
        {
            // Arrange
            var salary = 50000.50M;
            var effectiveDate = new DateTime(2021, 7, 14);
            var employee = "16a596ae-edd3-4847-99fe-c4518e82c86f";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employee}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(salary, compensation.salary);
            Assert.AreEqual(effectiveDate, compensation.effectiveDate);
            Assert.AreEqual(employee, compensation.employee);
        }
    }
}
