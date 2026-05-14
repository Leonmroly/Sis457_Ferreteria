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
        public static int crear(Cliente cliente)
        {
            using (var context = new LabFerreteriaEntities())
            {
                cliente.usuarioRegistro = "SISTEMA";
                cliente.fechaRegistro = DateTime.Now;
                cliente.estado = 1;
                context.Clientes.Add(cliente);
                context.SaveChanges();
                return cliente.id;
            }
        }

        public static int actualizar(Cliente cliente)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.Clientes.Find(cliente.id);
                existente.cedulaIdentidad = cliente.cedulaIdentidad;
                existente.nombreCompleto = cliente.nombreCompleto;
                existente.direccion = cliente.direccion;
                existente.telefono = cliente.telefono;
                existente.email = cliente.email;
                existente.usuarioRegistro = "SISTEMA";
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var cliente = context.Clientes.Find(id);
                cliente.estado = -1;
                return context.SaveChanges();
            }
        }

        public static List<Cliente> listar()
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.Clientes
                    .Where(x => x.estado == 1)
                    .ToList();
            }
        }
    }
}
