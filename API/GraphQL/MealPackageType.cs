using Core.Domain;
using HotChocolate.Types;
using System.Numerics;

namespace API.GraphQL
{
    public class MealPackageType : ObjectType<MealPackage>
    {
        protected override void Configure(IObjectTypeDescriptor<MealPackage> descriptor)
        {
            descriptor.Field(m => m.Id).Type<IdType>().Name("id");
            descriptor.Field(m => m.Name).Type<StringType>().Name("name");
            descriptor.Field(m => m.Price).Type<IntType>().Name("price");

        }
    }
}
