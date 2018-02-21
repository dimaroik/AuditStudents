using Honeycombsoft.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycombsoft.DAL.Interfaces
{
    public interface IStudentRepository<T> : IDisposable where T: class
    {
        void Add(T item);
        Task<T> GetByIdAsync(string id);
        Task<T> GetByEmailAsync(string email);
        Task SetStudyDate(string email, BeginStudy date);
        IEnumerable<T> GetAllUsers();
        Task SetConfirmEmailAsync(string email);
       
    }
}
