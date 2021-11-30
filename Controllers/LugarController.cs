using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bakend.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using modelos;

namespace bakend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class LugarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LugarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Lugar
        [HttpGet]
        public async Task<ActionResult<IEnumerable<lugar>>> Getlugares()
        {
            try
            {
              return await _context.lugares.Where(x => x.active==true).ToListAsync();  
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al obtener los lugares.");
            }
            
        }

        // GET: api/Lugar/5
        [HttpGet("{id}")]
        public async Task<ActionResult<lugar>> Getlugar(int id)
        {
            var lugar = await _context.lugares.FindAsync(id);
            try
            {
                if (lugar == null || lugar.active==false)
            {
                return NotFound("No se encontro el lugar");
            }

            return lugar;
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al obtener el lugar.");
            }
            
        }

        // PUT: api/Lugar/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putlugar(int id, lugar lugar)
        {
            try
            {
                 if ((id != lugar.LugarId ) || (lugar.active = false))
            {
                return BadRequest("Lugar no encontrado");
            }

            _context.Entry(lugar).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al actualizar el lugar.");
            }
           

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!lugarExists(id))
                {
                    return NotFound("No se encontro el lugar");
                }
                else
                {
                    ELog.Add(ex.ToString());
                    return NotFound("Ocurrio un error al actualizar  el lugar en la base de datos.");
                }
            }

            return Ok("Actualizado con exito");
        }

        // POST: api/Lugar
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<lugar>> Postlugar(lugar lugar)
        {

            try
            {
                _context.lugares.Add(lugar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getlugar", new { id = lugar.LugarId }, lugar);
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al registrar el lugar.");;
            }
            
        }

        // DELETE: api/Lugar/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletelugar(int id)
        {
            try
            {
                var lugar = await _context.lugares.FindAsync(id);
            if (lugar == null || lugar.active == false)
            {
                return NotFound("No se encontro el lugar");
            }
            lugar.active = false;
            _context.lugares.Update(lugar);
            await _context.SaveChangesAsync();

           return Ok("Eliminado con exito");
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al borrar el lugar.");
            }
            
        }

        private bool lugarExists(int id)
        {
            return _context.lugares.Any(e => e.LugarId == id);
        }
    }
}
