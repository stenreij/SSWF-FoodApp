namespace Application.Services
{
    public interface IMealPackageService
    {
        bool ReserveMealPackage(int mealPackageId, int studentId);
    }
}