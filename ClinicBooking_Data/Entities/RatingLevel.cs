using System;
using System.Collections.Generic;

namespace ClinicBooking.Entities;

public partial class RatingLevel
{
    public int RatingLevelId { get; set; }

    public string RatingDescription { get; set; } = null!;

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
