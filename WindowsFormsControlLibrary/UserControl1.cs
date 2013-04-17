using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsControlLibrary
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //让它执行一个异步方法
            D DD = new D(d);
            DD.BeginInvoke(null,null);
        
        }
        public delegate void D();
        public void d()
        {
            Thread.Sleep(10000);
            this.BeginInvoke(new D(updatelab));
            Form fsc = this.ParentForm;
            MessageBox.Show(this.ToString());
            MessageBox.Show((fsc == null).ToString());
        }

        private void updatelab()
        {
            MessageBox.Show("五秒后Control1方法执行");
 
        }
    }
}
