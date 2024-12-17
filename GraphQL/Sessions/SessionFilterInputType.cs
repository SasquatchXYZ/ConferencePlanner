using ConferencePlanner.GraphQL.Data;
using HotChocolate.Data.Filters;

namespace ConferencePlanner.GraphQL.Sessions;

public sealed class SessionFilterInputType : FilterInputType<Session>
{
    protected override void Configure(IFilterInputTypeDescriptor<Session> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(session => session.Title);
        descriptor.Field(session => session.Abstract);
        descriptor.Field(session => session.StartTime);
        descriptor.Field(session => session.EndTime);
    }
}
