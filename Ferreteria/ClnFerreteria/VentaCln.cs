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
            using (var context = new LabFerreteriaEntities())
            {
                var ventas = context.Set<Venta>().Where(x => x.estado != -1).ToList();
                var clientes = context.Set<Cliente>().ToList();

                var resultado = from v in ventas
                                join c in clientes on v.idCliente equals c.id into joinCliente
                                from cl in joinCliente.DefaultIfEmpty()
                                select new VentaDto
                                {
                                    id = v.id,
                                    fecha = v.fecha,
                                    total = v.total,
                                    usuarioRegistro = v.usuarioRegistro,
                                    cliente = cl != null ? cl.nombreCompleto : "Cliente General"
                                };

                return resultado.ToList();
            }
        }

        public static int GuardarVenta(Venta venta, List<VentaDetalle> detalles, out string mensajeError)
        {
            mensajeError = string.Empty;

            try
            {
                using (var context = new LabFerreteriaEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            // 1. INVOCAMOS AL PROCEDIMIENTO ALMACENADO NATIVO DE SQL
                            var idVentaParam = new System.Data.Entity.Core.Objects.ObjectParameter("idVenta", typeof(long));

                            context.paVentaGuardar(
                                venta.idCliente,
                                venta.usuarioRegistro ?? "Cajero",
                                venta.total,
                                idVentaParam
                            );

                            // Extraemos el ID generado físicamente en la BD
                            long idVentaNuevo = Convert.ToInt64(idVentaParam.Value);

                            // 2. PROCESAMOS EL BLOQUE DE DETALLES
                            foreach (var detalle in detalles)
                            {
                                var producto = context.Productoes.FirstOrDefault(x => x.id == detalle.idProducto);

                                if (producto == null)
                                {
                                    transaction.Rollback();
                                    mensajeError = $"El producto con ID {detalle.idProducto} no existe.";
                                    return -1;
                                }

                                if (producto.saldo < detalle.cantidad)
                                {
                                    transaction.Rollback();
                                    mensajeError = $"Stock insuficiente para '{producto.descripcion}'. Disponible: {producto.saldo}, Solicitado: {detalle.cantidad}";
                                    return 0;
                                }

                                // Descontamos stock del inventario físico
                                producto.saldo -= detalle.cantidad;

                                // Enlazamos al ID maestro devuelto por SQL
                                detalle.idVenta = (int)idVentaNuevo;
                                detalle.estado = 1;

                                // =======================================================================
                                // SOLUCIÓN DIRECTA PARA LA FECHA DEL DETALLE:
                                // Asignamos la fecha directamente sobre el objeto C# usando sus nombres reales
                                // para que Entity Framework no mande valores vacíos (0001-01-01) a la BD.
                                // =======================================================================
                                detalle.fechaRegistro = DateTime.Now;

                                // Agregamos el detalle al contexto de EF
                                context.Set<VentaDetalle>().Add(detalle);
                            }

                            // Guardamos todo el lote de detalles y actualizaciones de stock en un solo bloque
                            context.SaveChanges();

                            transaction.Commit();
                            return 1;
                        }
                        catch (System.Data.Entity.Infrastructure.DbUpdateException dbEx)
                        {
                            transaction.Rollback();
                            var innerMessage = dbEx.InnerException != null && dbEx.InnerException.InnerException != null
                                ? dbEx.InnerException.InnerException.Message
                                : dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;

                            mensajeError = $"Error de base de datos: {innerMessage}";
                            return -1;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            mensajeError = $"Error interno: {ex.Message}";
                            return -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mensajeError = $"Error de conexión: {ex.Message}";
                return -1;
            }
        }

        // 3. Método para el Doble Clic (Historial detallado de una venta específica)
        public static List<object> obtenerDetalle(int idVenta)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.Set<VentaDetalle>()
                    .Where(x => x.idVenta == idVenta)
                    .ToList()
                    .Select(x => new
                    {
                        Producto = context.Productoes.FirstOrDefault(p => p.id == x.idProducto)?.descripcion ?? "Desconocido",
                        Cantidad = x.cantidad,
                        PrecioUnitario = x.precioUnitario,
                        Subtotal = x.cantidad * x.precioUnitario
                    })
                    .ToList<object>();
            }
        }
    }
}