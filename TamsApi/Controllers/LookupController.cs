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
using NPOI.XSSF.UserModel;
using TamsApi.Core.Excel;
using TamsApi.Core.Identity;
using TamsApi.Core.Tsas;
using TamsApi.Data;
using TamsApi.Models;

namespace TamsApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class LookupController : AuthenticatedControllerBase
    {
        #region Member variables

        private readonly ILookupRepository _repository;
        private readonly IAuthenticatedUserBuilder builder;

        #endregion

        #region Constructor

        public LookupController(ILookupRepository repository,
            IAuthenticatedUserBuilder builder) : base(builder)
        {
            _repository = repository;
            this.builder = builder;
        }

        #endregion

        #region Public methods

        [HttpGet("lookup/log-types")]
        public async Task<ApiResponse<List<LookupItemDto>>> GetLogTypes()
        {
            try
            {
                var results = await _repository.GetLookupDataAsync<LogType>(c => c.IsActive == true);

                return new ApiResponse<List<LookupItemDto>>(results.Select(c => new LookupItemDto
                {
                    Id = c.LogTypeId,
                    Name = c.Name
                }).ToList());
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<LookupItemDto>>(exception.Message);
            }
        }

        [HttpGet("lookup/log-statuses")]
        public async Task<ApiResponse<List<LookupItemDto>>> GetLogStatuses()
        {
            try
            {
                var results = await _repository.GetLookupDataAsync<LogStatus>(c => c.IsActive == true);

                return new ApiResponse<List<LookupItemDto>>(results.Select(c => new LookupItemDto
                {
                    Id = c.LogStatusId,
                    Name = c.Name,
                    ReferenceId = c.LogTypeId
                }).ToList());
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<LookupItemDto>>(exception.Message);
            }
        }

        [HttpGet("lookup/tsa")]
        public async Task<ApiResponse<List<LookupTsaDto>>> GetTSAs()
        {
            try
            {
                var results = await _repository.GetTsaLookupDataAsync();

                return new ApiResponse<List<LookupTsaDto>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<LookupTsaDto>>(exception.Message);
            }
        }

        [HttpGet("lookup/users")]
        public async Task<ApiResponse<List<UserDto>>> GetUsers()
        {
            try
            {
                var results = await _repository.GetUserLookupDataAsync();

                return new ApiResponse<List<UserDto>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<UserDto>>(exception.Message);
            }
        }

        [HttpGet("lookup/dispute-types")]
        public async Task<ApiResponse<List<LookupItemDto>>> GetDisputeTypes()
        {
            try
            {
                var results = await _repository.GetLookupDataAsync<DisputeType>(c => c.IsActive == true);

                return new ApiResponse<List<LookupItemDto>>(results.Select(c => new LookupItemDto
                {
                    Id = c.DisputeTypeId,
                    Name = c.Name
                }).ToList());
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<LookupItemDto>>(exception.Message);
            }
        }

        [HttpGet("lookup/discrepancies")]
        public async Task<ApiResponse<List<LookupItemDto>>> GetDiscrepancies()
        {
            try
            {
                var results = await _repository.GetLookupDataAsync<DisputeDiscrepancy>(c => c.IsActive == true);

                return new ApiResponse<List<LookupItemDto>>(results.Select(c => new LookupItemDto
                {
                    Id = c.DisputeDiscrepancyId,
                    Name = c.Name
                }).ToList());
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<LookupItemDto>>(exception.Message);
            }
        }

        [HttpGet("lookup/currencies")]
        public ApiResponse<List<LookupItemDto>> GetCurrencies()
        {
            try
            {
                var results = _repository.GetCurrencyData();

                return new ApiResponse<List<LookupItemDto>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<LookupItemDto>>(exception.Message);
            }
        }

        [HttpGet("lookup/add-types")]
        public async Task<ApiResponse<List<LookupItemDto>>> GetAddTypes()
        {
            try
            {
                var results = await _repository.GetLookupDataAsync<AddType>(c => c.IsActive == true);

                return new ApiResponse<List<LookupItemDto>>(results.Select(c => new LookupItemDto
                {
                    Id = c.AddTypeId,
                    Name = c.Name
                }).ToList());
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<LookupItemDto>>(exception.Message);
            }
        }

        [HttpGet("lookup/roles")]
        public async Task<ApiResponse<List<LookupItemDto>>> GetRoles()
        {
            try
            {
                var results = await _repository.GetLookupDataAsync<Role>(c => c.Disabled == false);

                return new ApiResponse<List<LookupItemDto>>(results.Select(c => new LookupItemDto
                {
                    Id = (int)c.Id,
                    Name = c.Name
                }).ToList());
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<LookupItemDto>>(exception.Message);
            }
        }

        [HttpGet("lookup/log-type-status")]
        public async Task<ApiResponse<List<VLogTypeStatus>>> GetLogTypeStatuses()
        {
            try
            {
                var results = await _repository.GetLogTypeStatus();

                return new ApiResponse<List<VLogTypeStatus>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<VLogTypeStatus>>(exception.Message);
            }
        }
        #endregion

    }
}
