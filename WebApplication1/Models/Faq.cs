using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Faq
{
    public int Faqid { get; set; }

    public string Question { get; set; } = null!;

    public string? Answer { get; set; }

    public int? UserId { get; set; }

    public int? DoctorId { get; set; }

    public DateTime? SentDate { get; set; }

    public string? Notes { get; set; }

    public int Status { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual User? User { get; set; }
}
