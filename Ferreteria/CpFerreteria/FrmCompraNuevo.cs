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
    public partial class FrmCompraNuevo : Form
    {
        public FrmCompraNuevo()
        {
            InitializeComponent();
        }

        private void FrmCompraNuevo_Load(object sender, EventArgs e)
        {
            dgvLista.DataSource = null;
            dgvLista.Columns.Clear();
            dgvLista.Columns.Add("idProducto", "ID");
            dgvLista.Columns.Add("descripcion", "Producto");
            dgvLista.Columns.Add("cantidad", "Cantidad");
            dgvLista.Columns.Add("precioCompra", "Precio Unitario");
            dgvLista.Columns.Add("subtotal", "Subtotal");
            dgvLista.Columns["idProducto"].Visible = false;

            dgvLista.AutoGenerateColumns = false;
            cargarCombos();
        }

        private void cargarCombos()
        {
            cbxProveedor.DataSource = ProveedorCln.listar();
            cbxProveedor.DisplayMember = "razonSocial";
            cbxProveedor.ValueMember = "id";
            cbxProveedor.SelectedIndex = -1;

            cbxProducto.DataSource = ProductoCln.listarPa("");
            cbxProducto.DisplayMember = "descripcion";
            cbxProducto.ValueMember = "id";
            cbxProducto.SelectedIndex = -1;
        }

        private void limpiar()
        {
            dgvLista.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cbxProveedor.SelectedIndex = -1;
            cbxProducto.SelectedIndex = -1;
            nudCantidad.Value = 0;
            nudPrecioCompra.Value = 0;
            dgvLista.Rows.Clear();
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

            // SOLUCIÓN AL CAST EXCEPTION: Se mapea directamente con la clase que arroja tu Procedimiento Almacenado
            var productoSeleccionado = (paProductoListar_Result)cbxProducto.SelectedItem;

            int idProd = productoSeleccionado.id;
            string nombreProd = productoSeleccionado.descripcion;
            int cant = (int)nudCantidad.Value;
            decimal precio = nudPrecioCompra.Value;
            decimal subtotal = cant * precio;

            dgvLista.Rows.Add(idProd, nombreProd, cant, precio, subtotal);
            CalcularTotal();

            cbxProducto.SelectedIndex = -1;
            nudCantidad.Value = 0;
            nudPrecioCompra.Value = 0;
        }

        private void CalcularTotal()
        {
            decimal totalGeneral = 0;
            foreach (DataGridViewRow fila in dgvLista.Rows)
            {
                if (fila.Cells["subtotal"].Value != null)
                {
                    totalGeneral += Convert.ToDecimal(fila.Cells["subtotal"].Value);
                }
            }
            lblTotal.Text = "TOTAL: " + totalGeneral.ToString("N2");
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvLista.Rows.Count == 0)
                {
                    MessageBox.Show("Debe agregar al menos un producto al carrito.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                List<CompraDetalle> listaDetalles = new List<CompraDetalle>();
                decimal totalAcumuladoCompra = 0; // <--- AQUÍ VAMOS A SUMAR EL TOTAL REAL

                // 1. RECORRIDO ULTRA SEGURO DE LAS FILAS
                foreach (DataGridViewRow fila in dgvLista.Rows)
                {
                    if (fila.IsNewRow) continue;

                    if (fila.Cells["idProducto"].Value == null || string.IsNullOrEmpty(fila.Cells["idProducto"].Value.ToString()))
                        continue;

                    int cantidad = 0;
                    if (fila.Cells["cantidad"].Value != null)
                    {
                        int.TryParse(fila.Cells["cantidad"].Value.ToString(), out cantidad);
                    }

                    decimal precio = 0;
                    if (fila.Cells["precioCompra"].Value != null)
                    {
                        // Limpieza de caracteres extraños para el precio unitario
                        string precioTexto = fila.Cells["precioCompra"].Value.ToString().Replace("$", "").Trim();
                        decimal.TryParse(precioTexto, out precio);
                    }

                    if (cantidad == 0) continue;

                    // Calculamos el subtotal de esta fila y lo sumamos al total general de la compra
                    totalAcumuladoCompra += (cantidad * precio);

                    CompraDetalle detalle = new CompraDetalle
                    {
                        idProducto = Convert.ToInt32(fila.Cells["idProducto"].Value),
                        cantidad = cantidad,
                        precioCompra = precio,
                        estado = 1
                    };

                    listaDetalles.Add(detalle);
                }

                if (listaDetalles.Count == 0)
                {
                    MessageBox.Show("No se encontraron productos válidos con cantidades numéricas en el carrito.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. INSTANCIAMOS EL MAESTRO PASANDO EL TOTAL CALCULADO POR C#
                Compra compra = new Compra
                {
                    idProveedor = Convert.ToInt32(cbxProveedor.SelectedValue),
                    total = totalAcumuladoCompra, // <--- ADIÓS AL TXTTOTAL.TEXT, AHORA ES MATEMÁTICA PURA
                    usuarioRegistro = "Iroly",
                    estado = 1,
                    fecha = DateTime.Now
                };

                // 3. ENVIAMOS A GUARDAR
                string mensajeError = string.Empty;
                bool exito = CompraCln.GuardarCompra(compra, listaDetalles, out mensajeError);

                if (exito)
                {
                    MessageBox.Show("¡Compra registrada y stock actualizado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"No se pudo guardar la compra: {mensajeError}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en el formulario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
