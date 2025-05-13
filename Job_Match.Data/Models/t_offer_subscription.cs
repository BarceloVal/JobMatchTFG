using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models
{
    [Table("t_offer_subscription")]
    public partial class t_offer_subscription
    {
        public Guid id_task_request { get; set; }

        public Guid id_user_provider { get; set; }

        public bool active { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime created_on { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? last_update { get; set; }

        [ForeignKey("id_task_request")]
        [InverseProperty("t_offer_subscriptions")]
        public virtual t_task_request id_task_requestNavigation { get; set; } = null!;

        [ForeignKey("id_user_provider")]
        [InverseProperty("t_offer_subscriptions")]
        public virtual m_user_provider id_user_providerNavigation { get; set; } = null!;
    }
}
