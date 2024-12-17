using ConferencePlanner.GraphQL.Data;
using GreenDonut.Selectors;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Sessions;

public static class SessionDataLoaders
{
    [DataLoader]
    public static async Task<IReadOnlyDictionary<int, Session>> SessionByIdAsync(
        IReadOnlyList<int> ids,
        ApplicationDbContext dbContext,
        ISelectorBuilder selectorBuilder,
        CancellationToken cancellationToken)
    {
        return await dbContext.Sessions
            .AsNoTracking()
            .Where(session => ids.Contains(session.Id))
            .Select(session => session.Id, selectorBuilder)
            .ToDictionaryAsync(session => session.Id, cancellationToken: cancellationToken);
    }

    [DataLoader]
    public static async Task<IReadOnlyDictionary<int, Speaker[]>> SpeakersBySessionIdAsync(
        IReadOnlyList<int> sessionIds,
        ApplicationDbContext dbContext,
        ISelectorBuilder selectorBuilder,
        CancellationToken cancellationToken)
    {
        return await dbContext.Sessions
            .AsNoTracking()
            .Where(session => sessionIds.Contains(session.Id))
            .Select(session => session.Id,
                session => session.SessionSpeakers.Select(sessionSpeaker => sessionSpeaker.Speaker), selectorBuilder)
            .ToDictionaryAsync(r => r.Key, r => r.Value.ToArray(), cancellationToken: cancellationToken);
    }

    [DataLoader]
    public static async Task<IReadOnlyDictionary<int, Attendee[]>> AttendeesBySessionIdAsync(
        IReadOnlyList<int> sessionIds,
        ApplicationDbContext dbContext,
        ISelectorBuilder selectorBuilder,
        CancellationToken cancellationToken)
    {
        return await dbContext.Sessions
            .AsNoTracking()
            .Where(session => sessionIds.Contains(session.Id))
            .Select(session => session.Id,
                session => session.SessionAttendees.Select(sessionAttendee => sessionAttendee.Attendee),
                selectorBuilder)
            .ToDictionaryAsync(r => r.Key, r => r.Value.ToArray(), cancellationToken: cancellationToken);
    }
}
