using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTesting
{
    /// <summary>
    ///   During Testing, we don't want the application to use or make default EF SQL Server
    ///   Instead, use EF in-memory collection
    ///   The reason why it is Factory<Program>, so it literally is the code in Program.cs
    ///   Nuget: AspNetCore.Mvc.Testing & EfCore.InMemory
    /// </summary>
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                // In the StartUp.cs, service ApplicationDbContext options is config to .UseSqlServer -- remove it
                // Descriptor is the service type and lifetime
                ServiceDescriptor? descriptor =  services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if(descriptor != null)
                {
                    // Instead, introduce an in-memory somthing
                    services.Remove(descriptor);
                }
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DatabaseForTesting");
                });
            });
        }
    }
}
