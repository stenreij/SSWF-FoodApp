using Core.Domain;
using HotChocolate.Types;
using System.Numerics;

namespace API.GraphQL
{
    public class StudentType : ObjectType<Student>
    {
        protected override void Configure(IObjectTypeDescriptor<Student> descriptor)
        {
            descriptor.Field(s => s.Id).Type<IdType>().Name("Id");
            descriptor.Field(s => s.FirstName).Type<StringType>().Name("FirstName");
            descriptor.Field(s => s.LastName).Type<StringType>().Name("LastName");
            descriptor.Field(s => s.StudentNr).Type<IntType>().Name("StudentNr");
            descriptor.Field(s => s.BirthDate).Type<DateTimeType>().Name("BirthDate");
            descriptor.Field(s => s.Email).Type<StringType>().Name("Email");
            descriptor.Field(s => s.PhoneNr).Type<IntType>().Name("PhoneNr");
        }
    }
}
