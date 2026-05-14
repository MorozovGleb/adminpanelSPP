using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Client.DTO
{
     public class ShiftDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string DayOfWeek { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}
