namespace ConferencePlanner.GraphQL.MutationData;

public sealed record AddSpeakerInput(
    string Name,
    string? Bio,
    string? Website);
