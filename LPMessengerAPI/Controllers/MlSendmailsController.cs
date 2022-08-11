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
    public class MlSendmailsController : ControllerBase
    {
        private readonly lpmessengerdbContext _context;

        public MlSendmailsController(lpmessengerdbContext context)
        {
            _context = context;
        }

        // GET: api/MlSendmails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MlSendmail>>> GetMlSendmails()
        {
            return await _context.MlSendmails.ToListAsync();
        }

        // GET: api/MlSendmails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MlSendmail>> GetMlSendmail(int id)
        {
            var mlSendmail = await _context.MlSendmails.FindAsync(id);

            if (mlSendmail == null)
            {
                return NotFound();
            }

            return mlSendmail;
        }

        // PUT: api/MlSendmails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{id}")]
        public async Task<IActionResult> PutMlSendmail(int id, MlSendmail mlSendmail)
        {
            if (id != mlSendmail.Id)
            {
                return BadRequest();
            }

            _context.Entry(mlSendmail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MlSendmailExists(id))
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

        // POST: api/MlSendmails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MlSendmail>> PostMlSendmail(MlSendmail mlSendmail)
        {
            _context.MlSendmails.Add(mlSendmail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMlSendmail", new { id = mlSendmail.Id }, mlSendmail);
        }

        // DELETE: api/MlSendmails/5
        [HttpPost("delete/{id}")]
        public async Task<ActionResult<MlSendmail>> DeleteMlSendmail(int id,int userId, string token)
        {
            var mlSendmail = await _context.MlSendmails.FindAsync(id);
            if (mlSendmail == null)
            {
                return NotFound();
            }

            _context.MlSendmails.Remove(mlSendmail);
            await _context.SaveChangesAsync();

            return mlSendmail;
        }

        private bool MlSendmailExists(int id)
        {
            return _context.MlSendmails.Any(e => e.Id == id);
        }
    }
}
