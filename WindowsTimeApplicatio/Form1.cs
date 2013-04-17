using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WindowsFormsControlLibrary;
using System.Threading;

namespace WindowsTimeApplicatio
{
    public partial class Form1 : Form
    {
        private UserControl UserControl;
        private int time=60;
        //private System.Threading.Timer ;
        //new System.Threading.Timer(CallBack,5,0,32 );
        //private static void CallBack(object state)
        //{

        //}
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.groupBox1.Controls.Clear();
            this.groupBox1.Controls.Add(new UserControl1());
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnSwitchControl_Click(object sender, EventArgs e)
        {
            this.groupBox1.Controls.Clear();
            this.groupBox1.Controls.Add(new UserControl2());
 
       
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(this.time>0)
                time=time-1;
            else
                if (this.time == 0)
                {
                    time = 60;
                    MessageBox.Show("Fucking");
                
                }
            this.label1.Text = time.ToString();
        }
    }
}
