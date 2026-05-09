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
    public partial class FrmUnidadMedida : Form
    {
        public FrmUnidadMedida()
        {
            InitializeComponent();
        }

        private void listar()
        {
            var lista = UnidadMedidaCln.listarPa(txtParametro.Text);
            dgvLista.DataSource = null;
            dgvLista.DataSource = lista;

            if (dgvLista.Columns.Contains("id")) dgvLista.Columns["id"].Visible = false;
            if (dgvLista.Columns.Contains("estado")) dgvLista.Columns["estado"].Visible = false;

            if (dgvLista.Columns.Contains("nombre"))
                dgvLista.Columns["nombre"].HeaderText = "Unidad de Medida";

            if (dgvLista.Columns.Contains("usuarioRegistro"))
                dgvLista.Columns["usuarioRegistro"].HeaderText = "Usuario";

            if (dgvLista.Columns.Contains("fechaRegistro"))
            {
                dgvLista.Columns["fechaRegistro"].HeaderText = "Fecha";
                dgvLista.Columns["fechaRegistro"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
        }

        private void FrmUnidadMedida_Load(object sender, EventArgs e)
        {
            listar();
        }

        private void txtParametro_TextChanged(object sender, EventArgs e)
        {
            listar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            var frm = new FrmUnidadMedidaEntry(0); // 0 significa nuevo
            frm.ShowDialog();
            listar();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow != null && dgvLista.Rows.Count > 0)
            {
                int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
                var frm = new FrmUnidadMedidaEntry(id);
                frm.ShowDialog();
                listar();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una unidad de la lista para editar.",
                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            string nombre = dgvLista.CurrentRow.Cells["nombre"].Value.ToString();

            DialogResult result = MessageBox.Show($"¿Está seguro que desea eliminar la UnidadMedida {nombre}?",
                "::: Ferreteria :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                UnidadMedidaCln.eliminar(id, "admin"); // Llamamos a la lógica, no a la DB directo
                listar();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
