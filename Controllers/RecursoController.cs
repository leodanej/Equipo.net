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
    public class RecursoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecursoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Recurso
        [HttpGet]
        public async Task<ActionResult<IEnumerable<recurso>>> Getrecursos()
        {
            try
            {
                return await _context.recursos.Where(x => x.active==true).ToListAsync();
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al obtener la lista de recursos.");;
            }

        }

        // GET: api/Recurso/5
        [HttpGet("{id}")]
        public async Task<ActionResult<recurso>> Getrecurso(int id)
        {

            try
            {
                var recurso = await _context.recursos.FindAsync(id);
                if (recurso == null || recurso.active==false)
                {
                    return NotFound("No se encontro el recurso");
                }

                return recurso;
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                  return NotFound("Ocurrio un error al obtener el recurso.");
            }

        }

        // PUT: api/Recurso/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putrecurso(int id, recurso recurso)
        {
            if ((id != recurso.RecursoId))
            {
                return BadRequest("Registro no encontrado");
            }

            _context.Entry(recurso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!recursoExists(id))
                {
                    return NotFound("No se encontro el recurso");
                }
                else
                {
                     ELog.Add(ex.ToString());
                    return NotFound("Ocurrio un error al actualizar el recurso.");
                }
            }

            return Ok("Actualizado con exito");
        }

        // POST: api/Recurso
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<recurso>> Postrecurso(recurso recurso)
        {
            try
            {
                 _context.recursos.Add(recurso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getrecurso", new { id = recurso.RecursoId }, recurso);
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al registrar el recurso.");;
            }
           
        }

        // DELETE: api/Recurso/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleterecurso(int id)
        {
            try
            {
                 var recurso = await _context.recursos.FindAsync(id);
            if (recurso == null || recurso.active == false)
            {
                return NotFound("No se encontro el recurso");
            }
             recurso.active = false;
            _context.recursos.Update(recurso);
            await _context.SaveChangesAsync();

            return Ok("Eliminado con exito");
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al borrar el recurso.");;
            }
           
        }

        private bool recursoExists(int id)
        {
            return _context.recursos.Any(e => e.RecursoId == id);
        }
    }
}
