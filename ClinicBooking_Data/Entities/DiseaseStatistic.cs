using System;
using System.Collections.Generic;

namespace ClinicBooking.Entities;

public partial class DiseaseStatistic
{
    public int StatisticId { get; set; }

    public string Country { get; set; } = null!;

    public int Year { get; set; }

    public string Disease { get; set; } = null!;

    public int Cases { get; set; }

    public int Deaths { get; set; }
}
