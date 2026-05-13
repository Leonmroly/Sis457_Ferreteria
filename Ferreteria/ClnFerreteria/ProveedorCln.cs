using CadFerreteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnFerreteria
{
    public class ProveedorCln
    {
        // 1. Guardar un nuevo proveedor
        public static int crear(Proveedor proveedor)
        {
            using (var context = new LabFerreteriaEntities())
            {
                proveedor.usuarioRegistro = "SISTEMA"; // Aquí luego pondrás el de tu Login
                proveedor.fechaRegistro = DateTime.Now;
                proveedor.estado = 1; // 1 = Activo
                context.Proveedors.Add(proveedor);
                context.SaveChanges();
                return proveedor.id;
            }
        }

        // 2. Modificar datos (NIT, Razón Social, etc.)
        public static int actualizar(Proveedor proveedor)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.Proveedors.Find(proveedor.id);
                if (existente != null)
                {
                    existente.nit = proveedor.nit;
                    existente.razonSocial = proveedor.razonSocial;
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        // 3. Borrado Lógico (Soft Delete)
        public static int eliminar(int id)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.Proveedors.Find(id);
                if (existente != null)
                {
                    existente.estado = -1; // Lo mandamos a la "papelera"
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        // 4. Listar solo los activos
        public static List<Proveedor> listar()
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.Proveedors.Where(x => x.estado == 1).ToList();
            }
        }
    }
}
