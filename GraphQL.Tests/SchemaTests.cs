using ConferencePlanner.GraphQL.Data;
using CookieCrumble;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.Tests;

public sealed class SchemaTests
{
    [Fact]
    public async Task SchemaChanged()
    {
        // Arrange & Act
        // Takes the service collection and builds a schema from it
        var schema = await new ServiceCollection()
            .AddDbContext<ApplicationDbContext>()
            .AddGraphQLServer()
            .AddGlobalObjectIdentification()
            .AddMutationConventions()
            .AddDbContextCursorPagingProvider()
            .AddPagingArguments()
            .AddFiltering()
            .AddSorting()
            .AddInMemorySubscriptions()
            .AddGraphQLTypes()
            .BuildSchemaAsync();

        // Assert
        // Creates a snapshot of the GraphQL SDL representation of the schema,
        // which is compared in subsequent test runs
        schema.MatchSnapshot();
    }
}
