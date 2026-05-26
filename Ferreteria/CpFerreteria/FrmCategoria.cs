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
    public partial class FrmCategoria : Form
    {
        public FrmCategoria()
        {
            InitializeComponent();
        }
        private void FrmCategoria_Load(object sender, EventArgs e)
        {
            listar();
        }

        private void listar()
        {
            var lista = CategoriaCln.listarPa(txtParametro.Text);
            dgvLista.DataSource = lista;

            if (dgvLista.Columns.Contains("id")) dgvLista.Columns["id"].Visible = false;
            if (dgvLista.Columns.Contains("estado")) dgvLista.Columns["estado"].Visible = false;

            if (dgvLista.Columns.Contains("nombre"))
                dgvLista.Columns["nombre"].HeaderText = "Categoría";

            if (dgvLista.Columns.Contains("usuarioRegistro"))
                dgvLista.Columns["usuarioRegistro"].HeaderText = "Usuario Registro";

            if (dgvLista.Columns.Contains("fechaRegistro"))
            {
                dgvLista.Columns["fechaRegistro"].HeaderText = "Fecha Registro";
                dgvLista.Columns["fechaRegistro"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
        }

        private void txtParametro_TextChanged(object sender, EventArgs e)
        {
            listar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            FrmCategoriaEntry frm = new FrmCategoriaEntry();
            frm.ShowDialog();
            listar();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            FrmCategoriaEntry frm = new FrmCategoriaEntry(id);
            frm.ShowDialog();
            listar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            string nombre = dgvLista.CurrentRow.Cells["nombre"].Value.ToString();

            DialogResult result = MessageBox.Show($"¿Está seguro que desea eliminar la categoria {nombre}?",
                "::: Ferreteria :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                CategoriaCln.eliminar(id, "admin");
                listar();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
