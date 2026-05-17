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
            dgvDetalle.DataSource = null;
            dgvDetalle.Columns.Clear();
            dgvDetalle.Columns.Add("idProducto", "ID");
            dgvDetalle.Columns.Add("descripcion", "Producto");
            dgvDetalle.Columns.Add("cantidad", "Cantidad");
            dgvDetalle.Columns.Add("precioCompra", "Precio Unitario");
            dgvDetalle.Columns.Add("subtotal", "Subtotal");
            dgvDetalle.Columns["idProducto"].Visible = false;

            dgvDetalle.AutoGenerateColumns = false;
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
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cbxProveedor.SelectedIndex = -1;
            cbxProducto.SelectedIndex = -1;
            nudCantidad.Value = 0;
            nudPrecioCompra.Value = 0;
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

            // SOLUCIÓN AL CAST EXCEPTION: Se mapea directamente con la clase que arroja tu Procedimiento Almacenado
            var productoSeleccionado = (paProductoListar_Result)cbxProducto.SelectedItem;

            int idProd = productoSeleccionado.id;
            string nombreProd = productoSeleccionado.descripcion;
            int cant = (int)nudCantidad.Value;
            decimal precio = nudPrecioCompra.Value;
            decimal subtotal = cant * precio;

            dgvDetalle.Rows.Add(idProd, nombreProd, cant, precio, subtotal);
            CalcularTotal();

            cbxProducto.SelectedIndex = -1;
            nudCantidad.Value = 0;
            nudPrecioCompra.Value = 0;
        }

        private void CalcularTotal()
        {
            decimal totalGeneral = 0;
            foreach (DataGridViewRow fila in dgvDetalle.Rows)
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
            if (cbxProveedor.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un proveedor para la compra.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dgvDetalle.Rows.Count == 0)
            {
                MessageBox.Show("El carrito de compras está vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var nuevaCompra = new Compra()
                {
                    idProveedor = Convert.ToInt32(cbxProveedor.SelectedValue),
                    fecha = DateTime.Now,
                    total = Convert.ToDecimal(lblTotal.Text.Replace("TOTAL: ", "").Trim()),
                    usuarioRegistro = Util.usuario.usuario1,
                    estado = 1
                };

                List<CompraDetalle> listaDetalles = new List<CompraDetalle>();

                foreach (DataGridViewRow fila in dgvDetalle.Rows)
                {
                    if (fila.Cells["idProducto"].Value != null)
                    {
                        var detalle = new CompraDetalle()
                        {
                            idProducto = Convert.ToInt32(fila.Cells["idProducto"].Value),
                            cantidad = Convert.ToInt32(fila.Cells["cantidad"].Value),
                            precioCompra = Convert.ToDecimal(fila.Cells["precioCompra"].Value)
                        };
                        listaDetalles.Add(detalle);
                    }
                }

                bool exito = CompraCln.GuardarCompra(nuevaCompra, listaDetalles);

                if (exito)
                {
                    MessageBox.Show("¡Compra registrada y stock actualizado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Cierra esta ventana y regresa al menú principal automáticamente limpio
                }
                else
                {
                    MessageBox.Show("Hubo un problema al guardar en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error crítico: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
