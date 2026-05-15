using CadFerreteria;
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
    public partial class FrmAutenticacion : Form
    {
        public FrmAutenticacion()
        {
            InitializeComponent();
            /// txtUsuario.Text = Util.Encrypt("hola123");
        }

        private bool validar()
        {
            bool esValido = true;
            erpUsuario.Clear();
            erpClave.Clear();
            if (string.IsNullOrEmpty(txtUsuario.Text))
            {
                erpUsuario.SetError(txtUsuario, "Ingrese el usuario");
                esValido = false;
            }

            if (string.IsNullOrEmpty(txtClave.Text))
            {

                erpClave.SetError(txtClave, "Ingrese la clave");
                esValido = false;
            }

            return esValido;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                // Llamamos al método que devuelve 'object'
                var resultado = UsuarioCln.validar(txtUsuario.Text, Util.Encrypt(txtClave.Text));

                if (resultado != null)
                {
                    // Verificamos si lo que devolvió es un Empleado (Usuario)
                    if (resultado is Usuario)
                    {
                        Util.usuario = (Usuario)resultado; // Aquí hacemos la conversión explícita
                        txtClave.Clear();
                        txtUsuario.Focus();
                        Hide();
                        new FrmPrincipal(this).ShowDialog();
                    }
                    // Verificamos si lo que devolvió es un Cliente
                    else if (resultado is Cliente)
                    {
                        Cliente clienteLogueado = (Cliente)resultado;

                        MessageBox.Show("Bienvenido Cliente: " + clienteLogueado.nombreCompleto);

                        txtClave.Clear();
                        //txtUsuario.Clear();

                        this.Hide();

                        FrmPrincipalCliente frmPrincipalCliente = new FrmPrincipalCliente(this);
                        frmPrincipalCliente.ShowDialog();

                    }
                }
                else
                {
                    MessageBox.Show("Usuario y/o clave incorrectos", "::: Mensaje - Ferreteria :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnIngresar.PerformClick();
            }
        }
    }
}
