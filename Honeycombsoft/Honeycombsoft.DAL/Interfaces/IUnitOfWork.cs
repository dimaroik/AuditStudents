using Honeycombsoft.DAL.Entities;
using Honeycombsoft.DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycombsoft.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        IStudentRepository<Student> StudentRepository { get; }
        IStudyRepository<BeginStudy> StudyRepository { get; }
        Task SaveAsync();
    }
}
