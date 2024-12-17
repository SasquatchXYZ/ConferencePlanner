using ConferencePlanner.GraphQL.Data;
using GreenDonut.Selectors;
using HotChocolate.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Tracks;

public static class TrackDataLoaders
{
    [DataLoader]
    public static async Task<IReadOnlyDictionary<int, Track>> TrackByIdAsync(
        IReadOnlyList<int> ids,
        ApplicationDbContext dbContext,
        ISelectorBuilder selectorBuilder,
        CancellationToken cancellationToken)
    {
        return await dbContext.Tracks
            .AsNoTracking()
            .Where(track => ids.Contains(track.Id))
            .Select(track => track.Id, selectorBuilder)
            .ToDictionaryAsync(track => track.Id, cancellationToken: cancellationToken);
    }

    [DataLoader]
    public static async Task<IReadOnlyDictionary<int, Page<Session>>> SessionsByTrackIdAsync(
        IReadOnlyList<int> trackIds,
        ApplicationDbContext dbContext,
        ISelectorBuilder selectorBuilder,
        PagingArguments pagingArguments,
        CancellationToken cancellationToken)
    {
        return await dbContext.Sessions
            .AsNoTracking()
            .Where(session => session.TrackId != null && trackIds.Contains((int) session.TrackId))
            .OrderBy(session => session.Id)
            .Select(session => session.TrackId, selectorBuilder)
            .ToBatchPageAsync(session => (int) session.TrackId!, pagingArguments, cancellationToken: cancellationToken);
    }
}
