using Honeycombsoft.BLL.DTO;
using Honeycombsoft.BLL.Infrastructure;
using Honeycombsoft.BLL.Interfaces;
using Honeycombsoft.DAL.Entities;
using Honeycombsoft.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure;



namespace Honeycombsoft.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork database;

        public UserService(IUnitOfWork uow)
        {
            database = uow;
        }

        public async Task<bool> Create(UserDTO userDto)
        {
            ApplicationUser user = await database.UserManager.FindByEmailAsync(userDto.Email);

            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };

                IdentityResult result = null;

                if (userDto.Password == null ) // Facebook user
                    result = await database.UserManager.CreateAsync(user);
                else                          // Simple user
                    result = await database.UserManager.CreateAsync(user, userDto.Password);
                
                if (result.Errors.Count() > 0)
                    return false;
               
                await database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                
                Student student = new Student { Id = user.Id, Name = userDto.Name, LastName = userDto.LastName,
                                        Age = userDto.Age, RegisteredDate = userDto.RegisteredDate };
                try
                {
                    database.StudentRepository.Add(student);
                    await database.SaveAsync();

                    return true;
                }
                catch (DbUpdateException)
                {
                    throw new ValidationException("Didn't success add new user:", student.Name + student.LastName);
                }
                      
            }
            else
            {
                return false;
            }
        }

        public async Task<UserDTO> FindById(string id)
        {

            Student user = await database.StudentRepository.GetByIdAsync(id);

            if (user != null)
            {
                if(user.BeginStudy != null)
                    return new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Name,
                        LastName = user.LastName,
                        Email = user.ApplicationUser.Email,
                        ConfirmEmail = user.ApplicationUser.EmailConfirmed,
                        Age = user.Age,
                        RegisteredDate = user.RegisteredDate,
                        StudyDate = user.BeginStudy.DateStudy
                    };
                else
                    return new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Name,
                        LastName = user.LastName,
                        Email = user.ApplicationUser.Email,
                        ConfirmEmail = user.ApplicationUser.EmailConfirmed,
                        Age = user.Age,
                        RegisteredDate = user.RegisteredDate
                       
                    };
            }
            
            throw new ValidationException("Not found user with id:", id.ToString());

        }
        public async Task<UserDTO> FindByEmail(string email)
        {

            Student user = await database.StudentRepository.GetByEmailAsync(email);

            if (user.BeginStudy != null)
                return new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    LastName = user.LastName,
                    Email = user.ApplicationUser.Email,
                    ConfirmEmail = user.ApplicationUser.EmailConfirmed,
                    Age = user.Age,
                    RegisteredDate = user.RegisteredDate,
                    StudyDate = user.BeginStudy.DateStudy
                };
            else
                return new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    LastName = user.LastName,
                    Email = user.ApplicationUser.Email,
                    ConfirmEmail = user.ApplicationUser.EmailConfirmed,
                    Age = user.Age,
                    RegisteredDate = user.RegisteredDate

                };
        }

        public async Task<bool> SetConfirmEmail(string email)
        {

            if (email != null)
            {
                try
                {
                    await database.StudentRepository.SetConfirmEmailAsync(email);
                    await database.SaveAsync();
                }
                catch (DbUpdateException)
                {
                    throw new ValidationException("problem with update confirm mail in : ", email);
                }
               
                return true;
            }
            
            throw new ValidationException("Problem with email" , email);
            
        }

        public IList<BeginStudyDTO> GetAllDateStudy()
        {
            IList<BeginStudyDTO> listDates = new List<BeginStudyDTO>();

            foreach (var beginStudy in database.StudyRepository.GetAll())
            {
                listDates.Add(new BeginStudyDTO
                {
                    Id = beginStudy.Id,
                    Date = beginStudy.DateStudy
                });
            }

            return listDates;
        }

        public async Task<bool> SetDateStudy(string email, DateTime date)
        {
            ApplicationUser user = await database.UserManager.FindByEmailAsync(email);

            if (user != null)
            {
                BeginStudy beginStudy = await database.StudyRepository.FindByDate(date);

                if (beginStudy == null)
                    throw new ValidationException("Not found this date:", date.ToString());

                try
                {
                    await database.StudentRepository.SetStudyDate(email, beginStudy);
                    await database.SaveAsync();
                }
                catch (DbUpdateException)
                {
                    
                    throw new ValidationException("Not success set StudyDate", date.ToString());
                }
               

                return true;
            }
            else
            {
                throw new ValidationException("Not found this email:", email);
            }
        }

        public async Task<BeginStudyDTO> FindDateById(int id)
        {
            BeginStudy beginStudy = await database.StudyRepository.FindById(id);

            return new BeginStudyDTO { 
                     Id = beginStudy.Id,
                     Date = beginStudy.DateStudy 
            };
        }

        





























        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
             ApplicationUser user = null;
            
             if (userDto.Password == null) // facebook
                 user = await database.UserManager.FindByEmailAsync(userDto.Email);
             else // simple registration
                 user = await database.UserManager.FindAsync(userDto.Email, userDto.Password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);

            return claim;
        }

        // initial data in db
        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }

        

        
        public void Dispose()
        {
            database.Dispose();
        }
    }   
}
