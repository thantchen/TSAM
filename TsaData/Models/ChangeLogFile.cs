﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TsaData.Models;

public partial class ChangeLogFile
{
    public int ChangeLogFileId { get; set; }

    public int ChangeLogId { get; set; }

    public int FileRepositoryId { get; set; }

    public virtual ChangeLog ChangeLog { get; set; }

    public virtual FileRepository FileRepository { get; set; }
}