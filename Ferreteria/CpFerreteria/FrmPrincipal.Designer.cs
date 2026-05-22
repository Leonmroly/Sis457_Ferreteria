namespace CpFerreteria
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            this.btnProductos = new System.Windows.Forms.Button();
            this.btnMarca = new System.Windows.Forms.Button();
            this.btnCategoria = new System.Windows.Forms.Button();
            this.btnUnidadMedida = new System.Windows.Forms.Button();
            this.pnlLateral = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnEmpleado = new System.Windows.Forms.Button();
            this.btnVenta = new System.Windows.Forms.Button();
            this.btnPerfil = new System.Windows.Forms.Button();
            this.btnUsuario = new System.Windows.Forms.Button();
            this.btnCompra = new System.Windows.Forms.Button();
            this.btnProveedor = new System.Windows.Forms.Button();
            this.btnCliente = new System.Windows.Forms.Button();
            this.pnlLateral.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnProductos
            // 
            this.btnProductos.FlatAppearance.BorderSize = 0;
            this.btnProductos.Location = new System.Drawing.Point(610, 360);
            this.btnProductos.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnProductos.Name = "btnProductos";
            this.btnProductos.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnProductos.Size = new System.Drawing.Size(189, 65);
            this.btnProductos.TabIndex = 0;
            this.btnProductos.Text = "Productos";
            this.btnProductos.UseVisualStyleBackColor = true;
            this.btnProductos.Click += new System.EventHandler(this.btnProductos_Click);
            // 
            // btnMarca
            // 
            this.btnMarca.Location = new System.Drawing.Point(294, 265);
            this.btnMarca.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnMarca.Name = "btnMarca";
            this.btnMarca.Size = new System.Drawing.Size(189, 65);
            this.btnMarca.TabIndex = 1;
            this.btnMarca.Text = "Marcas";
            this.btnMarca.UseVisualStyleBackColor = true;
            this.btnMarca.Click += new System.EventHandler(this.btnMarca_Click);
            // 
            // btnCategoria
            // 
            this.btnCategoria.Location = new System.Drawing.Point(610, 101);
            this.btnCategoria.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCategoria.Name = "btnCategoria";
            this.btnCategoria.Size = new System.Drawing.Size(189, 65);
            this.btnCategoria.TabIndex = 2;
            this.btnCategoria.Text = "Categorias";
            this.btnCategoria.UseVisualStyleBackColor = true;
            this.btnCategoria.Click += new System.EventHandler(this.btnCategoria_Click);
            // 
            // btnUnidadMedida
            // 
            this.btnUnidadMedida.Location = new System.Drawing.Point(914, 265);
            this.btnUnidadMedida.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnUnidadMedida.Name = "btnUnidadMedida";
            this.btnUnidadMedida.Size = new System.Drawing.Size(189, 65);
            this.btnUnidadMedida.TabIndex = 3;
            this.btnUnidadMedida.Text = "Unidades de Medida";
            this.btnUnidadMedida.UseVisualStyleBackColor = true;
            this.btnUnidadMedida.Click += new System.EventHandler(this.btnUnidadMedida_Click);
            // 
            // pnlLateral
            // 
            this.pnlLateral.BackColor = System.Drawing.Color.Gray;
            this.pnlLateral.Controls.Add(this.btnCerrar);
            this.pnlLateral.Controls.Add(this.btnEmpleado);
            this.pnlLateral.Controls.Add(this.label1);
            this.pnlLateral.Controls.Add(this.btnVenta);
            this.pnlLateral.Controls.Add(this.btnPerfil);
            this.pnlLateral.Controls.Add(this.btnUsuario);
            this.pnlLateral.Controls.Add(this.lblBienvenida);
            this.pnlLateral.Controls.Add(this.btnCompra);
            this.pnlLateral.Controls.Add(this.btnProveedor);
            this.pnlLateral.Controls.Add(this.btnCliente);
            this.pnlLateral.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLateral.Location = new System.Drawing.Point(0, 0);
            this.pnlLateral.Name = "pnlLateral";
            this.pnlLateral.Size = new System.Drawing.Size(258, 641);
            this.pnlLateral.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Dubai", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 40);
            this.label1.TabIndex = 12;
            this.label1.Text = "Bienvenido";
            // 
            // lblBienvenida
            // 
            this.lblBienvenida.AutoSize = true;
            this.lblBienvenida.Font = new System.Drawing.Font("Calibri", 13F);
            this.lblBienvenida.Location = new System.Drawing.Point(12, 101);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Size = new System.Drawing.Size(54, 22);
            this.lblBienvenida.TabIndex = 1;
            this.lblBienvenida.Text = "label1";
            // 
            // btnCerrar
            // 
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Image = global::CpFerreteria.Properties.Resources.cerrar_sesion;
            this.btnCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrar.Location = new System.Drawing.Point(19, 578);
            this.btnCerrar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(189, 41);
            this.btnCerrar.TabIndex = 13;
            this.btnCerrar.Text = "Cerrar Seccion";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // btnEmpleado
            // 
            this.btnEmpleado.FlatAppearance.BorderSize = 0;
            this.btnEmpleado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmpleado.Image = global::CpFerreteria.Properties.Resources.jefe_de_equipo;
            this.btnEmpleado.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmpleado.Location = new System.Drawing.Point(19, 435);
            this.btnEmpleado.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnEmpleado.Name = "btnEmpleado";
            this.btnEmpleado.Size = new System.Drawing.Size(189, 41);
            this.btnEmpleado.TabIndex = 6;
            this.btnEmpleado.Text = "Empleados";
            this.btnEmpleado.UseVisualStyleBackColor = true;
            this.btnEmpleado.Click += new System.EventHandler(this.btnEmpleado_Click);
            // 
            // btnVenta
            // 
            this.btnVenta.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnVenta.FlatAppearance.BorderSize = 0;
            this.btnVenta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVenta.Image = global::CpFerreteria.Properties.Resources.bienes;
            this.btnVenta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVenta.Location = new System.Drawing.Point(19, 178);
            this.btnVenta.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnVenta.Name = "btnVenta";
            this.btnVenta.Size = new System.Drawing.Size(186, 41);
            this.btnVenta.TabIndex = 9;
            this.btnVenta.Text = "Ventas";
            this.btnVenta.UseVisualStyleBackColor = true;
            this.btnVenta.Click += new System.EventHandler(this.btnVenta_Click);
            // 
            // btnPerfil
            // 
            this.btnPerfil.BackColor = System.Drawing.Color.Transparent;
            this.btnPerfil.BackgroundImage = global::CpFerreteria.Properties.Resources.Perfil;
            this.btnPerfil.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPerfil.FlatAppearance.BorderSize = 0;
            this.btnPerfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPerfil.Location = new System.Drawing.Point(150, 37);
            this.btnPerfil.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPerfil.Name = "btnPerfil";
            this.btnPerfil.Size = new System.Drawing.Size(95, 94);
            this.btnPerfil.TabIndex = 11;
            this.btnPerfil.UseVisualStyleBackColor = false;
            this.btnPerfil.Click += new System.EventHandler(this.btnPerfil_Click);
            // 
            // btnUsuario
            // 
            this.btnUsuario.FlatAppearance.BorderSize = 0;
            this.btnUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsuario.Image = global::CpFerreteria.Properties.Resources.asistencia_social;
            this.btnUsuario.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUsuario.Location = new System.Drawing.Point(16, 384);
            this.btnUsuario.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnUsuario.Name = "btnUsuario";
            this.btnUsuario.Size = new System.Drawing.Size(189, 41);
            this.btnUsuario.TabIndex = 7;
            this.btnUsuario.Text = "Usuarios";
            this.btnUsuario.UseVisualStyleBackColor = true;
            this.btnUsuario.Click += new System.EventHandler(this.btnUsuario_Click);
            // 
            // btnCompra
            // 
            this.btnCompra.FlatAppearance.BorderSize = 0;
            this.btnCompra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompra.Image = global::CpFerreteria.Properties.Resources.orden;
            this.btnCompra.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCompra.Location = new System.Drawing.Point(13, 224);
            this.btnCompra.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCompra.Name = "btnCompra";
            this.btnCompra.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnCompra.Size = new System.Drawing.Size(189, 41);
            this.btnCompra.TabIndex = 8;
            this.btnCompra.Text = "Compras";
            this.btnCompra.UseVisualStyleBackColor = true;
            this.btnCompra.Click += new System.EventHandler(this.btnCompra_Click);
            // 
            // btnProveedor
            // 
            this.btnProveedor.FlatAppearance.BorderSize = 0;
            this.btnProveedor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProveedor.Image = global::CpFerreteria.Properties.Resources.proveedor_hotelero__1_;
            this.btnProveedor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProveedor.Location = new System.Drawing.Point(19, 333);
            this.btnProveedor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnProveedor.Name = "btnProveedor";
            this.btnProveedor.Size = new System.Drawing.Size(186, 41);
            this.btnProveedor.TabIndex = 4;
            this.btnProveedor.Text = "Proveedores";
            this.btnProveedor.UseVisualStyleBackColor = true;
            this.btnProveedor.Click += new System.EventHandler(this.btnProveedor_Click);
            // 
            // btnCliente
            // 
            this.btnCliente.FlatAppearance.BorderSize = 0;
            this.btnCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCliente.Image = global::CpFerreteria.Properties.Resources.personas;
            this.btnCliente.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCliente.Location = new System.Drawing.Point(16, 277);
            this.btnCliente.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCliente.Name = "btnCliente";
            this.btnCliente.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnCliente.Size = new System.Drawing.Size(189, 41);
            this.btnCliente.TabIndex = 5;
            this.btnCliente.Text = "Clientes";
            this.btnCliente.UseVisualStyleBackColor = true;
            this.btnCliente.Click += new System.EventHandler(this.btnCliente_Click);
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 641);
            this.Controls.Add(this.pnlLateral);
            this.Controls.Add(this.btnUnidadMedida);
            this.Controls.Add(this.btnCategoria);
            this.Controls.Add(this.btnMarca);
            this.Controls.Add(this.btnProductos);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "::: Principal - Ferreteria :::";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.pnlLateral.ResumeLayout(false);
            this.pnlLateral.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnProductos;
        private System.Windows.Forms.Button btnMarca;
        private System.Windows.Forms.Button btnCategoria;
        private System.Windows.Forms.Button btnUnidadMedida;
        private System.Windows.Forms.Button btnProveedor;
        private System.Windows.Forms.Button btnCliente;
        private System.Windows.Forms.Button btnEmpleado;
        private System.Windows.Forms.Button btnUsuario;
        private System.Windows.Forms.Button btnCompra;
        private System.Windows.Forms.Button btnVenta;
        private System.Windows.Forms.Panel pnlLateral;
        private System.Windows.Forms.Label lblBienvenida;
        private System.Windows.Forms.Button btnPerfil;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCerrar;
    }
}