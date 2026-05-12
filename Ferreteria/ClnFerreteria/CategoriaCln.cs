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
                context.SaveChanges(); // Aquí se genera el ID de la Categoría

                var subcat = new SubCategoria()
                {
                    nombre = categoria.nombre,
                    idCategoria = categoria.id,
                    usuarioRegistro = categoria.usuarioRegistro,
                    fechaRegistro = DateTime.Now,
                    estado = 1
                };

                context.SubCategoria.Add(subcat);
                context.SaveChanges(); // Aquí se crea la subcategoría que usará el Producto

                return categoria.id;
            }
        }

        public static int modificar(Categoria categoria)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.Categoria.Find(categoria.id);
                if (existente != null)
                {
                    existente.nombre = categoria.nombre;
                    existente.usuarioRegistro = categoria.usuarioRegistro;
                    var subat = context.SubCategoria.FirstOrDefault(x => x.idCategoria == categoria.id);

                    if (subat != null)
                    {
                        subat.nombre = categoria.nombre;
                        subat.usuarioRegistro = categoria.usuarioRegistro;
                    }

                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static int eliminar(int id, string usuario)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.Categoria.Find(id);
                if (existente != null)
                {
                    existente.estado = -1;
                    existente.usuarioRegistro = usuario;

                    var subat = context.SubCategoria.FirstOrDefault(x => x.idCategoria == id);

                    if (subat != null)
                    {
                        subat.estado = -1;
                        subat.usuarioRegistro = usuario;
                    }

                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static List<paCategoriaListar_Result> listarPa(string parametro)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.paCategoriaListar(parametro).ToList();
            }
        }

        public static List<Categoria> listar()
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.Categoria
                    .Where(x => x.estado == 1)
                    .ToList();
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
