using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycombsoft.DAL.Entities
{
    public class BeginStudy
    {
        public int Id{ get; set; }
        public DateTime DateStudy { get; set; }

        public ICollection<Student> ClientProfiles { get; set; }
    }
}
