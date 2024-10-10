using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TamsApi.Core;
using TamsApi.Core.Identity;
using TamsApi.Models;
using TamsApi.Models.Lookups;

namespace TamsApi.Data
{
    public class TsaRepository : ITsaRepository, IDataFileRepository<TsaSchedule>,
        IDataFileRepository<Invoice>,
        IDataFileRepository<ChangeResolutionLog>,
        IDataFileRepository<Payment>,
        IDataFileRepository<SellSideNotificationChange>,
        IDataFileRepository<SellSideNotificationDispute>
    {
        private ApplicationContext _context;

        public TsaRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<FileRepository> AddRecordsAsync<TModel>(
            FileRepository file, 
            FileTypeId uploadType,
            List<TModel> parsedRows, 
            AuthenticatedUser user,
            Action<FileRepository, List<TModel>> importRows)
        {
            file.FileTypeId = (int) uploadType;
            importRows.Invoke(file, parsedRows);
            var upload = await _context.AddAsync(file);
            await _context.SaveChangesAsync(user);
            return upload.Entity;
        }

        public async Task<FileRepository> AddRecordsAsync(FileRepository file, List<TsaSchedule> parsedRows, AuthenticatedUser user)
        {
            return await AddRecordsAsync(
                file,
                FileTypeId.TSASchedule,
                parsedRows, user,
                (f, records) => f.TsaSchedule = records);
        }

        public async Task<FileRepository> AddRecordsAsync(FileRepository upload, List<Invoice> parsedRows, AuthenticatedUser user)
        {
            return await AddRecordsAsync(
                upload,
                FileTypeId.Invoice,
                parsedRows, user,
                (f, records) => f.Invoice = records);
        }

        public async Task<FileRepository> AddRecordsAsync(FileRepository upload, List<ChangeResolutionLog> parsedRows, AuthenticatedUser user)
        {
            return await AddRecordsAsync(
                upload,
                FileTypeId.ChangeLog,
                parsedRows, user,
                (f, records) => f.ChangeResolutionLog = records);
        }

        public async Task<FileRepository> AddRecordsAsync(FileRepository upload, List<Payment> parsedRows, AuthenticatedUser user)
        {
            return await AddRecordsAsync(
                upload,
                FileTypeId.Payment,
                parsedRows, user,
                (f, records) => f.Payment = records);
        }

        public async Task<FileRepository> AddRecordsAsync(FileRepository upload, List<SellSideNotificationChange> parsedRows, AuthenticatedUser user)
        {
            return await AddRecordsAsync(
                upload,
                FileTypeId.SellSideNotificationChange,
                parsedRows, user,
                (f, records) => f.SellSideNotificationChange = records);
        }

        public async Task<FileRepository> AddRecordsAsync(FileRepository upload, List<SellSideNotificationDispute> parsedRows, AuthenticatedUser user)
        {
            return await AddRecordsAsync(
                upload,
                FileTypeId.SellSideNotificationDispute,
                parsedRows, user,
                (f, records) => f.SellSideNotificationDispute = records);
        }

        public bool Exists(string tsaSubId)
        {
            return tsaSubId.IsPresent() && _context.TsaSchedule.Any(t => t.TsaSubId == tsaSubId);
        }

        public TsaSchedule FindTsa(string subId)
        {
            if (subId.IsBlank()) return null;

            return _context.TsaSchedule.FirstOrDefault(t => t.TsaSubId == subId.Trim());
        }

        public async Task<int> LogExcelFailuresAsync(string fileName, List<Core.Excel.FailedRow> failures, AuthenticatedUser user)
        {
            foreach(var failure in failures)
            {
                var log = new ExcelFailure
                {
                    FileName = fileName ?? "Unknown",
                    FileType = "TSA Schedule",
                    ExcelRow = failure.RowIndex,
                    CellData = failure.ParsedCells ?? new Dictionary<string, object>(),
                    Errors = string.Join(Environment.NewLine, failure.Errors)
                };
                _context.ExcelFailures.Add(log);
            }

            return await _context.SaveChangesAsync(user);
        }

        public async Task<int> LogExcelFailuresAsync(string fileName, Exception exception, AuthenticatedUser user)
        {
            _context.ExcelFailures.Add(new ExcelFailure
            {
                FileName = fileName ?? "Unknown",
                FileType = "TSA Schedule",
                Errors = exception.Message,
                StackTrace = exception.StackTrace
            });
            return await _context.SaveChangesAsync(user);
        }
    }
}
