using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TamsApi.Core.Excel;
using TamsApi.Core.Identity;
using TamsApi.Models;

namespace TamsApi.Data
{
    public interface IDataFileRepository
    {
        Task<FileRepository> AddFileRepositoryAsync(IFormFile file, int fileTypeId, AuthenticatedUser user);
        Task<FileRepository> GetFileAsync(int fileId);
        Task<List<FileRepository>> GetFileRepositoryAsync(int fileTypeId);
        Task AddChangeLogFileAsync(int fileRepositoryId, List<int> changeLogIds);
        Task AddAddLogFileAsync(int fileRepositoryId, List<int> addLogIds);
        Task AddDisputeLogFileAsync(int fileRepositoryId, List<int> disputeLogIds);
    }

    public interface IDataFileRepository<TModel> where TModel : class, new()
    {
        TsaSchedule FindTsa(string subId);
        Task<FileRepository> AddRecordsAsync(FileRepository upload, List<TModel> parsedRows, AuthenticatedUser user);
        Task<int> LogExcelFailuresAsync(string fileName, List<FailedRow> failures, AuthenticatedUser user);
        Task<int> LogExcelFailuresAsync(string fileName, Exception exception, AuthenticatedUser user);
    }
}
