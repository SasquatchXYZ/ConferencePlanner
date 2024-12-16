using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Attendee> Attendees { get; init; }
    public DbSet<Session> Sessions { get; init; }
    public DbSet<Speaker> Speakers { get; init; }
    public DbSet<Track> Tracks { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Attendee>()
            .HasIndex(attendee => attendee.Username)
            .IsUnique();

        // Many-to-many: Session <-> Attendee
        modelBuilder
            .Entity<SessionAttendee>()
            .HasKey(sessionAttendee => new { sessionAttendee.SessionId, sessionAttendee.AttendeeId });

        // Many-to-many: Speaker <-> Session
        modelBuilder
            .Entity<SessionSpeaker>()
            .HasKey(sessionSpeaker => new { sessionSpeaker.SessionId, sessionSpeaker.SpeakerId });
    }
}
