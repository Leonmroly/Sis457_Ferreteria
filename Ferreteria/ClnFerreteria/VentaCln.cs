using CadFerreteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnFerreteria
{
    public class VentaDto
    {
        public long id { get; set; }
        public DateTime fecha { get; set; }
        public decimal total { get; set; }
        public string usuarioRegistro { get; set; }
        public string cliente { get; set; }
    }

    public class VentaCln
    {
        public static List<VentaDto> listar()
        {
            try
            {
                using (var context = new LabFerreteriaEntities())
                {
                    // 1. Traemos las listas completas sin filtros agresivos para asegurar que se vea TODO
                    var ventas = context.Set<Venta>().ToList();
                    var clientes = context.Set<Cliente>().ToList();

                    // 2. Hacemos un Left Join robusto. Si el cliente no existe, igual muestra la venta.
                    var resultado = from v in ventas
                                    join c in clientes on v.idCliente equals c.id into joinCliente
                                    from cl in joinCliente.DefaultIfEmpty()
                                    select new VentaDto
                                    {
                                        id = v.id,
                                        fecha = v.fecha,
                                        total = v.total,
                                        usuarioRegistro = v.usuarioRegistro ?? "Iroly", // Si viene null, le ponemos el usuario por defecto
                                        cliente = cl != null ? cl.nombreCompleto : "Cliente General"
                                    };

                    // 3. Lo ordenamos de la venta más reciente a la más antigua y lo retornamos
                    return resultado.OrderByDescending(x => x.fecha).ToList();
                }
            }
            catch (Exception ex)
            {
                // En caso de cualquier error interno, no vacía la lista a ciegas, te avisa qué pasó
                throw new Exception("Error al listar las ventas: " + ex.Message);
            }
        }

        public static bool GuardarVenta(Venta venta, List<VentaDetalle> detalles, out string mensajeError)
        {
            mensajeError = string.Empty;

            try
            {
                using (var context = new LabFerreteriaEntities())
                {
                    // 1. Armamos el objeto de la cabecera directamente
                    var nuevaVenta = new Venta
                    {
                        idCliente = venta.idCliente,
                        fecha = DateTime.Now,
                        total = venta.total,
                        usuarioRegistro = venta.usuarioRegistro ?? "SISTEMA",
                        estado = 1
                    };

                    // 💡 SOLUCIÓN A LA LLAVE FORÁNEA: Buscamos si la columna se llama idUsuario o idEmpleado
                    var tipoVenta = typeof(Venta);

                    // Intentamos buscar las formas más comunes en las que se llama tu columna de relación
                    var propIdUsuario = tipoVenta.GetProperty("idUsuario") ?? tipoVenta.GetProperty("idEmpleado");

                    if (propIdUsuario != null)
                    {
                        // Le asignamos el ID 1 (el usuario administrador inicial de tu BD) 
                        // para cumplir con la restricción FK.
                        propIdUsuario.SetValue(nuevaVenta, 1);
                    }

                    // Forzamos también la fechaRegistro si existiera en la cabecera
                    var propFechaRegistro = tipoVenta.GetProperty("fechaRegistro");
                    if (propFechaRegistro != null)
                    {
                        propFechaRegistro.SetValue(nuevaVenta, DateTime.Now);
                    }

                    // Agregamos la cabecera al contexto
                    context.Set<Venta>().Add(nuevaVenta);
                    context.SaveChanges();

                    // 2. Procesamos el carrito de detalles
                    foreach (var detalle in detalles)
                    {
                        var producto = context.Set<Producto>().FirstOrDefault(x => x.id == detalle.idProducto);

                        if (producto != null)
                        {
                            int cantidadVendida = Convert.ToInt32(detalle.cantidad);

                            // Validación de stock
                            if (producto.cantidad < cantidadVendida)
                            {
                                mensajeError = $"Stock insuficiente para {producto.descripcion}. Disponible: {producto.cantidad}";
                                return false;
                            }

                            // Restamos del inventario
                            producto.cantidad -= cantidadVendida;

                            // Llenamos el detalle amarrándolo al nuevo ID
                            detalle.idVenta = nuevaVenta.id;
                            detalle.estado = 1;
                            detalle.fechaRegistro = DateTime.Now;
                            detalle.usuarioRegistro = venta.usuarioRegistro ?? "SISTEMA";

                            context.Set<VentaDetalle>().Add(detalle);
                        }
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Exception realEx = ex;
                while (realEx.InnerException != null) realEx = realEx.InnerException;
                mensajeError = realEx.Message;
                return false;
            }
        }

        // SOLO DEJAMOS UNA COPIA DE obtenerDetalle (Eliminando la duplicada de raíz)
        public static List<object> obtenerDetalle(int idVenta)
        {
            try
            {
                using (var context = new LabFerreteriaEntities())
                {
                    var consulta = from d in context.Set<VentaDetalle>()
                                   join p in context.Set<Producto>() on d.idProducto equals p.id
                                   where d.idVenta == idVenta && d.estado == 1
                                   select new
                                   {
                                       Producto = p.descripcion,
                                       Cantidad = d.cantidad,
                                       PrecioUnitario = d.precioUnitario,
                                       Subtotal = d.cantidad * d.precioUnitario
                                   };

                    return consulta.ToList<object>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar detalles de venta: " + ex.Message);
            }
        }
    }
}