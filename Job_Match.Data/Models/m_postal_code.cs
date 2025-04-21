using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Data.Models;

[Table("m_postal_code")]
public partial class m_postal_code
{
    [Key]
    public Guid id { get; set; }

    [NotMapped]
    public string DisplayText => $"{cod_auto_name}/{cod_pro_name}/{cod_loc_name}";


    [StringLength(2)]
    [Unicode(false)]
    public string cod_auto { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string cod_auto_name { get; set; } = null!;

    [StringLength(2)]
    [Unicode(false)]
    public string cod_pro { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string cod_pro_name { get; set; } = null!;

    [StringLength(3)]
    [Unicode(false)]
    public string cod_loc { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string cod_loc_name { get; set; } = null!;

    public bool active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime created_on { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? last_update { get; set; }

    [InverseProperty("id_postal_codeNavigation")]
    public virtual ICollection<m_user_client> m_user_clients { get; set; } = new List<m_user_client>();

    [InverseProperty("id_postal_codeNavigation")]
    public virtual ICollection<m_user_provider> m_user_providers { get; set; } = new List<m_user_provider>();

    [InverseProperty("id_postal_codeNavigation")]
    public virtual ICollection<t_task_request> t_task_requests { get; set; } = new List<t_task_request>();
}
