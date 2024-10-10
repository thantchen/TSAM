using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TamsApi.Core.Identity;
using TamsApi.Core.Tsas;
using TamsApi.Models;

namespace TamsApi.Controllers.Uploads
{
    [ApiController]
    [Route("api")]
    [Consumes("multipart/form-data")]
    public class SellSideNotificationChangesController : AuthenticatedControllerBase
    {
        private readonly IExcelImporter<SellSideNotificationChange> _importer;

        public SellSideNotificationChangesController(IExcelImporter<SellSideNotificationChange> importer, IAuthenticatedUserBuilder builder) : base(builder)
        {
            _importer = importer;
        }

        [HttpPost("upload/sell-side-notification-changes")]
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
