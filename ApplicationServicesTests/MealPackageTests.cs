using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Application.Services;
using Core.Domain;
using Core.DomainServices;
using Moq;
using Xunit;

namespace ApplicationServicesTests
{
    public class MealPackageTests
    {
        private readonly List<MealPackage> _mealPackageList = new List<MealPackage>()
        {

            new MealPackage
            {
                Id = 1,
                Name = "Fruit + sapje",
                Price = 2.25,
                PickUpDateTime = new DateTime(2023, 11, 10, 12, 0, 0),
                ExpireDateTime = new DateTime(2023, 11, 12, 12, 0, 0),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>(),
                MealType = MealType.Drinks,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Tila",
                    City = City.Breda,
                    Location = Location.Ta,
                },
                ReservedByStudent = null,
                },

            new MealPackage
            {
                Id = 2,
                Name = "Tosti + water",
                Price = 2.50,
                PickUpDateTime = new DateTime(2023, 11, 13, 12, 0, 0),
                ExpireDateTime = new DateTime(2023, 11, 15, 12, 0, 0),
                City = City.Den_Bosch,
                AdultsOnly = false,
                Products = new List<Product>(),
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Tila",
                    City = City.Breda,
                    Location = Location.Ta,
                },
                ReservedByStudent = new Student
                {
                    FirstName = "Sten",
                    LastName = "Reijerse",
                    StudentNr = 2175709,
                    BirthDate = new DateTime(2000, 10, 28),
                    Email = "sten@mail.com",
                    PhoneNr = 0612345678,
                    StudyCity = City.Breda,
                },
            },
            new MealPackage
            {
                Id = 3,
                Name = "Groentesoep + broodjes",
                Price = 4.50,
                PickUpDateTime = new DateTime(2023, 11, 14, 16, 0, 0),
                ExpireDateTime = new DateTime(2023, 11, 15, 12, 0, 0),
                City = City.Den_Bosch,
                AdultsOnly = false,
                Products = new List<Product>(),
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Boschie",
                    City = City.Den_Bosch,
                    Location = Location.DBb,
                },
                ReservedByStudent = new Student
                {
                    FirstName = "Sten",
                    LastName = "Reijerse",
                    StudentNr = 2175709,
                    BirthDate = new DateTime(2000, 10, 28),
                    Email = "sten@mail.com",
                    PhoneNr = 0612345678,
                    StudyCity = City.Breda,
                },
            },
            };

        [Fact]
        public void GetAvailableMealPackages()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();         

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);
            mealPackageRepoMock.Setup(m => m.GetAvailableMealPackages()).Returns(_mealPackageList);

            var availableMealPackages = mealPackageService.GetAvailableMealPackages();

            Assert.Single(availableMealPackages);
            Assert.Equal("Fruit + sapje", availableMealPackages.ElementAt(0).Name);
       
        }

        [Fact]
        public void GetReservedMealPackages()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);
            mealPackageRepoMock.Setup(m => m.GetReservedMealPackages()).Returns(_mealPackageList);

            var reservedMealPackages = mealPackageService.GetReservedMealPackages();

            Assert.Equal(2, reservedMealPackages.Count());
            Assert.Equal("Tosti + water", reservedMealPackages.ElementAt(0).Name);
            Assert.Equal("Groentesoep + broodjes", reservedMealPackages.ElementAt(1).Name);
        }

    };
}