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
    public class ServicesAvailablesController : ControllerBase
    {
        private readonly lpmessengerdbContext _context;

        public ServicesAvailablesController(lpmessengerdbContext context)
        {
            _context = context;
        }

        // GET: api/ServicesAvailables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicesAvailable>>> GetServicesAvailable()
        {
            return await _context.ServicesAvailables.ToListAsync();
        }

        // GET: api/ServicesAvailables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicesAvailable>> GetServicesAvailable(int id)
        {
            var servicesAvailable = await _context.ServicesAvailables.FindAsync(id);

            if (servicesAvailable == null)
            {
                return NotFound();
            }

            return servicesAvailable;
        }

        // PUT: api/ServicesAvailables/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{id}")]
        public async Task<IActionResult> PutServicesAvailable(int id, ServicesAvailable servicesAvailable)
        {
            if (id != servicesAvailable.Id)
            {
                return BadRequest();
            }

            _context.Entry(servicesAvailable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicesAvailableExists(id))
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

        // POST: api/ServicesAvailables
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ServicesAvailable>> PostServicesAvailable(ServicesAvailable servicesAvailable)
        {
            _context.ServicesAvailables.Add(servicesAvailable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServicesAvailable", new { id = servicesAvailable.Id }, servicesAvailable);
        }

        // DELETE: api/ServicesAvailables/5
        [HttpPost("delete/{id}")]
        public async Task<ActionResult<ServicesAvailable>> DeleteServicesAvailable(int id, int userId, string token)
        {
            var servicesAvailable = await _context.ServicesAvailables.FindAsync(id);
            if (servicesAvailable == null)
            {
                return NotFound();
            }

            _context.ServicesAvailables.Remove(servicesAvailable);
            await _context.SaveChangesAsync();

            return servicesAvailable;
        }

        private bool ServicesAvailableExists(int id)
        {
            return _context.ServicesAvailables.Any(e => e.Id == id);
        }
    }
}
