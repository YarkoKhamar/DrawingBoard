using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DrawingBoard
{

    public partial class DrawingBoard : Form
    {
        Point p;
        Point latestPoint;
        bool paint = false;
        bool selection;
        Bitmap temporarySelection;
        bool buttonClicked = false;
        Rectangle mRect;
        float zoom = 1;
        float startAngle = 0.0F;
        float sweepAngle = 360f - 45.0F;
        Point[] Points;
        int CounterSetPoint;
        private List<Point> polygonPoints = new List<Point>();
        public DrawingBoard()
        {
            InitializeComponent();

            Points = new Point[3];
            CounterSetPoint = 0;
            trackBarSize.Minimum = 1;
            trackBarSize.Maximum = 100;
            trackBarSize.Value = 5;
            labelScaleValue.Text = "100%";
            labelMinVal.Text = "" + trackBarSize.Minimum;
            labelMaxVal.Text = "" + trackBarSize.Maximum;
            pictureBox1.MouseMove += changeCoordinates;
            pictureBox1.MouseLeave += PictureBox1_MouseLeave;
            pictureBox1.MouseMove += getRGB;
        }
        //private RichTextBox GetCurrentDocument
        //{
        //    get { return (RichTextBox)this.Controls["Body"]; }
        //}

        public PictureBox getPictureBox()
        {
            return pictureBox1;
        }
        private void PictureBox1_MouseLeave(object sender, EventArgs e)
        {
            XCoordinates.Text = " ";
            YCoordinates.Text = " ";
        }


        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolBox_Paint(object sender, PaintEventArgs e)
        {

        }

        private void newBtn_Click(object sender, EventArgs e)
        {
            //Files.newFileDialog();
            NewCanvas nc = new NewCanvas();

            if (nc.ShowDialog() == DialogResult.OK)
            {
                //pictureBox1.Width = nc.Width;
                //pictureBox1.Height = nc.Height;
                if (nc.Height > 0 && nc.Width > 0 && nc.Width.ToString() != string.Empty && nc.Height.ToString() != string.Empty)
                {
                    var bmp = new Bitmap(nc.Width, nc.Height);
                    var g = Graphics.FromImage(bmp);
                    SolidBrush sbrsh = new SolidBrush(nc.CanvasColor);
                    g.FillRectangle(sbrsh, 0, 0, nc.Width, nc.Height);
                    pictureBox1.Image = bmp;
                    pictureBox1.Width = bmp.Width;
                    pictureBox1.Height = bmp.Height;
                }
            }

        }

        private void DrawingBoard_Load(object sender, EventArgs e)
        {
            //Files.newFileLoad();
            DrawingBoard dBoard = new DrawingBoard();
            var bmp = new Bitmap(300, 300);
            var g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.White, 0, 0, 300, 300);
            pictureBox1.Image = bmp;
            pictureBox1.Width = bmp.Width;
            pictureBox1.Height = bmp.Height;
            pictureSize();
            buttonPaste2.Enabled = false;
            pasteToolStripMenuItem.Enabled = false;
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Title = "Open Image";
            of.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (of.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(of.FileName);
                pictureBox1.Width = pictureBox1.Image.Width;
                pictureBox1.Height = pictureBox1.Image.Height;
            }
            of.Dispose();
        }
        public void changeCoordinates(object sender, MouseEventArgs e)
        {
            pictureBox1.Cursor = new Cursor(Cursor.Current.Handle);
            if (e.X > pictureBox1.Image.Width || e.Y > pictureBox1.Image.Height)
            {
                XCoordinates.Text = " ";
                YCoordinates.Text = " ";
            }
            else
            {
                XCoordinates.Text = e.X.ToString();
                YCoordinates.Text = e.Y.ToString();
            }
        }
        public void leaveCoordinates(object sender, MouseEventArgs e)
        {
            XCoordinates.Text = " ";
            YCoordinates.Text = " ";
        }
        public void getRGB(object sender, MouseEventArgs e)
        {
            var scale = (float)pictureBox1.Width / pictureBox1.Image.Width;
            var loc = new Point((int)(e.Location.X / scale), (int)(e.Location.Y / scale));
            if (loc.X < pictureBox1.Image.Size.Width && loc.X >= 0 && loc.Y >= 0 && loc.Y < pictureBox1.Image.Size.Height)
            {
                var color = (pictureBox1.Image as Bitmap).GetPixel(loc.X, loc.Y);
                panelR.BackColor = Color.FromArgb(color.R, 0, 0);
                panelG.BackColor = Color.FromArgb(0, color.G, 0);
                panelB.BackColor = Color.FromArgb(0, 0, color.B);
                labelR.Text = color.R.ToString();
                labelG.Text = color.G.ToString();
                labelB.Text = color.B.ToString();
            }
            else
            {
                labelR.Text = Color.Black.R.ToString();
                labelG.Text = Color.Black.G.ToString();
                labelB.Text = Color.Black.B.ToString();
            }
        }
        public void pictureSize()
        {
            labelCanvasSz.Text = "H: " + pictureBox1.Height + Environment.NewLine + "W: " + pictureBox1.Width;
        }
        public void getImageSize()
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (colorSampleButton.Checked)
            {
                var g = Graphics.FromImage(pictureBox1.Image);
                var scale = (float)pictureBox1.Width / pictureBox1.Image.Width;
                var loc = new Point((int)((e as MouseEventArgs).Location.X / scale), (int)((e as MouseEventArgs).Location.Y / scale));
                var color = (pictureBox1.Image as Bitmap).GetPixel(loc.X, loc.Y);
                foregroundColor.BackColor = Color.FromArgb(color.R, color.G, color.B);

                pictureBox1.Invalidate();
            }
            if (magnifyButton.Checked)
            {
                var me = e as MouseEventArgs;
                if (me.Button == MouseButtons.Left)
                {
                    zoom *= 2;
                }
                if (me.Button == MouseButtons.Right)
                {
                    zoom /= 2;
                }
                pictureBox1.Width = (int)(pictureBox1.Image.Width * zoom);
                pictureBox1.Height = (int)(pictureBox1.Image.Height * zoom);
                UpdateScale();
            }
        }

        private void XCoordinates_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void labelB_Click(object sender, EventArgs e)
        {

        }

        private void labelG_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            pictureSize();
        }

        private void YCoordinates_Click(object sender, EventArgs e)
        {

        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "Images|*.png|*.bmp|*.jpg";
            //ImageFormat format = ImageFormat.Png;
            //if (sfd.ShowDialog() == DialogResult.OK)
            //{
            //    string ext = Path.GetExtension(sfd.FileName);
            //    switch (ext)
            //    {
            //        case ".jpg":
            //            format = ImageFormat.Jpeg;
            //            break;
            //        case ".bmp":
            //            format = ImageFormat.Bmp;
            //            break;
            //    }
            //    pictureBox1.Image.Save(sfd.FileName, format);
            //}
            //sfd.Dispose();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|PNG|*.png";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                FileStream fs = (FileStream)saveFileDialog1.OpenFile();
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        pictureBox1.Image.Save(fs, ImageFormat.Jpeg);
                        break;

                    case 2:
                        pictureBox1.Image.Save(fs, ImageFormat.Bmp);
                        break;

                    case 3:
                        pictureBox1.Image.Save(fs, ImageFormat.Gif);
                        break;
                    case 4:
                        pictureBox1.Image.Save(fs, ImageFormat.Png);
                        break;
                }
                fs.Close();
            }

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            var scale = (float)pictureBox1.Width / pictureBox1.Image.Width;
            var loc = new Point((int)(e.Location.X / scale), (int)(e.Location.Y / scale));
            p = loc;
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                latestPoint = e.Location;
            }
            if (textButton.Checked)
            {
                RichTextBox txt = new RichTextBox();
                //txt.LostFocus += (s, r) => (s as RichTextBox).Focus();        
                txt.Location = e.Location;
                txt.ForeColor = foregroundColor.BackColor;
                txt.Width = 120;
                txt.MaxLength = 20;
                txt.Leave += new EventHandler(txtLeave);
                pictureBox1.Controls.Add(txt);
            }
        }
        private void txtLeave(object sender, EventArgs e)
        {
            using (SolidBrush brsh = new SolidBrush(foregroundColor.BackColor))
            {
                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {
                    var scale = (float)pictureBox1.Width / pictureBox1.Image.Width;
                    var pos = ((RichTextBox)sender).Location;
                    var loc = new Point((int)(pos.X / scale), (int)(pos.Y / scale));
                    g.DrawString(((RichTextBox)sender).Text, ((RichTextBox)sender).Font, brsh, loc);
                }
            }
            ((RichTextBox)sender).Leave -= new EventHandler(txtLeave);
            pictureBox1.Controls.Remove((RichTextBox)sender);
            ((RichTextBox)sender).Dispose();
            pictureBox1.Invalidate();
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            var scale = (float)pictureBox1.Width / pictureBox1.Image.Width;
            var loc = new Point((int)(e.Location.X / scale), (int)(e.Location.Y / scale));
            if (e.Button == MouseButtons.Left)
            {
                if (pencliButton.Checked)
                {
                    var g = Graphics.FromImage(pictureBox1.Image);
                    g.DrawLine(new Pen(foregroundColor.BackColor, trackBarSize.Value)
                    {
                        StartCap = System.Drawing.Drawing2D.LineCap.Round,
                        EndCap = System.Drawing.Drawing2D.LineCap.Round,
                        LineJoin = System.Drawing.Drawing2D.LineJoin.Round
                    }, p, loc);

                    p = loc;
                    pictureBox1.Invalidate();

                }
                if (eraseButton.Checked)
                {
                    var g = Graphics.FromImage(pictureBox1.Image);
                    g.DrawLine(new Pen(backgroundColor.BackColor, trackBarSize.Value)
                    {
                        StartCap = System.Drawing.Drawing2D.LineCap.Round,
                        EndCap = System.Drawing.Drawing2D.LineCap.Round,
                        LineJoin = System.Drawing.Drawing2D.LineJoin.Round
                    }, p, loc);

                    p = loc;
                    pictureBox1.Invalidate();
                }
                if (rectButton.Checked || circleButton.Checked || lineButton.Checked || triangleButton.Checked || pieButton.Checked || arrow1Button.Checked)
                {
                    Point pnt = loc;


                    int x = Math.Min(p.X, pnt.X);
                    int y = Math.Min(p.Y, pnt.Y);
                    int w = Math.Abs(pnt.X - p.X);
                    int h = Math.Abs(pnt.Y - p.Y);

                    mRect = new Rectangle(x, y, w, h);

                    pictureBox1.Invalidate();
                }
                if (selectionToolButton.Checked /*|| fillSelectedArea.Checked*/)
                {

                    Point pnt = loc;
                    int x = Math.Min(p.X, pnt.X);
                    int y = Math.Min(p.Y, pnt.Y);
                    int w = Math.Abs(pnt.X - p.X);
                    int h = Math.Abs(pnt.Y - p.Y);
                    if (!selection)
                    {
                        mRect = new Rectangle(x, y, w, h);
                    }
                    else
                    {
                        mRect.Location = new Point(mRect.Location.X - p.X + loc.X, mRect.Location.Y - p.Y + loc.Y);
                        p = loc;
                    }

                    pictureBox1.Invalidate();
                }
            }
        }

        private void foregroundColor_CheckedChanged(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == DialogResult.OK)
            {
                foregroundColor.BackColor = cd.Color;
            }
        }

        private void backgroundColor_CheckedChanged(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                backgroundColor.BackColor = cd.Color;
            }
        }
        private void UpdateScale()
        {
            var scale = 100 * pictureBox1.Width / pictureBox1.Image.Width;
            labelScaleValue.Text = "" + scale + "%";
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clip = new Region(new RectangleF(0, 0, pictureBox1.Width, pictureBox1.Height));
            var scale = (float)pictureBox1.Width / pictureBox1.Image.Width;
            g.ScaleTransform(scale, scale);
            using (Pen pen = new Pen(foregroundColor.BackColor, trackBarSize.Value) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round })
            using (Brush brush = new SolidBrush(backgroundColor.BackColor))
            {
                if (rectButton.Checked)
                {
                    if (fillButton.Checked)
                    {
                        g.FillRectangle(brush, mRect);
                        g.DrawRectangle(pen, mRect);
                    }
                    else
                    {
                        g.DrawRectangle(pen, mRect);
                    }
                }
                else if (circleButton.Checked)
                {
                    if (fillButton.Checked)
                    {
                        g.FillEllipse(brush, mRect);
                        g.DrawEllipse(pen, mRect);
                    }
                    else
                    {
                        g.DrawEllipse(pen, mRect);
                    }
                }
                else if (pieButton.Checked)
                {
                    using (Pen penA = new Pen(foregroundColor.BackColor, trackBarSize.Value) { EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor, StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor })
                    {
                        if (mRect.Height != 0 && mRect.Width != 0)
                        {
                            var p2 = new Point(0, 0);
                            p2.X = mRect.Left == p.X ? mRect.Right : mRect.Left;
                            p2.Y = mRect.Top == p.Y ? mRect.Bottom : mRect.Top;
                            //if (p2.Y != 0 || p2.X != 0)
                            g.DrawLine(penA, p, p2);
                        }
                    }
                }
                else if (arrow1Button.Checked)
                {
                    using (Pen penA = new Pen(foregroundColor.BackColor, trackBarSize.Value) { EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor })
                    {
                        if (mRect.Height != 0 && mRect.Width != 0)
                        {
                            var p2 = new Point(0, 0);
                            p2.X = mRect.Left == p.X ? mRect.Right : mRect.Left;
                            p2.Y = mRect.Top == p.Y ? mRect.Bottom : mRect.Top;
                            //if (p2.Y != 0 || p2.X != 0)
                            g.DrawLine(penA, p, p2);
                        }
                    }
                }
                else if (lineButton.Checked)
                {
                    using (Pen pen3 = new Pen(foregroundColor.BackColor, trackBarSize.Value) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round })
                    {
                        if (mRect.Height != 0 && mRect.Width != 0)
                        {
                            var p2 = new Point(0, 0);
                            p2.X = mRect.Left == p.X ? mRect.Right : mRect.Left;
                            p2.Y = mRect.Top == p.Y ? mRect.Bottom : mRect.Top;
                            //if (p2.Y != 0 || p2.X != 0)
                            g.DrawLine(pen3, p, p2);
                        }
                    }
                }
                else if (triangleButton.Checked)
                {
                    if (fillButton.Checked)
                    {
                        g.FillTriangle(brush, mRect);
                        g.DrawTriangle(pen, mRect);
                    }
                    else
                    {
                        g.DrawTriangle(pen, mRect);
                    }
                }
                else if (selectionToolButton.Checked)
                {
                    using (Pen pen1 = new Pen(Color.White, 3) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round })
                    {
                        pen.Color = Color.Black;
                        pen.Width = 3;
                        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        if (selection)
                        {
                            g.DrawImage(temporarySelection, mRect);
                        }
                        g.DrawRectangle(pen1, mRect);
                        g.DrawRectangle(pen, mRect);
                    }
                }
            }
        }
        private void toolStripButton_Click(object sender, EventArgs e)
        {
            toolBox.Controls.OfType<CheckBox>().Where(c => c != fillButton).Select(c => c.Checked = false).ToArray();
            var btn = sender as CheckBox;
            btn.Checked = true;
            mRect = new Rectangle(0, 0, 0, 0);
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            var g = Graphics.FromImage(pictureBox1.Image);
            using (Pen pen = new Pen(foregroundColor.BackColor, trackBarSize.Value) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round })
            using (Brush brush = new SolidBrush(backgroundColor.BackColor))
            {
                if (rectButton.Checked)
                {
                    if (fillButton.Checked)
                    {
                        g.FillRectangle(brush, mRect);
                        g.DrawRectangle(pen, mRect);
                    }
                    else
                    {
                        g.DrawRectangle(pen, mRect);
                    }

                }
                else if (lineButton.Checked)
                {
                    var p2 = new Point(0, 0);
                    p2.X = mRect.Left == p.X ? mRect.Right : mRect.Left;
                    p2.Y = mRect.Top == p.Y ? mRect.Bottom : mRect.Top;
                    g.DrawLine(pen, p, p2);
                }
                else if (circleButton.Checked)
                {
                    if (fillButton.Checked)
                    {
                        g.FillEllipse(brush, mRect);
                        g.DrawEllipse(pen, mRect);
                    }
                    else
                    {
                        g.DrawEllipse(pen, mRect);
                    }

                }
                else if (triangleButton.Checked)
                {
                    if (fillButton.Checked)
                    {
                        g.FillTriangle(brush, mRect);
                        g.DrawTriangle(pen, mRect);
                    }
                    else
                    {
                        g.DrawTriangle(pen, mRect);
                    }

                }
                else if (pieButton.Checked)
                {
                    using (Pen penA = new Pen(foregroundColor.BackColor, trackBarSize.Value) { EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor, StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor })
                    {
                        var p2 = new Point(0, 0);
                        p2.X = mRect.Left == p.X ? mRect.Right : mRect.Left;
                        p2.Y = mRect.Top == p.Y ? mRect.Bottom : mRect.Top;
                        g.DrawLine(penA, p, p2);
                    }

                }
                else if (arrow1Button.Checked)
                {
                    using (Pen penA = new Pen(foregroundColor.BackColor, trackBarSize.Value) { EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor })
                    {
                        var p2 = new Point(0, 0);
                        p2.X = mRect.Left == p.X ? mRect.Right : mRect.Left;
                        p2.Y = mRect.Top == p.Y ? mRect.Bottom : mRect.Top;
                        g.DrawLine(penA, p, p2);
                    }

                }
                else if (selectionToolButton.Checked && !selection)
                {
                    selection = true;
                    var pf = (pictureBox1.Image as Bitmap).PixelFormat;
                    if (mRect.X + mRect.Width > pictureBox1.Image.Width)
                    {
                        mRect.Width = pictureBox1.Image.Width - mRect.X - 1;
                    }
                    if (mRect.Y + mRect.Height > pictureBox1.Image.Height)
                    {
                        mRect.Height = pictureBox1.Image.Height - mRect.Y - 1;
                    }
                    if (mRect.Y < 0)
                    {
                        mRect.Height += mRect.Y;
                        mRect.Y = 1;
                    }
                    if (mRect.X < 0)
                    {
                        mRect.Width += mRect.X;
                        mRect.X = 1;
                    }
                    temporarySelection = (pictureBox1.Image as Bitmap).Clone(mRect, pf);
                    g.FillRectangle(new SolidBrush(backgroundColor.BackColor), mRect);
                    //pictureBox1.Invalidate();
                }
            }
            if (!selection)
            {
                mRect = new Rectangle(0, 0, 0, 0);

            }
            pictureBox1.Invalidate();
        }

        private void magnifyButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!selection)
            {
                pictureBox1.Image = pictureBox1.Image.CopyAsGrayscale();
            }
            else
            {
                temporarySelection = temporarySelection.CopyAsGrayscale();
                pictureBox1.Invalidate();
            }
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!selection)
            {
                pictureBox1.Image = pictureBox1.Image.CopyAsSepiaTone();
            }
            else
            {

                temporarySelection = temporarySelection.CopyAsSepiaTone();
                pictureBox1.Invalidate();
            }
        }

        private void transparencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!selection)
            {
                pictureBox1.Image = pictureBox1.Image.CopyWithTransparency();
            }
            else
            {
                temporarySelection = temporarySelection.CopyWithTransparency();
                pictureBox1.Invalidate();
            }
        }

        private void negativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!selection)
            {
                pictureBox1.Image = pictureBox1.Image.CopyAsNegative();
            }
            else
            {
                temporarySelection = temporarySelection.CopyAsNegative();
                pictureBox1.Invalidate();
            }
        }

        private void shearedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!selection)
            {
                pictureBox1.Image = pictureBox1.Image.DrawAsSheared();
            }
            else
            {
                temporarySelection = temporarySelection.DrawAsSheared();
                pictureBox1.Invalidate();
            }
        }
        private void textButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pencliButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void selectionToolButton_CheckedChanged(object sender, EventArgs e)
        {
            if (selection)
            {
                selection = false;
                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {
                    g.DrawImage(temporarySelection, mRect);
                }
                pictureBox1.Invalidate();
            }

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selection)
            {
                Clipboard.SetImage(temporarySelection);
                buttonPaste2.Enabled = true;
                pasteToolStripMenuItem.Enabled = true;
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                selectionToolButton.Checked = false;
                selectionToolButton.Checked = true;
                selection = true;
                temporarySelection = Clipboard.GetImage() as Bitmap;
                mRect = new Rectangle(1, 1, temporarySelection.Width, temporarySelection.Height);
                pictureBox1.Invalidate();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selection)
            {
                Clipboard.SetImage(temporarySelection);
                selection = false;
                selectionToolButton.Checked = false;
                pictureBox1.Invalidate();
                buttonPaste2.Enabled = true;
                pasteToolStripMenuItem.Enabled = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2.BackColor = Color.IndianRed;
            toolBox.BackColor = Color.IndianRed;
            fileLabel.ForeColor = Color.White;
            labelTools.ForeColor = Color.White;
            labelShapes.ForeColor = Color.White;
            labelSelection.ForeColor = Color.White;
            sizeAndTextLabel.ForeColor = Color.White;
            labelColors.ForeColor = Color.White;
            panel2.BackColor = Color.White;
            panel3.BackColor = Color.White;
            panel4.BackColor = Color.White;
            panel5.BackColor = Color.White;
            panel6.BackColor = Color.White;
            menuStrip.BackColor = Color.IndianRed;
            labelMinVal.BackColor = Color.IndianRed;
        }

        private void trackBarSize_Scroll(object sender, EventArgs e)
        {
            labelSizeValue.Text = "" + trackBarSize.Value;
        }

        private void boldTextButton_CheckedChanged(object sender, EventArgs e)
        {
            //var rtb = this.ActiveControl as RichTextBox;
            //rtb.Font = new Font(rtb.Font, FontStyle.Bold);
        }

        private void fillButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void DrawingBoard_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape)
            //{
            //    if (rectButton.Checked)
            //    {
            //        this.Invalidate();
            //    }
            //}
        }

        private void fillSelectedArea_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void fillSelectedArea_Click(object sender, EventArgs e)
        {
            if (selection)
            {
                using (var graphics = Graphics.FromImage(temporarySelection))
                {
                    graphics.Clear(backgroundColor.BackColor);
                }
                pictureBox1.Invalidate();
            }

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDB about = new AboutDB();
            about.ShowDialog();
        }

        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2.BackColor = Color.FloralWhite;
            toolBox.BackColor = Color.FloralWhite;
            fileLabel.ForeColor = Color.Gray;
            labelTools.ForeColor = Color.Gray;
            labelShapes.ForeColor = Color.Gray;
            labelSelection.ForeColor = Color.Gray;
            sizeAndTextLabel.ForeColor = Color.Gray;
            labelColors.ForeColor = Color.Gray;
            panel2.BackColor = Color.Gray;
            panel3.BackColor = Color.Gray;
            panel4.BackColor = Color.Gray;
            panel5.BackColor = Color.Gray;
            panel6.BackColor = Color.Gray;
            labelMinVal.BackColor = Color.FloralWhite;
            menuStrip.BackColor = SystemColors.Control;
        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2.BackColor = Color.DarkGray;
            toolBox.BackColor = Color.DarkGray;
            labelMinVal.BackColor = Color.DarkGray;
            fileLabel.ForeColor = Color.White;
            labelTools.ForeColor = Color.White;
            labelShapes.ForeColor = Color.White;
            labelSelection.ForeColor = Color.White;
            sizeAndTextLabel.ForeColor = Color.White;
            labelColors.ForeColor = Color.White;
            menuStrip.BackColor = Color.DarkGray;
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2.BackColor = Color.CornflowerBlue;
            toolBox.BackColor = Color.CornflowerBlue;
            labelMinVal.BackColor = Color.CornflowerBlue;
            fileLabel.ForeColor = Color.White;
            labelTools.ForeColor = Color.White;
            labelShapes.ForeColor = Color.White;
            labelSelection.ForeColor = Color.White;
            sizeAndTextLabel.ForeColor = Color.White;
            labelColors.ForeColor = Color.White;
            menuStrip.BackColor = Color.CornflowerBlue;
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2.BackColor = Color.MediumSpringGreen;
            toolBox.BackColor = Color.MediumSpringGreen;
            fileLabel.ForeColor = Color.DarkBlue;
            labelTools.ForeColor = Color.DarkBlue;
            labelShapes.ForeColor = Color.DarkBlue;
            labelSelection.ForeColor = Color.DarkBlue;
            sizeAndTextLabel.ForeColor = Color.DarkBlue;
            labelColors.ForeColor = Color.DarkBlue;
            labelMinVal.BackColor = Color.MediumSpringGreen;
            menuStrip.BackColor = Color.MediumSpringGreen;
        }

        private void ukrainianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileLabel.Text = "Файл";
            labelTools.Text = "Інструменти";
            labelShapes.Text = "Фігури";
            labelSelection.Text = "Виділення";
            sizeAndTextLabel.Text = "Розміри кисті";
            labelColors.Text = "Кольори :";
            labelColor1.Text = "Колір 1";
            labelColor2.Text = "Колір 2";
            titleLabel.Text = "Координати :";
            CanvasSizelabel.Text = "Розміри полотна :";
            RGBLabel.Text = "Палітра (RGB) :";
            helpToolStripMenuItem.Text = "Допомога";
            fileToolStripMenuItem.Text = "Файл";
            toolsToolStripMenuItem.Text = "Інструменти";
            editToolStripMenuItem.Text = "Редагувати";
            newToolStripMenuItem.Text = "Новий файл";
            openToolStripMenuItem.Text = "Відкрити";
            saveToolStripMenuItem.Text = "Зберегти";
            exitToolStripMenuItem.Text = "Вийти";
            imageToolStripMenuItem.Text = "Зображення";
            cutToolStripMenuItem.Text = "Вирізати";
            copyToolStripMenuItem.Text = "Скопіювати";
            pasteToolStripMenuItem.Text = "Вставити";
            fIltersToolStripMenuItem.Text = "Фільтри";
            negativeToolStripMenuItem.Text = "Негатив";
            sepiaToolStripMenuItem.Text = "Сепія";
            transparencyToolStripMenuItem.Text = "Прозорість";
            grayscaleToolStripMenuItem.Text = "Відтінки сірого";
            blackAndWhiteToolStripMenuItem.Text = "Чорно-біле";
            swapRGBToBGRToolStripMenuItem.Text = "Змінити порядок кольорів";
            whiteToAplhaToolStripMenuItem.Text = "Білий до прозорого";
            polaroidColorToolStripMenuItem.Text = "Поляроїд";
            brightnessToolStripMenuItem.Text = "Яскравість та контраст";
            mirrorToolStripMenuItem.Text = "Поворот";
            clockwise90ToolStripMenuItem.Text = "За годинниковою стрілкою";
            counterClockwise90ToolStripMenuItem.Text = "Проти годинникової стрілки";
            customizeToolStripMenuItem.Text = "Змінити";
            optionsToolStripMenuItem.Text = "Опції";
            languageToolStripMenuItem.Text = "Мова";
            themesToolStripMenuItem.Text = "Теми";
            lightToolStripMenuItem.Text = "Світла";
            darkToolStripMenuItem.Text = "Темна";
            defaultToolStripMenuItem.Text = "Червона";
            blueToolStripMenuItem.Text = "Синя";
            greenToolStripMenuItem.Text = "Зелена";
            ukrainianToolStripMenuItem.Text = "Українська";
            englishToolStripMenuItem.Text = "Англійська";
            contentsToolStripMenuItem.Text = "Інструкція";
            aboutToolStripMenuItem.Text = "Про програму";
            mirrorToolStripMenuItem1.Text = "Дзеркальне відображення";
            labelScale.Text = "Масштаб :";
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileLabel.Text = "File";
            labelTools.Text = "Tools";
            labelShapes.Text = "Shapes";
            labelSelection.Text = "Selection";
            sizeAndTextLabel.Text = "Pixel Size";
            labelColors.Text = "Colors";
            labelColor1.Text = "Color 1";
            labelColor2.Text = "Color 2";
            titleLabel.Text = "Coordinates :";
            CanvasSizelabel.Text = "Canvas Size :";
            RGBLabel.Text = "RGB :";
            helpToolStripMenuItem.Text = "Help";
            fileToolStripMenuItem.Text = "File";
            toolsToolStripMenuItem.Text = "Tools";
            editToolStripMenuItem.Text = "Edit";
            newToolStripMenuItem.Text = "New";
            openToolStripMenuItem.Text = "Open";
            saveToolStripMenuItem.Text = "Save";
            exitToolStripMenuItem.Text = "Exit";
            imageToolStripMenuItem.Text = "Image";
            cutToolStripMenuItem.Text = "Cut";
            copyToolStripMenuItem.Text = "Copy";
            pasteToolStripMenuItem.Text = "Paste";
            fIltersToolStripMenuItem.Text = "Filters";
            negativeToolStripMenuItem.Text = "Negative";
            sepiaToolStripMenuItem.Text = "Sepia";
            transparencyToolStripMenuItem.Text = "Transparency";
            grayscaleToolStripMenuItem.Text = "Grayscale";
            blackAndWhiteToolStripMenuItem.Text = "Black and White";
            swapRGBToBGRToolStripMenuItem.Text = "Swap RGB to BGR";
            whiteToAplhaToolStripMenuItem.Text = "White to Aplha";
            polaroidColorToolStripMenuItem.Text = "Polaroid Color";
            brightnessToolStripMenuItem.Text = "Brightness And Contrast";
            mirrorToolStripMenuItem.Text = "Rotate";
            clockwise90ToolStripMenuItem.Text = "Clockwise +90";
            counterClockwise90ToolStripMenuItem.Text = "Counter clockwise -90";
            customizeToolStripMenuItem.Text = "Customize";
            optionsToolStripMenuItem.Text = "Options";
            languageToolStripMenuItem.Text = "Language";
            themesToolStripMenuItem.Text = "Themes";
            lightToolStripMenuItem.Text = "Light";
            darkToolStripMenuItem.Text = "Dark";
            defaultToolStripMenuItem.Text = "Red";
            blueToolStripMenuItem.Text = "Blue";
            greenToolStripMenuItem.Text = "Green";
            ukrainianToolStripMenuItem.Text = "Ukrainian";
            englishToolStripMenuItem.Text = "English";
            contentsToolStripMenuItem.Text = "Guide";
            aboutToolStripMenuItem.Text = "About";
            mirrorToolStripMenuItem1.Text = "Mirror";
            labelScale.Text = "Scale :";
        }

        private void mirrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        private Bitmap RotateImage(Bitmap bmp, float angle)
        {
            
            Bitmap rotatedImage = new Bitmap(bmp.Height,bmp.Width);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
              

                g.RotateTransform(angle);
                
                if(angle == 90)
                {
                    g.TranslateTransform(0, -bmp.Width);
                }
                else
                {
                    g.TranslateTransform(-bmp.Height, 0);
                }
                g.DrawImage(bmp, new Point(0, 0));
            }

            return rotatedImage;
        }
        private void clockwise90ToolStripMenuItem_Click(object sender, EventArgs e)
        {  
            pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox1.Invalidate();
            pictureBox1.Width = pictureBox1.Image.Width;
            pictureBox1.Height = pictureBox1.Image.Height;
        }

        private void counterClockwise90ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            pictureBox1.Invalidate();
            pictureBox1.Width = pictureBox1.Image.Width;
            pictureBox1.Height = pictureBox1.Image.Height;
        }
        private void SetBrightness(int brightness)
        {
            brightness = 100;
            Bitmap temp = (Bitmap)pictureBox1.Image;
            Bitmap bmap = (Bitmap)temp.Clone();
            if (brightness < -255) brightness = -255;
            if (brightness > 255) brightness = 255;
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    int cR = c.R + brightness;
                    int cG = c.G + brightness;
                    int cB = c.B + brightness;

                    if (cR < 0) cR = 1;
                    if (cR > 255) cR = 255;

                    if (cG < 0) cG = 1;
                    if (cG > 255) cG = 255;

                    if (cB < 0) cB = 1;
                    if (cB > 255) cB = 255;

                    bmap.SetPixel(i, j, Color.FromArgb((byte)cR, (byte)cG, (byte)cB));
                }
            }
            pictureBox1.Image = (Bitmap)bmap.Clone();
            pictureBox1.Invalidate();
        }

        public void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image = pictureBox1.Image.GBC(1.3f,1.5f);
            BrightnessAndContrast brc = new BrightnessAndContrast();
            brc.ShowDialog();

            if (brc.DialogResult == DialogResult.OK)
            {
                if (!selection)
                {

                    pictureBox1.Image = pictureBox1.Image.GBC(brc.getBrightness() / 10, brc.getContrast() / 10);
                }
                else
                {
                    temporarySelection = temporarySelection.GBC(brc.getBrightness() / 10, brc.getContrast() / 10);
                    pictureBox1.Invalidate();
                }
            }
        }

        private void swapRGBToBGRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!selection)
            {
                pictureBox1.Image = pictureBox1.Image.SwapRGB();
            }
            else
            {
                temporarySelection = temporarySelection.SwapRGB();
                pictureBox1.Invalidate();
            }
        }

        private void blackAndWhiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!selection)
            {
                pictureBox1.Image = pictureBox1.Image.BW();
            }
            else
            {
                temporarySelection = temporarySelection.BW();
                pictureBox1.Invalidate();
            }
        }

        private void polaroidColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!selection)
            {
                pictureBox1.Image = pictureBox1.Image.PolaroidColor();
            }
            else
            {
                temporarySelection = temporarySelection.PolaroidColor();
                pictureBox1.Invalidate();
            }
        }

        private void whiteToAplhaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!selection)
            {
                pictureBox1.Image = pictureBox1.Image.WhiteToAplha();
            }
            else
            {
                temporarySelection = temporarySelection.WhiteToAplha();
                pictureBox1.Invalidate();
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image = null;
        }

        private void mirrorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
            pictureBox1.Invalidate();
            pictureBox1.Width = pictureBox1.Image.Width;
            pictureBox1.Height = pictureBox1.Image.Height;
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppFunctionality appf = new AppFunctionality();
            appf.Show();
        }
    }
}

