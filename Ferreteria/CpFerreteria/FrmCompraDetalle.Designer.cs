namespace CpFerreteria
{
    partial class FrmCompraDetalle
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.dgvDetalle = new System.Windows.Forms.DataGridView();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Ebrima", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(14, 9);
            this.lblTitulo.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(731, 34);
            this.lblTitulo.TabIndex = 3;
            this.lblTitulo.Text = "Detalles de Compras";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvDetalle
            // 
            this.dgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalle.Location = new System.Drawing.Point(13, 47);
            this.dgvDetalle.Name = "dgvDetalle";
            this.dgvDetalle.Size = new System.Drawing.Size(732, 307);
            this.dgvDetalle.TabIndex = 4;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Image = global::CpFerreteria.Properties.Resources.close;
            this.btnCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrar.Location = new System.Drawing.Point(347, 360);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(99, 40);
            this.btnCerrar.TabIndex = 11;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(504, 370);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(135, 21);
            this.lblTotal.TabIndex = 12;
            this.lblTotal.Text = "TOTAL COMPRA:";
            // 
            // FrmCompraDetalle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 412);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.dgvDetalle);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCompraDetalle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmCompraDetalle";
            this.Load += new System.EventHandler(this.FrmCompraDetalle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvDetalle;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label lblTotal;
    }
}