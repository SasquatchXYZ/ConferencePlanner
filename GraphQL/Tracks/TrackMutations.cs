using ConferencePlanner.GraphQL.Data;

namespace ConferencePlanner.GraphQL.Tracks;

[MutationType]
public static class TrackMutations
{
    public static async Task<Track> AddTrackAsync(
        AddTrackInput input,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var track = new Track { Name = input.Name };

        dbContext.Tracks.Add(track);

        await dbContext.SaveChangesAsync(cancellationToken);

        return track;
    }
}
