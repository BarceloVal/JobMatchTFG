using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class ChatMessageDto
    {
        public Guid Id { get; set; }
        public Guid IdSender { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime SentOn { get; set; }

        // Esto lo usarás en la interfaz para saber si mostrarlo alineado a la derecha o izquierda
        public bool IsMine { get; set; }
    }

}
