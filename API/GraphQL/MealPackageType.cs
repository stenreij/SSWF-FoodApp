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
            descriptor.Field(m => m.Id).Type<IdType>().Name("Id");
            descriptor.Field(m => m.Name).Type<StringType>().Name("Name");
            descriptor.Field(m => m.Price).Type<DecimalType>().Name("Price");
            descriptor.Field(m => m.PickUpDateTime).Type<DateTimeType>().Name("PickUpDateTime");
            descriptor.Field(m => m.ExpireDateTime).Type<DateTimeType>().Name("ExpireDateTime");
            descriptor.Field(m => m.AdultsOnly).Type<BooleanType>().Name("AdultsOnly");
            descriptor.Field(m => m.Canteen)
                    .Type<CanteenType>()
                    .Name("Canteen")
                    .Resolve(context =>
                    {
                        var canteenRepo = context.Service<ICanteenRepo>();
                        return canteenRepo.GetCanteens().FirstOrDefault(c => c.Id == context.Parent<MealPackage>().Canteen.Id);
                    });
        }
    }
}
