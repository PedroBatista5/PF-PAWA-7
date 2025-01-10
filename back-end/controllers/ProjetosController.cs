using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjetosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Projetos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Projetos>>> GetProjetos()
        {
            return await _context.Projetos.Include(p => p.InfoUsers).ToListAsync();
        }

        // GET: api/Projetos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Projetos>> GetProjetos(int id)
        {
            var projetos = await _context.Projetos
                .Include(p => p.InfoUsers)
                .FirstOrDefaultAsync(p => p.Id_projetos == id);

            if (projetos == null)
            {
                return NotFound();
            }

            return projetos;
        }

        // PUT: api/Projetos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjetos(int id, Projetos projetos)
        {
            if (id != projetos.Id_projetos)
            {
                return BadRequest();
            }

            _context.Entry(projetos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjetosExists(id))
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

        // POST: api/Projetos
        [HttpPost]
        public async Task<ActionResult<Projetos>> PostProjetos(Projetos projetos)
        {
            _context.Projetos.Add(projetos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjetos", new { id = projetos.Id_projetos }, projetos);
        }

        // DELETE: api/Projetos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Projetos>> DeleteProjetos(int id)
        {
            var projetos = await _context.Projetos.FindAsync(id);
            if (projetos == null)
            {
                return NotFound();
            }

            _context.Projetos.Remove(projetos);
            await _context.SaveChangesAsync();

            return projetos;
        }

        private bool ProjetosExists(int id)
        {
            return _context.Projetos.Any(e => e.Id_projetos == id);
        }
    }
}
