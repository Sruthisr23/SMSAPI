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
    public class SmsSendsController : ControllerBase
    {
        private readonly lpmessengerdbContext _context;

        public SmsSendsController(lpmessengerdbContext context)
        {
            _context = context;
        }

        // GET: api/SmsSends
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SmsSend>>> GetSmsSends()
        {
            return await _context.SmsSends.ToListAsync();
        }

        // GET: api/SmsSends/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SmsSend>> GetSmsSend(int id)
        {
            var smsSend = await _context.SmsSends.FindAsync(id);

            if (smsSend == null)
            {
                return NotFound();
            }

            return smsSend;
        }

        // PUT: api/SmsSends/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{id}")]
        public async Task<IActionResult> PutSmsSend(int id, SmsSend smsSend)
        {
            if (id != smsSend.Id)
            {
                return BadRequest();
            }

            _context.Entry(smsSend).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SmsSendExists(id))
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

        // POST: api/SmsSends
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SmsSend>> PostSmsSend(SmsSend smsSend)
        {
            _context.SmsSends.Add(smsSend);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSmsSend", new { id = smsSend.Id }, smsSend);
        }

        // DELETE: api/SmsSends/5
        [HttpPost("delete/{id}")]
        public async Task<ActionResult<SmsSend>> DeleteSmsSend(int id, int userId, string token)
        {
            var smsSend = await _context.SmsSends.FindAsync(id);
            if (smsSend == null)
            {
                return NotFound();
            }

            _context.SmsSends.Remove(smsSend);
            await _context.SaveChangesAsync();

            return smsSend;
        }

        private bool SmsSendExists(int id)
        {
            return _context.SmsSends.Any(e => e.Id == id);
        }
    }
}
