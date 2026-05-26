using CadFerreteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnFerreteria
{
    public class ProductoCln
    {
        public static int crear(Producto producto)
        {
            using (var context = new LabFerreteriaEntities())
            {
                context.Productoes.Add(producto);
                context.SaveChanges();
                return producto.id;
            }
        }


        public static int modificar(Producto producto)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.Productoes.Find(producto.id);

                if (existente != null)
                {
                    existente.codigo = producto.codigo;
                    existente.descripcion = producto.descripcion;
                    existente.idUnidadMedida = producto.idUnidadMedida;
                    existente.cantidad = producto.cantidad;
                    existente.precioVenta = producto.precioVenta;
                    existente.usuarioRegistro = producto.usuarioRegistro;
                    existente.idMarca = producto.idMarca;
                    existente.idSubCategoria = producto.idSubCategoria;

                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static int eliminar(int id, string usuarioRegistro)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.Productoes.Find(id);
                if (existente != null)
                {
                    existente.estado = -1;
                    existente.usuarioRegistro = usuarioRegistro;
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static Producto obtenerUno(int id)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.Productoes.Find(id);
                
            }
        }

        public static List<Producto> listar()
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.Productoes
                    .Where(x => x.estado == 1)
                    .OrderBy(x => x.descripcion)
                    .ToList();

            }
        }

        public static int obtenerCantidadProductos()
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.Productoes.Count(x => x.estado != -1);
            }
        }

        public static List<paProductoListar_Result> listarPa(string parametro)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.paProductoListar(parametro.Trim()).ToList();

            }
        }

        public static int obtenerProductosBajos()
        {
            using (var context = new LabFerreteriaEntities())
            {
                // 💡 Cambiado a '== 1' (Solo activos)
                return context.Productoes.Count(x => x.estado == 1 && x.cantidad <= 5);
            }
        }

        // 2. Para la tablita que se abre al hacer clic
        public static List<Producto> listarProductosBajos()
        {
            using (var context = new LabFerreteriaEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;

                // 💡 Cambiado a '== 1' (Solo activos)
                return context.Productoes
                    .Where(x => x.estado == 1 && x.cantidad <= 5)
                    .OrderBy(x => x.descripcion)
                    .ToList();
            }
        }

    }
}
