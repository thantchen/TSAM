using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
//using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TamsApi.Core.Identity;
using TamsApi.Models;

namespace TamsApi.Data
{
    public class DataRepository : IDataRepository
    {
        #region Member variables

        private ApplicationContext _context;

        #endregion Member variables

        #region Constructor

        public DataRepository(ApplicationContext context)
        {
            _context = context;
        }

        #endregion Constructor

        #region Public methods

        public async Task<List<TsaSchedule>> GetTsaSchedulesAsync()
        {
            return await _context.TsaSchedule.AsNoTracking().OrderBy(o => o.TsaId).ThenBy(o => o.TsaSubId).ToListAsync();
        }

        public async Task<List<ActivityReport>> GetActivityReportAsync(string logStatuses, DateTime? fromDate, DateTime? toDate)
        {
            return await _context.GetActivityReport.FromSqlRaw("EXECUTE get_activity_report {0}, {1}, {2}", logStatuses, fromDate, toDate).ToListAsync();
        }

        public async Task<List<VPaymentApprovalReport>> GetPaymentApprovalAsync()
        {
            return await _context.VPaymentApprovalReport.AsNoTracking().OrderBy(o => o.TsaId).ToListAsync();
        }

        #endregion Public mehtods

    }
}
