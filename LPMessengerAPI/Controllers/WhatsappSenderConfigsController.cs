using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;

namespace LPMessengerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsappSenderConfigsController : ControllerBase
    {
        private readonly lpmessengerdbContext _context;
        private readonly IUserService _userService;

        public WhatsappSenderConfigsController(lpmessengerdbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: api/WhatsappSenderConfigs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WhatsappSenderConfig>>> GetWhatsappSenderConfigs(int userId, string token)
        {
            var data = await _userService.GetToken(userId);
            if (data != token)// false  
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            return await _context.WhatsappSenderConfigs.ToListAsync();
        }

        // GET: api/WhatsappSenderConfigs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WhatsappSenderConfig>> GetWhatsappSenderConfig(int id, int userId, string token)
        {
            var data = await _userService.GetToken(userId);
            if (data != token)// false  
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            var whatsappSenderConfig = await _context.WhatsappSenderConfigs.FindAsync(id);

            if (whatsappSenderConfig == null)
            {
                return NotFound();
            }

            return whatsappSenderConfig;
        }

        // PUT: api/WhatsappSenderConfigs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{id}")]
        public async Task<IActionResult> PutWhatsappSenderConfig(int id, int userId, string token, WhatsappSenderConfig whatsappSenderConfig)
        {
            if (id != whatsappSenderConfig.Id)
            {
                return BadRequest();
            }

            var data = await _userService.GetToken(userId);
            if (data != token)// false  
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            _context.Entry(whatsappSenderConfig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WhatsappSenderConfigExists(id))
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

        // POST: api/WhatsappSenderConfigs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<WhatsappSenderConfig>> PostWhatsappSenderConfig(int userId, string token,WhatsappSenderConfig whatsappSenderConfig)
        {
            var data = await _userService.GetToken(userId);
            if (data != token)// false  
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            _context.WhatsappSenderConfigs.Add(whatsappSenderConfig);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWhatsappSenderConfig", new { id = whatsappSenderConfig.Id }, whatsappSenderConfig);
        }

        // DELETE: api/WhatsappSenderConfigs/5
        [HttpPost("delete")]
        public async Task<ActionResult<WhatsappSenderConfig>> DeleteWhatsappSenderConfig(int id, int userId, string token)
        {
            var data = await _userService.GetToken(userId);
            if (data != token)// false  
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            var whatsappSenderConfig = await _context.WhatsappSenderConfigs.FindAsync(id);
            if (whatsappSenderConfig == null)
            {
                return NotFound();
            }

            _context.WhatsappSenderConfigs.Remove(whatsappSenderConfig);
            await _context.SaveChangesAsync();

            return whatsappSenderConfig;
        }

        private bool WhatsappSenderConfigExists(int id)
        {
            return _context.WhatsappSenderConfigs.Any(e => e.Id == id);
        }
    }
}
