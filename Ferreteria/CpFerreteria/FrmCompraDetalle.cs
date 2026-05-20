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
    public partial class FrmCompraDetalle : Form
    {
        private int idCompraSeleccionada;
        private decimal totalCompra;
        public FrmCompraDetalle(int idCompraParam, decimal totalParam)
        {
            InitializeComponent();
            this.idCompraSeleccionada = idCompraParam;
            this.totalCompra = totalParam;
        }

        private void FrmCompraDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Limpieza total de residuos del diseñador
                dgvDetalle.DataSource = null;
                dgvDetalle.Columns.Clear();
                dgvDetalle.AutoGenerateColumns = true;

                // 2. Llamamos al método de la capa de negocio
                var listaArticulos = CompraCln.obtenerDetalle(idCompraSeleccionada);

                // 3. ¡CONTROL DE AUDITORÍA!: Si la lista viene vacía, te avisará con un mensaje
                if (listaArticulos == null || listaArticulos.Count == 0)
                {
                    MessageBox.Show(
                        $"Atención: La consulta no devolvió filas para la Compra ID: {idCompraSeleccionada}.\n" +
                        "Esto significa que la cabecera existe pero no hay registros reales guardados en la tabla CompraDetalle.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information
                    );
                    return;
                }

                // 4. Si tiene datos, los enlazamos
                dgvDetalle.DataSource = listaArticulos;
                dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // 5. Renombramos las cabeceras para que queden estéticas
                if (dgvDetalle.Columns.Contains("Producto")) dgvDetalle.Columns["Producto"].HeaderText = "Producto";
                if (dgvDetalle.Columns.Contains("Cantidad")) dgvDetalle.Columns["Cantidad"].HeaderText = "Cantidad";
                if (dgvDetalle.Columns.Contains("PrecioUnitario")) dgvDetalle.Columns["PrecioUnitario"].HeaderText = "Precio Unitario";
                if (dgvDetalle.Columns.Contains("Subtotal")) dgvDetalle.Columns["Subtotal"].HeaderText = "Subtotal";

                // 6. Seteamos la etiqueta del total
                lblTotal.Text = $"TOTAL COMPRA: {totalCompra.ToString("N2")} $";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error crítico al renderizar la grilla: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
