using ConferencePlanner.GraphQL.Data;

namespace ConferencePlanner.GraphQL.MutationData;

public sealed class AddSpeakerPayload(Speaker speaker)
{
    public Speaker Speaker { get; } = speaker;
}
