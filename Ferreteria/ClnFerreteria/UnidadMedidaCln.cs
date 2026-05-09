using CadFerreteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnFerreteria
{
    public class UnidadMedidaCln
    {
        public static List<UnidadMedida> listar()
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.UnidadMedida
                    .Where(x => x.estado == 1)
                    .OrderBy(x => x.nombre)
                    .ToList();
            }
        }

        public static List<paUnidadMedidaListar_Result> listarPa(string parametro)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.paUnidadMedidaListar(parametro).ToList();
            }
        }

        // 3. Crear nuevo registro
        public static int crear(UnidadMedida unidad)
        {
            using (var context = new LabFerreteriaEntities())
            {
                context.UnidadMedida.Add(unidad);
                context.SaveChanges();
                return unidad.id;
            }
        }

        // 4. Modificar registro existente (¡TE FALTABA ESTE!)
        public static int modificar(UnidadMedida unidad)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.UnidadMedida.Find(unidad.id);
                if (existente != null)
                {
                    existente.nombre = unidad.nombre;
                    existente.usuarioRegistro = unidad.usuarioRegistro;
                    // Aquí no cambiamos la fechaRegistro porque es la original de creación
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        // 5. Eliminar (Lógica: cambia estado a -1) (¡TAMBIÉN TE FALTABA!)
        public static int eliminar(int id, string usuarioRegistro)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.UnidadMedida.Find(id);
                if (existente != null)
                {
                    existente.estado = -1;
                    existente.usuarioRegistro = usuarioRegistro;
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        // 6. Obtener un registro por ID (Para cargar los datos al editar)
        public static UnidadMedida obtenerUno(int id)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.UnidadMedida.Find(id);
            }
        }
    }
}
