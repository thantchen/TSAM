﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TsaData.Models;

public partial class AddLogFile
{
    public int AddLogFileId { get; set; }

    public int AddLogId { get; set; }

    public int FileRepositoryId { get; set; }

    public virtual AddLog AddLog { get; set; }

    public virtual FileRepository FileRepository { get; set; }
}