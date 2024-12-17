using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.Extensions;
using GreenDonut.Selectors;
using HotChocolate.Execution.Processing;

namespace ConferencePlanner.GraphQL.Tracks;

[ObjectType<Track>]
public static partial class TrackType
{
    static partial void Configure(IObjectTypeDescriptor<Track> descriptor)
    {
        descriptor
            .Field(track => track.Name)
            .ParentRequires(nameof(Track.Name))
            .UseUpperCase();
    }

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
