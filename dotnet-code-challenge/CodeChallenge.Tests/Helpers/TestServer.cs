using System;
using System.Net.Http;
using System.Threading.Tasks;

using CodeChallenge.Data;
using CodeChallenge.Models;                  
using Microsoft.AspNetCore.Hosting;          
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace CodeCodeChallenge.Tests.Integration.Helpers
{
    public class TestServer : IDisposable, IAsyncDisposable
    {
        private WebApplicationFactory<Program> applicationFactory;

        public TestServer()
        {
            // Run tests in Production to skip App.cs seeding
            applicationFactory =
                new WebApplicationFactory<Program>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.UseEnvironment("Production");
                    });

            // Fresh in-memory DB, then seed the graph 
            using var scope = applicationFactory.Services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
            SeedBeatlesGraph(ctx);
        }

        public HttpClient NewClient()
        {
            return applicationFactory.CreateClient();
        }

        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)applicationFactory).DisposeAsync();
        }

        public void Dispose()
        {
            ((IDisposable)applicationFactory).Dispose();
        }

        
        //Helper because I could not for the life of me get the data seeded (sort of messy)
        private static void SeedBeatlesGraph(EmployeeContext ctx)
        {
            const string johnId   = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            const string paulId   = "b7839309-3348-463b-a7e3-5de1c168beb3";
            const string ringoId  = "03aa1462-ffa9-4978-901b-7c001562cf6f";
            const string peteId   = "62c1084e-6e34-4630-93fd-9153afb65309";
            const string georgeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c";

            var paul   = new Employee { EmployeeId = paulId,   FirstName = "Paul",   LastName = "McCartney",  Department = "Engineering", Position = "Developer I" };
            var pete   = new Employee { EmployeeId = peteId,   FirstName = "Pete",   LastName = "Best",       Department = "Engineering", Position = "Developer II" };
            var george = new Employee { EmployeeId = georgeId, FirstName = "George", LastName = "Harrison",   Department = "Engineering", Position = "Developer III" };
            var ringo  = new Employee { EmployeeId = ringoId,  FirstName = "Ringo",  LastName = "Starr",      Department = "Engineering", Position = "Developer V" };
            var john   = new Employee { EmployeeId = johnId,   FirstName = "John",   LastName = "Lennon",     Department = "Engineering", Position = "Development Manager" };

            ringo.DirectReports = new() { pete, george };
            john.DirectReports  = new() { paul, ringo };

            ctx.Employees.AddRange(john, paul, ringo, pete, george);
            ctx.SaveChanges();
        }
    }
}
