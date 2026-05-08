using CadFerreteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnFerreteria
{
    public class MarcaCln
    {
        public static int crear(Marca marca)
        {
            using (var context = new LabFerreteriaEntities())
            {
                context.Marca.Add(marca);
                context.SaveChanges();
                return marca.id;
            }
        }

        public static int modificar(Marca marca)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.Marca.Find(marca.id);
                existente.nombre = marca.nombre;
                existente.usuarioRegistro = marca.usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuario)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.Marca.Find(id);
                existente.estado = -1;
                existente.usuarioRegistro = usuario;
                return context.SaveChanges();
            }
        }

        public static List<paMarcaListar_Result> listarPa(string parametro)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.paMarcaListar(parametro).ToList();
            }
        }

        public static Marca obtenerUno(int id)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.Marca.Find(id);
            }
        }
    }
}
