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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();//取得目前畫面視窗

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int count);//取視窗title

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);


        /// <summary>
        /// 開始背景處理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startAsyncButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
            }
        }

        /// <summary>
        /// 結束背景處理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelAsyncButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
            }
        }

        /// <summary>
        /// 背景處理觸發DoWork
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            for (int i = 1; i <= 10; i++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    //Perform a time consuming operation and report progress.
                    System.Threading.Thread.Sleep(1000);
                    worker.ReportProgress(i * 10);
                }
                if(i == 10)
                    i=0;
            }

        }

        /// <summary>
        /// 背景處理延伸事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Returnlabel.Text = e.ProgressPercentage.ToString() + "%";

            if (e.ProgressPercentage.ToString() == "20")
            {
                //SendKeys.Send("{NUMLOCK}");
                label1.Text = GetActiveWindowTitle();
            }
            else if(e.ProgressPercentage.ToString() == "80")
            {
                label1.Text = "NULL";
            }
        }

        /// <summary>
        /// 背景處理狀況
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                Returnlabel.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                Returnlabel.Text = "Error: " + e.Error.Message;
            }
            //else
            //{
            //    Returnlabel.Text = "Done!";
            //}
        }

        /// <summary>
        /// 結束程式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_btn_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        /// <summary>
        /// 鍵盤事件偵測
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackKeyboard_KeyDown(object sender ,KeyEventArgs e)
        {
            label1.Text = e.KeyValue.ToString();
        }

        /// <summary>
        /// 視窗Title讀取
        /// </summary>
        /// <returns></returns>
        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        /// <summary>
        /// 視窗名稱
        /// </summary>
        /// <returns></returns>
        private string GetActiveWindowName()
        {
            IntPtr handle = GetForegroundWindow();
            int pId;
            GetWindowThreadProcessId(handle, out pId);
            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(pId);

            return p.ProcessName;

        }
    }
}
