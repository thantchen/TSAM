using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using TamsApi.Core;
using TamsApi.Core.Email;
using TamsApi.Core.Identity;
using TamsApi.Models;
using TamsEmail.Views.Templates;

namespace TamsApi.Data
{
    public class EmailRepository : IEmailRepository
    {
        #region Member variables

        private ApplicationContext _context;

        #endregion Member variables

        #region Constructor

        public EmailRepository(ApplicationContext context, IOptions<SmtpOptions> opts)
        {
            _context = context;
        }

        #endregion Constructor

        #region Public methods

        public async Task<AddRequestSubmittedViewModel> GetNewAddRequestAsync(List<int> idList)
        {
            var data = await _context.AddLog.AsNoTracking()
                                      .Include(i => i.RequestedByUser)
                                      .Include(i => i.SubmittedByUser)
                                      .Include(i => i.ChangeLogType)
                                      .Include(i => i.ChangeLogStatus)
                                      .Include(i => i.AddType)
                                      .Where(c => idList.Contains(c.AddLogId))
                                      .ToListAsync();

            var logNumbers = new List<TsaLogNumber>();
            data.ForEach(c => logNumbers.Add(new TsaLogNumber(c.AddId, c.TsaSubIds)));

            var fileList = await _context.AddLogFile.AsNoTracking()
                                        .Include(i => i.FileRepository)
                                        .Where(c => idList.Contains(c.AddLogId))
                                        .Select(c => c.FileRepository.FileName)
                                        .Distinct()
                                        .OrderBy(o => o)
                                        .ToListAsync();

            var record = data.FirstOrDefault();

            var model = new AddRequestSubmittedViewModel
            {
                Salutation = $"{record.SubmittedByUser.FirstName} {record.SubmittedByUser.LastName}",
                TsaId = record.TsaId,
                RequestDate = System.DateTime.UtcNow.ToEasternTimeZone().ToLongDateString(),
                RequestType = record.ChangeLogType.Name,
                Status = record.ChangeLogStatus.Name,
                RequestedBy = $"{record.RequestedByUser.FirstName} {record.RequestedByUser.LastName}",
                SubmittedBy = $"{record.SubmittedByUser.FirstName} {record.SubmittedByUser.LastName}",
                Comments = record.Comments.Substring(record.Comments.LastIndexOf("]" + System.Environment.NewLine) + 1).TrimStart(),
                Attachments = string.Join("; ", fileList),
                LogNumbers = logNumbers,
                SupportEmailAddress = SmtpOptions.DefaultSupportEmail,
                ToEmailAddress = record.SubmittedByUser.Email,
                AddType = record.AddType.Name
            };

            return model;
        }

        public async Task<StatusChangeAddLogViewModel> GetUpdatedAddRequestAsync(int id)
        {
            var record = await _context.AddLog.AsNoTracking()
                                      .Include(i => i.SubmittedByUser)
                                      .Include(i => i.ChangeLogStatus)
                                      .Include(i => i.AddType)
                                      .Where(c => c.AddLogId == id)
                                      .FirstOrDefaultAsync();

            var model = new StatusChangeAddLogViewModel
            {
                Salutation = $"{record.SubmittedByUser.FirstName} {record.SubmittedByUser.LastName}",
                Id = record.AddId,
                CurrentStatus = record.ChangeLogStatus.Name,
                TsaSubId = record.TsaSubIds,
                TsaId = record.TsaId,
                Comments = record.Comments.Substring(record.Comments.LastIndexOf("]" + System.Environment.NewLine) + 1).TrimStart(),
                SupportEmailAddress = SmtpOptions.DefaultSupportEmail,
                ToEmailAddress = record.SubmittedByUser.Email,
                AddType = record.AddType.Name
            };

            return model;
        }

        public async Task<ChangeRequestSubmittedViewModel> GetNewChangeRequestAsync(List<int> idList)
        {
            var data = await _context.ChangeLog.AsNoTracking()
                                      .Include(i => i.RequestedByUser)
                                      .Include(i => i.SubmittedByUser)
                                      .Include(i => i.ChangeLogType)
                                      .Include(i => i.ChangeLogStatus)
                                      .Where(c => idList.Contains(c.ChangeLogId))
                                      .ToListAsync();

            var logNumbers = new List<TsaLogNumber>();
            data.ForEach(c => logNumbers.Add(new TsaLogNumber(c.ChangeId, c.TsaSubId)));

            var fileList = await _context.ChangeLogFile.AsNoTracking()
                                        .Include(i => i.FileRepository)
                                        .Where(c => idList.Contains(c.ChangeLogId))
                                        .Select(c => c.FileRepository.FileName)
                                        .Distinct()
                                        .OrderBy(o => o)
                                        .ToListAsync();


            var record = data.FirstOrDefault();

            var model = new ChangeRequestSubmittedViewModel
            {
                Salutation = $"{record.SubmittedByUser.FirstName} {record.SubmittedByUser.LastName}",
                TsaId = record.TsaId,
                RequestDate = System.DateTime.UtcNow.ToEasternTimeZone().ToLongDateString(),
                RequestType = record.ChangeLogType.Name,
                Status = record.ChangeLogStatus.Name,
                RequestedBy = $"{record.RequestedByUser.FirstName} {record.RequestedByUser.LastName}",
                SubmittedBy = $"{record.SubmittedByUser.FirstName} {record.SubmittedByUser.LastName}",
                Comments = record.Comments.Substring(record.Comments.LastIndexOf("] + System.Environment.NewLine") + 1).TrimStart(),
                Attachments = string.Join("; ", fileList),
                LogNumbers = logNumbers,
                SupportEmailAddress = SmtpOptions.DefaultSupportEmail,
                ToEmailAddress = record.SubmittedByUser.Email
            };

            return model;
        }

