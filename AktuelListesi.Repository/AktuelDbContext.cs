using AktuelListesi.Data.Tables;
using Microsoft.EntityFrameworkCore;
using System;

namespace AktuelListesi.Repository
{
    public class AktuelDbContext : DbContext
    {
        public AktuelDbContext(DbContextOptions<AktuelDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Aktuel> Aktuels { get; set; }
        public virtual DbSet<AktuelPage> AktuelPages { get; set; }
    }
}
