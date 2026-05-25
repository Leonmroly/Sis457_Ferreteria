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
    public partial class FrmCliente : Form
    {
        private bool esNuevo = false;
        private int idSeleccionado = 0;
        public FrmCliente()
        {
            InitializeComponent();
        }

        private void listar()
        {
            var lista = ClienteCln.listar()
                .Where(x => x.nombreCompleto.ToLower().Contains(txtParametro.Text.ToLower()))
                .ToList();

            dgvLista.DataSource = null;
            dgvLista.DataSource = lista;

            // Ocultar basura técnica y sensible
            if (dgvLista.Columns.Contains("id")) dgvLista.Columns["id"].Visible = false;
            if (dgvLista.Columns.Contains("estado")) dgvLista.Columns["estado"].Visible = false;
            if (dgvLista.Columns.Contains("password")) dgvLista.Columns["password"].Visible = false; // ¡Seguridad!
            if (dgvLista.Columns.Contains("tipo")) dgvLista.Columns["tipo"].Visible = false;

            // Nombres para humanos
            dgvLista.Columns["cedulaIdentidad"].HeaderText = "C.I.";
            dgvLista.Columns["nombreCompleto"].HeaderText = "Cliente";
            dgvLista.Columns["telefono"].HeaderText = "Celular/Tel.";
            dgvLista.Columns["direccion"].HeaderText = "Dirección";
            dgvLista.Columns["email"].HeaderText = "Correo/Usuario"; // El email sirve de Login
            dgvLista.Columns["usuarioRegistro"].HeaderText = "Atendido por";
        }

        private void FrmCliente_Load(object sender, EventArgs e)
        {
            Size = new Size(762, 337);
            txtParametro.Clear();
            listar();

        }

        private void txtParametro_TextChanged(object sender, EventArgs e)
        {
            listar();
        }

        private bool validar()
        {
            bool esValido = true;
            resetearErrores();

            if (string.IsNullOrWhiteSpace(txtCi.Text))
            {
                erpCi.SetError(txtCi, "C.I. requerido");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                erpNombre.SetError(txtNombre, "Nombre requerido");
                esValido = false;
            }

            // SI QUIERE CREAR CUENTA, VALIDAMOS EMAIL Y CLAVE
            if (cbCrearCuenta.Checked)
            {
                // Para tener cuenta, el Email es obligatorio porque será su usuario
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    erpEmail.SetError(txtEmail, "El email es obligatorio para el acceso");
                    esValido = false;
                }

                if (string.IsNullOrWhiteSpace(txtClave.Text))
                {
                    erpClave.SetError(txtClave, "Debes asignar una contraseña");
                    esValido = false;
                }
            }

            return esValido;
        } 

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(762, 508); // O el tamaño que uses para mostrar los campos
            limpiar();
            txtCi.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow != null)
            {
                esNuevo = false;
                // Obtenemos el ID de la celda, esto es lo único que necesitamos del Grid
                idSeleccionado = (int)dgvLista.CurrentRow.Cells["id"].Value;

                // ¡ESTO ES LO QUE VA A SOLUCIONAR TU VIDA! Traemos el dato fresco del SQL
                var clienteDB = ClienteCln.obtenerPa(idSeleccionado);

                if (clienteDB != null)
                {
                    txtCi.Text = clienteDB.cedulaIdentidad.ToString();
                    txtNombre.Text = clienteDB.nombreCompleto;
                    txtDireccion.Text = clienteDB.direccion;
                    txtTelefono.Text = clienteDB.telefono;
                    txtEmail.Text = clienteDB.email;

                    // REVISIÓN REAL: Si el tipo es 2, el checkbox se marca porque se lo ordena el SQL
                    if (clienteDB.tipo == 2)
                    {
                        cbCrearCuenta.Checked = true;
                        txtClave.Text = "********"; // Marcador para saber que hay clave
                    }
                    else
                    {
                        cbCrearCuenta.Checked = false;
                        txtClave.Clear();
                    }
                }

                Size = new Size(762, 508);
                txtCi.Focus();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow != null)
            {
                int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
                string nombre = dgvLista.CurrentRow.Cells["nombreCompleto"].Value.ToString();

                DialogResult dialog = MessageBox.Show($"¿Está seguro de eliminar al cliente '{nombre}'?", "::: Mensaje :::",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialog == DialogResult.Yes)
                {
                    // Usamos el usuario del Login y la clase de Clientes
                    ClienteCln.eliminar(id, Util.usuario.usuario1);
                    listar();
                    MessageBox.Show("Cliente eliminado correctamente");
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                Cliente c = new Cliente();
                c.cedulaIdentidad = long.Parse(txtCi.Text);
                c.nombreCompleto = txtNombre.Text.Trim();
                c.direccion = txtDireccion.Text.Trim();
                c.telefono = txtTelefono.Text.Trim();
                c.email = txtEmail.Text.Trim();
                string usuarioActual = Util.usuario.usuario1;

                if (esNuevo)
                {
                    if (cbCrearCuenta.Checked)
                    {
                        c.tipo = 2; // CLIENTE CON CUENTA
                        c.password = Util.Encrypt(txtClave.Text.Trim());
                    }
                    else
                    {
                        c.tipo = 1; // CLIENTE SIN CUENTA
                        c.password = null;
                    }
                    ClienteCln.crear(c, usuarioActual, cbCrearCuenta.Checked);
                }
                else
                {
                    c.id = idSeleccionado;
                    var clienteDB = ClienteCln.obtenerPa(idSeleccionado);

                    if (cbCrearCuenta.Checked)
                    {
                        c.tipo = 2; // FORZAMOS QUE SEA TIPO 2
                                    // Solo encriptamos si el usuario escribió una clave REAL (no los asteriscos)
                        if (!string.IsNullOrWhiteSpace(txtClave.Text) && txtClave.Text != "********")
                        {
                            c.password = Util.Encrypt(txtClave.Text.Trim());
                        }
                        else
                        {
                            c.password = clienteDB.password; // Mantenemos la que ya estaba en SQL
                        }
                    }
                    else
                    {
                        c.tipo = 1;
                        c.password = null;
                    }
                    ClienteCln.actualizar(c, usuarioActual);
                }

                listar();
                limpiar();
                Size = new Size(762, 337);
                MessageBox.Show("¡Guardado correctamente!");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlAcciones.Enabled = true;
            Size = new Size(762, 337);
        }


        private void limpiar()
        {
            txtParametro.Clear();
            txtCi.Clear();
            txtNombre.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtClave.Clear();
            cbCrearCuenta.Checked = false;

            resetearErrores();
        }

        private void resetearErrores()
        {
            erpCi.Clear();
            erpNombre.Clear();
            erpNombre.Clear();
            erpEmail.Clear();
            erpClave.Clear();
        }


       

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
