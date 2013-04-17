using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsControlLibrary
{
    public partial class UserControl2 : UserControl
    {
        public UserControl2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.label1.Text = "控件2按钮事件";
            MessageBox.Show("控件2按钮事件");
            MessageBox.Show((this.ParentForm == null).ToString());
        }
    }
}
