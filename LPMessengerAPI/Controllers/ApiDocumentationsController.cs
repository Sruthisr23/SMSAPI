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
    public class ApiDocumentationsController : ControllerBase
    {
        private readonly lpmessengerdbContext _context;
        private readonly IApiDocumentationService _apiDocumentationService;
        private readonly IUserService _userService;

        public ApiDocumentationsController(lpmessengerdbContext context, IApiDocumentationService apiDocumentationService,
            IUserService userService)
        {
            _context = context;
            _apiDocumentationService = apiDocumentationService;
            _userService = userService;
        }

        // GET: api/ApiDocumentations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiDocumentation>>> GetApiDocumentation(int userId, string token)
        {
            var tokenResponse = await _userService.GetToken(userId);
            if (tokenResponse != token)// false
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            return await _context.ApiDocumentations.ToListAsync();
        }

        // GET: api/ApiDocumentations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiDocumentation>> GetApiDocumentation(int id, int userId, string token)
        {
            var tokenResponse = await _userService.GetToken(userId);
            if (tokenResponse != token)// false
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            var apiDocumentation = await _context.ApiDocumentations.FindAsync(id);

            if (apiDocumentation == null)
            {
                return NotFound();
            }

            return apiDocumentation;
        }

        [HttpGet("byServiceId")]
        public async Task<ActionResult<ApiDocumentationResponseDto>> GetApiDocumentation(int userId, string token, int? serviceId)
        {
            var tokenResponse = await _userService.GetToken(userId);
            if (tokenResponse != token)// false
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            var apiDocumentation = await _apiDocumentationService.GetApiDocumentationDetails(serviceId);

            if (apiDocumentation == null)
            {
                return NotFound();
            }

            return Ok(apiDocumentation);
        }

        // PUT: api/ApiDocumentations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{id}")]
        public async Task<IActionResult> PutApiDocumentation(int id, int userId, string token, ApiDocumentation apiDocumentation)
        {
            var tokenResponse = await _userService.GetToken(userId);
            if (tokenResponse != token)// false
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            if (id != apiDocumentation.Id)
            {
                return BadRequest();
            }

            _context.Entry(apiDocumentation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApiDocumentationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApiDocumentations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ApiDocumentation>> PostApiDocumentation(int userId, string token, ApiDocumentation apiDocumentation)
        {
            var tokenResponse = await _userService.GetToken(userId);
            if (tokenResponse != token)// false
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }
            apiDocumentation.Token = token;
            apiDocumentation.UserId = userId;
            _context.ApiDocumentations.Add(apiDocumentation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApiDocumentation", new { id = apiDocumentation.Id }, apiDocumentation);
        }

        // DELETE: api/ApiDocumentations/5
        [HttpPost("delete/{id}")]
        public async Task<ActionResult<ApiDocumentation>> DeleteApiDocumentation(int id, int userId, string token)
        {
            var tokenResponse = await _userService.GetToken(userId);
            if (tokenResponse != token)// false
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            var apiDocumentation = await _context.ApiDocumentations.FindAsync(id);
            if (apiDocumentation == null)
            {
                return NotFound();
            }

            _context.ApiDocumentations.Remove(apiDocumentation);
            await _context.SaveChangesAsync();

            return apiDocumentation;
        }

        private bool ApiDocumentationExists(int id)
        {
            return _context.ApiDocumentations.Any(e => e.Id == id);
        }
    }
}
