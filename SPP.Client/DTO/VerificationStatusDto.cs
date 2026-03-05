using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Client.DTO
{
    public class VerificationStatusDto
    {
        public string Name { get; set; }
        public DateTime? Date { get; set; }

        //public bool IsCompleted => Date.HasValue;

    }
}
