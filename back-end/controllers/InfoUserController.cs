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
    public class InfoUserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InfoUserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/InfoUser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InfoUser>>> GetInfoUsers()
        {
            return await _context.InfoUsers.Include(i => i.Projetos).Include(i => i.Utilizadores).ToListAsync();
        }

        // GET: api/InfoUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InfoUser>> GetInfoUser(int id)
        {
            var infoUser = await _context.InfoUsers
                .Include(i => i.Projetos)
                .Include(i => i.Utilizadores)
                .FirstOrDefaultAsync(i => i.Id_info == id);

            if (infoUser == null)
            {
                return NotFound();
            }

            return infoUser;
        }

        // PUT: api/InfoUser/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInfoUser(int id, InfoUser infoUser)
        {
            if (id != infoUser.Id_info)
            {
                return BadRequest();
            }

            _context.Entry(infoUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InfoUserExists(id))
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

        // POST: api/InfoUser
        [HttpPost]
        public async Task<ActionResult<InfoUser>> PostInfoUser(InfoUser infoUser)
        {
            _context.InfoUsers.Add(infoUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInfoUser", new { id = infoUser.Id_info }, infoUser);
        }

        // DELETE: api/InfoUser/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InfoUser>> DeleteInfoUser(int id)
        {
            var infoUser = await _context.InfoUsers.FindAsync(id);
            if (infoUser == null)
            {
                return NotFound();
            }

            _context.InfoUsers.Remove(infoUser);
            await _context.SaveChangesAsync();

            return infoUser;
        }

        private bool InfoUserExists(int id)
        {
            return _context.InfoUsers.Any(e => e.Id_info == id);
        }
    }
}
