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
    public class MlSenderConfigsController : ControllerBase
    {
        private readonly lpmessengerdbContext _context;

        public MlSenderConfigsController(lpmessengerdbContext context)
        {
            _context = context;
        }

        // GET: api/MlSenderConfigs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MlSenderConfig>>> GetMlSenderConfigs()
        {
            return await _context.MlSenderConfigs.ToListAsync();
        }

        // GET: api/MlSenderConfigs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MlSenderConfig>> GetMlSenderConfig(int id)
        {
            var mlSenderConfig = await _context.MlSenderConfigs.FindAsync(id);

            if (mlSenderConfig == null)
            {
                return NotFound();
            }

            return mlSenderConfig;
        }

        // PUT: api/MlSenderConfigs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{id}")]
        public async Task<IActionResult> PutMlSenderConfig(int id, MlSenderConfig mlSenderConfig)
        {
            if (id != mlSenderConfig.Id)
            {
                return BadRequest();
            }

            _context.Entry(mlSenderConfig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MlSenderConfigExists(id))
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

        [HttpPost]
        public async Task<ActionResult<MlSenderConfig>> PostMlSenderConfig(MlSenderConfig mlSenderConfig)
        {
            _context.MlSenderConfigs.Add(mlSenderConfig);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMlSenderConfig", new { id = mlSenderConfig.Id }, mlSenderConfig);
        }

        // DELETE: api/MlSenderConfigs/5
        [HttpPost("delete")]
        public async Task<ActionResult<MlSenderConfig>> DeleteMlSenderConfig(int id, int userId, string token)
        {
            var mlSenderConfig = await _context.MlSenderConfigs.FindAsync(id);
            if (mlSenderConfig == null)
            {
                return NotFound();
            }

            _context.MlSenderConfigs.Remove(mlSenderConfig);
            await _context.SaveChangesAsync();

            return mlSenderConfig;
        }

        private bool MlSenderConfigExists(int id)
        {
            return _context.MlSenderConfigs.Any(e => e.Id == id);
        }
    }
}
