using CadFerreteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ClnFerreteria
{
    public class CompraCln
    {


        public static List<CompraDto> listar()
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.Compras
                    .Include(x => x.Proveedor)
                    .Where(x => x.estado != -1)
                    .ToList() // Descargamos los datos a la memoria de la aplicación
                    .Select(x => new CompraDto
                    {
                        id = (int)x.id,
                        fecha = x.fecha,
                        total = x.total,
                        usuarioRegistro = x.usuarioRegistro,
                        proveedor = x.Proveedor != null ? x.Proveedor.razonSocial : "Sin Proveedor"
                    })
                    .ToList();
            }
        }

        public static bool GuardarCompra(Compra compra, List<CompraDetalle> detalles)
        {
            try
            {
                using (var context = new LabFerreteriaEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.Compras.Add(compra);
                            context.SaveChanges();

                            foreach (var detalle in detalles)
                            {
                                detalle.idCompra = compra.id;
                                context.CompraDetalles.Add(detalle);

                                var producto = context.Productoes.FirstOrDefault(x => x.id == detalle.idProducto);
                                if (producto != null)
                                {
                                    producto.saldo += detalle.cantidad;
                                }
                            }

                            context.SaveChanges();
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<object> obtenerDetalle(int idCompra)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.CompraDetalles
                    .Where(x => x.idCompra == idCompra)
                    .ToList() // Descargamos primero a memoria para evitar conflictos de contexto
                    .Select(x => new
                    {
                        Producto = context.Productoes.FirstOrDefault(p => p.id == x.idProducto)?.descripcion ?? "Desconocido",
                        Cantidad = x.cantidad,
                        // CAMBIAMOS AQUÍ: x.precioCompra en lugar de x.precioUnitario
                        PrecioUnitario = x.precioCompra,
                        Subtotal = x.cantidad * x.precioCompra
                    })
                    .ToList<object>();
            }
        }


    }
}
