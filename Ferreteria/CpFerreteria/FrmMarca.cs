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
    public partial class FrmMarca : Form
    {
        public FrmMarca()
        {
            InitializeComponent();
        }

        private void FrmMarca_Load(object sender, EventArgs e)
        {
            listar();
        }

        private void listar()
        {
            var lista = MarcaCln.listarPa(txtParametro.Text);
            dgvLista.DataSource = lista;

            // Solo ocultamos las columnas que realmente devuelve paMarcaListar
            if (dgvLista.Columns.Contains("id"))
                dgvLista.Columns["id"].Visible = false;

            // Esta línea era la del error, ya no es necesaria en este formulario
            // dgvLista.Columns["idMarca"].Visible = false; 

            if (dgvLista.Columns.Contains("estado"))
                dgvLista.Columns["estado"].Visible = false;

            // Mejoramos el aspecto de la tabla
            if (dgvLista.Columns.Contains("nombre"))
                dgvLista.Columns["nombre"].HeaderText = "Nombre de la Marca";

            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
        }

        private void txtParametro_TextChanged(object sender, EventArgs e)
        {
            listar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            FrmMarcaEntry frm = new FrmMarcaEntry(); // ID por defecto es 0
            frm.ShowDialog();
            listar();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            FrmMarcaEntry frm = new FrmMarcaEntry(id); // Le pasamos el ID para editar
            frm.ShowDialog();
            listar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            string nombre = dgvLista.CurrentRow.Cells["nombre"].Value.ToString();

            DialogResult result = MessageBox.Show($"¿Está seguro que desea eliminar la marca {nombre}?",
                "::: Ferreteria :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MarcaCln.eliminar(id, "admin"); // Llamamos a la lógica, no a la DB directo
                listar();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
