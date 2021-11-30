using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using modelos;
using bakend.DTO;
using Tools;
using System.Security.Claims;
using bakend.Tools;

namespace bakend.Controllers
{


    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        [NonAction]
        public async Task SaveUser(usuario usuario)
        {
            _context.Add(usuario);
            await _context.SaveChangesAsync();
        }
        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<usuario>>> Getusuarios()
        {
            try
            {
                
                return await _context.usuarios.Where(x => x.active==true).ToListAsync();
            }
            catch (Exception ex)
            {
                
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al obtener la lista de usuarios.");
            }

        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<usuario>> Getusuario(int id)
        {
            try
            {
                var usuario = await _context.usuarios.FindAsync(id);

                if (usuario == null || usuario.active==false)
                {
                    return NotFound("No existe el usuario");
                }

                return usuario;
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                  return NotFound("Ocurrio un error al obtener al usuario.");;
            }

        }

        // PUT: api/Usuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putusuario(int id, usuario usuario)
        {
            if (id != usuario.Usuarioid)
            {
                return BadRequest("Usuario no encontrado");
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                string hpass = Encrypt.GetSHA256(usuario.password);
                usuario.password = (hpass);
                await _context.SaveChangesAsync();
            
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!usuarioExists(id))
                {
                    return NotFound("Usuario no encontrado");
                }
                else
                {
                     ELog.Add(ex.ToString());
                      return NotFound("Ocurrio un error al actualizar al usuario.");;
                }
            }

            
            return Ok("Actualizado con exito");
        }
        //[Route("CambiarPassowrd")]
        [HttpPut("cambiarpass")]
        public async Task<IActionResult> CambiarPassword([FromBody] CambiarPasswordDTO cambiarPassword)
        {

            try
            {
                var Identity = HttpContext.User.Identity as ClaimsIdentity;
                int idUsuario = JwtConfigurator.GetTokenIdUsuario(Identity);
                string passwordEncriptado = Encrypt.GetSHA256(cambiarPassword.passwordAnterior);

                //var usuario = await _context.Usuario.Where(x => x.Usuarioid == idUsuario && x.password == passwordEncriptado).FirstOrDefaultAsync();
                var usuario = await _context.Usuario.FindAsync(idUsuario);

                if ((usuario == null) || (usuario.password != passwordEncriptado))
                {
                    return BadRequest(new { message = "La password es incorrecto " });
                }
                else
                {
                    usuario.password = Encrypt.GetSHA256(cambiarPassword.nuevaPassword);
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "El password fue actualizado con exito!" });

                }

            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("Ocurrio un error al cambiar el password");
            }
        }
        // POST: api/Usuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<usuario>> Postusuario([FromBody] usuario usuario)
        {
            try
            {

                var validateExistence = await _context.Usuario.AnyAsync(x => x.correo == usuario.correo); ;
                if (validateExistence)
                {
                    return BadRequest(new { message = "El usuario " + usuario.correo + " ya existe!" });
                }
                string hpass = Encrypt.GetSHA256(usuario.password);
                //string hpass = BCrypt.Net.BCrypt.HashPassword(usuario.password);
                usuario.password = (hpass);
                _context.usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return CreatedAtAction("Getusuario", new { id = usuario.Usuarioid }, usuario);
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                  return NotFound("Ocurrio un error al actualizar el usuario.");;
            }

        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteusuario(int id)
        {

            try
            {
                var usuario = await _context.usuarios.FindAsync(id);
            if (usuario == null || usuario.active == false)
            {
                return NotFound("El usuario no existe. No se hace nada");
            }

            usuario.active = false;
            _context.usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return Ok("Eliminado con exito");
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                  return NotFound("Ocurrio un error al borrar un usuario.");
            }
            
        }

        private bool usuarioExists(int id)
        {
            return _context.usuarios.Any(e => e.Usuarioid == id);
        }
    }
}
