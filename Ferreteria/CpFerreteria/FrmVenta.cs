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
    public partial class FrmVenta : Form
    {
        public FrmVenta()
        {
            InitializeComponent();
        }

        private void listar()
        {
            try
            {
                var lista = VentaCln.listar();

                string parametro = txtParametro.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(parametro))
                {
                    lista = lista.Where(x =>
                        x.cliente.ToLower().Contains(parametro) ||
                        x.usuarioRegistro.ToLower().Contains(parametro)
                    ).ToList();
                }

                // 💡 CONFIGURACIÓN DE COLORES PARA EVITAR FILAS INVISIBLES
                dgvLista.BackgroundColor = System.Drawing.Color.White;
                dgvLista.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                dgvLista.DefaultCellStyle.ForeColor = System.Drawing.Color.Black; // Texto negro para que se vea SIEMPRE

                dgvLista.DefaultCellStyle.Font = new System.Drawing.Font(dgvLista.Font, System.Drawing.FontStyle.Regular);

                // Configuración cuando la fila está seleccionada (Azul con letras blancas)
                dgvLista.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(0, 120, 215);
                dgvLista.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                // Quitar la columna vacía de la izquierda (la de la flechita)
                dgvLista.RowHeadersVisible = false;

                dgvLista.DataSource = null;
                dgvLista.DataSource = lista;

                dgvLista.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dgvLista.Columns.Contains("id")) dgvLista.Columns["id"].Visible = false;

                if (dgvLista.Columns.Contains("fecha")) dgvLista.Columns["fecha"].HeaderText = "Fecha Venta";
                if (dgvLista.Columns.Contains("cliente")) dgvLista.Columns["cliente"].HeaderText = "Cliente";
                if (dgvLista.Columns.Contains("total")) dgvLista.Columns["total"].HeaderText = "Total ($)";
                if (dgvLista.Columns.Contains("usuarioRegistro")) dgvLista.Columns["usuarioRegistro"].HeaderText = "Usuario";

                dgvLista.BorderStyle = BorderStyle.FixedSingle;
                dgvLista.GridColor = System.Drawing.Color.LightGray;

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

        private void FrmVenta_Load(object sender, EventArgs e)
        {
            listar();

            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void txtParametro_TextChanged(object sender, EventArgs e)
        {
            listar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            FrmVentaNuevo nuevoForm = new FrmVentaNuevo();
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
                int idVenta = Convert.ToInt32(dgvLista.CurrentRow.Cells["id"].Value);

                // 2. Extraemos el Total acumulado de la columna "total" de esa misma fila
                decimal total = Convert.ToDecimal(dgvLista.CurrentRow.Cells["total"].Value);

                // 3. Abrimos la ventana de detalles pasándole AMBOS valores de forma higiénica
                FrmVentaDetalle detalleForm = new FrmVentaDetalle(idVenta, total);
                detalleForm.ShowDialog();
            }
        }
    }
}
