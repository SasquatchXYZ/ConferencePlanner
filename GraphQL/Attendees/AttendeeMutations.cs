using ConferencePlanner.GraphQL.Data;

namespace ConferencePlanner.GraphQL.Attendees;

[MutationType]
public static class AttendeeMutations
{
    public static async Task<Attendee> RegisterAttendeeAsync(
        RegisterAttendeeInput input,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var attendee = new Attendee
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            Username = input.Username,
            EmailAddress = input.EmailAddress
        };

        dbContext.Attendees.Add(attendee);

        await dbContext.SaveChangesAsync(cancellationToken);

        return attendee;
    }
}
