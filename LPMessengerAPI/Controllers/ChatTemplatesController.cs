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
    public class ChatTemplatesController : ControllerBase
    {
        private readonly lpmessengerdbContext _context;

        public ChatTemplatesController(lpmessengerdbContext context)
        {
            _context = context;
        }

        // GET: api/ChatTemplates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatTemplate>>> GetChatTemplates()
        {
            return await _context.ChatTemplates.ToListAsync();
        }

        // GET: api/ChatTemplates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatTemplate>> GetChatTemplate(int id)
        {
            var chatTemplate = await _context.ChatTemplates.FindAsync(id);

            if (chatTemplate == null)
            {
                return NotFound();
            }

            return chatTemplate;
        }

        // PUT: api/ChatTemplates/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{id}")]
        public async Task<IActionResult> PutChatTemplate(int id, ChatTemplate chatTemplate)
        {
            if (id != chatTemplate.Id)
            {
                return BadRequest();
            }

            _context.Entry(chatTemplate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatTemplateExists(id))
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

        // POST: api/ChatTemplates
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ChatTemplate>> PostChatTemplate(ChatTemplate chatTemplate)
        {
            _context.ChatTemplates.Add(chatTemplate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChatTemplate", new { id = chatTemplate.Id }, chatTemplate);
        }

        // DELETE: api/ChatTemplates/5
        [HttpPost("delete/{id}")]
        public async Task<ActionResult<ChatTemplate>> DeleteChatTemplate(int id, int userId, string token)
        {
            var chatTemplate = await _context.ChatTemplates.FindAsync(id);
            if (chatTemplate == null)
            {
                return NotFound();
            }

            _context.ChatTemplates.Remove(chatTemplate);
            await _context.SaveChangesAsync();

            return chatTemplate;
        }

        private bool ChatTemplateExists(int id)
        {
            return _context.ChatTemplates.Any(e => e.Id == id);
        }
    }
}
