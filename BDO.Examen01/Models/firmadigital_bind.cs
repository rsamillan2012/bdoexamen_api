using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDO.Examen01.Models
{
    public class firmadigital_bind
    {
        public long IdFirma { get; set; }
        public string TipoFirma { get; set; }
        public string RazonSocial { get; set; }
        public string RepresentanteLegal { get; set; }
        public string EmpresaAcreditadora { get; set; }
        public string FechaEmision { get; set; }
        public string FechaVencimiento { get; set; }
        public string RutaRubrica { get; set; }
        public string CertificadoDigital { get; set; }
    }
}
