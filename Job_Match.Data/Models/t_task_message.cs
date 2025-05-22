using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Data.Models;

[Table("t_task_message")]
public partial class t_task_message
{
    [Key]
    public Guid id { get; set; }

    public Guid? id_task_request { get; set; }

    public Guid id_user_sender { get; set; }

    public Guid id_user_reciver { get; set; }

    [StringLength(2000)]
    [Unicode(false)]
    public string message { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime sent_on { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? read_on { get; set; }

    public bool active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime created_on { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? last_update { get; set; }

    [ForeignKey("id_task_request")]
    [InverseProperty("t_task_messages")]
    public virtual t_task_request id_task_requestNavigation { get; set; } = null!;

    [ForeignKey("id_user_reciver")]
    [InverseProperty("t_task_messageid_user_reciverNavigations")]
    public virtual m_user id_user_reciverNavigation { get; set; } = null!;

    [ForeignKey("id_user_sender")]
    [InverseProperty("t_task_messageid_user_senderNavigations")]
    public virtual m_user id_user_senderNavigation { get; set; } = null!;
}
