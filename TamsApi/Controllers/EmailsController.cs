using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TamsApi.Core;
using TamsApi.Core.Email;
using TamsApi.Core.Identity;
using TamsApi.Data;
using TamsApi.InputModels;
using TamsEmail.Templates;
using TamsEmail.Views.Templates;
using TamsApi.Models;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using TamsApi.Models.Lookups;

namespace TamsApi.Controllers
{
    [ApiController]
    [Route("emails")]
    public class EmailsController : AuthenticatedControllerBase
    {
        private readonly ISmtpService _smtpService;
        private readonly IUserRepository _userRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailRenderEngine engine;
        private readonly SmtpOptions _options;
        private static readonly Regex AnkuraDomain = new Regex(@"@ankura.com\Z", RegexOptions.IgnoreCase);

        public EmailsController(
            ISmtpService smtpService,
            IUserRepository userRepository,
            IEmailRepository emailRepository,
            IEmailRenderEngine engine,
            IOptions<SmtpOptions> opts,
            IAuthenticatedUserBuilder builder) : base(builder)
        {
            _smtpService = smtpService;
            _userRepository = userRepository;
            _emailRepository = emailRepository;
            this.engine = engine;
            _options = opts.Value;
        }

        [HttpGet("show")]
        [Authorize(Roles = "admin")]
        public async Task<ContentResult> ShowAsync(string template = null)
        {
            var requestedTemplate = (template.IsPresent() ? template : "Preview");
            var modelKey = requestedTemplate.ToLowerInvariant().Trim();
            if (ViewModels.ContainsKey(modelKey))
            {
                var model = ViewModels[modelKey];
                var rawHtml = await this.engine.RenderViewToStringAsync(requestedTemplate, model);
                var contentResult = Content(rawHtml, "text/html");
                contentResult.StatusCode = (int)HttpStatusCode.OK;
                return contentResult;
            }

            return Content($"Unrecognized template: {requestedTemplate}", "text/plain");
        }

