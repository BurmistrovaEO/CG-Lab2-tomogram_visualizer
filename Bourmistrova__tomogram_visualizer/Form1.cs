using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bourmistrova_tomogram_visualizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Bin bin = new Bin();
        View view = new View();
        int currentLayer;
        bool loaded = false;
        bool needReload = false;

        int FrameCount;
        DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);
        void displayFPS()
        {
            if(DateTime.Now >= NextFPSUpdate)
            {
                //this.Text = String.Format("CT Visualizer fps= ");//(0))", 
                this.Text = String.Format(FrameCount.ToString());//(0))", 
                NextFPSUpdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }
            FrameCount++;
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = dialog.FileName;
                bin.readBIN(str);
                view.SetupView(glControl1.Width, glControl1.Height);
                trackBar1.Maximum = Bin.Z-1;
                trackBar2.Maximum = 255;
                trackBar3.Maximum = 2000;
                trackBar3.Minimum = 1400;
                loaded = true;
                glControl1.Invalidate();

            }
        }
        void Application_Idle(object sender, EventArgs e)
        {
            while(glControl1.IsIdle)
            {
                displayFPS();
                //needReload = true;
                glControl1.Invalidate();
            }
        }
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (loaded)
            {
                //view.DrawQuads(currentLayer);
                if(needReload == true)
                {
                    view.generateTextureImage(currentLayer);
                    view.Load2DTexture();
                    needReload = false;
                }
                if(radioButton2.Checked)
                    view.DrawTexture();
                if (radioButton1.Checked)
                    view.DrawQuads(currentLayer);
                glControl1.SwapBuffers();//
            }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
            if(radioButton2.Checked)
                needReload = true;
            if(radioButton1.Checked)
                glControl1.Invalidate();
            textBox1.Text = currentLayer.ToString();
            textBox1.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            view.min = trackBar2.Value;
            needReload = true;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            view.wid = trackBar3.Value;
            needReload = true;
        }
    }
}