using LoanProfile.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanProfile.Data
{
    public class ProfileDbContext : DbContext
    {
        public ProfileDbContext(DbContextOptions<ProfileDbContext> options)
            : base(options) { }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}