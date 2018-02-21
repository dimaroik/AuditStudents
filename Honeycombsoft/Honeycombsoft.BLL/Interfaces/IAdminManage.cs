using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Honeycombsoft.BLL.DTO;

namespace Honeycombsoft.BLL.Interfaces
{
    public interface IAdminManage :  IDisposable 
    {
        IList<UserDTO> GetListUsers();
        Task AddStudyDate(DateTime date);
        IList<BeginStudyDTO> GetAllDateStudy();
    }
}
