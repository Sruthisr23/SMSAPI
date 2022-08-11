using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LPMessengerAPI.Model;
using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Service.Interface;

namespace LPMessengerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MlMailsController : ControllerBase
    {
        private readonly lpmessengerdbContext _context;
        private readonly IMailService _mailService;
        

        public MlMailsController(lpmessengerdbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        // GET: api/MlMails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MlMail>>> GetMlMails()
        {
            return await _context.MlMails.ToListAsync();
        }

        [HttpGet("groups")]
        public async Task<ActionResult<IEnumerable<MlMailDto>>> GetGroupMails(int? groupId)
        {
            var result = await _mailService.GetGroupMailList(groupId);
               
            return result;
        }

        // GET: api/MlMails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MlMail>> GetMlMail(int id)
        {
            var mlMail = await _context.MlMails.FindAsync(id);

            if (mlMail == null)
            {
                return NotFound();
            }

            return mlMail;
        }

        // PUT: api/MlMails/5
        [HttpPost("{id}")]
        public async Task<IActionResult> PutMlMail(int id, MlMail mlMail)
        {
            if (id != mlMail.Id)
            {
                return BadRequest();
            }

            _context.Entry(mlMail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MlMailExists(id))
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

        // POST: api/MlMails
        [HttpPost]
        public async Task<ActionResult<MlMail>> PostMlMail(MlMail mlMail)
        {
            _context.MlMails.Add(mlMail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MlMailExists(mlMail.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMlMail", new { id = mlMail.Id }, mlMail);
        }

        // DELETE: api/MlMails/5
        [HttpPost("delete/{id}")]
        public async Task<ActionResult<MlMail>> DeleteMlMail(int id)
        {
            var mlMail = await _context.MlMails.FindAsync(id);
            if (mlMail == null)
            {
                return NotFound();
            }

            _context.MlMails.Remove(mlMail);
            await _context.SaveChangesAsync();

            return mlMail;
        }

        private bool MlMailExists(int id)
        {
            return _context.MlMails.Any(e => e.Id == id);
        }
    }
}
