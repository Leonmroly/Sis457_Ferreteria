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
    public partial class FrmPrincipalCliente : Form
    {
        FrmAutenticacion frmAutenticacion;
        public FrmPrincipalCliente(FrmAutenticacion frmAutenticacion)
        {
            InitializeComponent();
            this.frmAutenticacion = frmAutenticacion;


        }

        private void FrmPrincipalCliente_FormClosing(object sender, FormClosingEventArgs e)
        {
            Util.usuario = null;
            frmAutenticacion.Show();
        }
    }
}
