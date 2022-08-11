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
    public class SmsTransactionMessageStatusController : ControllerBase
    {
        private readonly lpmessengerdbContext _context;

        public SmsTransactionMessageStatusController(lpmessengerdbContext context)
        {
            _context = context;
        }

        // GET: api/SmsTransactionMessageStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SmsTransactionMessageStatus>>> GetSmsTransactionMessageStatuses()
        {
            return await _context.SmsTransactionMessageStatuses.ToListAsync();
        }

        // GET: api/SmsTransactionMessageStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SmsTransactionMessageStatus>> GetSmsTransactionMessageStatus(long id)
        {
            var smsTransactionMessageStatus = await _context.SmsTransactionMessageStatuses.FindAsync(id);

            if (smsTransactionMessageStatus == null)
            {
                return NotFound();
            }

            return smsTransactionMessageStatus;
        }

        // PUT: api/SmsTransactionMessageStatus/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{id}")]
        public async Task<IActionResult> PutSmsTransactionMessageStatus(long id, SmsTransactionMessageStatus smsTransactionMessageStatus)
        {
            if (id != smsTransactionMessageStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(smsTransactionMessageStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SmsTransactionMessageStatusExists(id))
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

        // POST: api/SmsTransactionMessageStatus
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SmsTransactionMessageStatus>> PostSmsTransactionMessageStatus(SmsTransactionMessageStatus smsTransactionMessageStatus)
        {
            _context.SmsTransactionMessageStatuses.Add(smsTransactionMessageStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSmsTransactionMessageStatus", new { id = smsTransactionMessageStatus.Id }, smsTransactionMessageStatus);
        }

        // DELETE: api/SmsTransactionMessageStatus/5
        [HttpPost("delete/{id}")]
        public async Task<ActionResult<SmsTransactionMessageStatus>> DeleteSmsTransactionMessageStatus(long id, int userId, string token)
        {
            var smsTransactionMessageStatus = await _context.SmsTransactionMessageStatuses.FindAsync(id);
            if (smsTransactionMessageStatus == null)
            {
                return NotFound();
            }

            _context.SmsTransactionMessageStatuses.Remove(smsTransactionMessageStatus);
            await _context.SaveChangesAsync();

            return smsTransactionMessageStatus;
        }

        private bool SmsTransactionMessageStatusExists(long id)
        {
            return _context.SmsTransactionMessageStatuses.Any(e => e.Id == id);
        }
    }
}
