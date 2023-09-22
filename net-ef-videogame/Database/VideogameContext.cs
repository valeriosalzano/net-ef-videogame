using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using net_ef_videogame.Models;

namespace net_ef_videogame.Database
{
    public class VideogameContext : DbContext
    {
        public DbSet<Videogame> Videogames { get; set; }
        public DbSet<SoftwareHouse> SoftwareHouse { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=EFVideogameContext;Integrated Security=True;TrustServerCertificate=True");
        }
    }
}
