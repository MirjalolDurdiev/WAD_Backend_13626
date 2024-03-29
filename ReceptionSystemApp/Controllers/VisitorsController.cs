﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceptionSystemApp.Data;
using ReceptionSystemApp.Models;

namespace ReceptionSystemApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorsController : ControllerBase
    {
        private readonly ReceptionDbContext _dbContext;

        public VisitorsController(ReceptionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Visitors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Visitor>>> GetVisitors()
        {
            return await _dbContext.Visitors.ToListAsync();
        }

        // GET: api/Visitors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Visitor>> GetVisitor(int id)
        {
            var visitor = await _dbContext.Visitors.FindAsync(id);

            if (visitor == null)
            {
                return NotFound();
            }

            return visitor;
        }

        // PUT: api/Visitors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVisitor(int id, Visitor visitor)
        {
            if (id != visitor.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(visitor).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitorExists(id))
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

        // POST: api/Visitors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Visitor>> PostVisitor(Visitor visitor)
        {
            if (_dbContext.Visitors.Any(v => v.Email == visitor.Email))
            {
                return Conflict("Visitor with this email already exists.");
            }
            _dbContext.Visitors.Add(visitor);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetVisitor", new { id = visitor.Id }, visitor);
        }

        // DELETE: api/Visitors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisitor(int id)
        {
            var visitor = await _dbContext.Visitors.FindAsync(id);
            if (visitor == null)
            {
                return NotFound();
            }

            _dbContext.Visitors.Remove(visitor);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool VisitorExists(int id)
        {
            return _dbContext.Visitors.Any(e => e.Id == id);
        }
    }
}
