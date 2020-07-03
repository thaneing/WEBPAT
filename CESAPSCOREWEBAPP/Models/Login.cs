using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Login
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int CheckUserId { get; set; }
        public int PermisionId { get; set; }
        public int TypeOfUserId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public Permision Permisions { get; set; }
        public CheckUser CheckUsers { get; set; }
        public TypeOfUser TypeOfUsers { get; set; }
        public User Users { get; set; }


    }
}
