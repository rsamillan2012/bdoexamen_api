using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Transactions;
using System.IO;
using Microsoft.Extensions.Configuration;
using BDO.Examen.Datos;
using BDO.Examen.Entidades;
using BDO.Examen01.Models;

namespace BDO.Examen01.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class FirmaDigitalController : ControllerBase
    {
        private readonly DbContextExamen _context;
        private readonly ILogger<FirmaDigitalController> _logger;
        private readonly IConfiguration _config;
        private IHttpContextAccessor _accessor;

        public FirmaDigitalController(DbContextExamen context, ILogger<FirmaDigitalController> logger, IConfiguration config, IHttpContextAccessor accessor)
        {
            _context = context;
            _logger = logger;
            _config = config;
            _accessor = accessor;

        }



        // POST: api/firmadigital/modificarfirma
        [HttpPut("[action]")]
        public async Task<IActionResult> ModificarFirma([FromBody] firmadigital_bind model)
        {
            try
            {
                var entidad = await _context.Firmadigital.FirstOrDefaultAsync(u => u.IdFirma == model.IdFirma);

                if (entidad == null)
                {
                    return NotFound();
                }

                entidad.TipoFirma = model.TipoFirma;
                entidad.RazonSocial = model.RazonSocial;
                entidad.RepresentanteLegal = model.RepresentanteLegal;
                entidad.EmpresaAcreditadora = model.EmpresaAcreditadora;

                if (model.RutaRubrica == null || !string.IsNullOrEmpty(model.RutaRubrica)) {
                    entidad.RutaRubrica = model.RutaRubrica;                    
                }

                if (model.CertificadoDigital == null || !string.IsNullOrEmpty(model.CertificadoDigital)) {
                    entidad.CertificadoDigital = model.CertificadoDigital;
                }
                
                entidad.FechaEmision = DateTime.Parse(model.FechaEmision);
                entidad.FechaVencimiento = DateTime.Parse(model.FechaVencimiento);

                _context.Entry(entidad).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error Opcion: " + ex.Message + ", pila : " + ex.StackTrace.ToString());
                return BadRequest();
            }
        }


        // POST: api/firmadigital/eliminarfirma
        [HttpPost("[action]")]
        public async Task<IActionResult> EliminarFirma([FromBody] firmadigital_bind model)
        {
            using (var transaction = _context.Database.BeginTransaction()) {
                try
                {
                    var entidad = await _context.Firmadigital.SingleOrDefaultAsync(x => x.IdFirma == model.IdFirma);

                    _context.Entry(entidad).State = EntityState.Deleted;
                    _context.Firmadigital.Remove(entidad);

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return Ok();
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    _logger.LogInformation("Error Opcion: " + ex.Message + ", pila : " + ex.StackTrace.ToString());
                    return BadRequest();
                }

            }
        }

        // POST: api/firmadigital/guardarfirma
        [HttpPost("[action]")]
        public async Task<IActionResult> GuardarFirma([FromBody] firmadigital_bind model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var ent = new Firmadigital();

                    ent.TipoFirma = model.TipoFirma;
                    ent.RazonSocial = model.RazonSocial;
                    ent.RepresentanteLegal = model.RepresentanteLegal;
                    ent.EmpresaAcreditadora = model.EmpresaAcreditadora;

                    ent.RutaRubrica = model.RutaRubrica;
                    ent.CertificadoDigital = model.CertificadoDigital;

                    ent.FechaEmision = DateTime.Parse(model.FechaEmision);
                    ent.FechaVencimiento = DateTime.Parse(model.FechaVencimiento);
 
                    _context.Entry(ent).State = EntityState.Added;
                    _context.Firmadigital.Add(ent);

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogInformation("Error Opcion: " + ex.Message + ", pila : " + ex.StackTrace.ToString());
                    return BadRequest();
                }
            }



        }


        // GET: api/firmadigital/getentidadfirma
        [HttpGet("[action]/{cod_firma}")]
        public async Task<ActionResult<firmadigital_bind>> GetEntidadFirma(int cod_firma)
        {
            try
            {
                string msg_comercio = string.Empty;
                string msg_cliente = string.Empty;

                var entidad = await _context.Firmadigital.SingleOrDefaultAsync(x => x.IdFirma == cod_firma);

                return new firmadigital_bind()
                {
                    IdFirma = entidad.IdFirma,
                    TipoFirma = entidad.TipoFirma,
                    RazonSocial = entidad.RazonSocial,
                    RepresentanteLegal = entidad.RepresentanteLegal,
                    EmpresaAcreditadora = entidad.EmpresaAcreditadora,
                    FechaEmision = (entidad.FechaEmision.HasValue) ? entidad.FechaEmision.Value.ToString("yyyy-MM-dd") : "",
                    FechaVencimiento = (entidad.FechaVencimiento.HasValue) ? entidad.FechaVencimiento.Value.ToString("yyyy-MM-dd") : "",
                    RutaRubrica = entidad.RutaRubrica,
                    CertificadoDigital = entidad.CertificadoDigital
                };

                //FechaEmision = (entidad.FechaEmision.HasValue) ? entidad.FechaEmision.Value.ToString("dd/MM/yyyy") : "",
                //FechaVencimiento = (entidad.FechaVencimiento.HasValue) ? entidad.FechaVencimiento.Value.ToString("dd/MM/yyyy") : "",


            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error Opcion: " + ex.Message + ", pila : " + ex.StackTrace.ToString());
                return BadRequest();
            }

        }



        // GET: api/firmadigital/listarfirmascriterio/0/criterio        
        [HttpGet("[action]/{indicePagina}/{criterio}")]
        public async Task<json_result_list<firmadigital_bind>> ListarFirmasCriterio(int indicePagina, string criterio)
        {
            //var numberFormatInfo = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            //numberFormatInfo.CurrencySymbol = "USD";

            try
            {
                int tamañoPagina = 10;


                var query = await _context.Firmadigital.Where(a => a.RazonSocial.Contains(criterio))
                                                    .OrderByDescending(z => z.IdFirma)
                                                    .Skip((indicePagina - 1) * (tamañoPagina)).Take(tamañoPagina)
                                                    .ToListAsync();

                var _total = await _context.Firmadigital.Where(a => a.RazonSocial.Contains(criterio)).ToListAsync();

                IEnumerable<firmadigital_bind> _result = query.Select(p => new firmadigital_bind
                {
                    IdFirma = p.IdFirma,
                    TipoFirma = (p.TipoFirma == "1") ? "Firma Digital" :
                                (p.TipoFirma == "2") ? "Rubrica Escaneada" :
                                (p.TipoFirma == "3") ? "Firma Eletrónica" : "",
                    RazonSocial = p.RazonSocial,
                    RepresentanteLegal = p.RepresentanteLegal,
                    EmpresaAcreditadora = p.EmpresaAcreditadora,
                    FechaEmision = (p.FechaEmision.HasValue) ? p.FechaEmision.Value.ToString("dd/MM/yyyy") : "",
                    FechaVencimiento = (p.FechaVencimiento.HasValue) ? p.FechaVencimiento.Value.ToString("dd/MM/yyyy") : "",
                });

                decimal _lastpage = 0;

                if (_total.Count() % tamañoPagina == 0)
                {
                    _lastpage = _total.Count() / tamañoPagina;
                }
                else
                {
                    _lastpage = (_total.Count() / tamañoPagina) + 1;
                }

                var _pagination = new pagination_bind()
                {
                    total = _total.Count(),
                    current_page = indicePagina,
                    per_page = tamañoPagina,
                    last_page = Math.Ceiling(_lastpage),
                    from = 0,
                    to = tamañoPagina
                };

                return new json_result_list<firmadigital_bind>()
                {
                    result = _result.AsEnumerable(),
                    pagination = _pagination
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error Opcion: " + ex.Message + ", pila : " + ex.StackTrace.ToString());
                throw;
            }
        }

        // GET: api/firmadigital/listarfirmas/0        
        [HttpGet("[action]/{indicePagina}")]
        public async Task<json_result_list<firmadigital_bind>> ListarFirmas(int indicePagina)
        {
            //var numberFormatInfo = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            //numberFormatInfo.CurrencySymbol = "USD";

            try
            {
                int tamañoPagina = 10;


                var query = await _context.Firmadigital.OrderByDescending(z => z.IdFirma)
                                                    .Skip((indicePagina - 1) * (tamañoPagina)).Take(tamañoPagina)
                                                    .ToListAsync();

                var _total = await _context.Firmadigital.ToListAsync();

                IEnumerable<firmadigital_bind> _result = query.Select(p => new firmadigital_bind
                {
                    IdFirma = p.IdFirma,
                    TipoFirma = (p.TipoFirma == "1") ? "Firma Digital" :
                                (p.TipoFirma == "2") ? "Rubrica Escaneada" :
                                (p.TipoFirma == "3") ? "Firma Eletrónica" : "",
                    RazonSocial = p.RazonSocial,
                    RepresentanteLegal = p.RepresentanteLegal,
                    EmpresaAcreditadora = p.EmpresaAcreditadora,
                    FechaEmision = (p.FechaEmision.HasValue) ? p.FechaEmision.Value.ToString("dd/MM/yyyy") : "",
                    FechaVencimiento = (p.FechaVencimiento.HasValue) ? p.FechaVencimiento.Value.ToString("dd/MM/yyyy") : "",
                });

                decimal _lastpage = 0;

                if (_total.Count() % tamañoPagina == 0)
                {
                    _lastpage = _total.Count() / tamañoPagina;
                }
                else
                {
                    _lastpage = (_total.Count() / tamañoPagina) + 1;
                }

                var _pagination = new pagination_bind()
                {
                    total = _total.Count(),
                    current_page = indicePagina,
                    per_page = tamañoPagina,
                    last_page = Math.Ceiling(_lastpage),
                    from = 0,
                    to = tamañoPagina
                };

                return new json_result_list<firmadigital_bind>()
                {
                    result = _result.AsEnumerable(),
                    pagination = _pagination
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error Opcion: " + ex.Message + ", pila : " + ex.StackTrace.ToString());
                throw;
            }

        }



        // GET: api/firmadigital/listarfirmastodo
        [HttpGet("[action]")]
        public async Task<json_result_list<firmadigital_bind>> ListarFirmasTodo()
        {
            //var numberFormatInfo = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            //numberFormatInfo.CurrencySymbol = "USD";

            try
            {

                var query = await _context.Firmadigital.OrderByDescending(z => z.IdFirma)
                                                    .ToListAsync();

                var _total = await _context.Firmadigital.ToListAsync();

                IEnumerable<firmadigital_bind> _result = query.Select(p => new firmadigital_bind
                {
                    IdFirma = p.IdFirma,
                    TipoFirma = (p.TipoFirma == "1") ? "Firma Digital" :
                                (p.TipoFirma == "2") ? "Rubrica Escaneada" :
                                (p.TipoFirma == "3") ? "Firma Eletrónica" : "",
                    RazonSocial = p.RazonSocial,
                    RepresentanteLegal = p.RepresentanteLegal,
                    EmpresaAcreditadora = p.EmpresaAcreditadora,
                    FechaEmision = (p.FechaEmision.HasValue) ? p.FechaEmision.Value.ToString("dd/MM/yyyy") : "",
                    FechaVencimiento = (p.FechaVencimiento.HasValue) ? p.FechaVencimiento.Value.ToString("dd/MM/yyyy") : "",
                });

                
                return new json_result_list<firmadigital_bind>()
                {
                    result = _result.AsEnumerable(),
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error Opcion: " + ex.Message + ", pila : " + ex.StackTrace.ToString());
                throw;
            }

        }



    }
}
