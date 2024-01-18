using Core.Domain;
using HotChocolate.Types;
using System.Numerics;

namespace API.GraphQL
{
    public class CanteenType : ObjectType<Canteen>
    {
        protected override void Configure(IObjectTypeDescriptor<Canteen> descriptor)
        {
            descriptor.Field(c => c.Id).Type<IdType>().Name("id");
        }
    }
}
