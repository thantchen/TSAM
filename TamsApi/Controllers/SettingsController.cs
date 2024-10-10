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
using TamsApi.Core.Identity;
using TamsApi.Data;
using TamsApi.Models;

namespace TamsApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class SettingsController : AuthenticatedControllerBase
    {
        #region Member variables

        private readonly ISettingsRepository _repository;
        private readonly IAuthenticatedUserBuilder builder;

        #endregion

        #region Constructor

        public SettingsController(ISettingsRepository repository,
            IAuthenticatedUserBuilder builder) : base(builder)
        {
            _repository = repository;
            this.builder = builder;
        }

        #endregion

        #region Public methods

        [HttpGet("settings/users")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<User>>> GetUsers()
        {
            try
            {
                var results = await _repository.GetUsersAsync();

                return new ApiResponse<List<User>>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<List<User>>(exception.Message);
            }
        }

        [HttpPut("settings/user")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<User>> UpdateUser(User user)
        {
            try
            {
                var results = await _repository.UpdateUserAsync(user, CurrentUser);

                return new ApiResponse<User>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<User>(exception.Message);
            }
        }

        [HttpPost("settings/user")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<User>> AddUser(User user)
        {
            try
            {
                var results = await _repository.AddUserAsync(user, CurrentUser);

                return new ApiResponse<User>(results);
            }
            catch (Exception exception)
            {
                return new ApiResponse<User>(exception.Message);
            }
        }

        #endregion

    }
}
