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
    public partial class FrmEmpleado : Form
    {
        private bool esNuevo = false;
        private int idSeleccionado = 0;
        public FrmEmpleado()
        {
            InitializeComponent();
        }

        private void listar()
        {
            var lista = EmpleadoCln.listar();

            // Filtrar si hay algo en el buscador
            if (!string.IsNullOrWhiteSpace(txtParametro.Text))
            {
                lista = lista.Where(x => x.nombres.ToLower().Contains(txtParametro.Text.ToLower()) ||
                                         x.primerApellido.ToLower().Contains(txtParametro.Text.ToLower()) ||
                                         x.cedulaIdentidad.Contains(txtParametro.Text)).ToList();
            }

            dgvLista.DataSource = null;
            dgvLista.DataSource = lista;

            // Ocultamos columnas internas de auditoría y relaciones
            if (dgvLista.Columns["id"] != null) dgvLista.Columns["id"].Visible = false;
            if (dgvLista.Columns["usuarioRegistro"] != null) dgvLista.Columns["usuarioRegistro"].Visible = false;
            if (dgvLista.Columns["fechaRegistro"] != null) dgvLista.Columns["fechaRegistro"].Visible = false;
            if (dgvLista.Columns["estado"] != null) dgvLista.Columns["estado"].Visible = false;
            if (dgvLista.Columns["Usuarios"] != null) dgvLista.Columns["Usuarios"].Visible = false;

            // Encabezados bonitos en la grilla
            dgvLista.Columns["cedulaIdentidad"].HeaderText = "Cédula Identidad";
            dgvLista.Columns["nombres"].HeaderText = "Nombres";
            dgvLista.Columns["primerApellido"].HeaderText = "Primer Apellido";
            dgvLista.Columns["segundoApellido"].HeaderText = "Segundo Apellido";
            dgvLista.Columns["fechaNacimiento"].HeaderText = "F. Nacimiento";
            dgvLista.Columns["direccion"].HeaderText = "Dirección";
            dgvLista.Columns["celular"].HeaderText = "Celular";
            dgvLista.Columns["cargo"].HeaderText = "Cargo";

            // Foco inicial seguro
            if (lista.Count > 0) dgvLista.CurrentCell = dgvLista.Rows[0].Cells["cedulaIdentidad"];

            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
        }

        private void FrmEmpleado_Load(object sender, EventArgs e)
        {
            Size = new Size(759, 352);
            listar();
            gbxDatos.Enabled = false;
        }
        private void CargarLista(string buscar = "")
        {
            List<Empleado> lista = EmpleadoCln.listar();

            // Si el usuario escribió algo en el cuadro de búsqueda superior
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                lista = lista.Where(x => x.nombres.ToLower().Contains(buscar.ToLower()) ||
                                         x.primerApellido.ToLower().Contains(buscar.ToLower()) ||
                                         x.cedulaIdentidad.Contains(buscar)).ToList();
            }

            dgvLista.DataSource = lista;

            // Ocultamos lo innecesario para el usuario final
            if (dgvLista.Columns["usuarioRegistro"] != null) dgvLista.Columns["usuarioRegistro"].Visible = false;
            if (dgvLista.Columns["fechaRegistro"] != null) dgvLista.Columns["fechaRegistro"].Visible = false;
            if (dgvLista.Columns["estado"] != null) dgvLista.Columns["estado"].Visible = false;
            if (dgvLista.Columns["Usuarios"] != null) dgvLista.Columns["Usuarios"].Visible = false;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }
        private void txtParametro_TextChanged(object sender, EventArgs e)
        {
            listar();
        }

        private void limpiar()
        {
            txtCedulaIdentidad.Clear();
            txtNombres.Clear();
            txtPrimerApellido.Clear();
            txtSegundoApellido.Clear();
            dtpFechaNacimiento.Value = DateTime.Now.AddYears(-18); // Mayor de edad por defecto
            txtDireccion.Clear();
            txtCelular.Clear();
            cbxCargo.SelectedIndex = -1;
            resetearErrores();
        }

        private void resetearErrores()
        {
            erpCedula.Clear();
            erpNombres.Clear();
            erpDireccion.Clear();
            erpCelular.Clear();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            idSeleccionado = 0;
            if (pnlAcciones != null) pnlAcciones.Enabled = false; // Desactiva barra de botones principal

            Size = new Size(759, 568); // Despliega el panel de abajo
            limpiar();
            gbxDatos.Enabled = true;
            txtCedulaIdentidad.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow == null)
            {
                MessageBox.Show("Por favor, selecciona un empleado de la lista para editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            esNuevo = false;
            if (pnlAcciones != null) pnlAcciones.Enabled = false;

            Size = new Size(759, 568);
            resetearErrores();
            gbxDatos.Enabled = true;

            // Mapeo directo de la grilla a los campos
            DataGridViewRow fila = dgvLista.CurrentRow;
            idSeleccionado = Convert.ToInt32(fila.Cells["id"].Value);
            txtCedulaIdentidad.Text = fila.Cells["cedulaIdentidad"].Value.ToString();
            txtNombres.Text = fila.Cells["nombres"].Value.ToString();
            txtPrimerApellido.Text = fila.Cells["primerApellido"].Value?.ToString();
            txtSegundoApellido.Text = fila.Cells["segundoApellido"].Value?.ToString();
            dtpFechaNacimiento.Value = Convert.ToDateTime(fila.Cells["fechaNacimiento"].Value);
            txtDireccion.Text = fila.Cells["direccion"].Value.ToString();
            txtCelular.Text = fila.Cells["celular"].Value.ToString();
            cbxCargo.Text = fila.Cells["cargo"].Value.ToString();

            txtCedulaIdentidad.Focus();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow == null) return;

            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            string nombre = dgvLista.CurrentRow.Cells["nombres"].Value.ToString();

            DialogResult dialog = MessageBox.Show($"¿Está seguro de eliminar al empleado '{nombre}'?", "::: Mensaje - Ferreteria :::",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                // Usando la auditoría global real de tu Login
                EmpleadoCln.eliminar(id, Util.usuario.usuario1);
                listar();
                MessageBox.Show("Empleado eliminado correctamente", "::: Mensaje - Ferreteria :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                Empleado emp = new Empleado
                {
                    cedulaIdentidad = txtCedulaIdentidad.Text.Trim(),
                    nombres = txtNombres.Text.Trim(),
                    primerApellido = txtPrimerApellido.Text.Trim(),
                    segundoApellido = txtSegundoApellido.Text.Trim(),
                    fechaNacimiento = dtpFechaNacimiento.Value,
                    direccion = txtDireccion.Text.Trim(),
                    celular = Convert.ToInt64(txtCelular.Text),
                    cargo = cbxCargo.Text
                };

                int resultado = 0;
                // ¡AQUÍ ESTÁ LA MAGIA!: Usamos Util.usuario.usuario1 idéntico a Producto
                string usuarioActual = Util.usuario.usuario1;

                if (esNuevo)
                {
                    resultado = EmpleadoCln.crear(emp, usuarioActual);
                }
                else
                {
                    emp.id = idSeleccionado;
                    resultado = EmpleadoCln.actualizar(emp, usuarioActual);
                }

                if (resultado > 0)
                {
                    listar();
                    btnCancelar.PerformClick(); // Hace el efecto de cerrar el panel solo
                    MessageBox.Show("¡Empleado guardado con éxito!", "::: Mensaje - Ferreteria :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (pnlAcciones != null) pnlAcciones.Enabled = true; // Libera los botones de arriba
            Size = new Size(759, 352); // Contrae el formulario de nuevo
            limpiar();
            gbxDatos.Enabled = false;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LimpiarCampos()
        {
            txtCedulaIdentidad.Clear();
            txtNombres.Clear();
            txtPrimerApellido.Clear();
            txtSegundoApellido.Clear();
            dtpFechaNacimiento.Value = DateTime.Now.AddYears(-18);
            txtDireccion.Clear();
            txtCelular.Clear();
            cbxCargo.SelectedIndex = -1;
        }

        private bool validar()
        {
            bool esValido = true;
            resetearErrores();

            if (string.IsNullOrWhiteSpace(txtCedulaIdentidad.Text))
            {
                erpCedula.SetError(txtCedulaIdentidad, "Ingrese la cédula de identidad, carajo");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtNombres.Text))
            {
                erpNombres.SetError(txtNombres, "Ingrese el nombre del empleado");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                erpDireccion.SetError(txtDireccion, "Ingrese la dirección de domicilio");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtCelular.Text) || !long.TryParse(txtCelular.Text, out _))
            {
                erpCelular.SetError(txtCelular, "Ingrese un número de celular válido");
                esValido = false;
            }

            return esValido;
        }

    }
}
