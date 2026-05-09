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
    public partial class FrmUnidadMedidaEntry : Form
    {
        private int id;
        public FrmUnidadMedidaEntry(int id)
        {
            InitializeComponent();
            this.id = id;
            if (id > 0) cargarDatos();
        }

        private void cargarDatos()
        {
            var unidad = UnidadMedidaCln.obtenerUno(id);
            txtNombre.Text = unidad.nombre;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Debe ingresar un nombre");
                return;
            }

            var unidad = new UnidadMedida();
            unidad.nombre = txtNombre.Text.Trim();
            unidad.usuarioRegistro = "admin"; // Aquí podrías usar una variable global de sesión

            if (id == 0) // NUEVO
            {
                unidad.fechaRegistro = DateTime.Now;
                unidad.estado = 1;
                UnidadMedidaCln.crear(unidad);
            }
            else // EDITAR
            {
                unidad.id = id;
                UnidadMedidaCln.modificar(unidad);
            }
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
