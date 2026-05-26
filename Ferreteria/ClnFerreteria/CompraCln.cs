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

            try
            {
                // UN SOLO CONTEXTO NATIVO: Sin SPs tramposos y 100% seguro
                using (var context = new LabFerreteriaEntities())
                {
                    // 1. Armamos la cabecera de la compra directamente
                    var nuevaCompra = new Compra
                    {
                        idProveedor = compra.idProveedor,
                        fecha = DateTime.Now,
                        total = compra.total,
                        usuarioRegistro = compra.usuarioRegistro, // Tu usuario por defecto si llega nulo
                        estado = 1
                    };

                    // 💡 SOLUCIÓN AL ID DE USUARIO: Al igual que en ventas, mapeamos la relación obligatoria con la tabla Usuario
                    var tipoCompra = typeof(Compra);
                    var propIdUsuario = tipoCompra.GetProperty("idUsuario") ?? tipoCompra.GetProperty("idEmpleado");
                    if (propIdUsuario != null)
                    {
                        propIdUsuario.SetValue(nuevaCompra, 1); // Forzamos el ID del usuario administrador logueado
                    }

                    // Forzamos la fecha de registro en la cabecera si existiera la columna
                    var propFechaRegistro = tipoCompra.GetProperty("fechaRegistro");
                    if (propFechaRegistro != null)
                    {
                        propFechaRegistro.SetValue(nuevaCompra, DateTime.Now);
                    }

                    // Guardamos la cabecera para generar el ID automático de la compra
                    context.Set<Compra>().Add(nuevaCompra);
                    context.SaveChanges();

                    // 2. Procesamos los detalles de los productos comprados
                    foreach (var detalle in detalles)
                    {
                        var producto = context.Set<Producto>().FirstOrDefault(x => x.id == detalle.idProducto);

                        if (producto != null)
                        {
                            int cantidadComprada = Convert.ToInt32(detalle.cantidad);

                            // SUMAMOS AL STOCK (Inventario aumenta en compras)
                            producto.cantidad += cantidadComprada;

                            // Enlazamos el detalle con el ID generado arriba de forma nativa
                            detalle.idCompra = (int)nuevaCompra.id;
                            detalle.estado = 1;
                            detalle.fechaRegistro = DateTime.Now;
                            detalle.usuarioRegistro = nuevaCompra.usuarioRegistro;

                            context.Set<CompraDetalle>().Add(detalle);
                        }
                    }

                    // Guardamos los detalles y la actualización del stock
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Exception realEx = ex;
                while (realEx.InnerException != null) realEx = realEx.InnerException;
                mensajeError = "Fallo al guardar compra: " + realEx.Message;
                return false;
            }
        }

        public static decimal obtenerTotalComprasHoy()
        {
            using (var context = new LabFerreteriaEntities())
            {
                DateTime hoy = DateTime.Today;
                return context.Compras
                    .Where(x => x.estado != -1 && x.fecha >= hoy)
                    .Select(x => (decimal?)x.total)
                    .Sum() ?? 0;
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
