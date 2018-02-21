using Honeycombsoft.BLL.DTO;
using Honeycombsoft.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Honeycombsoft.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<bool> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task SetInitialData(UserDTO adminDto, List<string> roles);
        Task<UserDTO> FindByEmail(string userName);
        Task<UserDTO> FindById(string id);
        Task<bool> SetDateStudy(string email, DateTime date);
        IList<BeginStudyDTO> GetAllDateStudy();
        Task<bool> SetConfirmEmail(string email);
        Task<BeginStudyDTO> FindDateById(int id);
       


        
    } 
}
