using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDO.Examen01.Models
{
    public class pagination_bind
    {
        public int total { get; set; }
        public int current_page { get; set; }
        public int per_page { get; set; }
        public decimal last_page { get; set; }
        public int from { get; set; }
        public int to { get; set; }
    }
}
