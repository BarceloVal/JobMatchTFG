using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Data.Models;

[Table("t_task_request")]
public partial class t_task_request
{
    [Key]
    public Guid id { get; set; }

    public Guid id_user_client { get; set; }

    public Guid id_postal_code { get; set; }

    public Guid id_service { get; set; }

    public Guid id_status { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? title { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime date_start { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? date_end { get; set; }


    [StringLength(200)]
    [Unicode(false)]
    public string? description { get; set; }

    public bool active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime created_on { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? last_update { get; set; }

    [ForeignKey("id_postal_code")]
    [InverseProperty("t_task_requests")]
    public virtual m_postal_code id_postal_codeNavigation { get; set; } = null!;

    [ForeignKey("id_service")]
    [InverseProperty("t_task_requests")]
    public virtual m_service id_serviceNavigation { get; set; } = null!;

    [ForeignKey("id_status")]
    [InverseProperty("t_task_requests")]
    public virtual m_task_status id_statusNavigation { get; set; } = null!;

    [ForeignKey("id_user_client")]
    [InverseProperty("t_task_requests")]
    public virtual m_user_client id_user_clientNavigation { get; set; } = null!;

    [InverseProperty("id_task_requestNavigation")]
    public virtual ICollection<t_task_contact> t_task_contacts { get; set; } = new List<t_task_contact>();

    [InverseProperty("id_task_requestNavigation")]
    public virtual ICollection<t_task_message> t_task_messages { get; set; } = new List<t_task_message>();

    [InverseProperty("id_task_requestNavigation")]
    public virtual ICollection<t_task_score> t_task_scores { get; set; } = new List<t_task_score>();

    [InverseProperty("id_task_requestNavigation")]
    public virtual ICollection<t_offer_subscription> t_offer_subscriptions { get; set; } = new List<t_offer_subscription>();
}
