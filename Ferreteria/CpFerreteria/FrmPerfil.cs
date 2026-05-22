using ClnFerreteria;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpFerreteria
{
    public partial class FrmPerfil : Form
    {
        public FrmPerfil()
        {
            InitializeComponent();
        }

        private void FrmPerfil_Load(object sender, EventArgs e)
        {
            try
            {
                if (Util.usuario != null)
                {
                    // 💡 RECONECTAMOS: Buscamos al usuario de forma fresca con su empleado incluido
                    var usuarioCompleto = UsuarioCln.ObtenerUsuarioConEmpleado(Util.usuario.id);

                    if (usuarioCompleto != null)
                    {
                        lblUsuario.Text = "Nombre de Usuario:   " + usuarioCompleto.usuario1;
                        lblCargo.Text = "Cargo / Rol:           " + (usuarioCompleto.rol ?? "Administrador");

                        // Evaluamos los datos de la relación de forma segura
                        if (usuarioCompleto.Empleado != null)
                        {
                            lblNombreCompleto.Text = "Nombre Completo:    " + usuarioCompleto.Empleado.nombres + " " + usuarioCompleto.Empleado.primerApellido;
                            lblCedula.Text = "Cédula de Identidad: " + usuarioCompleto.Empleado.cedulaIdentidad;
                        }
                        else
                        {
                            lblNombreCompleto.Text = "Nombre Completo: Administrador Global";
                            lblCedula.Text = "Cédula de Identidad: S/D";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el perfil: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
