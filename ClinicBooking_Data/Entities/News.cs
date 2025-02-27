using System;
using System.Collections.Generic;

namespace ClinicBooking.Entities;

public partial class News
{
    public int NewsId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? Image { get; set; }

    public string? Summary { get; set; }

    public DateTime PublishDate { get; set; }

    public string Category { get; set; } = null!;
}
