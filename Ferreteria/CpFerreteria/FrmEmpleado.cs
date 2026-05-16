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
        private void FrmEmpleado_Load(object sender, EventArgs e)
        {
            Size = new Size(759, 352);
            listar();
            gbxDatos.Enabled = false;


            // 1. BLOQUEAR ESCRITURA: El usuario solo puede elegir de la lista
            cbxCargo.DropDownStyle = ComboBoxStyle.DropDownList;

            // 2. CARGAR LOS CARGOS DIRECTAMENTE
            cbxCargo.Items.Clear();
            cbxCargo.Items.Add("Administrador");
            cbxCargo.Items.Add("Vendedor");
            cbxCargo.Items.Add("Almacenero");
            cbxCargo.Items.Add("Cajero");

            cbxCargo.SelectedIndex = -1;
        }

        private void listar()
        {
            var lista = EmpleadoCln.listar();

            if (!string.IsNullOrWhiteSpace(txtParametro.Text))
            {
                lista = lista.Where(x => x.nombres.ToLower().Contains(txtParametro.Text.ToLower()) ||
                                         x.primerApellido.ToLower().Contains(txtParametro.Text.ToLower()) ||
                                         x.cedulaIdentidad.Contains(txtParametro.Text)).ToList();
            }

            dgvLista.DataSource = null;

            // IGUAL QUE EN PRODUCTO: Pasamos un objeto plano anónimo para que no de error
            dgvLista.DataSource = lista.Select(x => new
            {
                x.id,
                x.cedulaIdentidad,
                x.nombres,
                x.primerApellido,
                x.segundoApellido,
                x.fechaNacimiento,
                x.direccion,
                x.celular,
                x.cargo
            }).ToList();

            // Encabezados limpios
            dgvLista.Columns["cedulaIdentidad"].HeaderText = "Cédula Identidad";
            dgvLista.Columns["nombres"].HeaderText = "Nombres";
            dgvLista.Columns["primerApellido"].HeaderText = "Primer Apellido";
            dgvLista.Columns["segundoApellido"].HeaderText = "Segundo Apellido";
            dgvLista.Columns["fechaNacimiento"].HeaderText = "F. Nacimiento";
            dgvLista.Columns["direccion"].HeaderText = "Dirección";
            dgvLista.Columns["celular"].HeaderText = "Celular";
            dgvLista.Columns["cargo"].HeaderText = "Cargo";

            // Ocultamos el ID de forma segura
            if (dgvLista.Columns["id"] != null) dgvLista.Columns["id"].Visible = false;

            if (lista.Count > 0) dgvLista.CurrentCell = dgvLista.Rows[0].Cells["cedulaIdentidad"];

            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
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
            dtpFechaNacimiento.Value = DateTime.Now.AddYears(-18);
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
            if (pnlAcciones != null) pnlAcciones.Enabled = false;

            Size = new Size(759, 568);
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

            // Mapeo directo desde las celdas de la grilla plana
            DataGridViewRow fila = dgvLista.CurrentRow;
            idSeleccionado = Convert.ToInt32(fila.Cells["id"].Value);

            txtCedulaIdentidad.Text = fila.Cells["cedulaIdentidad"].Value.ToString();
            txtNombres.Text = fila.Cells["nombres"].Value.ToString();
            txtPrimerApellido.Text = fila.Cells["primerApellido"].Value?.ToString() ?? "";
            txtSegundoApellido.Text = fila.Cells["segundoApellido"].Value?.ToString() ?? "";
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
                    btnCancelar.PerformClick();
                    MessageBox.Show("¡Empleado guardado con éxito!", "::: Mensaje - Ferreteria :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (pnlAcciones != null) pnlAcciones.Enabled = true;
            Size = new Size(759, 352);
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
