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
                    .ToList()
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

        public static bool GuardarCompra(Compra compra, List<CompraDetalle> detalles, out string mensajeError)
        {
            mensajeError = string.Empty;
            long idCompraNuevo = 0;

            try
            {
                // BLOQUE 1: Guardamos la cabecera con el SP
                using (var context1 = new LabFerreteriaEntities())
                {
                    context1.paCompraGuardar(
                        compra.idProveedor,
                        compra.total,
                        compra.usuarioRegistro ?? "SISTEMA"
                    );

                    context1.SaveChanges();
                }

                // BLOQUE 2: Guardamos el detalle apuntando al ID incremental más alto
                using (var context2 = new LabFerreteriaEntities())
                {
                    // CAMBIO CLAVE: Ordenamos por ID de forma descendente. El ID más grande es SÍ O SÍ la compra actual.
                    idCompraNuevo = context2.Compras
                        .Where(x => x.idProveedor == compra.idProveedor)
                        .OrderByDescending(x => x.id) // <--- Cambiado de 'fecha' a 'id'
                        .Select(x => x.id)
                        .FirstOrDefault();

                    if (idCompraNuevo == 0)
                    {
                        mensajeError = "La compra se guardó, pero no se pudo recuperar el ID incremental generado.";
                        return false;
                    }

                    // Procesamos e insertamos los detalles en la tabla secundaria
                    foreach (var detalle in detalles)
                    {
                        var producto = context2.Set<Producto>().FirstOrDefault(x => x.id == detalle.idProducto);

                        if (producto != null)
                        {
                            int cantidadComprada = Convert.ToInt32(detalle.cantidad);
                            producto.cantidad += cantidadComprada;

                            detalle.idCompra = (int)idCompraNuevo; // Aseguramos que herede el ID correcto
                            detalle.estado = 1;
                            detalle.fechaRegistro = DateTime.Now;
                            detalle.usuarioRegistro = compra.usuarioRegistro ?? "SISTEMA";

                            context2.Set<CompraDetalle>().Add(detalle);
                        }
                    }

                    // Confirmamos la inserción de los detalles
                    context2.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Exception realEx = ex;
                while (realEx.InnerException != null)
                {
                    realEx = realEx.InnerException;
                }

                mensajeError = "Fallo en detalles: " + realEx.Message;
                return false;
            }
        }

        public static List<object> obtenerDetalle(int idCompra)
        {
            try
            {
                using (var context = new LabFerreteriaEntities())
                {
                    var consulta = from d in context.CompraDetalles
                                   join p in context.Productoes on d.idProducto equals p.id
                                   where d.idCompra == idCompra && d.estado == 1
                                   select new
                                   {
                                       Producto = p.descripcion,
                                       Cantidad = d.cantidad,
                                       PrecioUnitario = d.precioCompra,
                                       Subtotal = d.cantidad * d.precioCompra
                                   };

                    return consulta.ToList<object>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error interno en obtenerDetalle: " + ex.Message);
            }
        }


    }
}
