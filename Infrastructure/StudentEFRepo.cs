using Core.Domain;
using Core.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class StudentEFRepo : IStudentRepo
    {
        private readonly FoodAppDbContext _context;

        public StudentEFRepo(FoodAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Student> GetStudents()
        {
            return _context.Student.ToList();
        } 
        
        public Student GetStudentById(int id)
        {
            return _context.Student.First(p => p.Id == id);
        }
    }
}
