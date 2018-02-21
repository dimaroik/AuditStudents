using Honeycombsoft.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycombsoft.DAL.Interfaces
{
    public interface IStudyRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> GetAll();
        void Add(T date);
        Task<T> FindById(int id);
        Task<T> FindByDate(DateTime date);

    }
}
