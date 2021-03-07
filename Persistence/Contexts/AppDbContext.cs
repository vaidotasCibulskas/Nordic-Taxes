using Microsoft.EntityFrameworkCore;
using Nordic.Taxes.Domain.Models;
using System;

namespace Nordic.Taxes.Persistence.Contexts
{
	public class AppDbContext : DbContext
	{
		public DbSet<Municipality> Municipalities { get; set; }
		public DbSet<Tax> Taxes { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Municipality>().ToTable("Municipalities");
			builder.Entity<Municipality>().HasKey(p => p.Id);
			builder.Entity<Municipality>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
			builder.Entity<Municipality>().Property(p => p.Name).IsRequired().HasMaxLength(128);
			builder.Entity<Municipality>().HasIndex(tx => new { tx.Name}).IsUnique();
			builder.Entity<Municipality>().HasMany(p => p.Taxes).WithOne(p => p.Municipality).HasForeignKey(p => p.MunicipalityId);

			builder.Entity<Tax>().ToTable("Taxes");
			builder.Entity<Tax>().HasKey(p => p.Id);
			builder.Entity<Tax>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
			builder.Entity<Tax>().Property(p => p.TaxType).IsRequired();
			builder.Entity<Tax>().Property(p => p.From).IsRequired();
			builder.Entity<Tax>().Property(p => p.To).IsRequired();
			builder.Entity<Tax>().Property(p => p.TaxSize).IsRequired();
			builder.Entity<Tax>().HasIndex(tx => new { tx.MunicipalityId, tx.From, tx.To }).IsUnique();


		}
	}
}


