using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Client.Models
{
    public class ConfirmationVerification
    {
        public int ID { get; set; }
        public int ID_User { get; set; }
        public int ID_Verification { get; set; }
        public DateTime _Date { get; set; }
    }
}
