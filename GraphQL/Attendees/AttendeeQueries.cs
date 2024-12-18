using ConferencePlanner.GraphQL.Data;
using GreenDonut.Selectors;
using HotChocolate.Execution.Processing;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Attendees;

[QueryType]
public class AttendeeQueries
{
    [UsePaging]
    public static IQueryable<Attendee> GetAttendees(ApplicationDbContext dbContext)
    {
        return dbContext.Attendees.AsNoTracking().OrderBy(attendee => attendee.Username);
    }

    [NodeResolver]
    public static async Task<Attendee?> GetAttendeeByIdAsync(
        int id,
        IAttendeeByIdDataLoader attendeeByIdDataLoader,
        ISelection selection,
        CancellationToken cancellationToken)
    {
        return await attendeeByIdDataLoader.Select(selection).LoadAsync(id, cancellationToken);
    }

    public static async Task<IEnumerable<Attendee>> GetAttendeesByIdAsync(
        [ID<Attendee>] int[] ids,
        IAttendeeByIdDataLoader attendeeByIdDataLoader,
        ISelection selection,
        CancellationToken cancellationToken)
    {
        return await attendeeByIdDataLoader.Select(selection).LoadRequiredAsync(ids, cancellationToken);
    }
}