        public async Task<StatusChangeChangeLogViewModel> GetUpdatedChangeRequestAsync(int id)
        {
            var record = await _context.ChangeLog.AsNoTracking()
                                      .Include(i => i.SubmittedByUser)
                                      .Include(i => i.ChangeLogStatus)
                                      .Include(i => i.ChangeLogType)
                                      .Where(c => c.ChangeLogId == id)
                                      .FirstOrDefaultAsync();

            var model = new StatusChangeChangeLogViewModel
            {
                Salutation = $"{record.SubmittedByUser.FirstName} {record.SubmittedByUser.LastName}",
                Id = record.ChangeId,
                CurrentStatus = record.ChangeLogStatus.Name,
                TsaSubId = record.TsaSubId,
                TsaId = record.TsaId,
                Comments = record.Comments.Substring(record.Comments.LastIndexOf("]" + System.Environment.NewLine) + 1).TrimStart(),
                SupportEmailAddress = SmtpOptions.DefaultSupportEmail,
                ToEmailAddress = record.SubmittedByUser.Email,
                RequestType = record.ChangeLogType.Name
            };

            return model;
        }

        public async Task<DisputeRequestSubmittedViewModel> GetNewDisputeRequestAsync(List<int> idList)
        {
            var data = await _context.DisputeLog.AsNoTracking()
                                      .Include(i => i.RequestedByUser)
                                      .Include(i => i.SubmittedByUser)
                                      .Include(i => i.DisputeType)
                                      .Include(i => i.DisputeDiscrepancy)
                                      .Where(c => idList.Contains(c.DisputeLogId))
                                      .ToListAsync();

            var logNumbers = new List<TsaLogNumber>();
            data.ForEach(c => logNumbers.Add(new TsaLogNumber(c.DisputeId, c.TsaSubId)));

            var fileList = await _context.DisputeLogFile.AsNoTracking()
                                        .Include(i => i.FileRepository)
                                        .Where(c => idList.Contains(c.DisputeLogId))
                                        .Select(c => c.FileRepository.FileName)
                                        .Distinct()
                                        .OrderBy(o => o)
                                        .ToListAsync();


            var record = data.FirstOrDefault();

            var model = new DisputeRequestSubmittedViewModel
            {
                Salutation = $"{record.SubmittedByUser.FirstName} {record.SubmittedByUser.LastName}",
                TsaId = record.TsaId,
                RequestDate = System.DateTime.UtcNow.ToEasternTimeZone().ToLongDateString(),
                RequestType = record.DisputeType.Name,
                InvoiceNumber = record.InvoiceNumber,
                RequestedBy = $"{record.RequestedByUser.FirstName} {record.RequestedByUser.LastName}",
                SubmittedBy = $"{record.SubmittedByUser.FirstName} {record.SubmittedByUser.LastName}",
                Discrepancy = record.DisputeType.Name,
                ServicePeriod = record.ServicePeriod.HasValue ? record.ServicePeriod.Value.ToLongDateString() : "",
                Comments = record.Comments.Substring(record.Comments.LastIndexOf("]" + System.Environment.NewLine) + 1).TrimStart(),
                Attachments = string.Join("; ", fileList),
                LogNumbers = logNumbers,
                SupportEmailAddress = SmtpOptions.DefaultSupportEmail,
                ToEmailAddress = record.SubmittedByUser.Email
            };

            return model;
        }

        public async Task<StatusChangeDisputeLogViewModel> GetUpdatedDisputeRequestAsync(int id)
        {
            var record = await _context.DisputeLog.AsNoTracking()
                                      .Include(i => i.SubmittedByUser)
                                      .Include(i => i.DisputeType)
                                      .Where(c => c.DisputeLogId == id)
                                      .FirstOrDefaultAsync();

            CultureInfo cultureInfo;
            switch (record.Currency)
            {
                case "USD":
                    cultureInfo = new CultureInfo("en-US");
                    break;
                case "CAD":
                    cultureInfo = new CultureInfo("en-CA");
                    break;
                case "INR":
                    cultureInfo = new CultureInfo("hi-IN");
                    break;
                default:
                    cultureInfo = CultureInfo.CurrentCulture;
                    break;
            }


            var model = new StatusChangeDisputeLogViewModel
            {
                Salutation = $"{record.SubmittedByUser.FirstName} {record.SubmittedByUser.LastName}",
                Id = record.DisputeId,
                CurrentStatus = record.DisputeType.Name,
                TsaSubId = record.TsaSubId,
                TsaId = record.TsaId,
                Comments = record.Comments.Substring(record.Comments.LastIndexOf("]" + System.Environment.NewLine) + 1).TrimStart(),
                InvoiceNumber = record.InvoiceNumber,
                AgreedCost = record.AgreedCost.HasValue ? $"{record.AgreedCost.Value.ToString("C", cultureInfo)}" : "0",
                Currency = record.Currency,
                SupportEmailAddress = SmtpOptions.DefaultSupportEmail,
                ToEmailAddress = record.SubmittedByUser.Email
            };

            return model;
        }

        #endregion Public methods

    }
}
