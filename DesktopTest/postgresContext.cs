using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql;

#nullable disable

namespace DesktopTest
{
    public partial class postgresContext : DbContext
    {
        public postgresContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<enms>();
        }

        public postgresContext(DbContextOptions<postgresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Subunit> Subunits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=1234");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum(null, "enms", new[] { "male", "female" })
                .HasPostgresExtension("adminpack")
                .HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Birthdate)
                    .HasColumnType("date")
                    .HasColumnName("birthdate");

                entity.Property(e => e.Firstname).HasColumnName("firstname");

                entity.Property(e => e.Lastname).HasColumnName("lastname");

                entity.Property(e => e.Middlename).HasColumnName("middlename");

                entity.Property(e => e.Subunit).HasColumnName("subunit");

                entity.Property(e => e.Gender).HasColumnName("gender");

                
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Creator).HasColumnName("creator");

                //entity.Property(e => e.Orderdate).HasColumnType("time without time zone");
                entity.Property(e => e.Orderdate)
                    .HasColumnType("date")
                    .HasColumnName("Orderdate");

            });

            modelBuilder.Entity<Subunit>(entity =>
            {
                entity.ToTable("Subunit");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Leader).HasColumnName("leader");

                entity.Property(e => e.Name).HasColumnName("name");

               
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
