using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LPMessengerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadFileController : ControllerBase
    {
        private readonly lpmessengerdbContext _context;
        private readonly IUserService _userService;
        private readonly IServiceAvailableService _serviceAvailableService;
        private readonly IOptions<AppSettings> _appSettings;

        public DownloadFileController(lpmessengerdbContext context, IUserService userService,
            IOptions<AppSettings> appSettings, IServiceAvailableService serviceAvailableService)
        {
            _context = context;
            _userService = userService;
            _serviceAvailableService = serviceAvailableService;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<ActionResult> GetFile(int serviceId, int userId, string token)
        {
            var _token = await _userService.GetToken(userId);
            if (_token != token)
            {
                 return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            var service = await _serviceAvailableService.GetServiceAvailableById(serviceId);

            var fileName = service.Name+".csv";
            var filePath = _appSettings.Value.Location + fileName; //@"C:\Joseph\22072022\USABizDB_Alabama.csv";

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/force-download", fileName);
        }

        [HttpGet("DownloadRemoteUri")]
        public async Task<ActionResult> DownloadRemoteUri(string remotePath, int userId, string token)
        {
            var _token = await _userService.GetToken(userId);
            if (_token != token)
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            string remoteUri = "https://www.collinsdictionary.com/images/full/";
            string fileName = "lp_51033703_1000.jpg", myStringWebResource = null;
            // Create a new WebClient instance.
            WebClient myWebClient = new WebClient();
            myStringWebResource = remoteUri + fileName;
            myWebClient.DownloadFile(myStringWebResource, @"C:\\Joseph\22072022\\"+ fileName);

            return Ok();
        }

        	
    }

}
