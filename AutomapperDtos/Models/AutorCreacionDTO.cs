using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutomapperDtos.Models
{
    public class AutorCreacionDTO
    {
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
