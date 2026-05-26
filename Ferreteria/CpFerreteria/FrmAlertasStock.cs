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
    public partial class FrmAlertasStock : Form
    {
        public FrmAlertasStock()
        {
            InitializeComponent();
        }

        private void FrmAlertasStock_Load(object sender, EventArgs e)
        {
            dgvAlertas.DataSource = ProductoCln.listarProductosBajos();

            if (dgvAlertas.Columns["fechaReg"] != null) dgvAlertas.Columns["fechaReg"].Visible = false;
            if (dgvAlertas.Columns["estado"] != null) dgvAlertas.Columns["estado"].Visible = false;
            if (dgvAlertas.Columns["usuarioRegistro"] != null) dgvAlertas.Columns["usuarioRegistro"].Visible = false;
            if (dgvAlertas.Columns["id"] != null) dgvAlertas.Columns["id"].Visible = false;
            if (dgvAlertas.Columns["idSubCategoria"] != null) dgvAlertas.Columns["fechaRegistro"].Visible = false;
            if (dgvAlertas.Columns["idUnidadMedida"] != null) dgvAlertas.Columns["idUnidadMedida"].Visible = false;
            if (dgvAlertas.Columns["idMarca"] != null) dgvAlertas.Columns["idMarca"].Visible = false;
            if (dgvAlertas.Columns["idProveedor"] != null) dgvAlertas.Columns["idProveedor"].Visible = false;
            if (dgvAlertas.Columns["idCategoria"] != null) dgvAlertas.Columns["idCategoria"].Visible = false;
            if (dgvAlertas.Columns["idSubCategoria"] != null) dgvAlertas.Columns["idSubCategoria"].Visible = false;
            if (dgvAlertas.Columns["VentaDetalles"] != null) dgvAlertas.Columns["VentaDetalles"].Visible = false;
            if (dgvAlertas.Columns["CompraDetalles"] != null) dgvAlertas.Columns["CompraDetalles"].Visible = false;


            dgvAlertas.Columns["codigo"].HeaderText = "Código";
            dgvAlertas.Columns["descripcion"].HeaderText = "Producto en Riesgo";
            dgvAlertas.Columns["cantidad"].HeaderText = "Cantidad Actual";

            dgvAlertas.ReadOnly = true;
            dgvAlertas.AllowUserToAddRows = false;
            dgvAlertas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlertas.MultiSelect = false;
            dgvAlertas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
