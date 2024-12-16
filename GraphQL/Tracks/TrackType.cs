using ConferencePlanner.GraphQL.Data;
using GreenDonut.Selectors;
using HotChocolate.Execution.Processing;

namespace ConferencePlanner.GraphQL.Tracks;

[ObjectType<Track>]
public static partial class TrackType
{
    public static async Task<IEnumerable<Session>> GetSessionsAsync(
        [Parent] Track track,
        ISessionsByTrackIdDataLoader sessionsByTrackIdDataLoader,
        ISelection selection,
        CancellationToken cancellationToken)
    {
        return await sessionsByTrackIdDataLoader
            .Select(selection)
            .LoadRequiredAsync(track.Id, cancellationToken);
    }
}
