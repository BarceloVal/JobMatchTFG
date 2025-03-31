using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Data.Models;

[PrimaryKey("id_task_request", "id_user_reviewer", "id_user_reviewed")]
[Table("t_task_score")]
public partial class t_task_score
{
    [Key]
    public Guid id_task_request { get; set; }

    [Key]
    public Guid id_user_reviewer { get; set; }

    [Key]
    public Guid id_user_reviewed { get; set; }

    public int rating { get; set; }

    [StringLength(2000)]
    [Unicode(false)]
    public string? comment { get; set; }

    public bool active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime created_on { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? last_update { get; set; }

    [ForeignKey("id_task_request")]
    [InverseProperty("t_task_scores")]
    public virtual t_task_request id_task_requestNavigation { get; set; } = null!;

    [ForeignKey("id_user_reviewed")]
    [InverseProperty("t_task_scoreid_user_reviewedNavigations")]
    public virtual m_user id_user_reviewedNavigation { get; set; } = null!;

    [ForeignKey("id_user_reviewer")]
    [InverseProperty("t_task_scoreid_user_reviewerNavigations")]
    public virtual m_user id_user_reviewerNavigation { get; set; } = null!;
}
