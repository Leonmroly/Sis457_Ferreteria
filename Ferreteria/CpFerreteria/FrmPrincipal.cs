using ClnFerreteria;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpFerreteria
{
    public partial class FrmPrincipal : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );


        private void FrmPrincipal_Activated(object sender, EventArgs e)
        {
            actualizarDashboardAutomatico();
        }


        FrmAutenticacion frmAutenticacion;
        public FrmPrincipal(FrmAutenticacion frmAutenticacion)
        {
            InitializeComponent();
            this.frmAutenticacion = frmAutenticacion;
        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Util.usuario = null;
            frmAutenticacion.Show();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            new FrmProducto().ShowDialog();
        }

        private void btnMarca_Click(object sender, EventArgs e)
        {
            new FrmMarca().ShowDialog();
        }

        private void btnCategoria_Click(object sender, EventArgs e)
        {
            new FrmCategoria().ShowDialog();
        }

        private void btnUnidadMedida_Click(object sender, EventArgs e)
        {
            new FrmUnidadMedida().ShowDialog();
        }

        private void btnProveedor_Click(object sender, EventArgs e)
        {
            new FrmProveedor().ShowDialog();
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            new FrmCliente().ShowDialog();
        }

        private void btnEmpleado_Click(object sender, EventArgs e)
        {
            new FrmEmpleado().ShowDialog();
        }

        private void btnUsuario_Click(object sender, EventArgs e)
        {
            new FrmUsuario().ShowDialog();
        }

        private void btnCompra_Click(object sender, EventArgs e)
        {
            new FrmCompra().ShowDialog();
        }

        private void btnVenta_Click(object sender, EventArgs e)
        {
            new FrmVenta().ShowDialog();
        }

        private void btnPerfil_Click(object sender, EventArgs e)
        {
            FrmPerfil pantallaPerfil = new FrmPerfil();

            pantallaPerfil.StartPosition = FormStartPosition.CenterParent;
            pantallaPerfil.ShowDialog();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            lblBienvenida.Text = $"¡Hola, bienvenido\n de nuevo,  {Util.usuario.usuario1}!";
            actualizarDashboardAutomatico();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void actualizarDashboardAutomatico()
        {
            try
            {
                decimal totalVentas = VentaCln.obtenerTotalVentasHoy();
                lblNumVentas.Text = totalVentas.ToString("N2") + " Bs.";

                int totalProductos = ProductoCln.obtenerCantidadProductos();
                lblNumCantidad.Text = totalProductos.ToString();

                int totalClientes = ClienteCln.obtenerCantidadClientes();
                lblNumClientes.Text = totalClientes.ToString();

                decimal totalCompras = CompraCln.obtenerTotalComprasHoy();
                lblNumCompras.Text = totalCompras.ToString("N2") + " Bs.";

                int alertas = ProductoCln.obtenerProductosBajos();
                lblNumAlertas.Text = alertas.ToString();

                if (alertas > 0) lblNumAlertas.ForeColor = Color.Red;
                else lblNumAlertas.ForeColor = Color.Black;

                int totalProveedores = ProveedorCln.obtenerCantidadProveedores();
                lblNumProveedores.Text = totalProveedores.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos automáticos: {ex.Message}", "Error de Conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            actualizarDashboardAutomatico();
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            int alertas = ProductoCln.obtenerProductosBajos();

            if (alertas > 0)
            {
                FrmAlertasStock ventana = new FrmAlertasStock();
                ventana.StartPosition = FormStartPosition.CenterParent;
                ventana.ShowDialog();
            }
            else
            {
                MessageBox.Show("¡Todo excelente! No hay productos con stock bajo en este momento.",
                                "Inventario al día", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int alertas = ProductoCln.obtenerProductosBajos();

            if (alertas > 0)
            {
                FrmAlertasStock ventana = new FrmAlertasStock();
                ventana.StartPosition = FormStartPosition.CenterParent;
                ventana.ShowDialog();
            }
            else
            {
                MessageBox.Show("¡Todo excelente! No hay productos con stock bajo en este momento.",
                                "Inventario al día", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lblNumAlertas_Click(object sender, EventArgs e)
        {
            panel5_Click(sender, e);
        }

        private void label22_Click(object sender, EventArgs e)
        {
            panel5_Click(sender, e);
        }

    }
}
