using Microsoft.EntityFrameworkCore;
using MemoryKeeper.Domain.Entities;
using MemoryKeeper.Domain.Enums;
using MemoryKeeper.Domain.Common;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryKeeper.Infrastructure.Data
{
    public class MemoryKeeperDbContext : DbContext
    {
        public MemoryKeeperDbContext(DbContextOptions<MemoryKeeperDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Memory> Memories { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Domain.Entities.Object> Objects { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<MemoryPlace> MemoryPlaces { get; set; }
        public DbSet<MemoryObject> MemoryObjects { get; set; }
        public DbSet<MemoryNote> MemoryNotes { get; set; }
        public DbSet<MemoryPerson> MemoryPeople { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // Memory Configuration
            modelBuilder.Entity<Memory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Status).HasConversion<int>();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany(u => u.CreatedMemories)
                    .HasForeignKey(e => e.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ConfirmedByUser)
                    .WithMany(u => u.ConfirmedMemories)
                    .HasForeignKey(e => e.ConfirmedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.CreatedByUserId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.OccurredAt);
            });

            // Place Configuration
            modelBuilder.Entity<Place>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Address).HasMaxLength(300);
                entity.Property(e => e.Latitude).HasPrecision(10, 8);
                entity.Property(e => e.Longitude).HasPrecision(11, 8);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany(u => u.CreatedPlaces)
                    .HasForeignKey(e => e.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Object Configuration
            modelBuilder.Entity<Domain.Entities.Object>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany(u => u.CreatedObjects)
                    .HasForeignKey(e => e.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Note Configuration
            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany(u => u.CreatedNotes)
                    .HasForeignKey(e => e.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Person Configuration
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany(u => u.CreatedPeople)
                    .HasForeignKey(e => e.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // MemoryPlace Configuration
            modelBuilder.Entity<MemoryPlace>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AssociatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.Memory)
                    .WithMany(m => m.MemoryPlaces)
                    .HasForeignKey(e => e.MemoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Place)
                    .WithMany(p => p.MemoryPlaces)
                    .HasForeignKey(e => e.PlaceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.AssociatedByUser)
                    .WithMany(u => u.AssociatedMemoryPlaces)
                    .HasForeignKey(e => e.AssociatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.MemoryId);
            });

            // MemoryObject Configuration
            modelBuilder.Entity<MemoryObject>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AssociatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.Memory)
                    .WithMany(m => m.MemoryObjects)
                    .HasForeignKey(e => e.MemoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Object)
                    .WithMany(o => o.MemoryObjects)
                    .HasForeignKey(e => e.ObjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.AssociatedByUser)
                    .WithMany(u => u.AssociatedMemoryObjects)
                    .HasForeignKey(e => e.AssociatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.MemoryId);
            });

            // MemoryNote Configuration
            modelBuilder.Entity<MemoryNote>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AssociatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.Memory)
                    .WithMany(m => m.MemoryNotes)
                    .HasForeignKey(e => e.MemoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Note)
                    .WithMany(n => n.MemoryNotes)
                    .HasForeignKey(e => e.NoteId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.AssociatedByUser)
                    .WithMany(u => u.AssociatedMemoryNotes)
                    .HasForeignKey(e => e.AssociatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.MemoryId);
            });

            // MemoryPerson Configuration
            modelBuilder.Entity<MemoryPerson>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AssociatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.Memory)
                    .WithMany(m => m.MemoryPeople)
                    .HasForeignKey(e => e.MemoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Person)
                    .WithMany(p => p.MemoryPeople)
                    .HasForeignKey(e => e.PersonId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.AssociatedByUser)
                    .WithMany(u => u.AssociatedMemoryPeople)
                    .HasForeignKey(e => e.AssociatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.MemoryId);
            });

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Pepe as the main user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "pepe@memorykeeper.com",
                    FirstName = "Pepe",
                    LastName = "García",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Pepe123!"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
                
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}