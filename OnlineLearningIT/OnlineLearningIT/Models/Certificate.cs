using System;
using System.Collections.Generic;

namespace OnlineLearningIT.Models;

public partial class Certificate
{
    public int CertificateId { get; set; }

    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? TemplateUrl { get; set; }

    public int? MinCompletionPercentage { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<UserCertificate> UserCertificates { get; set; } = new List<UserCertificate>();
}
