using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TamsApi.Migrations;

namespace TamsApi.Models
{
    public class ActivityReport
    {
        public ActivityReport()
        {
        }

        [Key]
        [Column("id")]
        public string Id { get; set; }
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
        [Column("tsa_id")]
        public string TsaId { get; set; }
        [Column("tsa_sub_id")]
        public string TsaSubId { get; set; }
        [Column("function")]
        public string Function { get; set; }
        [Column("primary_owner")]
        public string PrimaryOwner { get; set; }
        [Column("service_name")]
        public string ServiceName { get; set; }
        [Column("effective_date")]
        public DateTime? EffectiveDate { get; set; }
        [Column("log_type")]
        public string LogType { get; set; }
        [Column("log_status")]
        public string LogStatus { get; set; }
        [Column("comments")]
        public string Comments { get; set; }
        [Column("submitted_by")]
        public string SubmittedBy { get; set; }
        [Column("invoice_period_end_date")]
        public DateTime? InvoicePeriodEndDate { get; set; }
        [Column("invoice_number")]
        public string InvoiceNumber { get; set; }
        [Column("invoice_amount", TypeName = "decimal(18,2)")]
        public decimal? InvoiceAmount { get; set; }
        [Column("agreed_cost", TypeName = "decimal(18,2)")]
        public decimal? AgreedCost { get; set; }

        [NotMapped]
        public int DaysOpen 
        {
            get
            {
                return DateTime.Today.Subtract(CreatedDate.Date).Days;
            }
        }

    }

}
