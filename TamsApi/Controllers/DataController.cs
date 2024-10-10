using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NPOI.XSSF.UserModel;
using TamsApi.Core.Configuration;
using TamsApi.Core.Excel;
using TamsApi.Core.Identity;
using TamsApi.Core.Tsas;
using TamsApi.Data;
using TamsApi.Models;

namespace TamsApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class DataController : AuthenticatedControllerBase
    {
        #region Member variables

        private readonly IDataRepository _repository;
        private readonly ReportSettings _reportSettings;
        private readonly IAuthenticatedUserBuilder builder;

        #endregion

        #region Constructor

        public DataController(IDataRepository repository,
            IOptions<ReportSettings> reportSettings,
            IAuthenticatedUserBuilder builder) : base(builder)
        {
            _repository = repository;
            _reportSettings = reportSettings.Value;
            this.builder = builder;
        }

        #endregion

        #region Public methods

        [HttpGet("data/trusted-ticket")]
        public ApiResponse<string> GetTrustedTicket()
        {
            try
            {
                var hostname = _reportSettings.Hostname;
                var username = _reportSettings.Username;
                var site = _reportSettings.TargetSite;

                var request = (HttpWebRequest)WebRequest.Create(hostname + "/trusted");
                var encoding = new UTF8Encoding();
                var postData = $"username={username}&target_site={site}";

                byte[] data = encoding.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                return new ApiResponse<string>(new StreamReader(response.GetResponseStream()).ReadToEnd());
            }
            catch (Exception exception)
            {
                return new ApiResponse<string>(exception.Message);
            }
        }

        [HttpGet("data/tsa-schedules")]
        public async Task<ApiResponse<List<TsaSchedule>>> GetLogTypes()
        {
            try
            {
                var results = await _repository.GetTsaSchedulesAsync();

                return new ApiResponse<List<TsaSchedule>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<TsaSchedule>>(exception.Message);
            }
        }

        [HttpGet("data/activity-report")]
        public async Task<ApiResponse<List<ActivityReport>>> GetActivityReport([FromQuery] string logStatuses, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            try
            {
                var results = await _repository.GetActivityReportAsync(logStatuses, fromDate, toDate);

                return new ApiResponse<List<ActivityReport>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<ActivityReport>>(exception.Message);
            }
        }

        [HttpGet("data/payment-approval")]
        public async Task<ApiResponse<List<VPaymentApprovalReport>>> GetPaymentApproval()
        {
            try
            {
                var results = await _repository.GetPaymentApprovalAsync();

                return new ApiResponse<List<VPaymentApprovalReport>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<VPaymentApprovalReport>>(exception.Message);
            }
        }

        #endregion

    }
}
