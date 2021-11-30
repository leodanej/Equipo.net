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
    public class GeolugController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GeolugController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Geolug
        [HttpGet]
        public async Task<ActionResult<IEnumerable<geolug>>> Getgeolugares()
        {
            try
            {
              return await _context.geolugares.Where(x => x.active==true).ToListAsync();  
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al obtener los Geolugares.");
            }
            
        }

        // GET: api/Geolug/5
        [HttpGet("{id}")]
        public async Task<ActionResult<geolug>> Getgeolug(int id)
        {
            var geolug = await _context.geolugares.FindAsync(id);
            try
            {
                if (geolug == null || geolug.active==false)
            {
                return NotFound("No se encontro el GeoLugar");
            }

            return geolug;
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al obtener el Geolugar.");
            }
            
        }

        // PUT: api/Geolug/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putgeolug(int id, geolug geolug)
        {
            if ((id != geolug.GLId ) || (geolug.active = false))
            {
                return BadRequest("GeoLugar no encontrado");
            }

            _context.Entry(geolug).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!geolugExists(id))
                {
                    return NotFound("No se encontro el Geolugar");
                }
                else
                {
                    ELog.Add(ex.ToString());
                    return NotFound("Ocurrio un error al actualizar el Geolugar.");;
                }
            }

            return Ok("Actualizado con exito");
        }

        // POST: api/Geolug
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<geolug>> Postgeolug(geolug geolug)
        {
            try
            {
              _context.geolugares.Add(geolug);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getgeolug", new { id = geolug.GLId }, geolug);  
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al registrar el Geolugar.");;
            }
            
        }

        // DELETE: api/Geolug/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletegeolug(int id)
        {
            try
            {
                var geolug = await _context.geolugares.FindAsync(id);
            if (geolug == null || geolug.active == false)
            {
                return NotFound("No se encontro el Geolugar");
            }
             geolug.active = false;
            _context.geolugares.Update(geolug);
            await _context.SaveChangesAsync();

            return Ok("Eliminado con exito");
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al eliminar el Geolugar.");;
            }
            
        }

        private bool geolugExists(int id)
        {
            return _context.geolugares.Any(e => e.GLId == id);
        }
    }
}
