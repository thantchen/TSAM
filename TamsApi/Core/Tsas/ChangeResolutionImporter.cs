using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TamsApi.Core.Excel;
using TamsApi.Core.Identity;
using TamsApi.Data;
using TamsApi.Models;

namespace TamsApi.Core.Tsas
{
    public class ChangeResolutionImporter : IExcelImporter<ChangeResolutionLog>
    {
        private readonly IDataFileRepository<ChangeResolutionLog> _repository;
        private readonly ISheetParser<ChangeResolutionLog> _parser;
        private readonly IMapper _mapper;
        private readonly ISystemClock _clock;

        public ChangeResolutionImporter(IDataFileRepository<ChangeResolutionLog> repository,
            ISheetParser<ChangeResolutionLog> parser,
            IMapper mapper,
            ISystemClock clock)
        {
            _repository = repository;
            _parser = parser;
            _mapper = mapper;
            _clock = clock;
        }

        public async Task<ApiResponse<UploadDto>> ImportAsync(IFormFile formFile, AuthenticatedUser user)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            ParseResult<ChangeResolutionLog> result;
            try
            {
                result = _parser.Parse(memoryStream);
            }
            catch (ExcelParseException e)
            {
                await _repository.LogExcelFailuresAsync(formFile.FileName, e, user);
                return new ApiResponse<UploadDto>($"Unable to parse spreadsheet {formFile.FileName}", e.Message);
            }

            if (result.Failed)
            {
                await _repository.LogExcelFailuresAsync(formFile.FileName, result.FailedRows, user);
                return new ApiResponse<UploadDto>(result.FailedRows.Flatten());
            }

            var originalUploadBytes = memoryStream.GetBuffer(); // Rewind the buffer to capture the file.
            var now = _clock.UtcNow.Date;
            //var effectiveRows = result.ParsedRows.Where(l => l.EffectiveDate == null || l.EffectiveDate.Value.Date <= now).ToList();
            var effectiveRows = result.ParsedRows.ToList();
            foreach (var row in effectiveRows)
            {
                var tsa = _repository.FindTsa(row.TsaSubId);
                _mapper.Map(row, tsa);
            }
            var upload = _mapper.Map<IFormFile, FileRepository>(formFile);
            upload.FileStream = originalUploadBytes;
            upload = await _repository.AddRecordsAsync(upload, effectiveRows, user);

            var dto = _mapper.Map<UploadDto>(upload);
            dto.UploadedBy = user.Email;
            dto.Size = formFile.Length;
            dto.ParsedCount = result.ParsedRows.Count;
            return new ApiResponse<UploadDto>(dto);
        }
    }
}
