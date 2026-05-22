using CadFerreteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnFerreteria
{
    public class UsuarioCln
    {
        public static object validar(string login, string clave)
        {
            using (var context = new LabFerreteriaEntities())
            {
                // AQUÍ: Cambiado 'context.Usuario' por 'context.Usuarios' (con S)
                var usuario = context.Usuarios
                    .FirstOrDefault(x => x.usuario1 == login && x.clave == clave && x.estado == 1);
                
                if (usuario != null) return usuario;

                // Busca cliente (Esta colección ya estaba bien en tu código)
                var cliente = context.Clientes
                    .FirstOrDefault(x => x.email == login && x.password == clave && x.tipo == 2 && x.estado == 1);
                
                if (cliente != null) return cliente;

                return null;
            }
        }

        public static int crear(Usuario usuario, string usuarioFormulario)
        {
            using (var context = new LabFerreteriaEntities())
            {
                usuario.usuarioRegistro = usuarioFormulario;
                usuario.fechaRegistro = DateTime.Now;
                usuario.estado = 1;

                context.Usuarios.Add(usuario);
                context.SaveChanges();
                return usuario.id;
            }
        }

        public static int actualizar(Usuario usuario, string usuarioFormulario)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.Usuarios.Find(usuario.id);
                if (existente != null)
                {
                    existente.idEmpleado = usuario.idEmpleado;
                    existente.usuario1 = usuario.usuario1;
                    existente.rol = usuario.rol;

                    // Solo cambia la contraseña si se escribió algo nuevo en el formulario
                    if (!string.IsNullOrWhiteSpace(usuario.clave))
                    {
                        existente.clave = usuario.clave;
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
                var usuario = context.Usuarios.Find(id);
                if (usuario != null)
                {
                    usuario.estado = -1; // Borrado lógico
                    usuario.usuarioRegistro = usuarioFormulario;
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static Usuario obtenerPa(int id)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.Usuarios.FirstOrDefault(x => x.id == id && x.estado == 1);
            }
        }

        public static List<Usuario> listar()
        {
            using (var context = new LabFerreteriaEntities())
            {
                // El .Include("Empleado") carga los datos del empleado dueño de la cuenta automáticamente
                return context.Usuarios.Include("Empleado").Where(x => x.estado == 1).ToList();
            }
        }

        public static Usuario ObtenerUsuarioConEmpleado(int idUsuario)
        {
            using (var context = new LabFerreteriaEntities())
            {
                // Pasamos el nombre de la relación como "string" para que EF lo entienda perfectamente
                return context.Usuarios
                    .Include("Empleado")
                    .FirstOrDefault(x => x.id == idUsuario);
            }
        }

    }
}
