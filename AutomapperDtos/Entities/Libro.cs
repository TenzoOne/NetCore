﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomapperDtos.Entities
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int AutorID { get; set; }
    }
}
