using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TamsApi.Core;
using TamsApi.Core.Identity;
using TamsApi.Models;
using TamsApi.Models.Lookups;

namespace TamsApi.Data
{
    public class InputRepository : IInputRepository
    {
        #region Member variables

        private ApplicationContext _context;

        #endregion Member variables

        #region Constructor

        public InputRepository(ApplicationContext context)
        {
            _context = context;
        }

        #endregion Constructor

        #region Public methods

        public async Task<List<ChangeLog>> UpdateChangeLogAsync(ChangeLog changeLog, AuthenticatedUser user)
        {
            var logs = new List<ChangeLog>();
            var currentDate = GetTimeStamp();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (changeLog.ChangeLogId == 0) // Add new
                    {
                        var currentLogId = _context.ChangeLog.AsNoTracking().Where(c => c.ChangeLogTypeId == changeLog.ChangeLogTypeId).Max(c => c.ChangeId);
                        var currentSeed = currentLogId == null ? 0 : Int32.Parse(Regex.Replace(currentLogId, "[^0-9 _]", ""));
                        var changeType = ((LogTypeId)changeLog.ChangeLogTypeId).ToString().Substring(0, 1);

                        var tsaSubIds = changeLog.TsaSubId.Split(",").ToList();
                        foreach (var tsaSubId in tsaSubIds)
                        {
                            var log = changeLog.DeepClone<ChangeLog>();

                            // Check to see to action is Cancellation again. If yes, get the last Cancellation and update instead of creating new record
                            var cancellation = _context.ChangeLog.Where(c => c.ChangeLogTypeId == (int)LogTypeId.Cancellation && c.TsaSubId == tsaSubId)
                                                                 .OrderByDescending(o => o.ChangeLogId)
                                                                 .FirstOrDefault();

                            if (cancellation != null && changeLog.ChangeLogTypeId == (int)LogTypeId.Cancellation)
                            {
                                cancellation.ChangeDate = changeLog.ChangeDate;
                                cancellation.ChangeLogStatusId = changeLog.ChangeLogStatusId;
                                cancellation.Comments = $"[{currentDate} - {user.Email}]" + System.Environment.NewLine + cancellation.Comments;
                                cancellation.RequestedByUserId = changeLog.RequestedByUserId;
                                cancellation.SubmittedByUserId = changeLog.SubmittedByUserId;
                                cancellation.Revision = cancellation.Revision + 1;

                                _context.ChangeLog.Update(cancellation);

                                log.ChangeLogId = cancellation.ChangeLogId;
                                log.TsaSubId = tsaSubId;
                                log.ChangeId = cancellation.ChangeId;
                                log.Comments = $"[{currentDate} - {user.Email}]" + System.Environment.NewLine + cancellation.Comments;
                                log.Revision = cancellation.Revision;
                            }
                            else
                            {
                                log.TsaSubId = tsaSubId;
                                log.ChangeId = changeType + (++currentSeed).ToString("0000");
                                log.Comments = $"[{currentDate} - {user.Email}]" + System.Environment.NewLine + changeLog.Comments;
                                log.Revision = 1;

                                await _context.ChangeLog.AddAsync(log);
                            }

                            _context.SaveChanges(user);

                            logs.Add(log);
                        }
                    }
                    else
                    {
                        var log = _context.ChangeLog.FirstOrDefault(c => c.ChangeLogId == changeLog.ChangeLogId);

                        log.ChangeLogStatusId = changeLog.ChangeLogStatusId;
                        log.Comments = log.Comments + System.Environment.NewLine + $"[{currentDate} - {user.Email}]" + System.Environment.NewLine + changeLog.Comments;

                        _context.SaveChanges(user);

                        logs.Add(log);
                    }

                    transaction.Commit();

                    var changeLogIdList = logs.Select(c => c.ChangeLogId).ToList();
                    return await GetChangeLogsAsync(changeLogIdList);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<List<ChangeLog>> GetChangeLogsAsync()
        {
            List<LogTypeId> logTypeIds = new List<LogTypeId>();
            logTypeIds.Add(LogTypeId.Cancellation);
            logTypeIds.Add(LogTypeId.Modification);

            return await GetChangeLogsAsync(logTypeIds);
        }

        public async Task<List<ChangeLog>> GetEscalationLogsAsync()
        {
            List<LogTypeId> logTypeIds = new List<LogTypeId>();
            logTypeIds.Add(LogTypeId.Escalation);

            return await GetChangeLogsAsync(logTypeIds);
        }

        public async Task<List<ChangeLog>> GetChangeLogsAsync(List<LogTypeId> logTypeIds)
        {
            var logTypeIdList = logTypeIds.Cast<int>().ToList();

            return await _context.ChangeLog.AsNoTracking()
                                           .OrderBy(o => o.ChangeLogId)
                                           .Include(i => i.ChangeLogType)
                                           .Include(i => i.ChangeLogStatus)
                                           .Include(i => i.SubmittedByUser)
                                           .Include(i => i.RequestedByUser)
                                           .Include(i => i.CreatedUser)
                                           .Include(i => i.LastModifiedUser)
                                           .Where(c => logTypeIdList.Contains(c.ChangeLogTypeId))
                                           .Select(s => new ChangeLog
                                           {
                                               ChangeLogId = s.ChangeLogId,
                                               TsaId = s.TsaId,
                                               TsaSubId = s.TsaSubId,
                                               ChangeId = s.ChangeId,
                                               ChangeDate = s.ChangeDate,
                                               ChangeLogTypeId = s.ChangeLogTypeId,
                                               ChangeLogTypeText = s.ChangeLogType.Name,
                                               ChangeLogStatusId = s.ChangeLogStatusId,
                                               ChangeLogStatusText = s.ChangeLogStatus.Name,
                                               SubmittedByUserId = s.SubmittedByUserId,
                                               SubmittedByUserText = s.SubmittedByUser.Email,
                                               RequestedByUserId = s.RequestedByUserId,
                                               RequestedByUserText = s.RequestedByUser.Email,
                                               Comments = s.Comments,
                                               CreatedDate = s.CreatedDate,
                                               CreatedUserId = s.CreatedUserId,
                                               CreatedUserText = s.CreatedUser.Email,
                                               LastModifiedDate = s.LastModifiedDate,
                                               LastModifiedUserId = s.LastModifiedUserId,
                                               LastModifiedUserText = s.LastModifiedUser.Email,
                                               Revision = s.Revision,
                                               AttachmentCount = _context.ChangeLogFile.AsNoTracking().Count(c => c.ChangeLogId == s.ChangeLogId)
                                           })
                                           .ToListAsync();
        }

        public async Task<List<ChangeLog>> GetChangeLogsAsync(List<int> changeLogIdList)
        {
            return await _context.ChangeLog.AsNoTracking()
                                           .OrderBy(o => o.ChangeLogId)
                                           .Include(i => i.ChangeLogType)
                                           .Include(i => i.ChangeLogStatus)
                                           .Include(i => i.SubmittedByUser)
                                           .Include(i => i.RequestedByUser)
                                           .Include(i => i.CreatedUser)
                                           .Include(i => i.LastModifiedUser)
                                           .Where(c => changeLogIdList.Contains(c.ChangeLogId))
                                           .Select(s => new ChangeLog
                                           {
                                               ChangeLogId = s.ChangeLogId,
                                               TsaId = s.TsaId,
                                               TsaSubId = s.TsaSubId,
                                               ChangeId = s.ChangeId,
                                               ChangeDate = s.ChangeDate,
                                               ChangeLogTypeId = s.ChangeLogTypeId,
                                               ChangeLogTypeText = s.ChangeLogType.Name,
                                               ChangeLogStatusId = s.ChangeLogStatusId,
                                               ChangeLogStatusText = s.ChangeLogStatus.Name,
                                               SubmittedByUserId = s.SubmittedByUserId,
                                               SubmittedByUserText = s.SubmittedByUser.Email,
                                               RequestedByUserId = s.RequestedByUserId,
                                               RequestedByUserText = s.RequestedByUser.Email,
                                               Comments = s.Comments,
                                               CreatedDate = s.CreatedDate,
                                               CreatedUserId = s.CreatedUserId,
                                               CreatedUserText = s.CreatedUser.Email,
                                               LastModifiedDate = s.LastModifiedDate,
                                               LastModifiedUserId = s.LastModifiedUserId,
                                               LastModifiedUserText = s.LastModifiedUser.Email,
                                               Revision = s.Revision
                                           })
                                           .ToListAsync();
        }

        public async Task<List<FileDto>> GetChangeLogAttachmentIdsAsync(int changeLogId)
        {
            return await _context.ChangeLogFile.AsNoTracking()
                                               .Include(i => i.FileRepository)
                                               .ThenInclude(i => i.CreatedUser)
                                               .Where(c => c.ChangeLogId == changeLogId)
                                               .Select(s => new FileDto
                                               {
                                                   ReferenceId = s.ChangeLogId,
                                                   Id = s.FileRepository.FileRepositoryId,
                                                   File = s.FileRepository.FileName,
                                                   Size = s.FileRepository.FileStream.Length,
                                                   UploadedBy = s.FileRepository.CreatedUser.Email,
                                                   UploadedDate = s.FileRepository.CreatedDate
                                               }).ToListAsync();
        }

        public async Task<List<AddLog>> UpdateAddLogAsync(AddLog addLog, AuthenticatedUser user)
        {
            var logs = new List<AddLog>();
            var currentDate = GetTimeStamp(); 

            if (addLog.AddLogId == 0) // Add new
            {
                var currentLogId = _context.AddLog.AsNoTracking().Where(c => c.ChangeLogTypeId == addLog.ChangeLogTypeId).Max(c => c.AddId);
                var currentSeed = currentLogId == null ? 0 : Int32.Parse(Regex.Replace(currentLogId, "[^0-9 _]", ""));
                var changeType = ((LogTypeId)addLog.ChangeLogTypeId).ToString().Substring(0, 1);

                addLog.AddId = changeType + (++currentSeed).ToString("0000");
                addLog.Comments = $"[{currentDate} - {user.Email}]" + System.Environment.NewLine + addLog.Comments;
                addLog.Revision = 1;

                await _context.AddLog.AddAsync(addLog);
                _context.SaveChanges(user);

                logs.Add(addLog);
            }
            else
            {
                var log = _context.AddLog.FirstOrDefault(c => c.AddLogId == addLog.AddLogId);

                log.ChangeLogStatusId = addLog.ChangeLogStatusId;
                log.Comments = log.Comments + System.Environment.NewLine + $"[{currentDate} - {user.Email}]" + System.Environment.NewLine + addLog.Comments;
                log.TsaId = addLog.TsaId;
                log.TsaSubId = addLog.TsaSubId;
                log.TsaSubIds = addLog.TsaSubIds;

                _context.SaveChanges(user);

                logs.Add(log);
            }

            var addLogIdList = logs.Select(c => c.AddLogId).ToList();
            return await GetAddLogsAsync(addLogIdList);
        }

        public async Task<List<AddLog>> GetAddLogsAsync()
        {
            return await _context.AddLog.AsNoTracking()
                                           .OrderBy(o => o.AddLogId)
                                           .Include(i => i.AddType)
                                           .Include(i => i.ChangeLogType)
                                           .Include(i => i.ChangeLogStatus)
                                           .Include(i => i.SubmittedByUser)
                                           .Include(i => i.RequestedByUser)
                                           .Include(i => i.CreatedUser)
                                           .Include(i => i.LastModifiedUser)
                                           .Select(s => new AddLog
                                           {
                                               AddLogId = s.AddLogId,
                                               TsaId = s.TsaId,
                                               TsaSubId = s.TsaSubId,
                                               AddId = s.AddId,
                                               ChangeDate = s.ChangeDate,
                                               ChangeLogTypeId = s.ChangeLogTypeId,
                                               ChangeLogTypeText = s.ChangeLogType.Name,
                                               ChangeLogStatusId = s.ChangeLogStatusId,
                                               ChangeLogStatusText = s.ChangeLogStatus.Name,
                                               SubmittedByUserId = s.SubmittedByUserId,
                                               SubmittedByUserText = s.SubmittedByUser.Email,
                                               RequestedByUserId = s.RequestedByUserId,
                                               RequestedByUserText = s.RequestedByUser.Email,
                                               Comments = s.Comments,
                                               CreatedDate = s.CreatedDate,
                                               CreatedUserId = s.CreatedUserId,
                                               CreatedUserText = s.CreatedUser.Email,
                                               LastModifiedDate = s.LastModifiedDate,
                                               LastModifiedUserId = s.LastModifiedUserId,
                                               LastModifiedUserText = s.LastModifiedUser.Email,
                                               Revision = s.Revision,
                                               AddTypeId = s.AddTypeId,
                                               AddTypeText = s.AddType.Name,
                                               TsaSubIds = s.TsaSubIds,
                                               AttachmentCount = _context.AddLogFile.AsNoTracking().Count(c => c.AddLogId == s.AddLogId)
                                           })
                                           .ToListAsync();
        }

        public async Task<List<AddLog>> GetAddLogsAsync(List<int> addLogIdList)
        {
            return await _context.AddLog.AsNoTracking()
                                           .OrderBy(o => o.AddLogId)
                                           .Include(i => i.AddType)
                                           .Include(i => i.ChangeLogType)
                                           .Include(i => i.ChangeLogStatus)
                                           .Include(i => i.SubmittedByUser)
                                           .Include(i => i.RequestedByUser)
                                           .Include(i => i.CreatedUser)
                                           .Include(i => i.LastModifiedUser)
                                           .Where(c => addLogIdList.Contains(c.AddLogId))
                                           .Select(s => new AddLog
                                           {
                                               AddLogId = s.AddLogId,
                                               TsaId = s.TsaId,
                                               TsaSubId = s.TsaSubId,
                                               AddId = s.AddId,
                                               ChangeDate = s.ChangeDate,
                                               ChangeLogTypeId = s.ChangeLogTypeId,
                                               ChangeLogTypeText = s.ChangeLogType.Name,
                                               ChangeLogStatusId = s.ChangeLogStatusId,
                                               ChangeLogStatusText = s.ChangeLogStatus.Name,
                                               SubmittedByUserId = s.SubmittedByUserId,
                                               SubmittedByUserText = s.SubmittedByUser.Email,
                                               RequestedByUserId = s.RequestedByUserId,
                                               RequestedByUserText = s.RequestedByUser.Email,
                                               Comments = s.Comments,
                                               CreatedDate = s.CreatedDate,
                                               CreatedUserId = s.CreatedUserId,
                                               CreatedUserText = s.CreatedUser.Email,
                                               LastModifiedDate = s.LastModifiedDate,
                                               LastModifiedUserId = s.LastModifiedUserId,
                                               LastModifiedUserText = s.LastModifiedUser.Email,
                                               Revision = s.Revision,
                                               AddTypeId = s.AddTypeId,
                                               AddTypeText = s.AddType.Name,
                                               TsaSubIds = s.TsaSubIds
                                           })
                                           .ToListAsync();
        }

        public async Task<List<FileDto>> GetAddLogAttachmentIdsAsync(int addLogId)
        {
            return await _context.AddLogFile.AsNoTracking()
                                               .Include(i => i.FileRepository)
                                               .ThenInclude(i => i.CreatedUser)
                                               .Where(c => c.AddLogId == addLogId)
                                               .Select(s => new FileDto
                                               {
                                                   ReferenceId = s.AddLogId,
                                                   Id = s.FileRepository.FileRepositoryId,
                                                   File = s.FileRepository.FileName,
                                                   Size = s.FileRepository.FileStream.Length,
                                                   UploadedBy = s.FileRepository.CreatedUser.Email,
                                                   UploadedDate = s.FileRepository.CreatedDate
                                               }).ToListAsync();
        }

        public async Task<List<DisputeLog>> UpdateDisputeLogAsync(DisputeLog disputeLog, AuthenticatedUser user)
        {
            var logs = new List<DisputeLog>();
            var currentDate = GetTimeStamp();

            if (disputeLog.DisputeLogId == 0) // Add new
            {
                var currentLogId = _context.DisputeLog.AsNoTracking().Max(c => c.DisputeId);
                var currentSeed = currentLogId == null ? 0 : Int32.Parse(Regex.Replace(currentLogId, "[^0-9 _]", ""));
                var disputeType = ((DisputeTypeId)disputeLog.DisputeTypeId).ToString().Substring(0, 1);

                var tsaSubIds = disputeLog.TsaSubId.Split(",").ToList();
                foreach (var tsaSubId in tsaSubIds)
                {
                    var log = disputeLog.DeepClone<DisputeLog>();

                    log.TsaSubId = tsaSubId;
                    log.DisputeId = disputeType + (++currentSeed).ToString("0000");
                    log.Comments = $"[{currentDate} - {user.Email}]" + System.Environment.NewLine + disputeLog.Comments;
                    log.ServicePeriod = disputeLog.ServicePeriod;
                    log.Revision = 1;

                    await _context.DisputeLog.AddAsync(log);

                    logs.Add(log);
                }
            }
            else
            {
                var log = await _context.DisputeLog.FirstOrDefaultAsync(c => c.DisputeLogId == disputeLog.DisputeLogId);

                log.DisputeTypeId = disputeLog.DisputeTypeId;
                log.Comments = log.Comments + System.Environment.NewLine + $"[{currentDate} - {user.Email}]" + System.Environment.NewLine + disputeLog.Comments;
                log.Currency = disputeLog.Currency;
                log.AgreedCost = disputeLog.AgreedCost.HasValue ? disputeLog.AgreedCost.Value : 0;

                logs.Add(log);
            }

            _context.SaveChanges(user);

            var disputeLogIdList = logs.Select(c => c.DisputeLogId).ToList();
            return await GetDisputeLogsAsync(disputeLogIdList);
        }

        public async Task<List<DisputeLog>> GetDisputeLogsAsync()
        {
            return await _context.DisputeLog.AsNoTracking()
                                           .OrderBy(o => o.DisputeLogId)
                                           .Include(i => i.DisputeType)
                                           .Include(i => i.DisputeDiscrepancy)
                                           .Include(i => i.SubmittedByUser)
                                           .Include(i => i.RequestedByUser)
                                           .Include(i => i.CreatedUser)
                                           .Include(i => i.LastModifiedUser)
                                           .Select(s => new DisputeLog
                                           {
                                               DisputeLogId = s.DisputeLogId,
                                               TsaId = s.TsaId,
                                               TsaSubId = s.TsaSubId,
                                               DisputeId = s.DisputeId,
                                               SubmissionDate = s.SubmissionDate,
                                               DisputeTypeId = s.DisputeTypeId,
                                               DisputeTypeText = s.DisputeType.Name,
                                               InvoiceNumber = s.InvoiceNumber,
                                               DisputeDiscrepancyId = s.DisputeDiscrepancyId,
                                               DisputeDiscrepancyText = s.DisputeDiscrepancy.Name,
                                               SubmittedByUserId = s.SubmittedByUserId,
                                               SubmittedByUserText = s.SubmittedByUser.Email,
                                               RequestedByUserId = s.RequestedByUserId,
                                               RequestedByUserText = s.RequestedByUser.Email,
                                               Comments = s.Comments,
                                               Currency = s.Currency,
                                               AgreedCost = s.AgreedCost,
                                               CreatedDate = s.CreatedDate,
                                               CreatedUserId = s.CreatedUserId,
                                               CreatedUserText = s.CreatedUser.Email,
                                               LastModifiedDate = s.LastModifiedDate,
                                               LastModifiedUserId = s.LastModifiedUserId,
                                               LastModifiedUserText = s.LastModifiedUser.Email,
                                               Revision = s.Revision,
                                               AttachmentCount = _context.DisputeLogFile.AsNoTracking().Count(c => c.DisputeLogId == s.DisputeLogId),
                                               ServicePeriod = s.ServicePeriod
                                           })
                                           .ToListAsync();
        }

        public async Task<List<DisputeLog>> GetDisputeLogsAsync(List<int> disputeLogIdList)
        {
            return await _context.DisputeLog.AsNoTracking()
                                           .OrderBy(o => o.DisputeLogId)
                                           .Include(i => i.DisputeType)
                                           .Include(i => i.DisputeDiscrepancy)
                                           .Include(i => i.SubmittedByUser)
                                           .Include(i => i.RequestedByUser)
                                           .Include(i => i.CreatedUser)
                                           .Include(i => i.LastModifiedUser)
                                           .Where(c => disputeLogIdList.Contains(c.DisputeLogId))
                                           .Select(s => new DisputeLog
                                           {
                                               DisputeLogId = s.DisputeLogId,
                                               TsaId = s.TsaId,
                                               TsaSubId = s.TsaSubId,
                                               DisputeId = s.DisputeId,
                                               SubmissionDate = s.SubmissionDate,
                                               DisputeTypeId = s.DisputeTypeId,
                                               DisputeTypeText = s.DisputeType.Name,
                                               InvoiceNumber = s.InvoiceNumber,
                                               DisputeDiscrepancyId = s.DisputeDiscrepancyId,
                                               DisputeDiscrepancyText = s.DisputeDiscrepancy.Name,
                                               SubmittedByUserId = s.SubmittedByUserId,
                                               SubmittedByUserText = s.SubmittedByUser.Email,
                                               RequestedByUserId = s.RequestedByUserId,
                                               RequestedByUserText = s.RequestedByUser.Email,
                                               Comments = s.Comments,
                                               Currency = s.Currency,
                                               AgreedCost = s.AgreedCost,
                                               CreatedDate = s.CreatedDate,
                                               CreatedUserId = s.CreatedUserId,
                                               CreatedUserText = s.CreatedUser.Email,
                                               LastModifiedDate = s.LastModifiedDate,
                                               LastModifiedUserId = s.LastModifiedUserId,
                                               LastModifiedUserText = s.LastModifiedUser.Email,
                                               Revision = s.Revision,
                                               ServicePeriod = s.ServicePeriod
                                           })
                                           .ToListAsync();
        }

        public async Task<List<FileDto>> GetDisputeLogAttachmentIdsAsync(int disputeLogId)
        {
            return await _context.DisputeLogFile.AsNoTracking()
                                               .Include(i => i.FileRepository)
                                               .ThenInclude(i => i.CreatedUser)
                                               .Where(c => c.DisputeLogId == disputeLogId)
                                               .Select(s => new FileDto
                                               {
                                                   ReferenceId = s.DisputeLogId,
                                                   Id = s.FileRepository.FileRepositoryId,
                                                   File = s.FileRepository.FileName,
                                                   Size = s.FileRepository.FileStream.Length,
                                                   UploadedBy = s.FileRepository.CreatedUser.Email,
                                                   UploadedDate = s.FileRepository.CreatedDate
                                               }).ToListAsync();
        }

        public async Task<List<VInputStreamSummary>> GetInputStreamSummaryAsync()
        {
            return await _context.VInputStreamSummary.ToListAsync();
        }

        #endregion Public methods

        #region Private methods

        private string GetTimeStamp()
        {
            DateTime dateTime_Eastern = DateTime.UtcNow.ToEasternTimeZone();

            return dateTime_Eastern.ToString() + " ET";
        }

        #endregion Private methods
    }
}
