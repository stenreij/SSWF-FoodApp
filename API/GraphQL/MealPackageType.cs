using Core.Domain;
using Core.DomainServices.Interfaces;
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
            descriptor.Field(m => m.Canteen)
                    .Type<CanteenType>()
                    .Name("canteen")
                    .Resolve(context =>
                    {
                        var canteenRepo = context.Service<ICanteenRepo>();
                        return canteenRepo.GetCanteens().FirstOrDefault(c => c.Id == context.Parent<MealPackage>().Canteen.Id);
                    });
        }
    }
}
