using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taskmanagerBackendC.Models;

namespace taskmanagerBackendC.Controllers
{
    [Route("api/[controller]")]
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
            return await _context.Labels.ToListAsync();
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

        private bool LabelsExists(int id)
        {
            return _context.Labels.Any(e => e.Id == id);
        }
    }
}
