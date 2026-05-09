using CadFerreteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnFerreteria
{
    public class CategoriaCln
    {
        public static int crear(Categoria categoria)
        {
            using (var context = new LabFerreteriaEntities())
            {
                context.Categoria.Add(categoria);
                context.SaveChanges();
                return categoria.id;
            }
        }

        public static int modificar(Categoria categoria)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.Categoria.Find(categoria.id);
                existente.nombre = categoria.nombre;
                existente.usuarioRegistro = categoria.usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuario)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.Categoria.Find(id);
                existente.estado = -1;
                existente.usuarioRegistro = usuario;
                return context.SaveChanges();
            }
        }

        public static List<paCategoriaListar_Result> listarPa(string parametro)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.paCategoriaListar(parametro).ToList();
            }
        }

        public static Categoria obtenerUno(int id)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.Categoria.Find(id);
            }
        }
    }
}
