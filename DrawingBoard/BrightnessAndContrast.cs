using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawingBoard
{
    public partial class BrightnessAndContrast : Form
    {
        public BrightnessAndContrast()
        {
            InitializeComponent();
            trackBarBr.Minimum = -20;
            trackBarBr.Maximum = 20;
            trackBarBr.TickFrequency = 5;
            trackBarContr.TickFrequency = 5;
            trackBarBr.SmallChange = 1;
            trackBarContr.SmallChange = 1;

            trackBarBr.Value = 0;
            trackBarContr.Minimum = -20;
            trackBarContr.Maximum = 20;
            trackBarContr.Value = 0;
            labelMinCon.Text = "" + trackBarContr.Minimum;
            labelMaxCon.Text = "" + trackBarContr.Maximum;
            labelMinBr.Text = "" + trackBarBr.Minimum;
            labelMaxBr.Text = "" + trackBarBr.Maximum;
        }
        //public CheckBox getPreview()
        //{
        //    return checkBoxPreview;
        //}
        public float getBrightness()
        {
            return trackBarBr.Value;
        }
        public float getContrast()
        {
            return trackBarContr.Value;
        }
        private void trackBarBr_Scroll(object sender, EventArgs e)
        {
            labelValueBr.Text = "" + trackBarBr.Value;
        }

        private void trackBarContr_Scroll(object sender, EventArgs e)
        {
            labelValueCon.Text = "" + trackBarContr.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BrightnessAndContrast_Load(object sender, EventArgs e)
        {
            //checkBoxPreview.Checked = true;
        }
    }
}
