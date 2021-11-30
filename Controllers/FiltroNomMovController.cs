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
    public class FiltroNomMovController: ControllerBase{
           private readonly ApplicationDbContext _context;

        public FiltroNomMovController(ApplicationDbContext context)
        {
            _context = context;
            
        } 
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable <movimiento>>> GetResult([FromQuery] string busqueda){

           try
           {
               Console.WriteLine(busqueda);
               return await _context.movimientos.Where(x => x.Usuario.nombre.Contains(busqueda)).ToListAsync();
           }
           catch (Exception ex)
           {
                ELog.Add(ex.ToString());
                return NotFound("No se encontro");
           }
             
              
        }
          
    }
}
     


      


 