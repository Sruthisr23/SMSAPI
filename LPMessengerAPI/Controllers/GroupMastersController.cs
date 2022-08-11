using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LPMessengerAPI.Model;
using Microsoft.AspNetCore.Authorization;
using LPMessengerAPI.Service.Interface;

namespace LPMessengerAPI.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class GroupMastersController : ControllerBase
    {
        private readonly lpmessengerdbContext _context;
        private readonly IUserService _userService;
        private readonly IGroupMasterService _groupMasterService;

        public GroupMastersController(lpmessengerdbContext context, IUserService userService, IGroupMasterService groupMasterService)
        {
            _context = context;
            _userService = userService;
            _groupMasterService = groupMasterService;
        }
         
        // GET: api/GroupMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupMaster>>> GetGroupMasters(int userId,string token)
        {
            var _token = await _userService.GetToken(userId);
            if (_token != token)
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }
            return await _context.GroupMasters.ToListAsync();
        }


        [HttpGet("ByServiceId")]
        public async Task<ActionResult<IEnumerable<GroupMaster>>> GetGroupBasedOnServiceId(int serviceId, int userId, string token)
        {
            var _token = await _userService.GetToken(userId);
            if (_token != token)
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            return await _groupMasterService.GetGroupBasedOnServiceId(serviceId);
        }
        // GET: api/GroupMasters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupMaster>> GetGroupMaster(int id, int userId, string token)
        {
            var _token = await _userService.GetToken(userId);
            if (_token != token)
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            var groupMaster = await _context.GroupMasters.FindAsync(id);

            if (groupMaster == null)
            {
                return NotFound();
            }

            return groupMaster;
        }

        // PUT: api/GroupMasters/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{id}")]
        public async Task<IActionResult> PutGroupMaster(int id, int userId, string token, GroupMaster groupMaster)
        {
            if (id != groupMaster.Id)
            {
                return BadRequest();
            }

            var _token = await _userService.GetToken(userId);
            if (_token != token)
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            _context.Entry(groupMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupMasterExists(id))
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

        // POST: api/GroupMasters
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GroupMaster>> PostGroupMaster(GroupMaster groupMaster,int userId, string token)
        {
            var _token = await _userService.GetToken(userId);
            if (_token != token)
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }
            _context.GroupMasters.Add(groupMaster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupMaster", new { id = groupMaster.Id }, groupMaster);
        }

        // DELETE: api/GroupMasters/5
        [HttpPost("delete/{id}")]
        public async Task<ActionResult<GroupMaster>> DeleteGroupMaster(int id, int userId, string token)
        {
            var _token = await _userService.GetToken(userId);
            if (_token != token)
            {
                return Unauthorized(System.Net.HttpStatusCode.Unauthorized);
            }

            var groupMaster = await _context.GroupMasters.FindAsync(id);
            if (groupMaster == null)
            {
                return NotFound();
            }

            _context.GroupMasters.Remove(groupMaster);
            await _context.SaveChangesAsync();

            return groupMaster;
        }

        private bool GroupMasterExists(int id)
        {
            return _context.GroupMasters.Any(e => e.Id == id);
        }
    }
}
