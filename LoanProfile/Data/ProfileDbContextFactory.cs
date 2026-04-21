using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LoanProfile.Data
{
    public class ProfileDbContextFactory : IDesignTimeDbContextFactory<ProfileDbContext>
    {
        public ProfileDbContext CreateDbContext(string[] args)
        {
            // Load configuration (appsettings.json)
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ProfileDbContext>();

            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
            );

            return new ProfileDbContext(optionsBuilder.Options);
        }
    }
}