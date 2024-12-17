using ConferencePlanner.GraphQL.Data;
using GreenDonut.Selectors;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Attendees;

public static class AttendeeDataLoaders
{
    [DataLoader]
    public static async Task<IReadOnlyDictionary<int, Attendee>> AttendeeByIdAsync(
        IReadOnlyList<int> ids,
        ApplicationDbContext dbContext,
        ISelectorBuilder selectorBuilder,
        CancellationToken cancellationToken)
    {
        return await dbContext.Attendees
            .AsNoTracking()
            .Where(attendee => ids.Contains(attendee.Id))
            .Select(attendee => attendee.Id, selectorBuilder)
            .ToDictionaryAsync(attendee => attendee.Id, cancellationToken: cancellationToken);
    }

    [DataLoader]
    public static async Task<IReadOnlyDictionary<int, Session[]>> SessionsByAttendeeIdAsync(
        IReadOnlyList<int> attendeeIds,
        ApplicationDbContext dbContext,
        ISelectorBuilder selectorBuilder,
        CancellationToken cancellationToken)
    {
        return await dbContext.Attendees
            .AsNoTracking()
            .Where(attendee => attendeeIds.Contains(attendee.Id))
            .Select(attendee => attendee.Id,
                attendee => attendee.SessionsAttendees.Select(sessionAttendee => sessionAttendee.Session),
                selectorBuilder)
            .ToDictionaryAsync(r => r.Key, r => r.Value.ToArray(), cancellationToken: cancellationToken);
    }
}
