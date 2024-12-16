using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.Tracks;
using GreenDonut.Selectors;
using HotChocolate.Execution.Processing;

namespace ConferencePlanner.GraphQL.Sessions;

[ObjectType<Session>]
public static partial class SessionType
{
    // Configuring the `TrackId` as the Relay ID
    static partial void Configure(IObjectTypeDescriptor<Session> descriptor)
    {
        descriptor
            .Field(session => session.TrackId)
            .ID<Track>();
    }

    public static TimeSpan Duration([Parent("StartTime EndTime")] Session session) =>
        session.Duration;

    [BindMember(nameof(Session.SessionSpeakers))]
    public static async Task<IEnumerable<Speaker>> GetSpeakersAsync(
        [Parent] Session session,
        ISpeakersBySessionIdDataLoader speakersBySessionIdDataLoader,
        ISelection selection,
        CancellationToken cancellationToken)
    {
        return await speakersBySessionIdDataLoader
            .Select(selection)
            .LoadRequiredAsync(session.Id, cancellationToken);
    }

    [BindMember(nameof(Session.SessionAttendees))]
    public static async Task<IEnumerable<Attendee>> GetAttendeesAsync(
        [Parent(nameof(Session.Id))] Session session,
        IAttendeesBySessionIdDataLoader attendeesBySessionIdDataLoader,
        ISelection selection,
        CancellationToken cancellationToken)
    {
        return await attendeesBySessionIdDataLoader
            .Select(selection)
            .LoadRequiredAsync(session.Id, cancellationToken);
    }

    public static async Task<Track?> GetTrackAsync(
        [Parent(nameof(Session.TrackId))] Session session,
        ITrackByIdDataLoader trackByIdDataLoader,
        ISelection selection,
        CancellationToken cancellationToken)
    {
        if (session.TrackId is null) return null;

        return await trackByIdDataLoader
            .Select(selection)
            .LoadAsync(session.TrackId.Value, cancellationToken);
    }
}
