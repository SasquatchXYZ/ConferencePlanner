using ConferencePlanner.GraphQL.Data;

namespace ConferencePlanner.GraphQL.Sessions;

public record AddSessionInput(
    string Title,
    string? Abstract,
    [property: ID<Speaker>] IReadOnlyList<int> SpeakerIds);
