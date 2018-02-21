using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycombsoft.DAL.Entities
{
    public class Student
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime RegisteredDate { get; set; }
        public int? BeginStudyId { get; set; }

        public virtual BeginStudy BeginStudy { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
