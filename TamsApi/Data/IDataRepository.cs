using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TamsApi.Core.Identity;
using TamsApi.Models;

namespace TamsApi.Data
{
    public interface IDataRepository
    {
        Task<List<TsaSchedule>> GetTsaSchedulesAsync();
        Task<List<ActivityReport>> GetActivityReportAsync(string logStatuses, DateTime? fromDate, DateTime? toDate);
        Task<List<VPaymentApprovalReport>> GetPaymentApprovalAsync();
    }

}
