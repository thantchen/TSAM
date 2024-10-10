using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using NPOI.XSSF.UserModel;
using TamsApi.Core.Excel;
using TamsApi.Core.Identity;
using TamsApi.Core.Tsas;
using TamsApi.Data;
using TamsApi.Models;
using TamsApi.Models.Lookups;

namespace TamsApi.Controllers
{
    [ApiController]
    [Route("api")]
    [Consumes("multipart/form-data")]
    public class FileController : AuthenticatedControllerBase
    {
        #region Member variables

        private readonly IDataFileRepository _repository;
        private readonly IAuthenticatedUserBuilder builder;
        private readonly IExcelImporter<TsaSchedule> _importer;
        private readonly IMapper mapper;

        #endregion

        #region Constructor

        public FileController(IDataFileRepository repository,
            IAuthenticatedUserBuilder builder,
            IExcelImporter<TsaSchedule> importer,
            IMapper mapper) : base(builder)
        {
            _repository = repository;
            this.builder = builder;
            _importer = importer;
            this.mapper = mapper;
        }

        #endregion

        #region Public methods

        [HttpPost("upload/tsa-schedules")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<UploadDto>>> UploadTsaSchedules(IFormFile file, CancellationToken cancellationToken)
        {
            var parseResult = await _importer.ImportAsync(file, CurrentUser);

            if (parseResult.Errors.Count > 0)
            {
                return BadRequest(parseResult);
            }

            return parseResult;
        }

        [HttpPost("upload/change-log-attachments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UploadChangeLogAttachments(List<IFormFile> files)
        {
            try
            {
                var uploadedFiles = new List<dynamic>();

                foreach (var file in files)
                {
                    var result = await _repository.AddFileRepositoryAsync(file, (int)FileTypeId.ChangeLogAttachment, CurrentUser);

                    uploadedFiles.Add(new { id = result.FileRepositoryId, file = result.FileName, size = file.Length, uploadedBy = CurrentUser.Email, uploadedDate = result.CreatedDate });

                    var logIds = Request.Form["logIds"];
                    if (logIds.Count > 0)
                    {
                        await _repository.AddChangeLogFileAsync(result.FileRepositoryId, logIds[0].Split(",").Select(Int32.Parse).ToList());
                    }
                }

                return Ok(uploadedFiles);
            }
            catch { return BadRequest(); }
        }

        [HttpPost("upload/add-log-attachments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UploadAddLogAttachments(List<IFormFile> files, CancellationToken cancellationToken)
        {
            try
            {
                var uploadedFiles = new List<dynamic>();

                foreach (var file in files)
                {
                    var result = await _repository.AddFileRepositoryAsync(file, (int)FileTypeId.AddLogAttachment, CurrentUser);

                    uploadedFiles.Add(new { id = result.FileRepositoryId, file = result.FileName, size = file.Length, uploadedBy = CurrentUser.Email, uploadedDate = result.CreatedDate });

                    var logIds = Request.Form["logIds"];
                    if (logIds.Count > 0)
                    {
                        await _repository.AddAddLogFileAsync(result.FileRepositoryId, logIds[0].Split(",").Select(Int32.Parse).ToList());
                    }
                }

                return Ok(uploadedFiles);
            }
            catch { return BadRequest(); }
        }

        [HttpPost("upload/dispute-log-attachments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UploadDisputeAttachments(List<IFormFile> files, CancellationToken cancellationToken)
        {
            try
            {
                var uploadedFiles = new List<dynamic>();

                foreach (var file in files)
                {
                    var result = await _repository.AddFileRepositoryAsync(file, (int)FileTypeId.DisputeLogAttachment, CurrentUser);

                    uploadedFiles.Add(new { id = result.FileRepositoryId, file = result.FileName, size = file.Length, uploadedBy = CurrentUser.Email, uploadedDate = result.CreatedDate });

                    var logIds = Request.Form["logIds"];
                    if (logIds.Count > 0)
                    {
                        await _repository.AddDisputeLogFileAsync(result.FileRepositoryId, logIds[0].Split(",").Select(Int32.Parse).ToList());
                    }
                }

                return Ok(uploadedFiles);
            }
            catch { return BadRequest(); }
        }

        [HttpGet("download/file/{id}", Name = "DownloadAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DownloadAsync(int id)
        {
            try
            {
                var mimeType = "application/octet-stream";
                var file = await _repository.GetFileAsync(id);

                if (file == null)
                {
                    return NotFound();
                }
                else
                {
                    var contentDispositionHeader = new System.Net.Mime.ContentDisposition
                    {
                        FileName = file.FileName,
                        Inline = true,
                    };
                    Response.Headers.Add("Content-Disposition", contentDispositionHeader.ToString());

                    return new FileContentResult(file.FileStream, mimeType);

                }
            }
            catch { return BadRequest(); }
        }

        [HttpGet("file-repository/file-type/{fileTypeId}")]
        public async Task<ApiResponse<dynamic>> GetFileRepositoryAsync(int fileTypeId)
        {
            try
            {
                var files = await _repository.GetFileRepositoryAsync(fileTypeId);

                var results = files.Select(s => new
                {
                    id = s.FileRepositoryId,
                    fileName = s.FileName,
                    uploadedDateTime = s.CreatedDate,
                    uploadedBy = s.CreatedUser.Email
                }).OrderByDescending(o => o.uploadedDateTime)
                .ToList();

                return new ApiResponse<dynamic>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<dynamic>(exception.Message);
            }
        }


        #endregion

    }
}
