using Honeycombsoft.DAL.EF;
using Honeycombsoft.DAL.Entities;
using Honeycombsoft.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;


namespace Honeycombsoft.DAL.Repositories
{
    public class StudyRepository : IStudyRepository<BeginStudy>
    {
        public ApplicationContext Database { get; set; }
        public StudyRepository(ApplicationContext db)
        {
            Database = db;
        }

        public IEnumerable<BeginStudy> GetAll()
        {
            return Database.BeginStudies;
        }

        public void Add(BeginStudy date)
        {
            Database.BeginStudies.Add(date);
        }

        public async Task<BeginStudy> FindById(int id)
        {
            BeginStudy beginStudy = await Database.BeginStudies.FirstOrDefaultAsync(b => b.Id == id);
            return beginStudy;
        }
       
        public async Task<BeginStudy> FindByDate(DateTime date)
        {
            BeginStudy beginStudy = await Database.BeginStudies.FirstOrDefaultAsync(b => DateTime.Compare(b.DateStudy, date) == 0);
            return beginStudy;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

    }
}
