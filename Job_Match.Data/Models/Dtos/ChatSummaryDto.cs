using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class ChatSummaryDto
    {
        public Guid IdUserProvider { get; set; }
        public string ProviderName { get; set; } = string.Empty;

        public Guid IdUserClient { get; set; }
        public string ClientName { get; set; } = string.Empty;

        public Guid? IdTaskRequest { get; set; }

        public string? LastMessage { get; set; }
        public DateTime? LastMessageDate { get; set; }

        public string LastMessageDateFormatted =>
            LastMessageDate.HasValue ? LastMessageDate.Value.ToString("dd/MM/yyyy HH:mm") : "";

    }
}
