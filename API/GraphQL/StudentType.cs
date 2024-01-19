using Core.Domain;
using HotChocolate.Types;
using System.Numerics;

namespace API.GraphQL
{
    public class StudentType : ObjectType<Student>
    {
        protected override void Configure(IObjectTypeDescriptor<Student> descriptor)
        {
            descriptor.Field(s => s.Id).Type<IdType>().Name("id");
            descriptor.Field(s => s.FirstName).Type<StringType>().Name("firstName");
            descriptor.Field(s => s.LastName).Type<StringType>().Name("lastName");
        }
    }
}
