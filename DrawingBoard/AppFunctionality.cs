using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawingBoard
{
    public partial class AppFunctionality : Form
    {
        public AppFunctionality()
        {
            InitializeComponent();
        }
        private void LoadFileToRTB(string fileName, RichTextBox rtb)
        {
            //rtb.LoadFile(File.OpenRead(fileName), RichTextBoxStreamType.PlainText); // second parameter you can change to fit for you
            //                                                                        // or
            //rtb.LoadFile(fileName);
            //// or
            rtb.LoadFile(fileName, RichTextBoxStreamType.RichText); // second parameter you can change to fit for you
        }
        private void AppFunctionality_Load(object sender, EventArgs e)
        {
            LoadFileToRTB("Instruction.rtf", richTextBox1);
        }
    }
}
