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
                .Where(x => x.nombreCompleto
                .ToLower().Contains(txtParametro.Text
                .ToLower()))
                .ToList(); // Aquí puedes usar ToList directamente sin el Select New si quieres

            dgvLista.DataSource = null;
            dgvLista.DataSource = lista;

            // Solo ocultamos lo que el usuario no ve
            if (dgvLista.Columns.Contains("id")) dgvLista.Columns["id"].Visible = false;
            if (dgvLista.Columns.Contains("estado")) dgvLista.Columns["estado"].Visible = false;
            if (dgvLista.Columns.Contains("usuarioRegistro")) dgvLista.Columns["usuarioRegistro"].Visible = false;
            if (dgvLista.Columns.Contains("fechaRegistro")) dgvLista.Columns["fechaRegistro"].Visible = false;

            // Nombres bonitos
            dgvLista.Columns["cedulaIdentidad"].HeaderText = "NIT/CI";
            dgvLista.Columns["nombreCompleto"].HeaderText = "Nombre / Dirección";
        }

        private void FrmCliente_Load(object sender, EventArgs e)
        {
            Size = new Size(762, 337);
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
                erpCi.SetError(txtCi, "Ingrese la cédula de identidad");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                erpNombre.SetError(txtNombre, "Ingrese su nombre completo");
                esValido = false;
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
                idSeleccionado = (int)dgvLista.CurrentRow.Cells["id"].Value;
                txtCi.Text = dgvLista.CurrentRow.Cells["cedulaIdentidad"].Value.ToString();
                txtNombre.Text = dgvLista.CurrentRow.Cells["nombreCompleto"].Value.ToString();

                Size = new Size(762, 508);
                txtCi.Focus();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            string codigo = dgvLista.CurrentRow.Cells["codigo"].Value.ToString();
            DialogResult dialog = MessageBox.Show($"¿Está seguro de eliminar el producto con código '{codigo}'?", "::: Mensaje - Ferreteria :::",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                ProductoCln.eliminar(id, Util.usuario.usuario1);
                listar();
                MessageBox.Show("Producto eliminado correctamente", "::: Mensaje - Ferreteria :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                Cliente c = new Cliente();
                // Ahora usamos los nombres exactos de la nueva tabla
                c.cedulaIdentidad = long.Parse(txtCi.Text);
                c.nombreCompleto = txtNombre.Text.Trim();
                c.direccion = txtDireccion.Text.Trim();
                c.telefono = txtTelefono.Text.Trim();
                c.email = txtEmail.Text.Trim();

                if (esNuevo)
                {
                    ClienteCln.crear(c);
                    MessageBox.Show("Cliente registrado");
                }
                else
                {
                    c.id = idSeleccionado;
                    ClienteCln.actualizar(c);
                    MessageBox.Show("Cliente actualizado");
                }
                listar();
                limpiar();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlAcciones.Enabled = true;
            Size = new Size(762, 337);
        }


        private void limpiar()
        {
            txtCi.Clear();
            txtNombre.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();

            resetearErrores();
        }

        private void resetearErrores()
        {
            erpCi.Clear();
            erpNombre.Clear();
            erpNombre.Clear();
        }


       

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
