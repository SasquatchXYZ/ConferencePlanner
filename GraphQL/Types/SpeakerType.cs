using ConferencePlanner.GraphQL.Data;
using GreenDonut.Selectors;
using HotChocolate.Execution.Processing;

namespace ConferencePlanner.GraphQL.Types;

[ObjectType<Speaker>]
public static partial class SpeakerType
{
    // Replaces the existing `sessionSpeakers` field property with a new field name `sessions`
    // using this `BindMember` attribute.  The new field exposes the sessions associated with the speaker
    [BindMember(nameof(Speaker.SessionSpeakers))]
    public static async Task<IEnumerable<Session>> GetSessionsAsync(
        [Parent] Speaker speaker,
        ISessionsBySpeakerIdDataLoader sessionsBySpeakerIdDataLoader,
        ISelection selection,
        CancellationToken cancellationToken)
    {
        return await sessionsBySpeakerIdDataLoader
            .Select(selection)
            .LoadRequiredAsync(speaker.Id, cancellationToken);
    }
}
