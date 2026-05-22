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
            ConfigurarControles();
        }

        
        // DISEÑO MODERNO
     

        private void ConfigurarFormulario()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 242, 245);

            // Redondear formulario
            RedondearControl(this, 30);
        }

        private void ConfigurarControles()
        {
            
            // TEXTBOX USUARIO
           
            txtUsuario.BorderStyle = BorderStyle.None;
            txtUsuario.BackColor = Color.White;
            txtUsuario.Font = new Font("Segoe UI", 12);
            txtUsuario.ForeColor = Color.Black;

         
            // TEXTBOX CLAVE

            txtClave.BorderStyle = BorderStyle.None;
            txtClave.BackColor = Color.White;
            txtClave.Font = new Font("Segoe UI", 12);
            txtClave.ForeColor = Color.Black;
            txtClave.PasswordChar = '●';

            // BOTÓN INGRESAR
            
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.BackColor = Color.FromArgb(52, 152, 219);
            btnIngresar.ForeColor = Color.White;
            btnIngresar.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnIngresar.Cursor = Cursors.Hand;

           
            // BOTÓN SALIR
           
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.BackColor = Color.FromArgb(231, 76, 60);
            btnSalir.ForeColor = Color.White;
            btnSalir.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnSalir.Cursor = Cursors.Hand;

            // REDONDEAR CONTROLES
          
            RedondearControl(txtUsuario, 20);
            RedondearControl(txtClave, 20);

            RedondearControl(btnIngresar, 20);
            RedondearControl(btnSalir, 20);

            // PLACEHOLDER
           
            txtUsuario.Text = "Usuario";
            txtUsuario.ForeColor = Color.Gray;

            txtClave.Text = "Contraseña";
            txtClave.ForeColor = Color.Gray;
            txtClave.PasswordChar = '\0';

            txtUsuario.Enter += TxtUsuario_Enter;
            txtUsuario.Leave += TxtUsuario_Leave;

            txtClave.Enter += TxtClave_Enter;
            txtClave.Leave += TxtClave_Leave;
        }

   
        // PLACEHOLDER USUARIO
   

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

       
        // PLACEHOLDER CLAVE
      

        private void TxtClave_Enter(object sender, EventArgs e)
        {
            if (txtClave.Text == "Contraseña")
            {
                txtClave.Text = "";
                txtClave.ForeColor = Color.Black;
                txtClave.PasswordChar = '●';
            }
        }

        private void TxtClave_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtClave.Text))
            {
                txtClave.Text = "Contraseña";
                txtClave.ForeColor = Color.Gray;
                txtClave.PasswordChar = '\0';
            }
        }

        // REDONDEAR CONTROLES
        private void RedondearControl(Control control, int radio)
        {
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();

            path.AddArc(new Rectangle(0, 0, radio, radio), 180, 90);
            path.AddArc(new Rectangle(control.Width - radio, 0, radio, radio), 270, 90);
            path.AddArc(new Rectangle(control.Width - radio, control.Height - radio, radio, radio), 0, 90);
            path.AddArc(new Rectangle(0, control.Height - radio, radio, radio), 90, 90);

            path.CloseFigure();

            control.Region = new Region(path);
        }

     
        // VALIDAR
     

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

     
        // LOGIN
       

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

  
        // ENTER PARA INGRESAR
      

        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnIngresar.PerformClick();
            }
        }

        
        // SALIR
     

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}