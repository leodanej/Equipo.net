
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
    public class FiltroEqMovController: ControllerBase{
           private readonly ApplicationDbContext _context;

        public FiltroEqMovController(ApplicationDbContext context)
        {
            _context = context;
            
        } 
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable <movimiento>>> GetResult([FromQuery] string busqueda){

            try
            {
                return await _context.movimientos.Where(x => x.Recurso.nombre == busqueda).ToListAsync();
            }
            catch (Exception ex)
            {
                ELog.Add(ex.ToString());
                return NotFound("No se encontro");
    
            }
            
              
        }
           
    }
}
     


      


 