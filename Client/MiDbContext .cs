using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDiseño.Client
{
    public class MiDbContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; }

    }
}
