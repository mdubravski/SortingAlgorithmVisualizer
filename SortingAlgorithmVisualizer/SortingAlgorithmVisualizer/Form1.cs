using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SortingAlgorithmVisualizer
{
    public partial class Form1 : Form
    {
        int[] array;
        Graphics g;

        //make worker to execute sort on another thread
        BackgroundWorker bgw = null;
        bool Paused = false;




        public Form1()
        {
            InitializeComponent();
            PopoulateDropdown();
        }

        private void PopoulateDropdown()
        {
            // find classes that inherit ISortEngine interface and add them to dropdown
            List<string> classList = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(ISortEngine).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.Name).ToList();

            // sort list alphabetically
            classList.Sort();

            // populate drop down
            foreach (string s in classList) 
            {
                comboBox1.Items.Add(s);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
             g = panel1.CreateGraphics();
            int numEntries = panel1.Width;
            int maxVal = panel1.Height;
            array = new int[numEntries];
            g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Black), 0, 0, numEntries, maxVal);
            Random rand = new Random();

            // populate array wiith random ints between 0 and height of panel
            for (int i = 0; i < numEntries; i++)
            {
                array[i] = rand.Next(0, maxVal);
            }

            // draw bars
            for (int i = 0; i < numEntries; i++)
            {
                g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), i, maxVal - array[i], 1, maxVal);
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ISortEngine se = new SortEngineBubble(array, g, panel1.Height);
            se.NextStep();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
