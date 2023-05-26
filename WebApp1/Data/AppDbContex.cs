using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApp1.Models;
using static System.Net.WebRequestMethods;

namespace WebApp1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
       


    }
}
