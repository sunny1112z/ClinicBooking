﻿using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
