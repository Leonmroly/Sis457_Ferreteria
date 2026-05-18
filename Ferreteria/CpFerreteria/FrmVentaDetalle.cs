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
    public partial class FrmVentaDetalle : Form
    {
        private int idVentaSeleccionada;
        private decimal totalVenta;

        public FrmVentaDetalle(int idVentaParam, decimal totalParam)
        {
            InitializeComponent();
            this.idVentaSeleccionada = idVentaParam;
            this.totalVenta = totalParam;
        }

        private void FrmVentaDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                // Cargamos la lista de artículos ventados
                dgvDetalle.DataSource = VentaCln.obtenerDetalle(idVentaSeleccionada);
                dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Modificamos las cabeceras para que tengan espacios legibles
                if (dgvDetalle.Columns.Contains("PrecioUnitario"))
                    dgvDetalle.Columns["PrecioUnitario"].HeaderText = "Precio Unitario";

                // PINTAMOS EL TOTAL EN LA ETIQUETA: Con formato de dos decimales
                lblTotal.Text = $"TOTAL VENTA: {totalVenta.ToString("N2")} $";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los artículos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
