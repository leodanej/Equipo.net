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
    public class MovimientoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MovimientoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Movimiento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<movimiento>>> Getmovimientos()
        {
            try
            {
                return await _context.movimientos.ToListAsync();
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al obtener los movimientos.");
            }
            
        }

        // GET: api/Movimiento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<movimiento>> Getmovimiento(int id)
        {
            try
            {
                 var movimiento = await _context.movimientos.FindAsync(id);

            if (movimiento == null)
            {
                return NotFound();
            }

            return movimiento;
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al obtener el movimiento.");
            }
           
        }

        // PUT: api/Movimiento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putmovimiento(int id, movimiento movimiento)
        {
            if ((id != movimiento.MovId) || (movimiento.active = false))
            {
                return BadRequest("No existe el movimiento");
            }

            _context.Entry(movimiento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!movimientoExists(id))
                {
                    return NotFound("No se encontro en la base de datos");
                }
                else
                {
                    ELog.Add(ex.ToString());
                    return NotFound("Ocurrio un error al actualizar el movimiento.");;
                }
            }

            return Ok("Actualizado con exito");
        }

        // POST: api/Movimiento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<movimiento>> Postmovimiento(movimiento movimiento)
        {
            try
            {
                _context.movimientos.Add(movimiento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getmovimiento", new { id = movimiento.MovId }, movimiento);
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al registrar el movimiento. (Verifica el usuario y recurso registrado)");
            }
            
        }

        // DELETE: api/Movimiento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletemovimiento(int id)
        {
            try
            {
                var movimiento = await _context.movimientos.FindAsync(id);
            if (movimiento == null)
            {
                return NotFound();
            }

            _context.movimientos.Remove(movimiento);
            await _context.SaveChangesAsync();

            return Ok("Eliminado con exito");
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al borrar el movimiento.");;
            }
            
        }

      
        private bool movimientoExists(int id)
        {
            return _context.movimientos.Any(e => e.MovId == id);
        }
    }
}
