﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TsaData.Models;

public partial class VMonthlyEstimate
{
    public string TsaSubId { get; set; }

    public string TsaId { get; set; }

    public string Function { get; set; }

    public string SubFunction { get; set; }

    public string ServiceName { get; set; }

    public string DetailedServiceDescription { get; set; }

    public string TsaVsRtsa { get; set; }

    public string PrimaryOwner { get; set; }

    public string ReceiverOwner { get; set; }

    public string UnitDescription { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? OriginalEndDate { get; set; }

    public DateOnly? ActiveEndDate { get; set; }

    public string SeparationOrStandalone { get; set; }

    public string CostType { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

    public DateOnly? EndOfMonth { get; set; }

    public decimal? MonthlyPricing { get; set; }

    public decimal? MonthlyCostForecast { get; set; }

    public double? MonthlyCostForecastProrated { get; set; }
}