using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpuMemMonitor
{
    public partial class Form1 : Form
    {
        private  System.Timers.Timer _timer;
        bool startflag = false;
        
        public Form1()
        {
            InitializeComponent();
            _timer = new System.Timers.Timer();
            _timer.Interval = 3000;
            _timer.Elapsed += OntimedEvent;
            _timer.AutoReset = true;
        }

        private  void OntimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            //CheckForIllegalCrossThreadCalls = false;


            // We will monitor and get the values of the cpu and memory
            int cpuValue = GetCpuValue();
            int memvalue = GetMemValue();


            if (progressBarCPU.InvokeRequired)
            {
                progressBarCPU.Invoke(new Action(() => progressBarCPU.Value = cpuValue));

            }
            else
            {
                progressBarCPU.Value = cpuValue;
            }

            if (lblCpuUsage.InvokeRequired)
            {
                lblCpuUsage.Invoke(new Action(() => lblCpuUsage.Text = cpuValue.ToString() + " %"));

            }
            else
            {
                lblCpuUsage.Text = cpuValue.ToString() + " %";
            }





            if (progressBarMem.InvokeRequired)
            {
                progressBarMem.Invoke(new Action(() => progressBarMem.Value = memvalue));

            }
            else
            {
                progressBarMem.Value = memvalue;
            }

            if (lblMemUsage.InvokeRequired)
            {
                lblMemUsage.Invoke(new Action(() => lblMemUsage.Text = memvalue.ToString() + " %"));

            }
            else
            {
                lblMemUsage.Text = memvalue.ToString() + " %";
            }





        }
        
        private  int GetCpuValue()
        {
            var CpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            CpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            int returnvalue = (int)CpuCounter.NextValue();
            return returnvalue;

        }

        private  int GetMemValue()
        {
            var MemCounter = new PerformanceCounter("Memory", "% Committed Bytes in Use");
            int returnvalue = (int)MemCounter.NextValue();
            return returnvalue;

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (startflag == false)
            {
                startflag = true;
                _timer.Enabled = true;
                buttonStart.Text = "Stop";
            }
            else
            {
                startflag = false;
                _timer.Enabled = false;
                buttonStart.Text = "Start";
            }


            

        }
    }
}
