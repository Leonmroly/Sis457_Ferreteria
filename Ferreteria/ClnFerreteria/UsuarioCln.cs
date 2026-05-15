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
                // Busca empleado (clave ya viene encriptada desde el form)
                var empleado = context.Usuario
                    .FirstOrDefault(x => x.usuario1 == login && x.clave == clave && x.estado == 1);
                if (empleado != null) return empleado;

                // Busca cliente (clave ya viene encriptada desde el form)
                var cliente = context.Clientes
                    .FirstOrDefault(x => x.email == login && x.password == clave && x.tipo == 2 && x.estado == 1);
                if (cliente != null) return cliente;

                return null;
            }
        }
    }
}
