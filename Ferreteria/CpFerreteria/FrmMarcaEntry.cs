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
    public partial class FrmMarcaEntry : Form
    {
        private int id = 0; // Guardará el ID si vamos a editar

        public FrmMarcaEntry(int id = 0) // El constructor recibe el ID (por defecto 0)
        {
            InitializeComponent();
            this.id = id;
            if (id > 0) cargarDatos(); // Si hay ID, cargamos los datos para editar
        }

        private void cargarDatos()
        {
            var marca = MarcaCln.obtenerUno(id);
            txtNombre.Text = marca.nombre;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Debe ingresar un nombre");
                return;
            }

            var marca = new Marca();
            marca.nombre = txtNombre.Text.Trim();
            marca.usuarioRegistro = "admin";

            if (id == 0) // NUEVO
            {
                marca.fechaRegistro = DateTime.Now;
                marca.estado = 1;
                MarcaCln.crear(marca);
            }
            else // EDITAR
            {
                marca.id = id;
                MarcaCln.modificar(marca);
            }

            this.Close(); // Cerramos al terminar
        }
    }
}
