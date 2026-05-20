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
    public partial class FrmVentaNuevo : Form
    {
        public FrmVentaNuevo()
        {
            InitializeComponent();
        }

        private void FrmVentaNuevo_Load(object sender, EventArgs e)
        {
            dgvDetalle.DataSource = null;
            dgvDetalle.Columns.Clear();
            dgvDetalle.Columns.Add("idProducto", "ID");
            dgvDetalle.Columns.Add("descripcion", "Producto");
            dgvDetalle.Columns.Add("cantidad", "Cantidad");
            dgvDetalle.Columns.Add("precioVenta", "Precio Unitario");
            dgvDetalle.Columns.Add("subtotal", "Subtotal");
            dgvDetalle.Columns["idProducto"].Visible = false;

            dgvDetalle.AutoGenerateColumns = false;
            cargarCombos();
        }
        private void cargarCombos()
        {
            cbxCliente.DataSource = ClienteCln.listar();
            cbxCliente.DisplayMember = "nombreCompleto";
            cbxCliente.ValueMember = "id";
            cbxCliente.SelectedIndex = -1;

            cbxProducto.DataSource = ProductoCln.listarPa("");
            cbxProducto.DisplayMember = "descripcion";
            cbxProducto.ValueMember = "id";
            cbxProducto.SelectedIndex = -1;
        }

        private void limpiar()
        {
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cbxCliente.SelectedIndex = -1;
            cbxProducto.SelectedIndex = -1;
            nudCantidad.Value = 0;
            nudPrecioVenta.Value = 0;
            dgvDetalle.Rows.Clear();
            lblTotal.Text = "TOTAL: 0.00";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cbxProducto.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione un producto.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (nudCantidad.Value <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a 0.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var productoSeleccionado = (paProductoListar_Result)cbxProducto.SelectedItem;

            int idProd = productoSeleccionado.id;
            string nombreProd = productoSeleccionado.descripcion;
            int cant = (int)nudCantidad.Value;
            decimal precio = nudPrecioVenta.Value;
            decimal subtotal = cant * precio;

            // Agregamos la fila de forma segura al modo unbound
            dgvDetalle.Rows.Add(idProd, nombreProd, cant, precio, subtotal);
            CalcularTotal();

            cbxProducto.SelectedIndex = -1;
            nudCantidad.Value = 0;
            nudPrecioVenta.Value = 0;
        }

        private void CalcularTotal()
        {
            decimal totalGeneral = 0;
            foreach (DataGridViewRow fila in dgvDetalle.Rows)
            {
                // Buscamos por el .Name exacto creado en el Load
                if (fila.Cells["subtotal"].Value != null)
                {
                    totalGeneral += Convert.ToDecimal(fila.Cells["subtotal"].Value);
                }
            }
            lblTotal.Text = "TOTAL: " + totalGeneral.ToString("N2");
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cbxCliente.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un cliente para la venta.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dgvDetalle.Rows.Count == 0)
            {
                MessageBox.Show("El carrito de ventas está vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string usuarioActual = Util.usuario != null ? Util.usuario.usuario1 : "Cajero";
                List<VentaDetalle> listaDetalles = new List<VentaDetalle>();
                decimal totalAcumuladoVenta = 0; // Calculamos el total puro en C#

                // 1. RECORREMOS EL CARRITO PARA ARMAR LOS DETALLES
                foreach (DataGridViewRow fila in dgvDetalle.Rows)
                {
                    if (fila.Cells["idProducto"].Value == null || string.IsNullOrEmpty(fila.Cells["idProducto"].Value.ToString()))
                        continue;

                    int cantidad = Convert.ToInt32(fila.Cells["cantidad"].Value);
                    decimal precio = Convert.ToDecimal(fila.Cells["precioVenta"].Value);

                    // Sumamos al total general de la venta
                    totalAcumuladoVenta += (cantidad * precio);

                    var detalle = new VentaDetalle()
                    {
                        idProducto = Convert.ToInt32(fila.Cells["idProducto"].Value),
                        cantidad = cantidad,
                        precioUnitario = precio, // Mapeo correcto según tu propiedad de BD
                        usuarioRegistro = usuarioActual,
                        estado = 1
                    };
                    listaDetalles.Add(detalle);
                }

                // 2. CREAMOS LA VENTA CON EL TOTAL CALCULADO
                var nuevaVenta = new Venta()
                {
                    idCliente = Convert.ToInt32(cbxCliente.SelectedValue),
                    fecha = DateTime.Now,
                    total = totalAcumuladoVenta, // Adiós problemas de texto
                    usuarioRegistro = usuarioActual,
                    estado = 1
                };

                // 3. ENVIAMOS A LA CAPA LÓGICA CAPTURANDO EL BOOLEANO CORRETO
                string msgError = "";
                bool exito = VentaCln.GuardarVenta(nuevaVenta, listaDetalles, out msgError);

                if (exito)
                {
                    MessageBox.Show("¡Venta registrada y stock actualizado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    // Si el método falló por stock insuficiente o error SQL, salta aquí
                    MessageBox.Show($"No se pudo registrar la venta:\n{msgError}", "Validación / Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error crítico en flujo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
