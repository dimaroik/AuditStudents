using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Honeycombsoft.BLL.Interfaces;
using Honeycombsoft.BLL.DTO;
using Honeycombsoft.DAL.Repositories;
using Honeycombsoft.DAL.Interfaces;
using System.Data.Entity;
using Honeycombsoft.DAL.Entities;
using System.Data.Entity.Infrastructure;
using Honeycombsoft.BLL.Infrastructure;

namespace Honeycombsoft.BLL.Management
{
    public class AdminManage : IAdminManage
    {
        IUnitOfWork Database { get; set; }

        public AdminManage(IUnitOfWork uow){
            Database = uow;
        }

        public IList<UserDTO> GetListUsers()
        {
            IList<UserDTO> listUsers = new List<UserDTO>();

            foreach (Student user in Database.StudentRepository.GetAllUsers())
            {
                if(user.BeginStudy != null)
                    listUsers.Add(new UserDTO{

                            Id = user.Id,
                            Name = user.Name,
                            LastName = user.LastName,
                            Email = user.ApplicationUser.Email,
                            Age = user.Age,
                            RegisteredDate = user.RegisteredDate,
                            StudyDate = user.BeginStudy.DateStudy
                    });
                else
                    listUsers.Add(new UserDTO
                    {

                        Id = user.Id,
                        Name = user.Name,
                        LastName = user.LastName,
                        Email = user.ApplicationUser.Email,
                        Age = user.Age,
                        RegisteredDate = user.RegisteredDate
                    });
            }

            return listUsers;
        }

        public async Task AddStudyDate(DateTime date)
        {
            try
            {
                Database.StudyRepository.Add(new BeginStudy { DateStudy = date });
                await Database.SaveAsync();
            }
            catch (DbUpdateException)
            {
                throw new ValidationException("Didn't success add new BeginStudy" , date.ToString());
            }
        }

        public IList<BeginStudyDTO> GetAllDateStudy()
        {
            IList<BeginStudyDTO> listDates = new List<BeginStudyDTO>();

            foreach (var beginStudy in Database.StudyRepository.GetAll())
            {
                listDates.Add(new BeginStudyDTO
                {
                    Id = beginStudy.Id,
                    Date = beginStudy.DateStudy
                });
            }

            return listDates;
        }

        

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
