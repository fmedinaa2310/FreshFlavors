using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDiseño.Client
{
    public class Cliente
    {
        public string Nombre {get; set;}
        public string PrimerApellido { get; set;}
        public string SegundoApellido { get; set;}

        public decimal DineroInicial { get; set; }

        public Cliente()
        {
            // Generar una cantidad aleatoria de dinero entre $100 y $1000
            Random random = new Random();
            DineroInicial = (decimal)(random.NextDouble() * 900) + 100;
        }
    }
}
