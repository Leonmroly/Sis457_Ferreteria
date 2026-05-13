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
            dgvLista.DataSource = null;
            dgvLista.DataSource = lista;

            // Ocultamos los IDs (Usando idSubCategoria que es el real)
            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["idSubCategoria"].Visible = false;
            dgvLista.Columns["idUnidadMedida"].Visible = false;
            dgvLista.Columns["idMarca"].Visible = false;
            dgvLista.Columns["estado"].Visible = false;

            // Encabezados
            dgvLista.Columns["codigo"].HeaderText = "Código";
            dgvLista.Columns["descripcion"].HeaderText = "Descripción";
            dgvLista.Columns["unidadMedida"].HeaderText = "U. Medida";
            dgvLista.Columns["marca"].HeaderText = "Marca";

            // Solo mostramos categoría si el EDMX ya se actualizó
            if (dgvLista.Columns.Contains("categoria"))
                dgvLista.Columns["categoria"].HeaderText = "Categoría";

            dgvLista.Columns["saldo"].HeaderText = "Saldo"; // Solo uno
            dgvLista.Columns["precioVenta"].HeaderText = "Precio";
            dgvLista.Columns["usuarioRegistro"].HeaderText = "Usuario";
            dgvLista.Columns["fechaRegistro"].HeaderText = "Fecha";

            // Evitamos el error de la celda invisible (foco en código)
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

        private void cargarCombos()
        {
            cbxMarca.DataSource = MarcaCln.listar();
            cbxMarca.DisplayMember = "nombre";
            cbxMarca.ValueMember = "id";
            cbxMarca.SelectedIndex = -1; // Esto lo deja vacío al inicio

            cbxCategoria.DataSource = SubCategoriaCln.listar();
            cbxCategoria.DisplayMember = "nombre"; // Mostrará "Martillos", "Cables", etc.
            cbxCategoria.ValueMember = "id";       // Mandará los IDs 1, 2, 3 que SQL SÍ tiene
            cbxCategoria.SelectedIndex = -1;
        }

        private void FrmProducto_Load(object sender, EventArgs e)
        {
            Size = new Size(758, 345);
            listar();
            cargarUnidadMedida();
            cargarCombos();
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
            cbxMarca.SelectedIndex = -1;
            cbxCategoria.SelectedIndex = -1;
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
            Size = new Size(758, 573);
            limpiar();
            txtCodigo.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            pnlAcciones.Enabled = false;
            Size = new Size(758, 573);
            resetearErrores();

            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            var producto = ProductoCln.obtenerUno(id);
            txtCodigo.Text = producto.codigo;
            txtDescripcion.Text = producto.descripcion;
            cbxUnidadMedida.SelectedValue = producto.idUnidadMedida;
            cbxMarca.SelectedValue = producto.idMarca;
            cbxCategoria.SelectedValue = producto.idSubCategoria;
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
                // ESTA LÍNEA ES LA CLAVE:
                // 'cbxCategoria' ahora tiene los IDs de la tabla SubCategoria
                int idSubCat = Convert.ToInt32(cbxCategoria.SelectedValue);

                var producto = new Producto()
                {
                    codigo = txtCodigo.Text.Trim(),
                    descripcion = txtDescripcion.Text.Trim(),
                    idUnidadMedida = Convert.ToInt32(cbxUnidadMedida.SelectedValue),
                    idMarca = Convert.ToInt32(cbxMarca.SelectedValue),
                    idSubCategoria = idSubCat, // AQUÍ pones el ID que sacaste del combo
                    saldo = nudSaldo.Value,
                    precioVenta = nudPrecioVenta.Value,
                    usuarioRegistro = Util.usuario.usuario1,
                    estado = 1
                };

                if (esNuevo)
                {
                    producto.fechaRegistro = DateTime.Now;
                    ProductoCln.crear(producto);
                }
                else
                {
                    // Para el 'modificar', también necesitamos el ID real del registro
                    producto.id = (int)dgvLista.CurrentRow.Cells["id"].Value;
                    ProductoCln.modificar(producto);
                }

                listar();
                btnCancelar.PerformClick();
                MessageBox.Show("¡Guardado con éxito!");
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
