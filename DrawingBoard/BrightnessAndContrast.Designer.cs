namespace DrawingBoard
{
    partial class BrightnessAndContrast
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.labelMaxBr = new System.Windows.Forms.Label();
            this.labelMinBr = new System.Windows.Forms.Label();
            this.labelValueBr = new System.Windows.Forms.Label();
            this.trackBarBr = new System.Windows.Forms.TrackBar();
            this.labelMaxCon = new System.Windows.Forms.Label();
            this.labelMinCon = new System.Windows.Forms.Label();
            this.labelValueCon = new System.Windows.Forms.Label();
            this.trackBarContr = new System.Windows.Forms.TrackBar();
            this.labelBr = new System.Windows.Forms.Label();
            this.labelContr = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarContr)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(151, 189);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelMaxBr
            // 
            this.labelMaxBr.AutoSize = true;
            this.labelMaxBr.Location = new System.Drawing.Point(206, 67);
            this.labelMaxBr.Name = "labelMaxBr";
            this.labelMaxBr.Size = new System.Drawing.Size(10, 13);
            this.labelMaxBr.TabIndex = 72;
            this.labelMaxBr.Text = " ";
            // 
            // labelMinBr
            // 
            this.labelMinBr.AutoSize = true;
            this.labelMinBr.BackColor = System.Drawing.SystemColors.Control;
            this.labelMinBr.Location = new System.Drawing.Point(113, 67);
            this.labelMinBr.Name = "labelMinBr";
            this.labelMinBr.Size = new System.Drawing.Size(10, 13);
            this.labelMinBr.TabIndex = 71;
            this.labelMinBr.Text = " ";
            // 
            // labelValueBr
            // 
            this.labelValueBr.AutoSize = true;
            this.labelValueBr.Location = new System.Drawing.Point(165, 62);
            this.labelValueBr.Name = "labelValueBr";
            this.labelValueBr.Size = new System.Drawing.Size(10, 13);
            this.labelValueBr.TabIndex = 70;
            this.labelValueBr.Text = " ";
            // 
            // trackBarBr
            // 
            this.trackBarBr.AutoSize = false;
            this.trackBarBr.Location = new System.Drawing.Point(104, 43);
            this.trackBarBr.Name = "trackBarBr";
            this.trackBarBr.Size = new System.Drawing.Size(132, 28);
            this.trackBarBr.TabIndex = 69;
            this.trackBarBr.TickFrequency = 10;
            this.trackBarBr.Scroll += new System.EventHandler(this.trackBarBr_Scroll);
            // 
            // labelMaxCon
            // 
            this.labelMaxCon.AutoSize = true;
            this.labelMaxCon.Location = new System.Drawing.Point(206, 138);
            this.labelMaxCon.Name = "labelMaxCon";
            this.labelMaxCon.Size = new System.Drawing.Size(10, 13);
            this.labelMaxCon.TabIndex = 76;
            this.labelMaxCon.Text = " ";
            // 
            // labelMinCon
            // 
            this.labelMinCon.AutoSize = true;
            this.labelMinCon.BackColor = System.Drawing.SystemColors.Control;
            this.labelMinCon.Location = new System.Drawing.Point(113, 138);
            this.labelMinCon.Name = "labelMinCon";
            this.labelMinCon.Size = new System.Drawing.Size(10, 13);
            this.labelMinCon.TabIndex = 75;
            this.labelMinCon.Text = " ";
            // 
            // labelValueCon
            // 
            this.labelValueCon.AutoSize = true;
            this.labelValueCon.Location = new System.Drawing.Point(165, 133);
            this.labelValueCon.Name = "labelValueCon";
            this.labelValueCon.Size = new System.Drawing.Size(10, 13);
            this.labelValueCon.TabIndex = 74;
            this.labelValueCon.Text = " ";
            // 
            // trackBarContr
            // 
            this.trackBarContr.AutoSize = false;
            this.trackBarContr.Location = new System.Drawing.Point(104, 114);
            this.trackBarContr.Name = "trackBarContr";
            this.trackBarContr.Size = new System.Drawing.Size(132, 28);
            this.trackBarContr.TabIndex = 73;
            this.trackBarContr.TickFrequency = 10;
            this.trackBarContr.Scroll += new System.EventHandler(this.trackBarContr_Scroll);
            // 
            // labelBr
            // 
            this.labelBr.AutoSize = true;
            this.labelBr.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelBr.Location = new System.Drawing.Point(13, 43);
            this.labelBr.Name = "labelBr";
            this.labelBr.Size = new System.Drawing.Size(82, 18);
            this.labelBr.TabIndex = 77;
            this.labelBr.Text = "Brightness:";
            // 
            // labelContr
            // 
            this.labelContr.AutoSize = true;
            this.labelContr.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelContr.Location = new System.Drawing.Point(13, 114);
            this.labelContr.Name = "labelContr";
            this.labelContr.Size = new System.Drawing.Size(69, 18);
            this.labelContr.TabIndex = 78;
            this.labelContr.Text = "Contrast:";
            // 
            // BrightnessAndContrast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(237, 223);
            this.Controls.Add(this.labelContr);
            this.Controls.Add(this.labelBr);
            this.Controls.Add(this.labelMaxCon);
            this.Controls.Add(this.labelMinCon);
            this.Controls.Add(this.labelValueCon);
            this.Controls.Add(this.trackBarContr);
            this.Controls.Add(this.labelMaxBr);
            this.Controls.Add(this.labelMinBr);
            this.Controls.Add(this.labelValueBr);
            this.Controls.Add(this.trackBarBr);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(253, 262);
            this.MinimumSize = new System.Drawing.Size(253, 262);
            this.Name = "BrightnessAndContrast";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "BrightnessAndContrast";
            this.Load += new System.EventHandler(this.BrightnessAndContrast_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarContr)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelMaxBr;
        private System.Windows.Forms.Label labelMinBr;
        private System.Windows.Forms.Label labelValueBr;
        private System.Windows.Forms.TrackBar trackBarBr;
        private System.Windows.Forms.Label labelMaxCon;
        private System.Windows.Forms.Label labelMinCon;
        private System.Windows.Forms.Label labelValueCon;
        private System.Windows.Forms.TrackBar trackBarContr;
        private System.Windows.Forms.Label labelBr;
        private System.Windows.Forms.Label labelContr;
    }
}