using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TokenGenerator.Data
{
    public class TokenGeneratorContext: DbContext
    {
        public TokenGeneratorContext(DbContextOptions<TokenGeneratorContext> options)
            : base(options)
        {
        }

        public DbSet<TokenGenerator.Models.CustomerCard> CustomerCard { get; set; }
    }
}
