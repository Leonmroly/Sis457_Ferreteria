using CadFerreteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnFerreteria
{
    public class UnidadMedidaCln
    {
        public static List<UnidadMedida> listar()
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.UnidadMedida
                    .Where(x => x.estado == 1)
                    .OrderBy(x => x.nombre)
                    .ToList();
            }
        }
    }
}
