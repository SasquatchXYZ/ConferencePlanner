using ConferencePlanner.GraphQL.Data;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace ConferencePlanner.GraphQL.Attendees;

[SubscriptionType]
public static class AttendeeSubscriptions
{
    // Represents our resolver, but rather than letting the system generate a subscribe resolver
    // that handles subscribing to the pub/sub system, we are creating it ourselves using `With` to
    // control how its done, or filter out events that we don't want to pass down.
    [Subscribe(With = nameof(SubscribeToOnAttendeeCheckedInAsync))]
    public static SessionAttendeeCheckIn OnAttendeeCheckedIn(
        [ID<Session>] int sessionId,
        [EventMessage] int attendeeId)
    {
        return new SessionAttendeeCheckIn(sessionId, attendeeId);
    }

    // A subscribe resolver can return `IAsyncEnumerable<T>`, `IEnumerable<T>`, or `IObservable<T>` to represent
    // the subscription stream.  The subscribe resolver has access to all the arguments the actual resolver has access to.
    public static async ValueTask<ISourceStream<int>> SubscribeToOnAttendeeCheckedInAsync(
        int sessionId,
        ITopicEventReceiver eventReceiver,
        CancellationToken cancellationToken)
    {
        return await eventReceiver.SubscribeAsync<int>(
            $"OnAttendeeCheckedIn_{sessionId}",
            cancellationToken);
    }
}
