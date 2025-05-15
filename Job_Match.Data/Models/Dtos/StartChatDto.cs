using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class StartChatDto
    {
        public Guid IdUserClient { get; set; }
        public Guid IdUserProvider { get; set; }
        public Guid? IdTaskRequest { get; set; } // null si es contacto directo (el cliente encontró al proveedor y quiere sus servicios)
        public string? InitialMessage { get; set; }
    }
}
