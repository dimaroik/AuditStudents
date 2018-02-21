using Honeycombsoft.DAL.EF;
using Honeycombsoft.DAL.Entities;
using Honeycombsoft.DAL.Identity;
using Honeycombsoft.DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycombsoft.DAL.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;

        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private IStudentRepository<Student> studentRepository;
        private IStudyRepository<BeginStudy> studyRepository;

        public IdentityUnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            studentRepository = new StudentRepository(db);
            studyRepository = new StudyRepository(db);
        }

        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }

        public IStudentRepository<Student> StudentRepository
        {
            get { return studentRepository; }
        }
        public IStudyRepository<BeginStudy> StudyRepository
        {
            get { return studyRepository; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    studentRepository.Dispose();
                    studyRepository.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
