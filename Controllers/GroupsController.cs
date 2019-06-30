using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using taskmanagerBackendC.Models;

namespace taskmanagerBackendC.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly taskmanagerContext _context;

        public GroupsController(taskmanagerContext context)
        {
            _context = context;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Groups>>> GetGroups()
        {
            var _userId = Convert.ToInt16(User.FindFirst("sub")?.Value);
            var result = await _context.Groups.FromSql(
                "SELECT * FROM taskmanager.groups WHERE taskmanager.groups.id NOT IN (SELECT groups_id FROM taskmanager.groups_user WHERE user_id = @userId) ", new MySqlParameter("@userId", _userId))
                .Select(group => new Groups
                {
                    Id = group.Id,
                    Name = group.Name,
                    Description = group.Description,
                    AdminId = group.AdminId,
                }).ToListAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("user")]
        public async Task<ActionResult<IEnumerable<Groups>>> GetUserGroups()
        {
            var _userId = Convert.ToInt16(User.FindFirst("sub")?.Value);
            var result = await _context.Groups.FromSql(
                "SELECT * FROM taskmanager.groups WHERE taskmanager.groups.id IN (SELECT groups_id FROM taskmanager.groups_user WHERE user_id = @userId) ", new MySqlParameter("@userId", _userId))
                .Select(group => new Groups
                {
                    Id = group.Id,
                    Name = group.Name,
                    Description = group.Description,
                    AdminId = group.AdminId,
                }).ToListAsync();

            return Ok(result);
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Groups>> GetGroups(int id)
        {
            var groups = await _context.Groups.FindAsync(id);

            if (groups == null)
            {
                return NotFound();
            }

            return groups;
        }

        // PUT: api/Groups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroups(int id, Groups groups)
        {
            if (id != groups.Id)
            {
                return BadRequest();
            }

            _context.Entry(groups).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupsExists(id))
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

        // POST: api/Groups
        [HttpPost]
        public async Task<ActionResult<Groups>> PostGroups(Groups groups)
        {
            var _userId = Convert.ToInt16(User.FindFirst("sub")?.Value);
            groups.AdminId = _userId;
            _context.Groups.Add(groups);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroups", new { id = groups.Id }, groups);
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Groups>> DeleteGroups(int id)
        {
            var groups = await _context.Groups.FindAsync(id);
            if (groups == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(groups);
            await _context.SaveChangesAsync();

            return groups;
        }

        [HttpPut]
        [Route("adduser")]
        public async Task<ActionResult<Groups>> AddUser([FromBody] GroupsUser model)
        {
            var _userId = Convert.ToInt16(User.FindFirst("sub")?.Value);
            var groupsUser = new GroupsUser
            {
                GroupsId = model.GroupsId,
                UserId = _userId
            };
            await _context.GroupsUser.AddAsync(groupsUser);
            var result = _context.SaveChangesAsync();

            if (result.Result == 1)
            {
                return Ok();
            }

            return BadRequest();

        }

        [HttpPut]
        [Route("removeuser")]
        public async Task<ActionResult<Groups>> RemoveUser([FromBody] GroupsUser model)
        {
            var _userId = Convert.ToInt16(User.FindFirst("sub")?.Value);
            var groupsUser = new GroupsUser
            {
                GroupsId = model.GroupsId,
                UserId = _userId
            };

            var connection = await _context.GroupsUser.Where(x => x.GroupsId == groupsUser.GroupsId && x.UserId == groupsUser.UserId).FirstAsync();
            _context.GroupsUser.Remove(connection);
            var result =  _context.SaveChangesAsync();

            if (result.Result == 1)
            {
                return Ok();
            }

            return BadRequest();

        }


        private bool GroupsExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
