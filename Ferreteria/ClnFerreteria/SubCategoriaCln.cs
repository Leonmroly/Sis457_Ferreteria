using CadFerreteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnFerreteria
{
    public class SubCategoriaCln
    {
        public static List<SubCategoria> listar()
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.SubCategoria.Where(x => x.estado == 1).ToList();
            }
        }
    }
}
