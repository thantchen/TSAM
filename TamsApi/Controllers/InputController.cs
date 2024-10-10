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
using TamsApi.Core.Email;
using TamsApi.Core.Excel;
using TamsApi.Core.Identity;
using TamsApi.Core.Tsas;
using TamsApi.Data;
using TamsApi.InputModels;
using TamsApi.Models;
using TamsEmail.Views.Templates;

namespace TamsApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class InputController : AuthenticatedControllerBase
    {
        #region Member variables

        private readonly IInputRepository _repository;
        private readonly IAuthenticatedUserBuilder builder;
        private readonly ISmtpService _smtpService;

        #endregion

        #region Constructor

        public InputController(IInputRepository repository,
            ISmtpService smtpService,
            IAuthenticatedUserBuilder builder) : base(builder)
        {
            _repository = repository;
            _smtpService = smtpService;
            this.builder = builder;
        }

        #endregion

        #region Public methods

        [HttpPut("input/change-log")]
        public async Task<ApiResponse<List<ChangeLog>>> UpdateChangeLog(ChangeLog changeLog)
        {
            try
            {
                var results = await _repository.UpdateChangeLogAsync(changeLog, CurrentUser);

                return new ApiResponse<List<ChangeLog>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<ChangeLog>>(exception.Message);
            }
        }

        [HttpGet("input/change-log")]
        public async Task<ApiResponse<List<ChangeLog>>> GetChangeLogs()
        {
            try
            {
                var results = await _repository.GetChangeLogsAsync();

                return new ApiResponse<List<ChangeLog>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<ChangeLog>>(exception.Message);
            }
        }

        [HttpGet("input/escalation-log")]
        public async Task<ApiResponse<List<ChangeLog>>> GetEscalationLogs()
        {
            try
            {
                var results = await _repository.GetEscalationLogsAsync();

                return new ApiResponse<List<ChangeLog>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<ChangeLog>>(exception.Message);
            }
        }

        [HttpGet("input/change-log/{id}/attachments")]
        public async Task<ApiResponse<List<FileDto>>> GetChangeLogAttachments(int id)
        {
            try
            {
                var results = await _repository.GetChangeLogAttachmentIdsAsync(id);

                return new ApiResponse<List<FileDto>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<FileDto>>(exception.Message);
            }
        }

        [HttpPut("input/add-log")]
        public async Task<ApiResponse<List<AddLog>>> UpdateAddLog(AddLog addLog)
        {
            try
            {
                var results = await _repository.UpdateAddLogAsync(addLog, CurrentUser);

                return new ApiResponse<List<AddLog>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<AddLog>>(exception.Message);
            }
        }

        [HttpGet("input/add-log")]
        public async Task<ApiResponse<List<AddLog>>> GetAddLogs()
        {
            try
            {
                var results = await _repository.GetAddLogsAsync();

                return new ApiResponse<List<AddLog>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<AddLog>>(exception.Message);
            }
        }

        [HttpGet("input/add-log/{id}/attachments")]
        public async Task<ApiResponse<List<FileDto>>> GetAddLogAttachments(int id)
        {
            try
            {
                var results = await _repository.GetAddLogAttachmentIdsAsync(id);

                return new ApiResponse<List<FileDto>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<FileDto>>(exception.Message);
            }
        }

        [HttpPut("input/dispute-log")]
        public async Task<ApiResponse<List<DisputeLog>>> UpdateDisputeLog(DisputeLog disputeLog)
        {
            try
            {
                var results = await _repository.UpdateDisputeLogAsync(disputeLog, CurrentUser);

                return new ApiResponse<List<DisputeLog>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<DisputeLog>>(exception.Message);
            }
        }

        [HttpGet("input/dispute-log")]
        public async Task<ApiResponse<List<DisputeLog>>> GetDisputeLogs()
        {
            try
            {
                var results = await _repository.GetDisputeLogsAsync();

                return new ApiResponse<List<DisputeLog>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<DisputeLog>>(exception.Message);
            }
        }

        [HttpGet("input/dispute-log/{id}/attachments")]
        public async Task<ApiResponse<List<FileDto>>> GetDisputeLogAttachments(int id)
        {
            try
            {
                var results = await _repository.GetDisputeLogAttachmentIdsAsync(id);

                return new ApiResponse<List<FileDto>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<FileDto>>(exception.Message);
            }
        }

        [HttpGet("input/input-stream-summary")]
        public async Task<ApiResponse<List<VInputStreamSummary>>> GetInputStreamSummary(int id)
        {
            try
            {
                var results = await _repository.GetInputStreamSummaryAsync();

                return new ApiResponse<List<VInputStreamSummary>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<VInputStreamSummary>>(exception.Message);
            }
        }

        #endregion

    }
}
