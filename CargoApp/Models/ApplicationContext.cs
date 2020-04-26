using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoApp.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Logistician> Logisticians { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Passport> Passports { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
             //Database.EnsureCreated();
        }

        public ApplicationContext()
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=cargo_app_db;Trusted_Connection=True;");
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rating>().HasKey(r => new { r.ClientId, r.CompanyId });

            modelBuilder.Entity<Company>().HasAlternateKey(c => c.Ogrn);
            modelBuilder.Entity<Company>().HasAlternateKey(c => new { c.Inn, c.Kpp });

            //HasIndex()

            //modelBuilder.Entity<UserRegData>().HasAlternateKey(u => u.Login);
            modelBuilder.Entity<Logistician>().HasAlternateKey(u => u.Login);
            modelBuilder.Entity<Driver>().HasAlternateKey(u => u.Login);
            modelBuilder.Entity<Client>().HasAlternateKey(u => u.Login);

            modelBuilder.Entity<UserRegData>()
                .Property(u => u.Name)
                .HasDefaultValue("Пользователь");

            modelBuilder.Entity<Logistician>()
                .HasOne(l => l.Company)
                .WithMany(c => c.Logisticians)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Driver>()
                .HasOne(d => d.Company)
                .WithMany(c => c.Drivers)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Logistician>()
                .HasMany(l => l.Messages)
                .WithOne(m => m.Logistician)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Client)
                .OnDelete(DeleteBehavior.ClientSetNull);


            //  modelBuilder.Entity<DeliveryArea>().HasKey(d => new { d.Latitude, d.Longitude, d.Radius });
            // modelBuilder.Entity<Passport>().HasKey(p => new { p.Series, p.Number });

            //  modelBuilder.Entity<Client>().HasAlternateKey(c => c.Passport);

            //modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            //modelBuilder.Entity<Client>(ClientConfigure);
        }

        /*
        public void ClientConfigure(EntityTypeBuilder<Client> builder)
        {
            //builder.ToTable("Mobiles").HasKey(p => p.Ident);
            //builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
        }
        */

        /*
        public class CompanyConfiguration : IEntityTypeConfiguration<Company>
        {
            public void Configure(EntityTypeBuilder<Company> builder)
            {
                //builder.ToTable("Manufacturers").Property(c => c.Name).IsRequired().HasMaxLength(30);
            }
        }
        */
    }
}
