using CadFerreteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnFerreteria
{
    public class ClienteCln
    {

        public static class Util
        {
            public static Usuario usuario; // Aquí se guarda el usuario que hizo Login
        }

        public static int crear(Cliente cliente, string usuarioFormulario, bool esCuentaNueva)
        {
            using (var context = new LabFerreteriaEntities())
            {
                cliente.usuarioRegistro = usuarioFormulario;
                cliente.fechaRegistro = DateTime.Now;
                cliente.estado = 1;

                if (esCuentaNueva)
                {
                    cliente.tipo = 2;
                }
                else
                {
                    cliente.tipo = 1;
                    cliente.password = null;
                }

                context.Clientes.Add(cliente);
                context.SaveChanges();
                return cliente.id;
            }
        }

        public static Cliente obtenerPa(int id)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.Clientes.FirstOrDefault(x => x.id == id && x.estado == 1);
            }
        }

        public static int actualizar(Cliente cliente, string usuarioFormulario)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.Clientes.Find(cliente.id);
                if (existente != null)
                {
                    existente.cedulaIdentidad = cliente.cedulaIdentidad;
                    existente.nombreCompleto = cliente.nombreCompleto;
                    existente.direccion = cliente.direccion;
                    existente.telefono = cliente.telefono;
                    existente.email = cliente.email;

                    if (!string.IsNullOrWhiteSpace(cliente.password))
                    {
                        existente.password = cliente.password;
                        existente.tipo = 2;
                    }
                    else
                    {
                        existente.tipo = 1;
                        existente.password = null;
                    }

                    existente.usuarioRegistro = usuarioFormulario;
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static int eliminar(int id, string usuarioFormulario)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var cliente = context.Clientes.Find(id);
                if (cliente != null)
                {
                    cliente.estado = -1;
                    cliente.usuarioRegistro = usuarioFormulario;
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static List<Cliente> listar()
        {
            try
            {
                using (var context = new LabFerreteriaEntities())
                {
                    context.Configuration.LazyLoadingEnabled = false;
                    context.Configuration.ProxyCreationEnabled = false;

                    var resultado = context.Set<Cliente>().Where(x => x.estado != -1).ToList();

                    return resultado;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar el listado de clientes: " + ex.Message);
            }
        }

    }
}
