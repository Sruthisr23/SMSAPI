using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using LPMessengerAPI.DtoModel;

namespace LPMessengerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateMastersController : ControllerBase
    {
        private readonly lpmessengerdbContext _context;
        private readonly IUserService _userService;
        private readonly ITemplateService _templateService;
        private readonly IFileAttachmentService _fileAttachmentService;
        private readonly IExternalService _externalService;

        public TemplateMastersController(lpmessengerdbContext context, IUserService userService, ITemplateService templateService, IFileAttachmentService fileAttachmentService, IExternalService externalService)
        {
            _context = context;
            _userService = userService;
            _templateService = templateService;
            _fileAttachmentService = fileAttachmentService;
            _externalService = externalService;
        }

        // GET: api/TemplateMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TemplateMaster>>> GetTemplateMasters(int userId, string token)
        {
            var _token = await _userService.GetToken(userId);
            if (_token != token)
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }
            return await _context.TemplateMasters.ToListAsync();
        }

        [HttpGet("ByServiceId")]
        public async Task<ActionResult<IEnumerable<TemplateMaster>>> GetTemplateBasedOnServiceId(int serviceId, int userId, string token)
        {
            var _token = await _userService.GetToken(userId);
            if (_token != token)
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            return await _templateService.GetTemplateByServiceId(serviceId);
        }

        // GET: api/TemplateMasters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TemplateMasterData>> GetTemplateMaster(int id, int userId, string token)
        {
            var _token = await _userService.GetToken(userId);
            if (_token != token)
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            var templateMaster = await _templateService.GetTemplateById(id);            

            if (templateMaster == null)
            {
                return NotFound();
            }

            return templateMaster;
        }

        // PUT: api/TemplateMasters/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{id}")]
        public async Task<IActionResult> PutTemplateMaster(int id, int userId, string token, TemplateMasterData templateMasterDto)
        {
            if (id != templateMasterDto.Id)
            {
                return BadRequest();
            }

            var _token = await _userService.GetToken(userId);
            if (_token != token)
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            var templateMaster = await _context.TemplateMasters.FindAsync(id);

            templateMaster.Name = templateMasterDto.Name;
            templateMaster.ServiceId = templateMasterDto.ServiceId;
            templateMaster.Subject = templateMasterDto.Subject;
            templateMaster.Body = templateMasterDto.Body;
            templateMaster.IsBodyHtml = templateMasterDto.IsBodyHtml;
            //templateMaster.UpdatedDate = templateMasterDto.UpdatedDate;
            //templateMaster.Id = templateMasterDto.Id;

            _context.Entry(templateMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                
                //FileAttachment fileAttachment = new FileAttachment();

                //foreach (var data in templateMasterDto.FormFiles)
                //{
                //    fileAttachment.TemplateId = templateMaster.Id;
                //    fileAttachment.FilePath = data.FileName;
                //    await _externalService.SaveFileInFolder(data);
                //    await _fileAttachmentService.SaveFileTemplate(fileAttachment);
                //}
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TemplateMasterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(templateMaster);
        }

        // POST: api/TemplateMasters
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TemplateMaster>> PostTemplateMaster(int userId, string token, TemplateMasterData templateMasterDto)
        {
            var _token = await _userService.GetToken(userId);
            if (_token != token)
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }
            TemplateMaster templateMaster = new TemplateMaster();
            templateMaster.Name = templateMasterDto.Name;
            templateMaster.ServiceId = templateMasterDto.ServiceId;
            templateMaster.Subject = templateMasterDto.Subject;
            templateMaster.Body = templateMasterDto.Body;
            templateMaster.IsBodyHtml = templateMasterDto.IsBodyHtml;
            templateMaster.UserId = templateMasterDto.UserId;
            //templateMaster.CreatedDate = DateTime.Now();
            _context.TemplateMasters.Add(templateMaster);
            await _context.SaveChangesAsync();

            FileAttachment fileAttachment = new FileAttachment();
            if (templateMasterDto.FormFiles != null)
            {
                foreach (var data in templateMasterDto.FormFiles)
                {
                    fileAttachment.TemplateId = templateMaster.Id;
                    fileAttachment.FilePath = data.FileName;
                    await _externalService.SaveFileInFolder(data);
                    await _fileAttachmentService.SaveFileTemplate(fileAttachment);
                }
            }

                       

            return CreatedAtAction("GetTemplateMaster", new { id = templateMaster.Id }, templateMaster);
        }

        // DELETE: api/TemplateMasters/5
        [HttpPost("delete")]
        public async Task<ActionResult<TemplateMaster>> DeleteTemplateMaster(int id, int userId, string token)
        {
            var _token = await _userService.GetToken(userId);
            if (_token != token)
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            var templateMaster = await _context.TemplateMasters.FindAsync(id);
            if (templateMaster == null)
            {
                return NotFound();
            }

            _context.TemplateMasters.Remove(templateMaster);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool TemplateMasterExists(int id)
        {
            return _context.TemplateMasters.Any(e => e.Id == id);
        }
    }
}