        [Authorize(Roles = "admin")]
        [HttpPost("preview")]
        public ActionResult Preview(PreviewEmail input)
        {
            var tamsUser = _userRepository.FindByEmail(input.To);
            if (tamsUser == null)
            {
                return BadRequest(new ApiResponse<string>(new List<string> { $"User is disabled or not found: {input.To}." }));
            }

            if (input.Cc.IsPresent() && !AnkuraDomain.IsMatch(input.Cc))
            {
                return BadRequest(new ApiResponse<string>(new List<string> { $"Only ankura emails may be CCed: {input.Cc}." }));
            }

            var emailRequest = new EmailRequest<PreviewViewModel>
            {
                Template = "Preview",
                Model = new PreviewViewModel(),
                To = input.To,
                Cc = input.Cc,
                Subject = "TAMS Email Sample"
            };

            try
            {
                _smtpService.Send(emailRequest);
            }
            catch (Exception e)
            {
                var messages = new List<string> { "Failed to send email.", e.ToString() };
                if (e.InnerException != null) { messages.Add(e.InnerException.ToString()); }
                return new JsonResult(ApiResponse.Failed(_options, messages))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            return Ok(new ApiResponse<string>("Email sent successfully.", new List<string>()));
        }

        public static List<TsaLogNumber> SampleLogNumbers = new List<TsaLogNumber>
        {
            new TsaLogNumber("37", "M.00.Sample.F1"),
            new TsaLogNumber("39", "M.00.Sample.F2"),
            new TsaLogNumber("43", "M.00.Sample.F3")
        };

        public static Dictionary<string, object> ViewModels = new Dictionary<string, object>
        {
            { "preview", new PreviewViewModel() },
            { "addrequestsubmitted", new AddRequestSubmittedViewModel
            {
                Salutation = "User Name",
                TsaId = "M.00.Sample",
                RequestDate = System.DateTime.UtcNow.ToLongDateString(),
                RequestType = "Add",
                Status = "Add Pending",
                RequestedBy = "A User",
                SubmittedBy = "Another User",
                Comments = "Sample comments",
                Attachments = "Attachment1.docx, Request.pdf",
                LogNumbers = SampleLogNumbers,
                SupportEmailAddress = SmtpOptions.DefaultSupportEmail,
                AddType = "Add a new TSA",
            } },
            { "statuschangeaddlog", new StatusChangeAddLogViewModel
            {
                Salutation = "User Name",
                Id = "24634",
                CurrentStatus = "Pending review",
                TsaSubId = "M.00.Sample.F7",
                TsaId = "M.00.Sample",
                Comments = "Just started the review process.",
                SupportEmailAddress = SmtpOptions.DefaultSupportEmail
            } },
            { "changerequestsubmitted", new ChangeRequestSubmittedViewModel
            {
                Salutation = "User Name",
                TsaId = "M.00.Sample",
                RequestDate = System.DateTime.UtcNow.ToLongDateString(),
                RequestType = "Change",
                Status = "Add Pending",
                RequestedBy = "A User",
                SubmittedBy = "Another User",
                Comments = "Sample comments",
                Attachments = "Attachment1.docx, Request.pdf",
                LogNumbers = SampleLogNumbers,
                SupportEmailAddress = SmtpOptions.DefaultSupportEmail
            } },
            { "statuschangechangelog", new StatusChangeChangeLogViewModel
            {
                Salutation = "User Name",
                Id = "24634",
                CurrentStatus = "Pending review",
                TsaSubId = "M.00.Sample.F7",
                TsaId = "M.00.Sample",
                Comments = "Just started the review process.",
                SupportEmailAddress = SmtpOptions.DefaultSupportEmail
            } },
            { "disputerequestsubmitted", new DisputeRequestSubmittedViewModel
            {
                Salutation = "User Name",
                TsaId = "M.00.Sample",
                RequestDate = System.DateTime.UtcNow.ToLongDateString(),
                RequestType = "Dispute",
                InvoiceNumber = "0005",
                RequestedBy = "A User",
                SubmittedBy = "Another User",
                Discrepancy = "Something is misaligned",
                Comments = "Dispute reasoning",
                Attachments = "Attachment1.docx, Request.pdf",
                LogNumbers = SampleLogNumbers,
                SupportEmailAddress = SmtpOptions.DefaultSupportEmail
            } },
            { "statuschangedisputelog", new StatusChangeDisputeLogViewModel
            {
                Salutation = "User Name",
                Id = "24634",
                CurrentStatus = "Pending review",
                TsaSubId = "M.00.Sample.F7",
                TsaId = "M.00.Sample",
                Comments = "Dispute is under review.",
                SupportEmailAddress = SmtpOptions.DefaultSupportEmail,
                InvoiceNumber = "0005",
                AgreedCost = "$10,2342.00",
                Currency = "USD"
            } }
        };

        #region Email API Route

        [HttpPost]
        [Route("/api/[controller]/add-log")]
        public async Task<ActionResult> SendNewAddLogEmail([FromBody] JsonElement body)
        {
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(body);
                var idList = json.ToList<int>("idList");

                if (idList != null && idList.Count > 0)
                {
                    var model = await _emailRepository.GetNewAddRequestAsync(idList);
                    var emailRequest = new EmailRequest<AddRequestSubmittedViewModel>
                    {
                        Template = "AddRequestSubmitted",
                        Model = model,
                        To = model.ToEmailAddress,
                        //Cc = input.Cc,
                        Subject = "TAMS Add Request Received"
                    };

                    _smtpService.Send(emailRequest);
                    return Ok(new ApiResponse<string>("Email sent successfully.", new List<string>()));
                }
            }
            catch (Exception e)
            {
                var messages = new List<string> { "Failed to send email.", e.ToString() };
                if (e.InnerException != null) { messages.Add(e.InnerException.ToString()); }
                return new JsonResult(ApiResponse.Failed(_options, messages))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            return Ok();
        }

        [HttpPut]
        [Route("/api/[controller]/add-log")]
        public async Task<ActionResult> SendUpdateAddLogEmail([FromBody] JsonElement body)
        {
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(body);
                var idList = json.ToList<int>("idList");

                var model = await _emailRepository.GetUpdatedAddRequestAsync(idList.FirstOrDefault());
                var emailRequest = new EmailRequest<StatusChangeAddLogViewModel>
                {
                    Template = "StatusChangeAddLog",
                    Model = model,
                    To = model.ToEmailAddress,
                    //Cc = input.Cc,
                    Subject = "TAMS Add Request Status Change"
                };

                _smtpService.Send(emailRequest);

                return Ok(new ApiResponse<string>("Email sent successfully.", new List<string>()));
            }
            catch (Exception e)
            {
                var messages = new List<string> { "Failed to send email.", e.ToString() };
                if (e.InnerException != null) { messages.Add(e.InnerException.ToString()); }
                return new JsonResult(ApiResponse.Failed(_options, messages))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpPost]
        [Route("/api/[controller]/change-log")]
        public async Task<ActionResult> SendNewChangeLogEmail([FromBody] JsonElement body)
        {
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(body);
                var idList = json.ToList<int>("idList");

                if (idList != null && idList.Count > 0)
                {
                    var model = await _emailRepository.GetNewChangeRequestAsync(idList);
                    var emailRequest = new EmailRequest<ChangeRequestSubmittedViewModel>
                    {
                        Template = "ChangeRequestSubmitted",
                        Model = model,
                        To = model.ToEmailAddress,
                        //Cc = input.Cc,
                        Subject = "TAMS " + (model.RequestType == LogTypeId.Escalation.ToString() ? "Performance Standard Escalation" : "Change") + " Request Received"
                    };

                    _smtpService.Send(emailRequest);
                    return Ok(new ApiResponse<string>("Email sent successfully.", new List<string>()));
                }
            }
            catch (Exception e)
            {
                var messages = new List<string> { "Failed to send email.", e.ToString() };
                if (e.InnerException != null) { messages.Add(e.InnerException.ToString()); }
                return new JsonResult(ApiResponse.Failed(_options, messages))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            return Ok();
        }

        [HttpPut]
        [Route("/api/[controller]/change-log")]
        public async Task<ActionResult> SendUpdateChangeLogEmail([FromBody] JsonElement body)
        {
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(body);
                var idList = json.ToList<int>("idList");

                var model = await _emailRepository.GetUpdatedChangeRequestAsync(idList.FirstOrDefault());
                var emailRequest = new EmailRequest<StatusChangeChangeLogViewModel>
                {
                    Template = "StatusChangeChangeLog",
                    Model = model,
                    To = model.ToEmailAddress,
                    //Cc = input.Cc,
                    Subject = "TAMS " + (model.RequestType == LogTypeId.Escalation.ToString() ? "Performance Standard Escalation" : "Change") + " Request Status Change"
                };

                _smtpService.Send(emailRequest);

                return Ok(new ApiResponse<string>("Email sent successfully.", new List<string>()));
            }
            catch (Exception e)
            {
                var messages = new List<string> { "Failed to send email.", e.ToString() };
                if (e.InnerException != null) { messages.Add(e.InnerException.ToString()); }
                return new JsonResult(ApiResponse.Failed(_options, messages))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpPost]
        [Route("/api/[controller]/dispute-log")]
        public async Task<ActionResult> SendNewDisputeLogEmail([FromBody] JsonElement body)
        {
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(body);
                var idList = json.ToList<int>("idList");

                if (idList != null && idList.Count > 0)
                {
                    var model = await _emailRepository.GetNewDisputeRequestAsync(idList);
                    var emailRequest = new EmailRequest<DisputeRequestSubmittedViewModel>
                    {
                        Template = "DisputeRequestSubmitted",
                        Model = model,
                        To = model.ToEmailAddress,
                        //Cc = input.Cc,
                        Subject = "TAMS Dispute Request Received"
                    };

                    _smtpService.Send(emailRequest);
                    return Ok(new ApiResponse<string>("Email sent successfully.", new List<string>()));
                }
            }
            catch (Exception e)
            {
                var messages = new List<string> { "Failed to send email.", e.ToString() };
                if (e.InnerException != null) { messages.Add(e.InnerException.ToString()); }
                return new JsonResult(ApiResponse.Failed(_options, messages))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            return Ok();
        }

        [HttpPut]
        [Route("/api/[controller]/dispute-log")]
        public async Task<ActionResult> SendUpdateDisputeLogEmail([FromBody] JsonElement body)
        {
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(body);
                var idList = json.ToList<int>("idList");

                var model = await _emailRepository.GetUpdatedDisputeRequestAsync(idList.FirstOrDefault());
                var emailRequest = new EmailRequest<StatusChangeDisputeLogViewModel>
                {
                    Template = "StatusChangeDisputeLog",
                    Model = model,
                    To = model.ToEmailAddress,
                    //Cc = input.Cc,
                    Subject = "TAMS Dispute Request Status Change"
                };

                _smtpService.Send(emailRequest);

                return Ok(new ApiResponse<string>("Email sent successfully.", new List<string>()));
            }
            catch (Exception e)
            {
                var messages = new List<string> { "Failed to send email.", e.ToString() };
                if (e.InnerException != null) { messages.Add(e.InnerException.ToString()); }
                return new JsonResult(ApiResponse.Failed(_options, messages))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        #endregion Email API Route
    }
}
