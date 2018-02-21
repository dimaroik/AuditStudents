
using Honeycombsoft.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Honeycombsoft.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser> 
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }


    }

 
}
