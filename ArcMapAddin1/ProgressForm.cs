using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArcMapAddin1
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            progressBar1.Value = 10;
        }
        public void AddProgress(int val)
        {
            if (progressBar1.Value < 100)
            {
                progressBar1.Value += val;
            }
        }
    }
}
