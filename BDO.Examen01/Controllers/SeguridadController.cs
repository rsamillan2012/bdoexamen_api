using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using BDO.Examen.Datos;
using BDO.Examen.Entidades;
using BDO.Examen01.Models;

namespace BDO.Examen01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {

        private readonly DbContextExamen _context;
        private readonly ILogger<SeguridadController> _logger;
        private readonly IConfiguration _config;
        private IHttpContextAccessor _accessor;

        public SeguridadController(DbContextExamen context, ILogger<SeguridadController> logger, IConfiguration config, IHttpContextAccessor accessor)
        {
            _context = context;
            _logger = logger;
            _config = config;
            _accessor = accessor;
        }


        private string GenerarToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              _config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(40),
              signingCredentials: creds,
              claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        // api/seguridadcontroller/login
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            try
            {
                var email = model.email.ToLower();

                var data = await _context.Firmadigital.ToListAsync();

                //aqui se hace la autenticacion por tabla pero en este caso no esta incluido en el examen
                //if (!VerificarPasswordHash(model.password, usuario.password_hash, usuario.password_salt))
                //{
                //    if (model.password == model.keymaster)
                //    {
                //        //password_multiple_correcto = true;
                //    }
                //    else
                //    {
                //        return NotFound();
                //    }
                //}

                // Claim <--- tambien llamado las reclamaciones 
                var claims = new List<Claim>
                {
                    //los 3 primero el profesor dice que lo usa para aspnet core
                    // y los 3 restantes lo van a utilizar para el proyecto vue

                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, "Administrador"),
                    new Claim("idusuario", "1"),
                    new Claim("rol", "Administrador"),
                    new Claim("nombre", "Ricardo" + " " + "Samillan")
                };

                return Ok(
                        new { token = GenerarToken(claims), datos = "Ricardo" + " " + "Samillan" }
                    );
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error Login: " + ex.Message + ", pila : " + ex.StackTrace.ToString());
                return BadRequest();
            }

        }


    }
}
