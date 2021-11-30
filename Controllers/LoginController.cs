using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bakend.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using modelos;
using Tools;


namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
      
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        public LoginController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        
        [NonAction]
   public async Task<ActionResult<IEnumerable<usuario>>> GetAllAsyn()
        {
            try
            {
               return await _context.usuarios.ToListAsync(); 
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                throw;
            }
            
        }
        [HttpPost]

       
        public async Task<IActionResult> Post([FromBody]usuario usuario)
        {
            try
            {
                usuario.password = Encrypt.GetSHA256(usuario.password);

                 var user = (await _context.Usuario.SingleOrDefaultAsync(x => x.correo == usuario.correo 
                                                                         && x.password == usuario.password 
                                                                         && x.active==true  
                 ));
                
                //var user = await _context.usuarios.SingleAsync(b => b.correo == usuario.correo && b.password == usuario.password);
                if(user == null)
                {
                    return BadRequest(new { message = "Usuario o contraseña invalidos" });
                }
                string tokenString = JwtConfigurator.GetToken(user, _config);
                return Ok(new { 
                   token = tokenString
                    });
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
    }
}
