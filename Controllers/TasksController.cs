using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taskmanagerBackendC.Models;

namespace taskmanagerBackendC.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly taskmanagerContext _context;

        public TasksController(taskmanagerContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetTasks()
        {
            var userId = Convert.ToInt16(User.FindFirst("sub")?.Value);

            var result = await _context.Tasks
                .Include(u => u.LabelsTasks)
                .ThenInclude(s => s.Labels)
                .Where(x => x.UserId == userId)
                .Where(t => t.Status == "working")
                .ToListAsync();

            return Ok(result);
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tasks>> GetTasks(int id)
        {
            var tasks = await _context.Tasks.FindAsync(id);

            if (tasks == null)
            {
                return NotFound();
            }

            return tasks;
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTasks(int id, Tasks tasks)
        {
            if (id != tasks.Id)
            {
                return BadRequest();
            }

            try
            {
                var task = await _context.Tasks.Where(x => x.Id == tasks.Id).FirstAsync();
                task.Status = tasks.Status;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasksExists(id))
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

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<Tasks>> PostTasks(Tasks tasks)
        {

            var userId = Convert.ToInt16(User.FindFirst("sub")?.Value);
            tasks.UserId = userId;
            _context.Tasks.Add(tasks);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTasks", new { id = tasks.Id }, tasks);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Tasks>> DeleteTasks(int id)
        {
            var tasks = await _context.Tasks.FindAsync(id);
            if (tasks == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(tasks);
            await _context.SaveChangesAsync();

            return tasks;
        }

        [HttpPut]
        [Route("addtogroup")]
        public async Task<ActionResult<GroupsTasks>> AddTaskToGroup([FromBody] GroupsTasks model)
        {
            var groupTask = new GroupsTasks
            {
                TasksId = model.TasksId,
                GroupsId = model.GroupsId
            };
            await _context.GroupsTasks.AddAsync(groupTask);
            var result = await _context.SaveChangesAsync();

            return Ok(result);
        }

        [HttpPut]
        [Route("removefromgroup")]
        public async Task<ActionResult<GroupsTasks>> RemoveTaskFromGroup([FromBody] GroupsTasks model)
        {

            var labelTask = new GroupsTasks
            {
                TasksId = model.TasksId,
                GroupsId = model.GroupsId
            };

            var connection = await _context.GroupsTasks.Where(x => x.TasksId == labelTask.TasksId && x.GroupsId == labelTask.GroupsId).FirstAsync();
            _context.GroupsTasks.Remove(connection);
            var result = _context.SaveChangesAsync();

            if (result.Result == 1)
            {
                return Ok();
            }

            return BadRequest();

        }

        private bool TasksExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
