using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LPMessengerAPI.Model;

namespace LPMessengerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsappSendsController : ControllerBase
    {
        private readonly lpmessengerdbContext _context;

        public WhatsappSendsController(lpmessengerdbContext context)
        {
            _context = context;
        }

        // GET: api/WhatsappSends
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WhatsappSend>>> GetWhatsappSends()
        {
            return await _context.WhatsappSends.ToListAsync();
        }

        // GET: api/WhatsappSends/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WhatsappSend>> GetWhatsappSend(int id)
        {
            var whatsappSend = await _context.WhatsappSends.FindAsync(id);

            if (whatsappSend == null)
            {
                return NotFound();
            }

            return whatsappSend;
        }

        // PUT: api/WhatsappSends/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{id}")]
        public async Task<IActionResult> PutWhatsappSend(int id, WhatsappSend whatsappSend)
        {
            if (id != whatsappSend.Id)
            {
                return BadRequest();
            }

            _context.Entry(whatsappSend).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WhatsappSendExists(id))
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

        // POST: api/WhatsappSends
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<WhatsappSend>> PostWhatsappSend(WhatsappSend whatsappSend)
        {
            _context.WhatsappSends.Add(whatsappSend);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWhatsappSend", new { id = whatsappSend.Id }, whatsappSend);
        }

        // DELETE: api/WhatsappSends/5
        [HttpPost("delete/{id}")]
        public async Task<ActionResult<WhatsappSend>> DeleteWhatsappSend(int id, int userId, string token)
        {
            var whatsappSend = await _context.WhatsappSends.FindAsync(id);
            if (whatsappSend == null)
            {
                return NotFound();
            }

            _context.WhatsappSends.Remove(whatsappSend);
            await _context.SaveChangesAsync();

            return whatsappSend;
        }

        private bool WhatsappSendExists(int id)
        {
            return _context.WhatsappSends.Any(e => e.Id == id);
        }
    }
}
