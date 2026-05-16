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
    }
}
