using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bakend.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using modelos;

namespace bakend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class EntSalController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EntSalController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EntSal
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ent_sal>>> GetEntradasSalidas()
        {
            try
            {
                return await _context.EntradasSalidas.Where(x => x.active==true).ToListAsync();
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al obtener las Entradas/Salidas.");
            }
            
        }

        // GET: api/EntSal/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ent_sal>> Getent_sal(int id)
        {
            try
            {
                var ent_sal = await _context.EntradasSalidas.FindAsync(id);

            if (ent_sal == null || ent_sal.active==false)
            {
                return NotFound("No se encontro la Entrada/Salida");
            }

            return ent_sal;
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al obtener la Entrada/Salida.");
            }
            
        }

        // PUT: api/EntSal/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putent_sal(int id, ent_sal ent_sal)
        {
            if ((id != ent_sal.EntSalId ) || (ent_sal.active = false))
            {
                return BadRequest("Entrada/Salida no encontrada");
            }

            _context.Entry(ent_sal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ent_salExists(id))
                {
                    return NotFound("No se encontro la Entrada/Salida");
                }
                else
                {
                    ELog.Add(ex.ToString());
                    return NotFound("Ocurrio un error al actualizar la Entrada/Salida.");
                }
            }

            return Ok("Actualizado con exito");
        }

        // POST: api/EntSal
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ent_sal>> Postent_sal(ent_sal ent_sal)
        {
            try
            {
                _context.EntradasSalidas.Add(ent_sal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getent_sal", new { id = ent_sal.EntSalId }, ent_sal);
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al registrar la Entrada/Salida.");
            }
            
        }

        // DELETE: api/EntSal/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteent_sal(int id)
        {
            try
            {
                 var ent_sal = await _context.EntradasSalidas.FindAsync(id);
            if (ent_sal == null || ent_sal.active == false)
            {
                return NotFound("No se encontro la entrada/salida");
            }
            ent_sal.active = false;
            _context.EntradasSalidas.Update(ent_sal);
            await _context.SaveChangesAsync();

          return Ok("Eliminado con exito");
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al borrar la Entrada/Salida.");;
            }
           
        }

        private bool ent_salExists(int id)
        {
            return _context.EntradasSalidas.Any(e => e.EntSalId == id);
        }
    }
}
