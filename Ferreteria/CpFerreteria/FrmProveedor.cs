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
    public partial class FrmProveedor : Form
    {
        private bool esNuevo = false;
        private int idSeleccionado = 0;
        public FrmProveedor()
        {
            InitializeComponent();
        }

        private void FrmProveedor_Load(object sender, EventArgs e)
        {
            Size = new Size(758, 335);
            listar();
            
        }

        private void listar()
        {
            var lista = ProveedorCln.listar()
        .Where(x => x.razonSocial.ToLower().Contains(txtParametro.Text.ToLower()))
            .Select(x => new {
                x.id,
                x.nit,
                x.razonSocial,
                x.direccion,
                x.telefono,
                x.email,
                x.usuarioRegistro,
                x.fechaRegistro,
                x.estado
            }).ToList(); ;

            dgvLista.DataSource = null;
            dgvLista.DataSource = lista;

            // --- ESTAS LÍNEAS SON LAS QUE EVITAN EL ERROR ---
            if (dgvLista.Columns.Contains("Compras")) dgvLista.Columns["Compras"].Visible = false;
            // ------------------------------------------------

            // El resto de tus ocultaciones que ya tenías
            if (dgvLista.Columns.Contains("id")) dgvLista.Columns["id"].Visible = false;
            if (dgvLista.Columns.Contains("estado")) dgvLista.Columns["estado"].Visible = false;
            if (dgvLista.Columns.Contains("usuarioRegistro")) dgvLista.Columns["usuarioRegistro"].Visible = false;
            if (dgvLista.Columns.Contains("fechaRegistro")) dgvLista.Columns["fechaRegistro"].Visible = false;

            // Encabezados (Asegúrate de que los nombres coincidan con tu base de datos)
            dgvLista.Columns["nit"].HeaderText = "NIT/CI";
            dgvLista.Columns["razonSocial"].HeaderText = "Razón Social";
            dgvLista.Columns["direccion"].HeaderText = "Dirección";
            dgvLista.Columns["telefono"].HeaderText = "Teléfono";
            dgvLista.Columns["email"].HeaderText = "Email";
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            pnlAcciones.Enabled = false;
            Size = new Size(758, 485);
            limpiar();
            txtNit.Focus();
        }



        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow != null)
            {
                esNuevo = false;
                pnlAcciones.Enabled = false;
                Size = new Size(758, 485);
                resetearErrores();

                idSeleccionado = (int)dgvLista.CurrentRow.Cells["id"].Value;

                // Subimos los datos del Grid a los cuadros de texto de abajo
                txtNit.Text = dgvLista.CurrentRow.Cells["nit"].Value.ToString();
                txtRazonSocial.Text = dgvLista.CurrentRow.Cells["razonSocial"].Value.ToString();
                txtDireccion.Text = dgvLista.CurrentRow.Cells["direccion"].Value?.ToString();
                txtTelefono.Text = dgvLista.CurrentRow.Cells["telefono"].Value?.ToString();
                txtEmail.Text = dgvLista.CurrentRow.Cells["email"].Value?.ToString();

                txtNit.Focus();
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow != null)
            {
                int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
                if (MessageBox.Show("¿Eliminar proveedor?", "Aviso", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProveedorCln.eliminar(id);
                    listar(); // <-- Corregido: antes decía actualizarGrid
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar()) // Crea un validar() que solo chequee NIT y Razón Social
            {
                Proveedor p = new Proveedor();
                p.nit = long.Parse(txtNit.Text);
                p.razonSocial = txtRazonSocial.Text.Trim();
                p.direccion = txtDireccion.Text.Trim();
                p.telefono = txtTelefono.Text.Trim();
                p.email = txtEmail.Text.Trim();

                if (esNuevo)
                {
                    ProveedorCln.crear(p);
                    MessageBox.Show("Proveedor guardado");
                }
                else
                {
                    p.id = idSeleccionado;
                    ProveedorCln.actualizar(p);
                    MessageBox.Show("Proveedor actualizado");
                }
                listar();
                btnCancelar.PerformClick();
                MessageBox.Show("¡Guardado con éxito!");
                limpiar();
            }
        }

        private void resetearErrores()
        {
            erpNit.Clear();
            erpRazonSocial.Clear();
            erpDireccion.Clear();
            erpTelefono.Clear();
            erpEmail.Clear();
        }

        private void limpiar()
        {
            txtNit.Clear();
            txtRazonSocial.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            resetearErrores();
        }


        private void txtParametro_TextChanged(object sender, EventArgs e)
        {
            listar();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlAcciones.Enabled = true;
            Size = new Size(758, 335);
        }

        private bool validar()
        {
            bool esValido = true;
            resetearErrores();

            if (string.IsNullOrWhiteSpace(txtNit.Text))
            {
                erpNit.SetError(txtNit, "Ingrese el NIT del proveedor");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtRazonSocial.Text))
            {
                erpRazonSocial.SetError(txtRazonSocial, "Ingrese la razón social");
                esValido = false;
            }

            return esValido;
        }


        private void dgvLista_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Esto silencia cualquier error visual del Grid
            e.ThrowException = false;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
