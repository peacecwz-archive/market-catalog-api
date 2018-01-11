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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_buffercache")
                        .HasPostgresExtension("pg_stat_statements");

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .UseNpgsqlSerialColumn();
            });

            modelBuilder.Entity<Aktuel>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .UseNpgsqlSerialColumn();
            });

            modelBuilder.Entity<AktuelPage>(entity =>
            {
                entity.HasKey(x => x.Id);
                
                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .UseNpgsqlSerialColumn();

            });
        }
    }
}
