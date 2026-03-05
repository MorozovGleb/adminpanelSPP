using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Client.DTO
{
    public class LoginRequestDto
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
