﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TsaData.Models;

public partial class AddType
{
    public int AddTypeId { get; set; }

    public string Name { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<AddLog> AddLogs { get; set; } = new List<AddLog>();
}