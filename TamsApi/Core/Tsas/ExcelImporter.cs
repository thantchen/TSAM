using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    public interface IExcelImporter<TModel> where TModel : class, new()
    {
        Task<ApiResponse<UploadDto>> ImportAsync(IFormFile formFile, AuthenticatedUser user);
    }

    public class ExcelImporter<TModel> : IExcelImporter<TModel> where TModel : class, new()
    {
        private readonly IDataFileRepository<TModel> _repository;
        private readonly ISheetParser<TModel> _parser;
        private readonly IMapper _mapper;

        public ExcelImporter(IDataFileRepository<TModel> repository, ISheetParser<TModel> parser, IMapper mapper)
        {
            _repository = repository;
            _parser = parser;
            _mapper = mapper;
        }

        public async Task<ApiResponse<UploadDto>> ImportAsync(IFormFile formFile, AuthenticatedUser user)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            ParseResult<TModel> result;
            try
            {
                result = _parser.Parse(memoryStream);
            }
            catch(ExcelParseException e)
            {
                await _repository.LogExcelFailuresAsync(formFile.FileName, e, user);
                return new ApiResponse<UploadDto>($"Unable to parse spreadsheet {formFile.FileName}", e.Message);
            }

            if (result.Failed)
            {
                await _repository.LogExcelFailuresAsync(formFile.FileName, result.FailedRows, user);
                return new ApiResponse<UploadDto>(result.FailedRows.Flatten());
            }

            var originalUploadBytes = memoryStream.ToArray(); // Rewind the buffer to capture the file.
            var upload = _mapper.Map<IFormFile, FileRepository>(formFile);
            upload.FileStream = originalUploadBytes;
            upload = await _repository.AddRecordsAsync(upload, result.ParsedRows, user);

            var dto = _mapper.Map<UploadDto>(upload);
            dto.UploadedBy = user.Email;
            dto.Size = formFile.Length;
            dto.ParsedCount = result.ParsedRows.Count;
            return new ApiResponse<UploadDto>(dto);
        }
    }
}
