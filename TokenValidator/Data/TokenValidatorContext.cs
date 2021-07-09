using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenValidator.Data
{
    public class TokenValidatorContext : DbContext
    {
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<TokenValidator.Models.CustomerData>().ToTable("CustomerCard");
        //}
        public TokenValidatorContext(DbContextOptions<TokenValidatorContext> options)
          : base(options)
        {
        }

        public DbSet<TokenValidator.Models.CustomerData> CustomerCard { get; set; }
    }
}
