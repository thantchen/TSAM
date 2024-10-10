using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TamsApi.Migrations;

namespace TamsApi.Models
{
    public class VLogTypeStatus
    {
        public VLogTypeStatus()
        {
        }

        [Column("id")]
        public string Id { get; set; }
        [Column("log_type_id")]
        public int LogTypeId { get; set; }
        [Column("log_type_name")]
        public string LogTypeName { get; set; }
        [Key]
        [Column("log_status_id")]
        public int LogStatusId { get; set; }
        [Column("log_status_name")]
        public string LogStatusName { get; set; }
        [Column("description")]
        public string Description { get; set; }

    }
}
