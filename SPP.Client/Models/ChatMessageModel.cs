using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Client.Models
{
    namespace SPP.Client.Models
    {
        public class ChatMessageModel
        {
            public int Id { get; set; }
            public Guid SessionId { get; set; }
            public string Role { get; set; } // "user" | "assistant" | "system"
            public string Content { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
