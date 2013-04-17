using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace PrintDemo
{
    public partial class PrintForm : Form
    {
        private Font mainTextFont = new Font("Times New Roman", 14);
        private Font subTextFont = new Font("Times New Roman", 12);
        //页面设置
        private PageSettings storedPageSettings;
        
        public PrintForm()
        {
            InitializeComponent();
        }



        private void PrintForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            PrintDocument(g);
        }

        private void PrintDocument(Graphics g)
        {
            g.PageUnit = GraphicsUnit.Point;
            g.DrawString("Simple Print Demo"
                , this.mainTextFont
                , Brushes.Black
                , new Rectangle(10, 30, 180, 30)
                );
            g.DrawString("This text and the box appear on the Screen and can be Printed too!"
              , this.mainTextFont
              , Brushes.Red
              , new Rectangle(30, 70, 150, 50)
              );
        }
        /// <summary>
        /// 页面设置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pageSteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                PageSetupDialog setupDialog = new PageSetupDialog();
                if (storedPageSettings == null)
                    storedPageSettings = new PageSettings();

                setupDialog.PageSettings = storedPageSettings;
                setupDialog.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }
        private void pagePreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 页面打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pagePrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDocument printDocu = new PrintDocument();
            //设置打印处理事件（这个是打印工作的主体）
            printDocu.PrintPage += new PrintPageEventHandler(this.PrintPageEventHandler);
            if (storedPageSettings != null)
            {
                printDocu.DefaultPageSettings = storedPageSettings;
            }
            //打印对话框
            //也可以不用对话框，直接调用printDocu.Print()进行打印
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocu;
            DialogResult result = printDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                printDocu.Print();
            }
        }
        protected void PrintPageEventHandler(Object sender, PrintPageEventArgs args)
        {
            Graphics g = args.Graphics;
            PrintDocument(g);
            args.HasMorePages = false;
        }
    }
}
