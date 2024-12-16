using ConferencePlanner.GraphQL.Data;
using GreenDonut.Selectors;
using HotChocolate.Execution.Processing;

namespace ConferencePlanner.GraphQL.Attendees;

[ObjectType<Attendee>]
public static partial class AttendeeType
{
    static partial void Configure(IObjectTypeDescriptor<Attendee> descriptor)
    {
        descriptor
            .ImplementsNode() // Marks the type as implementing the `Node` interface
            .IdField(attendee => attendee.Id) // Specifies the `id` member of the node type
            // Specifies a delegate to resolve the node from its ID
            .ResolveNode(
                async (ctx, id) =>
                    await ctx.DataLoader<IAttendeeByIdDataLoader>()
                        .LoadAsync(id, ctx.RequestAborted));
    }

    [BindMember(nameof(Attendee.SessionsAttendees))]
    public static async Task<IEnumerable<Session>> GetSessionsAsync(
        [Parent] Attendee attendee,
        ISessionsByAttendeeIdDataLoader sessionsByAttendeeIdDataLoader,
        ISelection selection,
        CancellationToken cancellationToken)
    {
        return await sessionsByAttendeeIdDataLoader
            .Select(selection)
            .LoadRequiredAsync(attendee.Id, cancellationToken);
    }
}
