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
                    existente.saldo = producto.saldo;
                    existente.precioVenta = producto.precioVenta;
                    existente.usuarioRegistro = producto.usuarioRegistro;

                    // FALTABAN ESTAS LÍNEAS PARA ACTUALIZAR LAS RELACIONES
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

        public static List<paProductoListar_Result> listarPa(string parametro)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.paProductoListar(parametro.Trim()).ToList();

            }
        }
    }
}
