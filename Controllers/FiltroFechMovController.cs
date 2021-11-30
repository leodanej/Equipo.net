
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

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class FiltroFechMovController : ControllerBase{
        private readonly ApplicationDbContext _context;

        public FiltroFechMovController(ApplicationDbContext context){
            _context = context;
        }

        [HttpGet("/api/ffechIni")]
        public async Task<ActionResult<IEnumerable <movimiento>>> GetResultIni([FromQuery] string busqueda){
            var fecha = DateTime.Parse(busqueda);
          try
          {
              return await _context.movimientos.Where(x => x.FInicio.Year == fecha.Year
            && x.FInicio.Month == fecha.Month
            && x.FInicio.Day == fecha.Day).ToListAsync();
          }
          catch (Exception ex)
          {
               ELog.Add(ex.ToString());
                return NotFound("No se encontro");
          }
            
        }
        [HttpGet("/api/ffechFin")]

        public async Task<ActionResult<IEnumerable <movimiento>>> GetResultFin([FromQuery] string busqueda){
            var fecha = DateTime.Parse(busqueda);
            
            try
            {
                 return await _context.movimientos.Where(x => x.FFin.Year == fecha.Year
            && x.FFin.Month == fecha.Month
            && x.FFin.Day == fecha.Day).ToListAsync();
            }
            catch (Exception ex)
            {
                
                ELog.Add(ex.ToString());
                return NotFound("No se encontro");
            }

            
           
        }

    }
}