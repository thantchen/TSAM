using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TamsApi.Core.Identity;
using TamsApi.Core.Tsas;
using TamsApi.Models;

namespace TamsApi.Controllers.Uploads
{
    [ApiController]
    [Route("api")]
    [Consumes("multipart/form-data")]
    public class InvoicesController : AuthenticatedControllerBase
    {
        private readonly IExcelImporter<Invoice> _importer;
        private readonly IAuthenticatedUserBuilder _builder;

        public InvoicesController(IExcelImporter<Invoice> importer, IAuthenticatedUserBuilder builder) : base(builder)
        {
            this._importer = importer;
            this._builder = builder;
        }

        [HttpPost("upload/invoices")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<UploadDto>>> CreateAsync(IFormFile file)
        {
            var importResult = await _importer.ImportAsync(file, CurrentUser);
            if (importResult.Failed)
            {
                return BadRequest(importResult);
            }

            return CreatedAtRoute(nameof(FileController.DownloadAsync), 
                new { controller = "file", id = importResult.Data.FileRepositoryId },
                importResult);
        }
    }
}
