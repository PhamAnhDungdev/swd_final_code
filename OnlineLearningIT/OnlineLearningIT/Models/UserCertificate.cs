using System;
using System.Collections.Generic;

namespace OnlineLearningIT.Models;

public partial class UserCertificate
{
    public int UserCertificateId { get; set; }

    public int UserId { get; set; }

    public int CertificateId { get; set; }

    public DateTime? IssueDate { get; set; }

    public string? CertificateUrl { get; set; }

    public string? VerificationCode { get; set; }

    public virtual Certificate Certificate { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
