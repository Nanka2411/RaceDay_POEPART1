
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Models;
namespace RaceDayAPI.Data
{
    public class RaceDayDbContext : DbContext
    {
        public RaceDayDbContext(DbContextOptions<RaceDayDbContext> options)
            : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Organiser> Organisers { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Result> Results { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // ACCOUNT
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");
                entity.HasKey(a => a.UserID);
                entity.Property(a => a.UserID)
                    .ValueGeneratedNever();
                entity.HasIndex(a => a.Email)
                    .IsUnique();
                entity.Property(a => a.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");
            });
            // ORGANISER
            modelBuilder.Entity<Organiser>(entity =>
            {
                entity.ToTable("Organiser");
                entity.HasKey(o => o.OrganiserID);
                entity.Property(o => o.OrganiserID)
                    .ValueGeneratedNever();
                entity.HasIndex(o => o.UserID)
                    .IsUnique();
                entity.HasOne(o => o.Account)
                    .WithOne(a => a.Organiser)
                    .HasForeignKey<Organiser>(o => o.UserID)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            // PARTICIPANT
            modelBuilder.Entity<Participant>(entity =>
            {
                entity.ToTable("Participant");
                entity.HasKey(p => p.ParticipantID);
                entity.Property(p => p.ParticipantID)
                    .ValueGeneratedNever();
                entity.HasIndex(p => p.UserID)
                    .IsUnique();
                entity.HasOne(p => p.Account)
                    .WithOne(a => a.Participant)
                    .HasForeignKey<Participant>(p => p.UserID)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            // EVENT
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");
                entity.HasKey(e => e.EventID);
                entity.Property(e => e.EventID)
                    .ValueGeneratedNever();
                entity.Property(e => e.Distance)
                    .HasPrecision(8, 2);
                entity.HasOne<Organiser>()
     .WithMany(o => o.Events)
     .HasForeignKey(e => e.OrganiserID)
     .OnDelete(DeleteBehavior.Restrict);
            });
            // CATEGORY
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(c => c.CategoryID);
                entity.Property(c => c.CategoryID)
                    .ValueGeneratedNever();
            });
            // EVENT CATEGORY
            modelBuilder.Entity<EventCategory>(entity =>
            {
                entity.ToTable("EventCategory");
                entity.HasKey(ec => new
                {
                    ec.EventID,
                    ec.CategoryID
                });
                entity.HasOne<Event>()
                    .WithMany()
                    .HasForeignKey(ec => ec.EventID)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne<Category>()
                    .WithMany()
                    .HasForeignKey(ec => ec.CategoryID)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            // ENTRY
            modelBuilder.Entity<Entry>(entity =>
            {
                entity.ToTable("Entry");
                entity.HasKey(e => e.EntryID);
                entity.Property(e => e.EntryID)
                    .ValueGeneratedNever();
                entity.HasIndex(e => new
                {
                    e.ParticipantID,
                    e.EventID
                }).IsUnique();
                entity.Property(e => e.EntryDate)
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.EntryStatus)
                    .HasDefaultValue("ENTERED");
                entity.HasOne<Participant>()
                    .WithMany()
                    .HasForeignKey(e => e.ParticipantID)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne<EventCategory>()
                    .WithMany()
                    .HasForeignKey(e => new
                    {
                        e.EventID,
                        e.CategoryID
                    })
                    .OnDelete(DeleteBehavior.Restrict);
            });
            // RESULT
            modelBuilder.Entity<Result>(entity =>
            {
                entity.ToTable("Result");

                entity.HasKey(r => r.ResultID);

                entity.Property(r => r.ResultID)
                    .ValueGeneratedNever();

                entity.HasIndex(r => r.EntryID)
                    .IsUnique();

                entity.Property(r => r.ResultStatus)
                    .HasDefaultValue("Completed");

                entity.HasOne(r => r.Entry)
                    .WithOne(e => e.Result)
                    .HasForeignKey<Result>(r => r.EntryID)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}