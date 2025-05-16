using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class SendMessageDto
    {
        public Guid IdSender { get; set; }
        public Guid IdReceiver { get; set; }
        public string Message { get; set; }
    }
}
