using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Client.DTO
{
    public class MyScheduleResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Role { get; set; }
        public int TotalShifts { get; set; }
        public List<ShiftDto> Shifts { get; set; }
    }
}
