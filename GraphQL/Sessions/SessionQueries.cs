using ConferencePlanner.GraphQL.Data;
using GreenDonut.Selectors;
using HotChocolate.Execution.Processing;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Sessions;

[QueryType]
public static class SessionQueries
{
    [UsePaging]
    public static IQueryable<Session> GetSessions(ApplicationDbContext dbContext)
    {
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
