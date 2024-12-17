using ConferencePlanner.GraphQL.Data;
using GreenDonut.Selectors;
using HotChocolate.Execution.Processing;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Tracks;

[QueryType]
public static class TrackQueries
{
    [UsePaging]
    public static IQueryable<Track> GetTracks(ApplicationDbContext dbContext)
    {
        return dbContext.Tracks.AsNoTracking().OrderBy(track => track.Name).ThenBy(track => track.Id);
    }

    [NodeResolver]
    public static async Task<Track?> GetTrackByIdAsync(
        int id,
        ITrackByIdDataLoader trackByIdDataLoader,
        ISelection selection,
        CancellationToken cancellationToken)
    {
        return await trackByIdDataLoader.Select(selection).LoadAsync(id, cancellationToken);
    }

    public static async Task<IEnumerable<Track>> GetTracksByIdAsync(
        [ID<Track>] int[] ids,
        ITrackByIdDataLoader trackByIdDataLoader,
        ISelection selection,
        CancellationToken cancellationToken)
    {
        return await trackByIdDataLoader.Select(selection).LoadRequiredAsync(ids, cancellationToken);
    }
}
