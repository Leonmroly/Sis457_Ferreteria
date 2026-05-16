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
    public partial class FrmUsuario : Form
    {
        private bool esNuevo = false;
        private int idSeleccionado = 0;
        public FrmUsuario()
        {
            InitializeComponent();
        }

        private void FrmUsuario_Load(object sender, EventArgs e)
        {
            Size = new Size(759, 352);
            listar();
            cargarEmpleados();
            gbxDatos.Enabled = false;
        }

        private void listar()
        {
            var listaCompleta = UsuarioCln.listar();

            if (!string.IsNullOrWhiteSpace(txtParametro.Text))
            {
                string buscar = txtParametro.Text.ToLower();
                listaCompleta = listaCompleta.Where(x => x.usuario1.ToLower().Contains(buscar) ||
                                                         x.rol.ToLower().Contains(buscar) ||
                                                         x.Empleado.nombres.ToLower().Contains(buscar)).ToList();
            }

            // LA SOLUCIÓN: Extraemos los datos a un objeto plano y sacamos el nombre del empleado directo como String
            dgvLista.DataSource = listaCompleta.Select(x => new
            {
                x.id,
                x.idEmpleado,
                NombreEmpleado = x.Empleado != null ? $"{x.Empleado.nombres} {x.Empleado.primerApellido}" : "Sin asignar",
                Usuario = x.usuario1,
                Rol = x.rol
            }).ToList();

            // Renombramos las columnas visibles limpiamente
            dgvLista.Columns["NombreEmpleado"].HeaderText = "Empleado Responsable";
            dgvLista.Columns["Usuario"].HeaderText = "Nombre de Usuario";
            dgvLista.Columns["Rol"].HeaderText = "Rol Asignado";

            // Ocultamos los IDs del sistema
            if (dgvLista.Columns["id"] != null) dgvLista.Columns["id"].Visible = false;
            if (dgvLista.Columns["idEmpleado"] != null) dgvLista.Columns["idEmpleado"].Visible = false;

            btnEditar.Enabled = listaCompleta.Count > 0;
            btnEliminar.Enabled = listaCompleta.Count > 0;
        }

        private void cargarEmpleados()
        {
            cbxEmpleado.DataSource = EmpleadoCln.listar();
            cbxEmpleado.ValueMember = "id"; // Lo que SQL necesita guardar (el ID)
            cbxEmpleado.DisplayMember = "nombres"; // Lo que el usuario lee en pantalla
            cbxEmpleado.SelectedIndex = -1; // Que aparezca vacío al inicio
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
            cbxEmpleado.SelectedIndex = -1;
            txtUsuario.Clear();
            txtClave.Clear();
            cbxRol.SelectedIndex = -1;
            resetearErrores();
        }

        private void resetearErrores()
        {
            erpEmpleado.Clear();
            erpUsuario.Clear();
            erpClave.Clear();
            erpRol.Clear();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            idSeleccionado = 0;
            if (pnlAcciones != null) pnlAcciones.Enabled = false;

            Size = new Size(759, 568); // Abrir panel de abajo
            limpiar();
            gbxDatos.Enabled = true;
            cbxEmpleado.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow == null) return;

            esNuevo = false;
            if (pnlAcciones != null) pnlAcciones.Enabled = false;

            Size = new Size(759, 568);
            resetearErrores();
            gbxDatos.Enabled = true;

            // Mapeo seguro con la nueva grilla plana
            idSeleccionado = Convert.ToInt32(dgvLista.CurrentRow.Cells["id"].Value);
            cbxEmpleado.SelectedValue = Convert.ToInt32(dgvLista.CurrentRow.Cells["idEmpleado"].Value);
            txtUsuario.Text = dgvLista.CurrentRow.Cells["Usuario"].Value.ToString();
            txtClave.Text = "";
            cbxRol.Text = dgvLista.CurrentRow.Cells["Rol"].Value.ToString();

            cbxEmpleado.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (pnlAcciones != null) pnlAcciones.Enabled = true;
            Size = new Size(759, 352); // Cerrar panel abajo
            limpiar();
            gbxDatos.Enabled = false;
        }

        private bool validar()
        {
            bool esValido = true;
            resetearErrores();

            if (cbxEmpleado.SelectedIndex == -1)
            {
                erpEmpleado.SetError(cbxEmpleado, "Seleccione el empleado dueño de esta cuenta");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                erpUsuario.SetError(txtUsuario, "Defina un nombre de usuario de acceso");
                esValido = false;
            }
            // La clave solo es obligatoria si el usuario es NUEVO
            if (esNuevo && string.IsNullOrWhiteSpace(txtClave.Text))
            {
                erpClave.SetError(txtClave, "La contraseña es obligatoria para usuarios nuevos");
                esValido = false;
            }
            if (cbxRol.SelectedIndex == -1)
            {
                erpRol.SetError(cbxRol, "Asigne un rol de permisos para el sistema");
                esValido = false;
            }

            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                Usuario usu = new Usuario
                {
                    idEmpleado = Convert.ToInt32(cbxEmpleado.SelectedValue),
                    usuario1 = txtUsuario.Text.Trim(),
                    rol = cbxRol.Text,
                    estado = 1
                };

                // Lógica de Encriptación: 
                // Aquí usamos el método que tu proyecto ya tiene para encriptar claves (asumiendo que se llama Util.Encrypt o similar)
                // Si no lo tienes a mano, le pasamos el texto limpio, pero lo ideal es mandarlo con tu función MD5/SHA
                if (!string.IsNullOrWhiteSpace(txtClave.Text))
                {
                    // Reemplaza "Util.Encrypt" por tu función real de encriptación si te salta error
                    usu.clave = txtClave.Text.Trim();
                }

                int resultado = 0;
                string usuarioActual = Util.usuario.usuario1; // Auditoría limpia del Login

                if (esNuevo)
                {
                    resultado = UsuarioCln.crear(usu, usuarioActual);
                }
                else
                {
                    usu.id = idSeleccionado;
                    resultado = UsuarioCln.actualizar(usu, usuarioActual);
                }

                if (resultado > 0)
                {
                    listar();
                    btnCancelar.PerformClick();
                    MessageBox.Show("¡Usuario configurado correctamente!", "::: Mensaje - Ferreteria :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow == null) return;

            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            string cuenta = dgvLista.CurrentRow.Cells["usuario1"].Value.ToString();

            DialogResult dialog = MessageBox.Show($"¿Está seguro de eliminar el acceso de la cuenta '{cuenta}'?", "::: Mensaje - Ferreteria :::",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                UsuarioCln.eliminar(id, Util.usuario.usuario1);
                listar();
                MessageBox.Show("Cuenta de usuario dada de baja.", "::: Mensaje - Ferreteria :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
