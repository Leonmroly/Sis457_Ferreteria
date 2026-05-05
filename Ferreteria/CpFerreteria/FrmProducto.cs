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
    public partial class FrmProducto : Form
    {
        public FrmProducto()
        {
            InitializeComponent();
        }

        private void listar()
        {
            dgvLista.DataSource = ProductoCln.listarPa(txtParametro.Text);
            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["idSubCategoria"].Visible = false;
            dgvLista.Columns["idUnidadMedida"].Visible = false;
            dgvLista.Columns["idMarca"].Visible = false;
            dgvLista.Columns["estado"].Visible = false;
            dgvLista.Columns["stock"].Visible = false;
        }

        private void FrmProducto_Load(object sender, EventArgs e)
        {
            listar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }
    }
}
