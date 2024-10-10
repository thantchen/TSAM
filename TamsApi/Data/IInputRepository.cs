using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TamsApi.Core.Identity;
using TamsApi.Models;
using TamsApi.Models.Lookups;

namespace TamsApi.Data
{
    public interface IInputRepository
    {
        Task<List<ChangeLog>> UpdateChangeLogAsync(ChangeLog changeLog, AuthenticatedUser user);
        Task<List<ChangeLog>> GetChangeLogsAsync(List<LogTypeId> logTypeIds);
        Task<List<ChangeLog>> GetChangeLogsAsync(List<int> changeLogIdList);
        Task<List<ChangeLog>> GetChangeLogsAsync();
        Task<List<ChangeLog>> GetEscalationLogsAsync();

        Task<List<FileDto>> GetChangeLogAttachmentIdsAsync(int changeLogId);

        Task<List<AddLog>> UpdateAddLogAsync(AddLog addLog, AuthenticatedUser user);
        Task<List<AddLog>> GetAddLogsAsync();
        Task<List<AddLog>> GetAddLogsAsync(List<int> addLogIdList);
        Task<List<FileDto>> GetAddLogAttachmentIdsAsync(int addLogId);

        Task<List<DisputeLog>> UpdateDisputeLogAsync(DisputeLog disputeLog, AuthenticatedUser user);
        Task<List<DisputeLog>> GetDisputeLogsAsync();
        Task<List<DisputeLog>> GetDisputeLogsAsync(List<int> disputeLogIdList);
        Task<List<FileDto>> GetDisputeLogAttachmentIdsAsync(int disputeLogId);

        Task<List<VInputStreamSummary>> GetInputStreamSummaryAsync();
    }

}
