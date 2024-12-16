using ConferencePlanner.GraphQL.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<ApplicationDbContext>( // Registers the `ApplicationDbContext` service so that it can be injected into resolvers
        options => options.UseNpgsql("Host=127.0.0.1;Username=graphql_workshop;Password=secret"));

var app = builder.Build();

app.Run();
