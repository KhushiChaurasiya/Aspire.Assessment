using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Assignment.API.Controllers;
using Assignment.Contracts.Data.Entities;
using Assignment.Contracts.DTO;
using Assignment.Core.Exceptions;
using Assignment.Core.Handlers.Commands;
using Assignment.Core.Handlers.Queries;
using Assignment.Providers.Handlers.Commands;
using Assignment.Providers.Handlers.Queries;
using Azure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;

namespace Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AppController> _logger;
        private IFormFile files;

        public AppController(IMediator mediator, ILogger<AppController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AppDTO>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(BaseResponseDTO))]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("fetch the all app details");
                var query = new GetAllAppQuery();
                var response = await _mediator.Send(query);
                if (response == null)
                {
                    _logger.LogError(new NullReferenceException(), "app details not found!");
                    throw new NullReferenceException("app details not found!");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {

                _logger.LogError($"app details not found!: {ex}");
                return NotFound(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [ProducesErrorResponseType(typeof(BaseResponseDTO))]
        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> Post([FromForm] CreateAppDTO model)
        {
            try
            {
                _logger.LogInformation("insert the app detials");
                var files = Request.Form.Files[0];
                if (files == null || files.Length == 0)
                {
                    _logger.LogError(new NullReferenceException(), "File not selected", DateTimeOffset.UtcNow);
                    throw new NullReferenceException("file not selected!");

                }
                model.Files = files.FileName;

                var command = new CreateAppCommand(model, files);
                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (Exception ex)
            {

                return NotFound(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(AppDTO), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(BaseResponseDTO))]
        [Authorize(Roles = "Developer,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation($"Get the app by id :{id}");
                var query = new GetAppByIdQuery(id);
                var response = await _mediator.Send(query);
                if(response == null)
                {
                    _logger.LogInformation($"Get the app by id :{id}");
                }
                return Ok(response);
            }
            catch(Exception ex) {
                _logger.LogError($" something went wrong: {ex}");
                return NotFound(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> Put(int id, [FromForm] CreateAppDTO model)
        {
            try
            {
                IFormFile files = null;
                var query = new GetAppByIdQuery(id);
                var res = await _mediator.Send(query);
                if (Request.Form.Files.Count > 0)
                {
                    files = Request.Form.Files[0];
                    if (files == null || files.Length == 0)
                    {
                        _logger.LogInformation("File not selected", DateTimeOffset.UtcNow);
                        throw new NullReferenceException("file not selected!");
                    }

                    model.Files = files.FileName;

                }

                var command = new UpdateAppCommand(model, files);
                var response = await _mediator.Send(command);

                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError("Put method return error: {ex}", ex);
                return NotFound(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(AppDTO), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(BaseResponseDTO))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var query = new DeleteAppByIdQuery(id);
                var response = await _mediator.Send(query);
                _logger.LogInformation("App Item {0} Deleted",id);

                return Ok(response);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }


        [HttpGet]
        [Route("download")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Download([FromQuery] IFormFile file)
        {

            try
            {
                var query = new GetDownloadFilesQuery(file);
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = new string[] { ex.Message }
                });
            }
            catch (Exception ex)
            {
                return NotFound(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }


        [HttpPut]
        [Route("AppDownload")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [ProducesErrorResponseType(typeof(BaseResponseDTO))]
        public async Task<IActionResult> PostAppDownload(int AppId, [FromForm] AppDownloadDTO model)
        {
            try
            {
                var command = new CreateAppDownloadCommand(AppId);
                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Getting error when download the app: {ex}", ex);
                return BadRequest(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(BaseResponseDTO))]
        [Route("DownloadedReport")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DownloadedReport(DateTime? fromdate,DateTime? todate)
        {
            try
            {
                var query = new GetDownloadedReportDetailsQuery(fromdate, todate);
                var response = await _mediator.Send(query);
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Getting error when see the downloaded report: {ex}", ex);

                return NotFound(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(BaseResponseDTO))]
        [Route("LogReport")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LogReport()
        {
            try
            {
                var query = new GetAllLogReportQuery();
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Getting error when see the report: {ex}", ex);

                return NotFound(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }
    }
}