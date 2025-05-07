using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class CreateTaskRequestDto
    {
        public Guid IdUserClient { get; set; } // Cliente que ofrece la oferta
        public Guid IdPostalCode { get; set; } // Ubicación del cliente / lugar de trabajo
        public Guid IdService { get; set; } // Servicio específico
        public Guid IdStatus { get; set; } // Estado de la tarea
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
