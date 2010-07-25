namespace Rockband_Drum_Kit
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cmbRedPad = new System.Windows.Forms.ComboBox();
            this.getDrums = new System.Windows.Forms.Button();
            this.cmbYellowPad = new System.Windows.Forms.ComboBox();
            this.cmbBluePad = new System.Windows.Forms.ComboBox();
            this.cmbGreenPad = new System.Windows.Forms.ComboBox();
            this.cmbOrangePedal = new System.Windows.Forms.ComboBox();
            this.lblDrumKit = new System.Windows.Forms.Label();
            this.cmb_MidiPercusions = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.metronome1 = new Rockband_Drum_Kit.Controls.Metronome();
            this.rockBandDrumControler1 = new Rockband_Drum_Kit.RockBandDrumControler();
            this.SuspendLayout();
            // 
            // cmbRedPad
            // 
            this.cmbRedPad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRedPad.FormattingEnabled = true;
            this.cmbRedPad.Items.AddRange(new object[] {
            "Kick",
            "Snare",
            "Hat",
            "Clap"});
            this.cmbRedPad.Location = new System.Drawing.Point(26, 188);
            this.cmbRedPad.Name = "cmbRedPad";
            this.cmbRedPad.Size = new System.Drawing.Size(77, 21);
            this.cmbRedPad.TabIndex = 1;
            this.cmbRedPad.SelectedIndexChanged += new System.EventHandler(this.cmbRedPad_SelectedIndexChanged);
            // 
            // getDrums
            // 
            this.getDrums.Location = new System.Drawing.Point(3, 379);
            this.getDrums.Name = "getDrums";
            this.getDrums.Size = new System.Drawing.Size(75, 23);
            this.getDrums.TabIndex = 3;
            this.getDrums.Text = "set Drums";
            this.getDrums.UseVisualStyleBackColor = true;
            this.getDrums.Click += new System.EventHandler(this.getDrums_Click);
            // 
            // cmbYellowPad
            // 
            this.cmbYellowPad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYellowPad.FormattingEnabled = true;
            this.cmbYellowPad.Items.AddRange(new object[] {
            "Kick",
            "Snare",
            "Hat",
            "Clap"});
            this.cmbYellowPad.Location = new System.Drawing.Point(62, 129);
            this.cmbYellowPad.Name = "cmbYellowPad";
            this.cmbYellowPad.Size = new System.Drawing.Size(77, 21);
            this.cmbYellowPad.TabIndex = 1;
            this.cmbYellowPad.SelectedIndexChanged += new System.EventHandler(this.cmbYellowPad_SelectedIndexChanged);
            // 
            // cmbBluePad
            // 
            this.cmbBluePad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBluePad.FormattingEnabled = true;
            this.cmbBluePad.Items.AddRange(new object[] {
            "Kick",
            "Snare",
            "Hat",
            "Clap"});
            this.cmbBluePad.Location = new System.Drawing.Point(399, 129);
            this.cmbBluePad.Name = "cmbBluePad";
            this.cmbBluePad.Size = new System.Drawing.Size(77, 21);
            this.cmbBluePad.TabIndex = 1;
            this.cmbBluePad.SelectedIndexChanged += new System.EventHandler(this.cmbBluePad_SelectedIndexChanged);
            // 
            // cmbGreenPad
            // 
            this.cmbGreenPad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGreenPad.FormattingEnabled = true;
            this.cmbGreenPad.Items.AddRange(new object[] {
            "Kick",
            "Snare",
            "Hat",
            "Clap"});
            this.cmbGreenPad.Location = new System.Drawing.Point(426, 188);
            this.cmbGreenPad.Name = "cmbGreenPad";
            this.cmbGreenPad.Size = new System.Drawing.Size(77, 21);
            this.cmbGreenPad.TabIndex = 1;
            this.cmbGreenPad.SelectedIndexChanged += new System.EventHandler(this.cmbGreenPad_SelectedIndexChanged);
            // 
            // cmbOrangePedal
            // 
            this.cmbOrangePedal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrangePedal.FormattingEnabled = true;
            this.cmbOrangePedal.Items.AddRange(new object[] {
            "Kick",
            "Snare",
            "Hat",
            "Clap"});
            this.cmbOrangePedal.Location = new System.Drawing.Point(233, 267);
            this.cmbOrangePedal.Name = "cmbOrangePedal";
            this.cmbOrangePedal.Size = new System.Drawing.Size(77, 21);
            this.cmbOrangePedal.TabIndex = 1;
            this.cmbOrangePedal.SelectionChangeCommitted += new System.EventHandler(this.cmbOrangePedal_SelectionChangeCommitted);
            // 
            // lblDrumKit
            // 
            this.lblDrumKit.AutoSize = true;
            this.lblDrumKit.BackColor = System.Drawing.Color.Transparent;
            this.lblDrumKit.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDrumKit.Location = new System.Drawing.Point(84, 384);
            this.lblDrumKit.Name = "lblDrumKit";
            this.lblDrumKit.Size = new System.Drawing.Size(96, 13);
            this.lblDrumKit.TabIndex = 12;
            this.lblDrumKit.Text = "Drums Not Loaded";
            // 
            // cmb_MidiPercusions
            // 
            this.cmb_MidiPercusions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_MidiPercusions.FormattingEnabled = true;
            this.cmb_MidiPercusions.Location = new System.Drawing.Point(416, 381);
            this.cmb_MidiPercusions.Name = "cmb_MidiPercusions";
            this.cmb_MidiPercusions.Size = new System.Drawing.Size(121, 21);
            this.cmb_MidiPercusions.TabIndex = 15;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(335, 381);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "Play";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // metronome1
            // 
            this.metronome1.BackColor = System.Drawing.Color.Transparent;
            this.metronome1.BPM = 120;
            this.metronome1.Location = new System.Drawing.Point(337, 325);
            this.metronome1.Name = "metronome1";
            this.metronome1.Size = new System.Drawing.Size(200, 50);
            this.metronome1.TabIndex = 21;
            this.metronome1.Text = "metronome1";
            // 
            // rockBandDrumControler1
            // 
            this.rockBandDrumControler1.BackColor = System.Drawing.Color.Transparent;
            this.rockBandDrumControler1.Location = new System.Drawing.Point(143, 118);
            this.rockBandDrumControler1.Name = "rockBandDrumControler1";
            this.rockBandDrumControler1.Size = new System.Drawing.Size(254, 140);
            this.rockBandDrumControler1.TabIndex = 13;
            this.rockBandDrumControler1.Text = "rockBandDrumControler1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(138)))), ((int)(((byte)(139)))));
            this.ClientSize = new System.Drawing.Size(549, 412);
            this.Controls.Add(this.metronome1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmb_MidiPercusions);
            this.Controls.Add(this.rockBandDrumControler1);
            this.Controls.Add(this.lblDrumKit);
            this.Controls.Add(this.getDrums);
            this.Controls.Add(this.cmbOrangePedal);
            this.Controls.Add(this.cmbGreenPad);
            this.Controls.Add(this.cmbBluePad);
            this.Controls.Add(this.cmbYellowPad);
            this.Controls.Add(this.cmbRedPad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rockband™ Drum Kit - by: Rickey Ward Pre-Beta V0.01";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbRedPad;
        private System.Windows.Forms.Button getDrums;
        private System.Windows.Forms.ComboBox cmbYellowPad;
        private System.Windows.Forms.ComboBox cmbBluePad;
        private System.Windows.Forms.ComboBox cmbGreenPad;
        private System.Windows.Forms.ComboBox cmbOrangePedal;
        private System.Windows.Forms.Label lblDrumKit;
        private RockBandDrumControler rockBandDrumControler1;
        private System.Windows.Forms.ComboBox cmb_MidiPercusions;
        private System.Windows.Forms.Button button1;
        private Rockband_Drum_Kit.Controls.Metronome metronome1;
    }
}

