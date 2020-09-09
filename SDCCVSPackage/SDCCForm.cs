using System;
using System.Windows.Forms;

namespace SDCCVSPackage
{
    public partial class SDCCForm : Form
    {
        public SDCCForm()
        {
            InitializeComponent();

            // Populate the drop downs.
            PortType.Items.Add("mcs51");
            PortType.Items.Add("ds390");
            PortType.Items.Add("ds400");
            PortType.Items.Add("hc08");
            PortType.Items.Add("s08");
            PortType.Items.Add("z80");
            PortType.Items.Add("z180");
            PortType.Items.Add("r2k");
            PortType.Items.Add("r3ka");
            PortType.Items.Add("gbz80");
            PortType.Items.Add("tlcs90");
            PortType.Items.Add("ez80_z80");
            PortType.Items.Add("stm8");
            PortType.Items.Add("pdk13");
            PortType.Items.Add("pdk14");
            PortType.Items.Add("pdk15");
            PortType.Items.Add("pic14");
            PortType.Items.Add("pic16");
            PortType.Items.Add("TININative");

            BuildFormat.Items.Add("Binary");
            BuildFormat.Items.Add("Library");
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FinishButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
