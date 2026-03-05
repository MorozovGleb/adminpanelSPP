using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SPP.Client.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone_number { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        
        public int ID_Role { get; set; }

        public string Role { get; set; }

    }
    public class UserNotRole
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone_number { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public int ID_Role { get; set; }
        public Role Role { get; set; }

    }
}
