using ConferencePlanner.GraphQL.Data;
using GreenDonut.Selectors;
using HotChocolate.Execution.Processing;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Speakers;

[QueryType]
public static class SpeakerQueries
{
    [UsePaging]
    public static IQueryable<Speaker> GetSpeakers(ApplicationDbContext dbContext)
    {
        return dbContext.Speakers.AsNoTracking().OrderBy(speaker => speaker.Name).ThenBy(speaker => speaker.Id);
    }

    [NodeResolver] // Marks the node resolver for a Relay node type.  It will also set the GraphQL type of the `id` parameter to `ID`
    public static async Task<Speaker?> GetSpeakerByIdAsync(
        int id,
        ISpeakerByIdDataLoader speakerByIdDataLoader,
        ISelection selection,
        CancellationToken cancellationToken)
    {
        return await speakerByIdDataLoader.Select(selection).LoadAsync(id, cancellationToken);
    }

    public static async Task<IEnumerable<Speaker>> GetSpeakersByIdAsync(
        [ID<Speaker>] int[] ids,
        ISpeakerByIdDataLoader speakerByIdDataLoader,
        CancellationToken cancellationToken)
    {
        return await speakerByIdDataLoader.LoadRequiredAsync(ids, cancellationToken);
    }
}
