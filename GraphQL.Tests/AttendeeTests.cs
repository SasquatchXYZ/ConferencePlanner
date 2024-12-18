using ConferencePlanner.GraphQL.Data;
using CookieCrumble;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace GraphQL.Tests;

public sealed class AttendeeTests : IAsyncLifetime
{
    // Testcontainers are used to run the database and Redis instances in Docker containers
    // as opposed to using in-memory providers.  See https://www.testcontainers.org/ for more information.
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:17.2")
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:7.4")
        .Build();

    private IRequestExecutor _requestExecutor = null!;

    public async Task InitializeAsync()
    {
        // Start test containers.
        await Task.WhenAll(_postgreSqlContainer.StartAsync(), _redisContainer.StartAsync());

        // Build request executor.
        _requestExecutor = await new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(
                options => options.UseNpgsql(_postgreSqlContainer.GetConnectionString()))
            .AddGraphQLServer()
            .AddGlobalObjectIdentification()
            .AddMutationConventions()
            .AddDbContextCursorPagingProvider()
            .AddPagingArguments()
            .AddFiltering()
            .AddSorting()
            .AddRedisSubscriptions(_ => ConnectionMultiplexer.Connect(_redisContainer.GetConnectionString()))
            .AddGraphQLTypes()
            .BuildRequestExecutorAsync(); // This returns an `IRequestExecutor` so we can execute against a schema.

        // Create database.
        var dbContext = _requestExecutor.Services
            .GetApplicationServices()
            .GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }

    [Fact]
    public async Task RegisterAttendee()
    {
        // Arrange & Act
        var result = await _requestExecutor.ExecuteAsync(
            """
            mutation {
                registerAttendee(
                    input: {
                        firstName: "Michael"
                        lastName: "Staib"
                        username: "mstaib"
                        email: "michael@chillicream.com"
                    }
                ) {
                    attendee {
                        id
                    }
                }
            }
            """);

        // Assert
        result.MatchSnapshot(extension: ".json");
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
        await _redisContainer.DisposeAsync();
    }
}
