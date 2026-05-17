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
                // Cargamos la lista de artículos comprados
                dgvDetalle.DataSource = CompraCln.obtenerDetalle(idCompraSeleccionada);
                dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Modificamos las cabeceras para que tengan espacios legibles
                if (dgvDetalle.Columns.Contains("PrecioUnitario"))
                    dgvDetalle.Columns["PrecioUnitario"].HeaderText = "Precio Unitario";

                // PINTAMOS EL TOTAL EN LA ETIQUETA: Con formato de dos decimales
                lblTotal.Text = $"TOTAL COMPRA: {totalCompra.ToString("N2")} $";
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
