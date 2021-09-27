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

            // set combobox entry to first element of classList
            comboBox1.SelectedIndex = 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            bgw = new BackgroundWorker();
            bgw.WorkerSupportsCancellation = true;
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            bgw.RunWorkerAsync(argument: comboBox1.SelectedItem);
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

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (!Paused)
            {
                bgw.CancelAsync();
                Paused = true;
            }
            else
            {
                int numEntries = panel1.Width;
                int maxVal = panel1.Height;
                Paused = false;

                // refresh display
                for (int i = 0; i < numEntries; i++)
                {
                    g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Black), i, 0, 1, maxVal);
                    g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), i, maxVal - array[i], i, maxVal);
                }

                // run worker on selected combo box item
                bgw.RunWorkerAsync(argument: comboBox1.SelectedItem);
            }
        }

        #region BackGroundStuff

        public void bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) 
        {
            // examine type and get list of constructors
            BackgroundWorker bw = sender as BackgroundWorker;
            string sortEngineName = (string)e.Argument;
            Type type = Type.GetType("SortingAlgorithmVisualizer." + sortEngineName);
            var constructors = type.GetConstructors();

            // create sort engine of identified type and invoke its constructor 
            try
            {
                ISortEngine se = (ISortEngine)constructors[0].Invoke(new object[] { array, g, panel1.Height });

                // run sorting algo
                while (!se.IsSorted() && (!bgw.CancellationPending)) 
                {
                    se.NextStep();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        private void label1_Click(object sender, EventArgs e)
        {

        }

        

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}
