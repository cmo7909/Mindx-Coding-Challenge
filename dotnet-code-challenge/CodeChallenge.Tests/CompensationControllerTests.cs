using System.Net;
using System.Net.Http;
using System.Text;
using System;
using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private HttpClient _httpClient;
        private TestServer _testServer;

        [TestInitialize]
        // Attribute ClassInitialize requires this signature (copied from other tests)
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public void InitializeTest()
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [TestCleanup]
        public void CleanUpTest()
        {
            _httpClient?.Dispose();
            _testServer?.Dispose();
        }
    

    [TestMethod]
        public void CreateCompensationRecord_ReturnsCreated_then_ReadByEmployeeId_Returns_Ok()
        {
            var johnId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var payload = new Compensation
            {
                EmployeeId = johnId,
                Salary = 75000m,
                EffectiveDate = new DateTime(2020, 1, 1)
            };
            var json = new JsonSerialization().ToJson(payload);

            var post = _httpClient.PostAsync("api/compensation", new StringContent(json, Encoding.UTF8, "application/json")).Result;

            Assert.AreEqual(HttpStatusCode.Created, post.StatusCode);
            var created = post.DeserializeContent<Compensation>();
            Assert.IsTrue(created.CompensationId > 0);
            Assert.AreEqual(johnId, created.EmployeeId);
            Assert.AreEqual(75000m, created.Salary);

            var get = _httpClient.GetAsync($"api/compensation/{johnId}").Result;

            Assert.AreEqual(HttpStatusCode.OK, get.StatusCode);
            var content = get.DeserializeContent<Compensation>();
            Assert.AreEqual(created.CompensationId, content.CompensationId);
            Assert.AreEqual(created.Salary, content.Salary);
            Assert.IsNotNull(content.Employee);
            Assert.AreEqual(johnId, content.Employee.EmployeeId);
        }

        [TestMethod]
        public void CreateCompensation_ForUnknownEmployee_Returns_NotFound()
        {
            var badCompensation = new Compensation
            {
                EmployeeId = "does-not-exist",
                Salary = 100m,
                EffectiveDate = new DateTime(2020, 1, 1)
            };

            var json = new JsonSerialization().ToJson(badCompensation);

            var post = _httpClient.PostAsync("api/compensation", new StringContent(json, Encoding.UTF8, "application/json")).Result;

            Assert.AreEqual(HttpStatusCode.NotFound, post.StatusCode);
        }

        [TestMethod]
        public void GetCompensation_WhenNoneExists_Returns_NotFound()
        {
            var paulId = "b7839309-3348-463b-a7e3-5de1c168beb3"; 
            var get = _httpClient.GetAsync($"api/compensation/{paulId}").Result;

            Assert.AreEqual(HttpStatusCode.NotFound, get.StatusCode);
        }
    }
    
}