using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class LabelsController : ControllerBase
    {
        private readonly taskmanagerContext _context;

        public LabelsController(taskmanagerContext context)
        {
            _context = context;
        }

        // GET: api/Labels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Labels>>> GetLabels()
        {
            var userId = Convert.ToInt16(User.FindFirst("sub")?.Value);
            return await _context.Labels.Where(x => x.UserId == userId).ToListAsync();
        }

        // GET: api/Labels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Labels>> GetLabels(int id)
        {
            var labels = await _context.Labels.FindAsync(id);

            if (labels == null)
            {
                return NotFound();
            }

            return labels;
        }

        // PUT: api/Labels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLabels(int id, Labels labels)
        {
            if (id != labels.Id)
            {
                return BadRequest();
            }

            _context.Entry(labels).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabelsExists(id))
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

        // POST: api/Labels
        [HttpPost]
        public async Task<ActionResult<Labels>> PostLabels(Labels labels)
        {
            short userId = Convert.ToInt16(User.FindFirst("sub")?.Value);
            labels.UserId = userId;
            _context.Labels.Add(labels);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLabels", new { id = labels.Id }, labels);
        }

        // DELETE: api/Labels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Labels>> DeleteLabels(int id)
        {
            var labels = await _context.Labels.FindAsync(id);
            if (labels == null)
            {
                return NotFound();
            }

            _context.Labels.Remove(labels);
            await _context.SaveChangesAsync();

            return labels;
        }

        [HttpPut]
        [Route("attach")]
        public async Task<ActionResult<Labels>> AttachLabel([FromBody] LabelsTasks model)
        {
            var labelsTasks = new LabelsTasks
            {
                TasksId = model.TasksId,
                LabelsId = model.LabelsId
            };
             await _context.LabelsTasks.AddAsync(labelsTasks);
             var result = _context.SaveChangesAsync();

            if(result.Result == 1)
            {
                return Ok();
            }

            return BadRequest();

        }

        [HttpPut]
        [Route("detach")]
        public async Task<ActionResult<Labels>> DetachLabel([FromBody] LabelsTasks model)
        {

            var labelTask = new LabelsTasks
            {
                TasksId = model.TasksId,
                LabelsId = model.LabelsId
            };
    
            var connection = await _context.LabelsTasks.Where(x => x.TasksId == labelTask.TasksId && x.LabelsId == labelTask.LabelsId).FirstAsync();
            _context.LabelsTasks.Remove(connection);
            var result = _context.SaveChangesAsync();

            if (result.Result == 1)
            {
                return Ok();
            }

            return BadRequest();

        }

        private bool LabelsExists(int id)
        {
            return _context.Labels.Any(e => e.Id == id);
        }
    }
}
