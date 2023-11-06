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
                PickUpDateTime = DateTime.Now.AddDays(1),
                ExpireDateTime = DateTime.Now.AddDays(2),
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
                PickUpDateTime = DateTime.Now.AddDays(1),
                ExpireDateTime = DateTime.Now.AddDays(2),
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
                PickUpDateTime = DateTime.Now.AddDays(2),
                ExpireDateTime = DateTime.Now.AddDays(3),
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
                PickUpDateTime = DateTime.Now.AddDays(1),
                ExpireDateTime = DateTime.Now.AddDays(2),
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

        [Fact]
        public void GetReservedMealPackagesByStudent()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);
            mealPackageRepoMock.Setup(m => m.GetReservedMealPackagesByStudent(2)).Returns(_mealPackageList);

            var reservedMealPackagesStudent = mealPackageService.GetReservedMealPackagesByStudent(2);

            Assert.Single(reservedMealPackagesStudent);
            Assert.Equal("Panini + frisdrank", reservedMealPackagesStudent.ElementAt(0).Name);
            Assert.Equal(2, reservedMealPackagesStudent.ElementAt(0).ReservedByStudent.Id);
            Assert.Equal("Freek", reservedMealPackagesStudent.ElementAt(0).ReservedByStudent.FirstName);
            Assert.Equal("Vonk", reservedMealPackagesStudent.ElementAt(0).ReservedByStudent.LastName);
        }


        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //Add MealPackages Tests----------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        [Fact]
        public void AddMealPackageValid()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);

            var newMealPackage = new MealPackage
            {
                Id = 101,
                Name = "Hartige broodjes",
                Price = 3.50,
                PickUpDateTime = DateTime.Now.AddDays(1),
                ExpireDateTime = DateTime.Now.AddDays(2),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Bolletje",
                        ContainsAlcohol = false,
                        PhotoUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP"
                    }
                },
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Canteen 1",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };

            mealPackageRepoMock.Setup(m => m.AddMealPackage(newMealPackage)).Returns(newMealPackage);

            var addedMealPackage = mealPackageService.AddMealPackage(newMealPackage);
            Assert.NotNull(addedMealPackage);
            Assert.Equal(101, addedMealPackage.Id);
            Assert.Equal("Hartige broodjes", addedMealPackage.Name);
            Assert.Equal("Bolletje", addedMealPackage.Products.ElementAt(0).Name);
        }

        [Fact]
        public void AddMealPackageInvalidPickUpDateTime()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);

            var newMealPackage = new MealPackage
            {
                Id = 101,
                Name = "Hartige broodjes",
                Price = 3.50,
                PickUpDateTime = DateTime.Now.AddDays(4),
                ExpireDateTime = DateTime.Now.AddDays(7),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Bolletje",
                        ContainsAlcohol = false,
                        PhotoUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP"
                    },
                },
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Canteen 1",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };
            var exception = Assert.Throws<ArgumentException>(() => mealPackageService.AddMealPackage(newMealPackage));
            Assert.Equal("Pickup date must be within 2 days.", exception.Message);
        }

        [Fact]
        public void AddMealPackageInvalidPickUpDateTimeLaterThanExpireDateTime()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);

            var newMealPackage = new MealPackage
            {
                Id = 101,
                Name = "Hartige broodjes",
                Price = 3.50,
                PickUpDateTime = DateTime.Now.AddDays(1),
                ExpireDateTime = DateTime.Now.AddDays(0),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Bolletje",
                        ContainsAlcohol = false,
                        PhotoUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP"
                    },
                },
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Canteen 1",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };

            var exception = Assert.Throws<ArgumentException>(() => mealPackageService.AddMealPackage(newMealPackage));
            Assert.Equal("Pickup date must be before expiration date.", exception.Message);
        }

        [Fact]
        public void AddMealPackageInvalidNoProducts()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);

            var newMealPackage = new MealPackage
            {
                Id = 101,
                Name = "Hartige broodjes",
                Price = 3.50,
                PickUpDateTime = DateTime.Now.AddDays(1),
                ExpireDateTime = DateTime.Now.AddDays(2),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>(),
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Canteen 1",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };

            var exception = Assert.Throws<ArgumentException>(() => mealPackageService.AddMealPackage(newMealPackage));
            Assert.Equal("At least one product must be added to the meal package.", exception.Message);
        }

        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //Update MealPackages Tests-------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------

        [Fact]
        public void UpdateMealPackageValid()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);
          
            var mealPackage = new MealPackage()
            {
                Id = 101,
                Name = "Hartige broodjes",
                Price = 3.50,
                PickUpDateTime = DateTime.Now.AddDays(1),
                ExpireDateTime = DateTime.Now.AddDays(2),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Bolletje",
                        ContainsAlcohol = false,
                        PhotoUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP"
                    },
                },
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Canteen 1",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };

            var mealPackageUpdated = new MealPackage()
            {
                Id = 101,
                Name = "Croissants",
                Price = 1.00,
                PickUpDateTime = DateTime.Now.AddDays(1),
                ExpireDateTime = DateTime.Now.AddDays(2),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Croissant",
                        ContainsAlcohol = false,
                        PhotoUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP"
                    },
                },
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "LALA",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };

            mealPackageRepoMock.Setup(m => m.GetMealPackageById(101)).Returns(mealPackage);
            mealPackageRepoMock.Setup(m => m.EditMealPackage(mealPackageUpdated)).Returns(mealPackageUpdated);


            var updatedMealPackage = mealPackageService.EditMealPackage(mealPackageUpdated);
            Assert.NotNull(updatedMealPackage);
            Assert.Equal(101, updatedMealPackage.Id);
            Assert.Equal("Croissants", updatedMealPackage.Name);
            Assert.Equal("Croissant", updatedMealPackage.Products.ElementAt(0).Name);
            Assert.Equal(1.00 , updatedMealPackage.Price);
        }

        [Fact]
        public void UpdateMealPackageInvalidPickUpDateTime()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);

            var mealPackage = new MealPackage()
            {
                Id = 101,
                Name = "Hartige broodjes",
                Price = 3.50,
                PickUpDateTime = DateTime.Now.AddDays(1),
                ExpireDateTime = DateTime.Now.AddDays(2),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Bolletje",
                        ContainsAlcohol = false,
                        PhotoUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP"
                    },
                },
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Canteen 1",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };

            var mealPackageUpdated = new MealPackage()
            {
                Id = 101,
                Name = "Croissants",
                Price = 1.00,
                PickUpDateTime = DateTime.Now.AddDays(4),
                ExpireDateTime = DateTime.Now.AddDays(7),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Croissant",
                        ContainsAlcohol = false,
                        PhotoUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP"
                    },
                },
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "LALA",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };

            mealPackageRepoMock.Setup(m => m.GetMealPackageById(101)).Returns(mealPackage);
            mealPackageRepoMock.Setup(m => m.EditMealPackage(mealPackageUpdated)).Returns(mealPackageUpdated);
         
            var exception = Assert.Throws<ArgumentException>(() => mealPackageService.AddMealPackage(mealPackageUpdated));
            Assert.Equal("Pickup date must be within 2 days.", exception.Message);
        }

        [Fact]
        public void UpdateMealPackageInvalidPickUpDateTimeLaterThanExpireDateTime()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);

            var mealPackage = new MealPackage()
            {
                Id = 101,
                Name = "Hartige broodjes",
                Price = 3.50,
                PickUpDateTime = DateTime.Now.AddDays(1),
                ExpireDateTime = DateTime.Now.AddDays(2),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Bolletje",
                        ContainsAlcohol = false,
                        PhotoUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP"
                    },
                },
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Canteen 1",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };

            var mealPackageUpdated = new MealPackage()
            {
                Id = 101,
                Name = "Croissants",
                Price = 1.00,
                PickUpDateTime = DateTime.Now.AddDays(2),
                ExpireDateTime = DateTime.Now.AddDays(1),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Croissant",
                        ContainsAlcohol = false,
                        PhotoUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP"
                    },
                },
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "LALA",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };

            mealPackageRepoMock.Setup(m => m.GetMealPackageById(101)).Returns(mealPackage);

            mealPackageRepoMock.Setup(m => m.EditMealPackage(mealPackageUpdated)).Returns(mealPackageUpdated);

            var exception = Assert.Throws<ArgumentException>(() => mealPackageService.AddMealPackage(mealPackageUpdated));
            Assert.Equal("Pickup date must be before expiration date.", exception.Message);
        }

        [Fact]
        public void UpdateMealPackageInvalidNoProducts()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);

            var mealPackage = new MealPackage()
            {
                Id = 101,
                Name = "Hartige broodjes",
                Price = 3.50,
                PickUpDateTime = DateTime.Now.AddDays(1),
                ExpireDateTime = DateTime.Now.AddDays(2),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Bolletje",
                        ContainsAlcohol = false,
                        PhotoUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP"
                    },
                },
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Canteen 1",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };

            var mealPackageUpdated = new MealPackage()
            {
                Id = 101,
                Name = "Croissants",
                Price = 1.00,
                PickUpDateTime = DateTime.Now.AddDays(2),
                ExpireDateTime = DateTime.Now.AddDays(3),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>(),
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "LALA",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };

            mealPackageRepoMock.Setup(m => m.GetMealPackageById(101)).Returns(mealPackage);
            mealPackageRepoMock.Setup(m => m.EditMealPackage(mealPackageUpdated)).Returns(mealPackageUpdated);

            var exception = Assert.Throws<ArgumentException>(() => mealPackageService.AddMealPackage(mealPackageUpdated));
            Assert.Equal("At least one product must be added to the meal package.", exception.Message);
        }

        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //Delete MealPackages Tests-------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------

        [Fact]
        public void DeleteMealPackageValid()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);

            var mealPackage = new MealPackage()
            {
                Id = 101,
                Name = "Hartige broodjes",
                Price = 3.50,
                PickUpDateTime = DateTime.Now.AddDays(1),
                ExpireDateTime = DateTime.Now.AddDays(2),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Bolletje",
                        ContainsAlcohol = false,
                        PhotoUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP"
                    },
                },
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Canteen 1",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };

            mealPackageRepoMock.Setup(m => m.GetMealPackageById(101)).Returns(mealPackage);
            var deletedMealPackage = mealPackageService.DeleteMealPackage(101);
            Assert.True(deletedMealPackage);
        }

        [Fact]
        public void DeleteMealPackageInvalidAlreadyReserved()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);

            var mealPackage = new MealPackage()
            {
                Id = 101,
                Name = "Hartige broodjes",
                Price = 3.50,
                PickUpDateTime = DateTime.Now.AddDays(1),
                ExpireDateTime = DateTime.Now.AddDays(2),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Bolletje",
                        ContainsAlcohol = false,
                        PhotoUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP"
                    },
                },
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Canteen 1",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = new Student
                {
                    Id = 2,
                    FirstName = "Freek",
                    LastName = "Vonk",
                    StudentNr = 2175709,
                    BirthDate = new DateTime(2000, 10, 28),
                    Email = "freek@mail.com",
                    PhoneNr = 0612345678,
                    StudyCity = City.Den_Bosch,
                },
            };

            mealPackageRepoMock.Setup(m => m.GetMealPackageById(101)).Returns(mealPackage);
            var mealPackageToDelete = mealPackageService.DeleteMealPackage(101);
            Assert.False(mealPackageToDelete);
        }


        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //Reserve MealPackage Tests-------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------

        [Fact]
        public void ReserveMealPackageValid()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);

            var mealPackage = new MealPackage()
            {
                Id = 101,
                Name = "Hartige broodjes",
                Price = 3.50,
                PickUpDateTime = DateTime.Now.AddDays(2),
                ExpireDateTime = DateTime.Now.AddDays(3),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Bolletje",
                        ContainsAlcohol = false,
                        PhotoUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP"
                    },
                },
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Canteen 1",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };

            var student = new Student()
            {
                Id = 2020,
                FirstName = "Leonardo",
                LastName = "Da Vinci",
                Email = "leo@mail.com",
                BirthDate = new DateTime(1960, 08, 08),
                StudentNr = 1235479,
                StudyCity = City.Breda,
                PhoneNr = 0612345678,
            };

            mealPackageRepoMock.Setup(m => m.GetMealPackageById(101)).Returns(mealPackage);
            studentRepoMock.Setup(s => s.GetStudentById(2020)).Returns(student);

            var mealPackageToReserve = mealPackageService.ReserveMealPackage(101, 2020);
            Assert.Equal("Leonardo", student.FirstName);
            Assert.True(mealPackageToReserve);
        }

        [Fact]
        public void ReserveMealPackageInvalidAlreadyReservedMealPackageForThatDay()
        {
            var mealPackageRepoMock = new Mock<IMealPackageRepo>();
            var studentRepoMock = new Mock<IStudentRepo>();
            var canteenRepoMock = new Mock<ICanteenRepo>();

            var mealPackageService = new MealPackageService(mealPackageRepoMock.Object, studentRepoMock.Object, canteenRepoMock.Object);

            var mealPackage = new MealPackage()
            {
                Id = 101,
                Name = "Hartige broodjes",
                Price = 3.50,
                PickUpDateTime = DateTime.Now.AddDays(2),
                ExpireDateTime = DateTime.Now.AddDays(3),
                City = City.Breda,
                AdultsOnly = false,
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Bolletje",
                        ContainsAlcohol = false,
                        PhotoUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP"
                    },
                },
                MealType = MealType.Bread,
                Canteen = new Canteen
                {
                    Id = 1,
                    CanteenName = "Canteen 1",
                    City = City.Breda,
                    Location = Location.La,
                },
                ReservedByStudent = null,
            };

            var student = new Student()
            {
                Id = 1,
                FirstName = "Sten",
                LastName = "Reijerse", 
                Email = "sten@mail.com",
                BirthDate = new DateTime(2000, 10, 28), 
                StudentNr = 2175709,                                    
                StudyCity = City.Breda,
                PhoneNr = 0612345678,
            };

            mealPackageRepoMock.Setup(m => m.GetMealPackageById(101)).Returns(mealPackage);
            mealPackageRepoMock.Setup(m => m.GetReservedMealPackagesByStudent(1)).Returns(_mealPackageList);
            studentRepoMock.Setup(s => s.GetStudentById(1)).Returns(student);

            var reservedMealPackagesStudent = mealPackageService.GetReservedMealPackagesByStudent(1);

            var exception = Assert.Throws<ArgumentException>(() => mealPackageService.ReserveMealPackage(mealPackage.Id, student.Id));
            Assert.Equal("You have already reserved a mealpackage for this day.", exception.Message);
            Assert.Null(mealPackage.ReservedByStudent);
        }

        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //Cancel MealPackage Reservation Tests--------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
    };
}