using AuthenticationJwt.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationJwt.Web.Data
{
    public class MyAppDbContext:DbContext
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
        {
        }

        public DbSet<Prodects> Prodects { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
