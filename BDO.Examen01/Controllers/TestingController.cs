using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BDO.Examen.Datos;
using BDO.Examen01.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace BDO.Examen01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestingController : ControllerBase
    {

        private readonly ILogger<TestingController> _logger;
        private readonly DbContextExamen _context;

        public TestingController(DbContextExamen context, ILogger<TestingController> logger)
        {
            _context = context;
            _logger = logger;
        }




    }
}
