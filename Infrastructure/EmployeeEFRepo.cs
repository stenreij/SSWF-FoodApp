using Core.Domain;
using Core.DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class EmployeeEFRepo : IEmployeeRepo
    {
        private readonly FoodAppDbContext _context;
        public EmployeeEFRepo(FoodAppDbContext context)
        {
            _context = context;
        }
        public Employee GetEmployeeByEmail(string email)
        {
            return _context.Employee.FirstOrDefault(e => e.Email == email);
        }

        public Employee GetEmployeeById(int id)
        {
            return _context.Employee.First(e => e.Id == id);
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _context.Employee.ToList();
        }
    }
}
