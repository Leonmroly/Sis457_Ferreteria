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
    public partial class FrmCompra : Form
    {
        public FrmCompra()
        {
            InitializeComponent();
        }

        private void listar()
        {
            try
            {
                var lista = CompraCln.listar();

                string parametro = txtParametro.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(parametro))
                {
                    lista = lista.Where(x =>
                        x.proveedor.ToLower().Contains(parametro) ||
                        x.usuarioRegistro.ToLower().Contains(parametro)
                    ).ToList();
                }

                dgvLista.DataSource = null;
                dgvLista.DataSource = lista;

                dgvLista.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dgvLista.Columns.Contains("id")) dgvLista.Columns["id"].Visible = false;

                if (dgvLista.Columns.Contains("fecha")) dgvLista.Columns["fecha"].HeaderText = "Fecha Compra";
                if (dgvLista.Columns.Contains("proveedor")) dgvLista.Columns["proveedor"].HeaderText = "Proveedor";
                if (dgvLista.Columns.Contains("total")) dgvLista.Columns["total"].HeaderText = "Total ($)";
                if (dgvLista.Columns.Contains("usuarioRegistro")) dgvLista.Columns["usuarioRegistro"].HeaderText = "Usuario";

                if (lista.Count > 0 && dgvLista.Columns.Contains("fecha"))
                    dgvLista.CurrentCell = dgvLista.Rows[0].Cells["fecha"];

                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en el flujo de datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmCompra_Load(object sender, EventArgs e)
        {
            this.Size = new Size(758, 363);
            listar();

            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
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

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            FrmCompraNuevo nuevoForm = new FrmCompraNuevo();
            nuevoForm.ShowDialog();
            listar();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvLista_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLista.CurrentRow != null)
            {
                // 1. Extraemos el ID de la fila seleccionada
                int idCompra = Convert.ToInt32(dgvLista.CurrentRow.Cells["id"].Value);

                // 2. Extraemos el Total acumulado de la columna "total" de esa misma fila
                decimal total = Convert.ToDecimal(dgvLista.CurrentRow.Cells["total"].Value);

                // 3. Abrimos la ventana de detalles pasándole AMBOS valores de forma higiénica
                FrmCompraDetalle detalleForm = new FrmCompraDetalle(idCompra, total);
                detalleForm.ShowDialog();
            }
        }

        private void dgvLista_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLista.CurrentRow != null)
            {
                // 1. Extraemos el ID de la fila seleccionada
                int idCompra = Convert.ToInt32(dgvLista.CurrentRow.Cells["id"].Value);

                // 2. Extraemos el Total acumulado de la columna "total" de esa misma fila
                decimal total = Convert.ToDecimal(dgvLista.CurrentRow.Cells["total"].Value);

                // 3. Abrimos la ventana de detalles pasándole AMBOS valores de forma higiénica
                FrmCompraDetalle detalleForm = new FrmCompraDetalle(idCompra, total);
                detalleForm.ShowDialog();
            }
        }
    }
}