using ConferencePlanner.GraphQL.Data;

namespace ConferencePlanner.GraphQL.Sessions;

[MutationType]
public static class SessionMutations
{
    // Registers a middleware that will catch all exceptions of the listed types on mutations and queries.
    // By annotating the attribute the response type f the annotated resolver will be automatically extended
    [Error<TitleEmptyException>]
    [Error<NoSpeakerException>]
    public static async Task<Session> AddSessionAsync(
        AddSessionInput input,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(input.Title)) throw new TitleEmptyException();

        if (input.SpeakerIds.Count == 0) throw new NoSpeakerException();

        var session = new Session
        {
            Title = input.Title,
            Abstract = input.Abstract,
        };

        foreach (var speakerId in input.SpeakerIds)
        {
            session.SessionSpeakers.Add(new SessionSpeaker
            {
                SpeakerId = speakerId
            });
        }

        dbContext.Sessions.Add(session);

        await dbContext.SaveChangesAsync(cancellationToken);

        return session;
    }
}
