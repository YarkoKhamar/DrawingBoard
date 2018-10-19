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
    public partial class NewCanvas : Form
    {
        public int Width
        {
            
            get { return (int)(numericUpDown1.Value); }
        }
        public int Height
        {
            get { return (int)(numericUpDown2.Value); }
        }
        public Color CanvasColor
        {
            get { return canvasColorStripButton1.BackColor; }
        }
        public NewCanvas()
        {

            InitializeComponent();
        }

        private void buttonOKCanvas_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void canvasColorStripButton1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                canvasColorStripButton1.BackColor = cd.Color;

                if (cd.Color.IsNamedColor && cd.Color != Color.Black)
                {
                    canvasColorStripButton1.ToolTipText = cd.Color.Name;
                }
                else
                {
                    canvasColorStripButton1.ToolTipText = "Background Color";
                }
            }
        }
    }
}
