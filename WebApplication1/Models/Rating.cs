using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Rating
{
    public int RatingId { get; set; }

    public string? Content { get; set; }

    public int RatingLevelId { get; set; }

    public int? UserId { get; set; }

    public int? DoctorId { get; set; }

    public int? AppointmentId { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual RatingLevel RatingLevel { get; set; } = null!;

    public virtual User? User { get; set; }
}
