using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using TamsApi.Core.Identity;
using TamsApi.Models;

namespace TamsApi.Data
{
    public class DataFileRepository : IDataFileRepository
    {
        #region Member variables

        private ApplicationContext _context;

        #endregion Member variables

        #region Constructor

        public DataFileRepository(ApplicationContext context)
        {
            _context = context;
        }

        #endregion Constructor

        #region Public methods

        public async Task<FileRepository> AddFileRepositoryAsync(IFormFile file, int fileTypeId, AuthenticatedUser user)
        {
            try
            {
                var stream = await ReadAllBytes(file);

                var fileRepo = new FileRepository
                {
                    FileName = file.FileName,
                    FileStream = stream,
                    FileTypeId = fileTypeId
                };

                await _context.FileRepository.AddAsync(fileRepo);
                await _context.SaveChangesAsync(user);

                return fileRepo;
            }
            catch (Exception exeption)
            {
                // TODO: Add logging here
                throw;
            };
        }

        public async Task<FileRepository> GetFileAsync(int fileId)
        {
            return await _context.FileRepository.FindAsync(fileId);
        }

        public async Task<List<FileRepository>> GetFileRepositoryAsync(int fileTypeId)
        {
            return await _context.FileRepository.Include(i => i.CreatedUser)
                                                .Where(c => c.FileTypeId == fileTypeId)
                                                .ToListAsync();
        }

        public async Task AddChangeLogFileAsync(int fileRepositoryId, List<int> changeLogIds)
        {
            try
            {
                foreach (var changeLogId in changeLogIds)
                {
                    await _context.ChangeLogFile.AddAsync(new ChangeLogFile { ChangeLogId = changeLogId, FileRepositoryId = fileRepositoryId });
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddAddLogFileAsync(int fileRepositoryId, List<int> addLogIds)
        {
            try
            {
                foreach (var addLogId in addLogIds)
                {
                    await _context.AddLogFile.AddAsync(new AddLogFile { AddLogId = addLogId, FileRepositoryId = fileRepositoryId });
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddDisputeLogFileAsync(int fileRepositoryId, List<int> disputeLogIds)
        {
            try
            {
                foreach (var disputeLogId in disputeLogIds)
                {
                    await _context.DisputeLogFile.AddAsync(new DisputeLogFile { DisputeLogId = disputeLogId, FileRepositoryId = fileRepositoryId });
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Public mehtods

        #region Private methods

        /// <summary>
        /// Read all bytes in file.
        /// </summary>
        /// <param name="file">File data</param>
        /// <returns>A byte array</returns>
        private async Task<byte[]> ReadAllBytes(IFormFile file)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    return stream.ToArray();
                }
            }
            catch { throw; }
        }

        #endregion Private methods
    }
}
