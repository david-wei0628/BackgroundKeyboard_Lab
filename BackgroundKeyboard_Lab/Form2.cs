using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace BackgroundKeyboard_Lab
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        private void BackKeyboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                this.button1.Size = this.button1.Size - new System.Drawing.Size(20, 10);
            }
            if(e.KeyCode == Keys.Q)
            {
                SendKeys.Send("{CAPSLOCK}");
            }

        }
    }
}
