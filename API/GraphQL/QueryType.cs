using Core.Domain;
using Core.DomainServices.Interfaces;
using HotChocolate;
using HotChocolate.Types;


namespace API.GraphQL
{
    public class QueryType
    {
        private readonly IMealPackageRepo _mealPackageRepo;
        private readonly ICanteenRepo _canteenRepo;
        private readonly IStudentRepo _studentRepo;
        private readonly IProductRepo _productRepo;

        public QueryType(
            IMealPackageRepo mealPackageRepo,
            ICanteenRepo canteenRepo,
            IStudentRepo studentRepo,
            IProductRepo productRepo)
        {
            _mealPackageRepo = mealPackageRepo;
            _canteenRepo = canteenRepo;
            _studentRepo = studentRepo;
            _productRepo = productRepo;
        }

        [UsePaging]
        public IEnumerable<MealPackage> GetMealPackages(
            [Service] IMealPackageRepo mealPackageRepo) =>
        mealPackageRepo.GetMealPackages().AsQueryable();

        public MealPackage GetMealPackageById(int id, [Service] IMealPackageRepo mealPackageRepo, [Service] ICanteenRepo canteenRepo) =>
        mealPackageRepo.GetMealPackageById(id);
    }
}
