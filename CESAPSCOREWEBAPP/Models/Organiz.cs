using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Organiz
    {
        [Key]
        public int organizId { get; set; }
        public int DepartmentId { get; set; }
        public int Department1Id { get; set; }
        public int PositionId { get; set; }
        public int Power { get; set; }
        public Department Departments { get; set; }
        public Department1 Department1s { get; set; }
        public Position Positions { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<HRRecruite> hRRecruites { get; set; }

    }
}
