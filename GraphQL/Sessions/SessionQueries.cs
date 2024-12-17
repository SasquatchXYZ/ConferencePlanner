using ConferencePlanner.GraphQL.Data;
using GreenDonut.Selectors;
using HotChocolate.Execution.Processing;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Sessions;

[QueryType]
public static class SessionQueries
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public static IQueryable<Session> GetSessions(ApplicationDbContext dbContext)
    {
        // By default, the filter middleware would infer a filter type that exposes all the fields of the entity
        // In our case, it would be better to be explicit by specifying exactly which fields our users can filter by.
        return dbContext.Sessions.AsNoTracking().OrderBy(session => session.Title).ThenBy(session => session.Id);
    }

    [NodeResolver]
    public static async Task<Session?> GetSessionByIdAsync(
        int id,
        ISessionByIdDataLoader sessionByIdDataLoader,
        ISelection selection,
        CancellationToken cancellationToken)
    {
        return await sessionByIdDataLoader.Select(selection).LoadAsync(id, cancellationToken);
    }

    public static async Task<IEnumerable<Session>> GetSessionsByIdAsync(
        [ID<Session>] int[] ids,
        ISessionByIdDataLoader sessionByIdDataLoader,
        ISelection selection,
        CancellationToken cancellationToken)
    {
        return await sessionByIdDataLoader.Select(selection).LoadRequiredAsync(ids, cancellationToken);
    }
}
