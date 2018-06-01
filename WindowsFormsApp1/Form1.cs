using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Inner/MessageFromBusiness/AddYEMAssetList
            int count = Convert.ToInt32(this.textBox1.Text.Trim());
            if (count > 0)
            {
            }
        }
    }
}