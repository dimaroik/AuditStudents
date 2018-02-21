using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Honeycombsoft.DAL.Entities;
using System.Data.Entity;

namespace Honeycombsoft.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string conectionString) : base(conectionString) { }

        public ApplicationContext() 
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<BeginStudy> BeginStudies{ get; set; }
    }
}
