using Core.Domain;
using HotChocolate.Types;
using System.Numerics;

namespace API.GraphQL
{
    public class ProductType : ObjectType<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
        {
            descriptor.Field(p => p.Id).Type<IdType>().Name("Id");
            descriptor.Field(p => p.Name).Type<StringType>().Name("Name");
            descriptor.Field(p => p.ContainsAlcohol).Type<BooleanType>().Name("ContainsAlcohol");
        }
    }
}
