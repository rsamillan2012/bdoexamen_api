using System;
using System.Collections.Generic;
using System.Text;

namespace BDO.Examen.Entidades
{
    public partial class Firmadigital
    {
        public long IdFirma { get; set; }
        public string TipoFirma { get; set; }
        public string RazonSocial { get; set; }
        public string RepresentanteLegal { get; set; }
        public string EmpresaAcreditadora { get; set; }
        public DateTime? FechaEmision { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string RutaRubrica { get; set; }
        public string CertificadoDigital { get; set; }
    }
}
