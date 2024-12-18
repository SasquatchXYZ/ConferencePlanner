using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.Sessions;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Attendees;

public sealed class SessionAttendeeCheckIn(int attendeeId, int sessionId)
{
    [ID<Attendee>] public int AttendeeId { get; } = attendeeId;

    [ID<Session>] public int SessionId { get; } = sessionId;

    public async Task<int> CheckInCountAsync(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return await dbContext.Sessions
            .AsNoTracking()
            .Where(session => session.Id == SessionId)
            .SelectMany(session => session.SessionAttendees)
            .CountAsync(cancellationToken);
    }

    public async Task<Attendee> GetAttendeeAsync(
        IAttendeeByIdDataLoader attendeeByIdDataLoader,
        CancellationToken cancellationToken)
    {
        return await attendeeByIdDataLoader.LoadRequiredAsync(AttendeeId, cancellationToken);
    }

    public async Task<Session> GetSessionAsync(
        ISessionByIdDataLoader sessionByIdDataLoader,
        CancellationToken cancellationToken)
    {
        return await sessionByIdDataLoader.LoadRequiredAsync(SessionId, cancellationToken);
    }
}
