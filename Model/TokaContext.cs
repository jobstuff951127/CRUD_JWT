using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Model
{
    public partial class TokaContext : IdentityDbContext<AppUser>
    {
        public TokaContext()
        {
        }

        public TokaContext(DbContextOptions<TokaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Costumer> Costumers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Costumer>(entity =>
            {
                entity.Property(e => e.Adress)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.BirthDate)
                                    .IsRequired()
                                    .HasMaxLength(15)
                                    .IsUnicode(false);

                entity.Property(e => e.Cellphone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
