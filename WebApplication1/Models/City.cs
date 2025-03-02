using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class City
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
