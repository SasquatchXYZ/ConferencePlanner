using ConferencePlanner.GraphQL.Data;
using GreenDonut.Selectors;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Speakers;

public static class SpeakerDataLoaders
{
    [DataLoader]
    public static async Task<IReadOnlyDictionary<int, Speaker>> SpeakerByIdAsync(
        IReadOnlyList<int> ids,
        ApplicationDbContext dbContext,
        ISelectorBuilder selectorBuilder,
        CancellationToken cancellationToken)
    {
        return await dbContext.Speakers
            .AsNoTracking()
            .Where(speaker => ids.Contains(speaker.Id))
            .Select(speaker => speaker.Id, selectorBuilder)
            .ToDictionaryAsync(speaker => speaker.Id, cancellationToken: cancellationToken);
    }

    [DataLoader]
    public static async Task<IReadOnlyDictionary<int, Session[]>> SessionsBySpeakerIdAsync(
        IReadOnlyList<int> speakerIds,
        ApplicationDbContext dbContext,
        ISelectorBuilder selectorBuilder,
        CancellationToken cancellationToken)
    {
        return await dbContext.Speakers
            .AsNoTracking()
            .Where(speaker => speakerIds.Contains(speaker.Id))
            .Select(speaker => speaker.Id,
                speaker => speaker.SessionSpeakers.Select(sessionSpeaker => sessionSpeaker.Session), selectorBuilder)
            .ToDictionaryAsync(r => r.Key, r => r.Value.ToArray(), cancellationToken: cancellationToken);
    }
}
