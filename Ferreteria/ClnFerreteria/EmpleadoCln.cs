using CadFerreteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnFerreteria
{
    public class EmpleadoCln
    {
        public static int crear(Empleado empleado, string usuarioFormulario)
        {
            using (var context = new LabFerreteriaEntities())
            {
                empleado.usuarioRegistro = usuarioFormulario;
                empleado.fechaRegistro = DateTime.Now;
                empleado.estado = 1;

                context.Empleadoes.Add(empleado);
                context.SaveChanges();
                return empleado.id;
            }
        }

        public static int actualizar(Empleado empleado, string usuarioFormulario)
        {
            using (var context = new LabFerreteriaEntities())
            {
                var existente = context.Empleadoes.Find(empleado.id);
                if (existente != null)
                {
                    existente.cedulaIdentidad = empleado.cedulaIdentidad;
                    existente.nombres = empleado.nombres;
                    existente.primerApellido = empleado.primerApellido;
                    existente.segundoApellido = empleado.segundoApellido;
                    existente.fechaNacimiento = empleado.fechaNacimiento;
                    existente.direccion = empleado.direccion;
                    existente.celular = empleado.celular;
                    existente.cargo = empleado.cargo;

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
                var empleado = context.Empleadoes.Find(id);
                if (empleado != null)
                {
                    empleado.estado = -1;
                    empleado.usuarioRegistro = usuarioFormulario;

                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static Empleado obtenerPa(int id)
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.Empleadoes.FirstOrDefault(x => x.id == id && x.estado == 1);
            }
        }

        public static List<Empleado> listar()
        {
            using (var context = new LabFerreteriaEntities())
            {
                return context.Empleadoes.Where(x => x.estado == 1).ToList();
            }
        }
    }
}
