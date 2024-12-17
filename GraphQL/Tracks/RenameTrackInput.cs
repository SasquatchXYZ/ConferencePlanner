using ConferencePlanner.GraphQL.Data;

namespace ConferencePlanner.GraphQL.Tracks;

public record RenameTrackInput([property: ID<Track>] int Id, string Name);
