using System.Net;
using System.Net.Http;
using System.Text;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature (copied from other tests)
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient?.Dispose();
            _testServer?.Dispose();
        }

        [TestMethod]
        public void GetReportingStructure_For_John_Returns_Ok_AndCountIs4()
        {
            var johnId = "16a596ae-edd3-4847-99fe-c4518e82c86f";

            var response = _httpClient.GetAsync($"api/employee/{johnId}/reportingStructure").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var rs = response.DeserializeContent<ReportingStructure>();
            Assert.IsNotNull(rs);
            Assert.IsNotNull(rs.Employee);
            Assert.AreEqual(johnId, rs.Employee.EmployeeId);
            Assert.AreEqual(4, rs.NumberOfReports); 
        }

        [TestMethod]
        public void GetReportingStructure_ForUnknownEmployee_Returns_NotFound()
        {
            var response = _httpClient.GetAsync("api/employee/not-a-real-id/reportingStructure").Result;
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
    
}