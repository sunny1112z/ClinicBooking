using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public string? Qualification { get; set; }

    public string? DoctorInfo { get; set; }

    public string? ProfileImage { get; set; }

    public string? ZoomInfo { get; set; }

    public bool Status { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<Faq> Faqs { get; set; } = new List<Faq>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Role Role { get; set; } = null!;
}
