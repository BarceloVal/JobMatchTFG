using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Data.Models;

[Table("t_task_score")]
public partial class t_task_score
{
    public Guid id_user_reviewer { get; set; }

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

    [ForeignKey("id_user_reviewed")]
    [InverseProperty("t_task_scoreid_user_reviewedNavigations")]
    public virtual m_user id_user_reviewedNavigation { get; set; } = null!;

    [ForeignKey("id_user_reviewer")]
    [InverseProperty("t_task_scoreid_user_reviewerNavigations")]
    public virtual m_user id_user_reviewerNavigation { get; set; } = null!;
}
