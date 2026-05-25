using CadFerreteria;
using ClnFerreteria;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CpFerreteria
{
    public partial class FrmAutenticacion : Form
    {
        public FrmAutenticacion()
        {
            InitializeComponent();
            ConfigurarFormulario();

        }

        private void ConfigurarFormulario()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 242, 245);

        }
        private void TxtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "Usuario")
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.Black;
            }
        }

        private void TxtUsuario_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                txtUsuario.Text = "Usuario";
                txtUsuario.ForeColor = Color.Gray;
            }
        }

       
        private bool Validar()
        {
            bool esValido = true;

            erpUsuario.Clear();
            erpClave.Clear();

            if (string.IsNullOrWhiteSpace(txtUsuario.Text)
                || txtUsuario.Text == "Usuario")
            {
                erpUsuario.SetError(txtUsuario, "Ingrese el usuario");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtClave.Text)
                || txtClave.Text == "Contraseña")
            {
                erpClave.SetError(txtClave, "Ingrese la clave");
                esValido = false;
            }

            return esValido;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                var resultado = UsuarioCln.validar(
                    txtUsuario.Text,
                    Util.Encrypt(txtClave.Text)
                );

                if (resultado is Usuario usuario)
                {
                    Util.usuario = usuario;

                    txtClave.Clear();

                    this.Hide();

                    new FrmPrincipal(this).ShowDialog();

                    this.Show();
                }
                else if (resultado is Cliente cliente)
                {
                    MessageBox.Show(
                        "Bienvenido Cliente: " + cliente.nombreCompleto,
                        "Ferretería",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    txtClave.Clear();

                    this.Hide();

                    new FrmPrincipalCliente(this).ShowDialog();

                    this.Show();
                }
                else
                {
                    MessageBox.Show(
                        "Usuario y/o clave incorrectos",
                        "::: Mensaje - Ferretería :::",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }
        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnIngresar.PerformClick();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}