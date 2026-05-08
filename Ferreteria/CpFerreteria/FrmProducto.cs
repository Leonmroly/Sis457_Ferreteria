using CadFerreteria;
using ClnFerreteria;
using System;
using System.Collections;
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
    public partial class FrmProducto : Form
    {
        private bool esNuevo = false;
        public FrmProducto()
        {
            InitializeComponent();
        }

        private void listar()
        {
            var lista = ProductoCln.listarPa(txtParametro.Text);
            dgvLista.DataSource = lista;
            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["idSubCategoria"].Visible = false;
            dgvLista.Columns["idUnidadMedida"].Visible = false;
            dgvLista.Columns["idMarca"].Visible = false;
            dgvLista.Columns["estado"].Visible = false;
            dgvLista.Columns["stock"].Visible = false;
            dgvLista.Columns["codigo"].HeaderText = "Código";
            dgvLista.Columns["descripcion"].HeaderText = "Descripción";
            dgvLista.Columns["unidadMedida"].HeaderText = "Unidad de Medida";
            dgvLista.Columns["stock"].HeaderText = "stock";
            dgvLista.Columns["precioVenta"].HeaderText = "Precio de Venta";
            dgvLista.Columns["usuarioRegistro"].HeaderText = "Usuario Registro";
            dgvLista.Columns["fechaRegistro"].HeaderText = "Fecha Registro";

            if (lista.Count > 0) dgvLista.CurrentCell = dgvLista.Rows[0].Cells["codigo"];
            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
        }

        private void cargarUnidadMedida()
        {
            cbxUnidadMedida.DataSource = UnidadMedidaCln.listar();
            cbxUnidadMedida.ValueMember = "id";
            cbxUnidadMedida.DisplayMember = "nombre";
            cbxUnidadMedida.SelectedIndex = -1;
        }

        private void FrmProducto_Load(object sender, EventArgs e)
        {
            Size = new Size(758, 342);
            listar();
            cargarUnidadMedida();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void txtParametro_TextChanged(object sender, EventArgs e)
        {
            listar();
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private void limpiar()
        {
            txtCodigo.Clear();
            txtDescripcion.Clear();
            cbxUnidadMedida.SelectedIndex = -1;
            nudSaldo.Value = 0;
            nudPrecioVenta.Value = 0;
            resetearErrores();
        }

        private void resetearErrores()
        {
            erpCodigo.Clear();
            erpDescripcion.Clear();
            erpUnidadMedida.Clear();
            erpSaldo.Clear();
            erpPrecioVenta.Clear();
        }
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            pnlAcciones.Enabled = false;
            Size = new Size(758, 505);
            limpiar();
            txtCodigo.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            pnlAcciones.Enabled = false;
            Size = new Size(758, 505);
            resetearErrores();

            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            var producto = ProductoCln.obtenerUno(id);
            txtCodigo.Text = producto.codigo;
            txtDescripcion.Text = producto.descripcion;
            cbxUnidadMedida.SelectedValue = producto.idUnidadMedida;
            nudSaldo.Value = producto.saldo;
            nudPrecioVenta.Value = producto.precioVenta;

            txtCodigo.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlAcciones.Enabled = true;
            Size = new Size(758, 342);
        }

        private bool validar()
        {
            bool esValido = true;
            erpCodigo.Clear();
            erpDescripcion.Clear();
            erpPrecioVenta.Clear();
            erpSaldo.Clear();
            erpUnidadMedida.Clear();
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                erpCodigo.SetError(txtCodigo, "Ingrese el código del producto");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                erpDescripcion.SetError(txtDescripcion, "Ingrese la descripción del producto");
                esValido = false;
            }
            if (string.IsNullOrEmpty(cbxUnidadMedida.Text))
            {
                erpUnidadMedida.SetError(cbxUnidadMedida, "Seleccione la unidad de medida");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(nudSaldo.ToString()))
            {
                erpSaldo.SetError(nudSaldo, "Ingrese un saldo válido");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(nudPrecioVenta.ToString()))
            {
                erpPrecioVenta.SetError(nudPrecioVenta, "Ingrese un precio de venta válido");
                esValido = false;
            }
            if (nudPrecioVenta.Value <= 0)
            {
                erpPrecioVenta.SetError(nudPrecioVenta, "El precio de venta debe ser mayor a cero");
                esValido = false;
            }

            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                var producto = new Producto()
                {
                    codigo = txtCodigo.Text.Trim(),
                    descripcion = txtDescripcion.Text.Trim(),
                    idUnidadMedida = (int)cbxUnidadMedida.SelectedValue,
                    saldo = nudSaldo.Value,
                    precioVenta = nudPrecioVenta.Value,
                    usuarioRegistro = Util.usuario.usuario1,

                    idSubCategoria = 1,
                    idMarca = 1
                };

                if (esNuevo)
                {
                    producto.fechaRegistro = DateTime.Now;
                    producto.estado = 1;
                    ProductoCln.crear(producto);
                }
                else
                {
                    producto.id = (int)dgvLista.CurrentRow.Cells["id"].Value;

                    producto.fechaRegistro = DateTime.Now;

                    producto.idSubCategoria = 1;
                    producto.idMarca = 1;
                    producto.estado = 1;

                    ProductoCln.modificar(producto);
                }
                listar();
                btnCancelar.PerformClick();
                MessageBox.Show("Producto guardado correctamente", "::: Mensaje - Ferreteria :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
