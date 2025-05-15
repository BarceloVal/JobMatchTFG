using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Data.Models;

[PrimaryKey("id_user_provider", "id_task_request")]
[Table("t_task_contact")]
public partial class t_task_contact
{
    [Key]
    public Guid id_user_provider { get; set; }

    [Key]
    public Guid id_task_request { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime date_add { get; set; }

    public int status_provider { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? date_status { get; set; }

    public bool can_chat { get; set; }
    public bool active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime created_on { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? last_update { get; set; }

    [ForeignKey("id_task_request")]
    [InverseProperty("t_task_contacts")]
    public virtual t_task_request id_task_requestNavigation { get; set; } = null!;

    [ForeignKey("id_user_provider")]
    [InverseProperty("t_task_contacts")]
    public virtual m_user_provider id_user_providerNavigation { get; set; } = null!;
}
