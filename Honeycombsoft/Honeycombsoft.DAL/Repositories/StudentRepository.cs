using Honeycombsoft.DAL.EF;
using Honeycombsoft.DAL.Entities;
using Honeycombsoft.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycombsoft.DAL.Repositories
{
    public class StudentRepository : IStudentRepository<Student>
    {
        public ApplicationContext Database { get; set; }

        public StudentRepository(ApplicationContext db)
        {
            Database = db;
        }

        public void Add(Student student)
        {      
             Database.Students.Add(student);      
        }

        public async Task<Student> GetByIdAsync(string id)
        {
            Student student = await Database.Students.Include(s => s.ApplicationUser).Include(s => s.BeginStudy)
                                    .FirstOrDefaultAsync(s => s.Id == id);
            return student;
        }

        public async Task<Student> GetByEmailAsync(string email)
        {
          
            return await Database.Students.Include(s => s.ApplicationUser).Include(s => s.BeginStudy)
                                    .FirstOrDefaultAsync(s => s.ApplicationUser.Email == email);
        }

        public async Task SetStudyDate(string email, BeginStudy date)
        {
            Student student = await GetByEmailAsync(email);
            student.BeginStudy = date;

            Database.Entry(student).State = EntityState.Modified;   
        }

        public async Task SetConfirmEmailAsync(string email)
        {

            Student student = await GetByEmailAsync(email);

            student.ApplicationUser.EmailConfirmed = true;
            Database.Entry(student).State = EntityState.Modified;
                
        }

        public IEnumerable<Student> GetAllUsers()
        {
            return Database.Students.Include(s=>s.ApplicationUser).Include(s=>s.BeginStudy);
        }


        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
