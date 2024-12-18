using ConferencePlanner.GraphQL.Data;

namespace ConferencePlanner.GraphQL.Sessions;

[SubscriptionType]
public static class SessionSubscriptions
{
    [Subscribe] // Tells the schema builder that this resolver needs to be hooked up to the pub/sub system.
    [Topic] // Can by put on the method or a parameter of the method and will infer the pub/sub topic for this subscription.
    public static async Task<Session> OnSessionScheduledAsync(
        // Attribute marks the parameter where the execution engine will inject the message payload of the pub/sub system
        [EventMessage] int sessionId,
        ISessionByIdDataLoader sessionByIdDataLoader,
        CancellationToken cancellationToken)
    {
        return await sessionByIdDataLoader.LoadRequiredAsync(sessionId, cancellationToken);
    }
}
