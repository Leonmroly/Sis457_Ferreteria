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
    public partial class FrmCategoriaEntry : Form
    {
        private int id = 0; // Guardará el ID si vamos a editar
        public FrmCategoriaEntry(int id = 0)
        {
            InitializeComponent();
            this.id = id;
            if (id > 0) cargarDatos(); // Si hay ID, cargamos los datos para editar
        }
        
        private void cargarDatos()
        {
            var categoria = CategoriaCln.obtenerUno(id);
            if (categoria != null)
            {
                txtNombre.Text = categoria.nombre;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Debe ingresar un nombre");
                return;
            }

            var categoria = new Categoria();
            categoria.nombre = txtNombre.Text.Trim();

            // 💡 SOLUCIÓN: Cambiamos el texto fijo por el usuario real de la sesión
            categoria.usuarioRegistro = Util.usuario.usuario1;

            if (id == 0) // NUEVO
            {
                categoria.fechaRegistro = DateTime.Now;
                categoria.estado = 1;
                CategoriaCln.crear(categoria);
            }
            else // EDITAR
            {
                categoria.id = id;
                CategoriaCln.modificar(categoria);
            }

            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
