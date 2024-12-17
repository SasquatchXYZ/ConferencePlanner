using ConferencePlanner.GraphQL.Data;
using GreenDonut.Selectors;
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
    public static async Task<IReadOnlyDictionary<int, Session[]>> SessionsByTrackIdAsync(
        IReadOnlyList<int> trackIds,
        ApplicationDbContext dbContext,
        ISelectorBuilder selectorBuilder,
        CancellationToken cancellationToken)
    {
        return await dbContext.Tracks
            .AsNoTracking()
            .Where(track => trackIds.Contains(track.Id))
            .Select(track => track.Id, track => track.Sessions, selectorBuilder)
            .ToDictionaryAsync(r => r.Key, r => r.Value.ToArray(), cancellationToken: cancellationToken);
    }
}
