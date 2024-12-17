using ConferencePlanner.GraphQL.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    // Registers the `ApplicationDbContext` service so that it can be injected into resolvers
    .AddDbContext<ApplicationDbContext>(
        options => options.UseNpgsql("Host=127.0.0.1;Username=graphql_workshop;Password=secret"))
    .AddGraphQLServer()
    .AddGlobalObjectIdentification()
    // This enables the mutation conventions to minimize boilerplate code.  Instead of manually creating payload
    // types, Hot Chocolate can generate these types for us automatically.
    .AddMutationConventions()
    // Adds the cursor paging provider to the schema configuration that uses native keyset pagination
    .AddDbContextCursorPagingProvider()
    // This registers all types in the assembly using a source generator (`HotChocolate.Types.Analyzers`)
    // The name of the `AddGraphQLTypes` method is based on the assembly name by default,
    // but can be changed using the `[Module]` attribute on the assembly.
    .AddGraphQLTypes();

var app = builder.Build();

app.MapGraphQL();

await app.RunWithGraphQLCommandsAsync(args);
