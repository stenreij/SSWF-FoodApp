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
                    CanteenName = "La",
                    City = City.Breda,
                    Location = Location.La,
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
                City = City.Tilburg,
                AdultsOnly = false,
                Products = new List<Product>(),
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 2,
                    CanteenName = "Tila",
                    City = City.Tilburg,
                    Location = Location.Ta,
                },
                ReservedByStudent = new Student
                {
                    Id = 1,
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
                    Id = 3,
                    CanteenName = "Boschie",
                    City = City.Den_Bosch,
                    Location = Location.DBb,
                },
                ReservedByStudent = new Student
                {
                    Id= 1,
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
                Id = 4,
                Name = "Panini + frisdrank",
                Price = 4.75,
                PickUpDateTime = new DateTime(2023, 11, 15, 16, 0, 0),
                ExpireDateTime = new DateTime(2023, 11, 16, 12, 0, 0),
                City = City.Den_Bosch,
                AdultsOnly = false,
                Products = new List<Product>(),
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 3,
                    CanteenName = "Boschie",
                    City = City.Den_Bosch,
                    Location = Location.DBb,
                },
                ReservedByStudent = new Student
                {
                    Id= 2,
                    FirstName = "Freek",
                    LastName = "Vonk",
                    StudentNr = 2175709,
                    BirthDate = new DateTime(2000, 10, 28),
                    Email = "freek@mail.com",
                    PhoneNr = 0612345678,
                    StudyCity = City.Den_Bosch,
                },
            },
            };

        //--------------------------------------------------------------------------------------------------------------
        //Get MealPackages Tests----------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------

        [Fact]
        public void GetMealPackages()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);
            mealPackageRepoMock.Setup(m => m.GetMealPackages()).Returns(_mealPackageList);

            var mealPackages = mealPackageService.GetMealPackages();

            Assert.Equal(4, mealPackages.Count());
            Assert.Equal("Fruit + sapje", mealPackages.ElementAt(0).Name);
            Assert.Equal("Tosti + water", mealPackages.ElementAt(1).Name);
            Assert.Equal("Groentesoep + broodjes", mealPackages.ElementAt(2).Name);
            Assert.Equal("Panini + frisdrank", mealPackages.ElementAt(3).Name);
        }

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

            Assert.Equal(3, reservedMealPackages.Count());
            Assert.Equal("Tosti + water", reservedMealPackages.ElementAt(0).Name);
            Assert.Equal("Groentesoep + broodjes", reservedMealPackages.ElementAt(1).Name);
            Assert.Equal("Panini + frisdrank", reservedMealPackages.ElementAt(2).Name);
        }

        [Fact]
        public void GetMealPackageById()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageToReturn = new MealPackage
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
                    Id = 2,
                    CanteenName = "Tila",
                    City = City.Breda,
                    Location = Location.Ta,
                },
                ReservedByStudent = null,
            };

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);
            mealPackageRepoMock.Setup(m => m.GetMealPackageById(1)).Returns(mealPackageToReturn);

            var mealPackageById = mealPackageService.GetMealPackageById(1);

            Assert.Equal("Fruit + sapje", mealPackageById.Name);
            Assert.Equal(2.25, mealPackageById.Price);
        }

        [Fact]
        public void GetMealPackagesByCanteenId()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);
            mealPackageRepoMock.Setup(m => m.GetMealPackagesByCanteenId(3)).Returns(_mealPackageList);

            var mealPackagesCanteen = mealPackageService.GetMealPackagesByCanteenId(3);

            Assert.Equal(2, mealPackagesCanteen.Count());
            Assert.Equal("Groentesoep + broodjes", mealPackagesCanteen.ElementAt(0).Name);
            Assert.Equal(3, mealPackagesCanteen.ElementAt(0).Canteen.Id);
            Assert.Equal("Panini + frisdrank", mealPackagesCanteen.ElementAt(1).Name);
            Assert.Equal(3, mealPackagesCanteen.ElementAt(1).Canteen.Id);
        }

        [Fact]
        public void GetMealPackagesOtherCanteens()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);
            mealPackageRepoMock.Setup(m => m.GetMealPackages()).Returns(_mealPackageList);

            var mealPackagesCanteen = mealPackageService.GetMealPackagesOtherCanteens(3);

            Assert.Equal(2, mealPackagesCanteen.Count());
            Assert.Equal("Fruit + sapje", mealPackagesCanteen.ElementAt(0).Name);
            Assert.Equal(1, mealPackagesCanteen.ElementAt(0).Canteen.Id);
            Assert.Equal("Tosti + water", mealPackagesCanteen.ElementAt(1).Name);
            Assert.Equal(2, mealPackagesCanteen.ElementAt(1).Canteen.Id);
        }


        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //Add MealPackages Tests----------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //Update MealPackages Tests-------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //Delete MealPackages Tests-------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //Reserve MealPackage Tests-------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //Cancel MealPackage Reservation Tests--------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
    };
}