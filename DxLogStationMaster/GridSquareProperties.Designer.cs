
namespace DXLog.net
{
    partial class GridSquareProperties
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDefaults = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkZoomToQsos = new System.Windows.Forms.CheckBox();
            this.chkShowGridSubsquaresLabel = new System.Windows.Forms.CheckBox();
            this.chkShowGridSquaresLabel = new System.Windows.Forms.CheckBox();
            this.chkShowGridFieldsLabel = new System.Windows.Forms.CheckBox();
            this.chkShowGridLabelsMaster = new System.Windows.Forms.CheckBox();
            this.chkColourWorkedGridSquares = new System.Windows.Forms.CheckBox();
            this.chkDisplayContacts = new System.Windows.Forms.CheckBox();
            this.chkDisplaySpots = new System.Windows.Forms.CheckBox();
            this.chkShowGridSubsquares = new System.Windows.Forms.CheckBox();
            this.chkShowGridSquares = new System.Windows.Forms.CheckBox();
            this.chkShowGridFields = new System.Windows.Forms.CheckBox();
            this.chkShowGridMaster = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboStartZoom = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboMaxZoom = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboMinZoom = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboMapProvider = new System.Windows.Forms.ComboBox();
            this.chkCentreMapOnQth = new System.Windows.Forms.CheckBox();
            this.txtCentreMapOnLocator = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(516, 322);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 91;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(435, 322);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 90;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDefaults
            // 
            this.btnDefaults.Location = new System.Drawing.Point(342, 322);
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.Size = new System.Drawing.Size(75, 23);
            this.btnDefaults.TabIndex = 89;
            this.btnDefaults.Text = "Defaults";
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkZoomToQsos);
            this.groupBox1.Controls.Add(this.chkShowGridSubsquaresLabel);
            this.groupBox1.Controls.Add(this.chkShowGridSquaresLabel);
            this.groupBox1.Controls.Add(this.chkShowGridFieldsLabel);
            this.groupBox1.Controls.Add(this.chkShowGridLabelsMaster);
            this.groupBox1.Controls.Add(this.chkColourWorkedGridSquares);
            this.groupBox1.Controls.Add(this.chkDisplayContacts);
            this.groupBox1.Controls.Add(this.chkDisplaySpots);
            this.groupBox1.Controls.Add(this.chkShowGridSubsquares);
            this.groupBox1.Controls.Add(this.chkShowGridSquares);
            this.groupBox1.Controls.Add(this.chkShowGridFields);
            this.groupBox1.Controls.Add(this.chkShowGridMaster);
            this.groupBox1.Location = new System.Drawing.Point(310, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(281, 304);
            this.groupBox1.TabIndex = 92;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // chkZoomToQsos
            // 
            this.chkZoomToQsos.AutoSize = true;
            this.chkZoomToQsos.Location = new System.Drawing.Point(15, 275);
            this.chkZoomToQsos.Name = "chkZoomToQsos";
            this.chkZoomToQsos.Size = new System.Drawing.Size(100, 17);
            this.chkZoomToQsos.TabIndex = 6;
            this.chkZoomToQsos.Text = "Zoom To QSOs";
            this.chkZoomToQsos.UseVisualStyleBackColor = true;
            // 
            // chkShowGridSubsquaresLabel
            // 
            this.chkShowGridSubsquaresLabel.AutoSize = true;
            this.chkShowGridSubsquaresLabel.Location = new System.Drawing.Point(28, 206);
            this.chkShowGridSubsquaresLabel.Name = "chkShowGridSubsquaresLabel";
            this.chkShowGridSubsquaresLabel.Size = new System.Drawing.Size(181, 17);
            this.chkShowGridSubsquaresLabel.TabIndex = 10;
            this.chkShowGridSubsquaresLabel.Text = "Show Subsquares. (EG: IO91CL)";
            this.chkShowGridSubsquaresLabel.UseVisualStyleBackColor = true;
            this.chkShowGridSubsquaresLabel.CheckedChanged += new System.EventHandler(this.chkShowGridLabel_CheckedChanged);
            // 
            // chkShowGridSquaresLabel
            // 
            this.chkShowGridSquaresLabel.AutoSize = true;
            this.chkShowGridSquaresLabel.Location = new System.Drawing.Point(28, 183);
            this.chkShowGridSquaresLabel.Name = "chkShowGridSquaresLabel";
            this.chkShowGridSquaresLabel.Size = new System.Drawing.Size(151, 17);
            this.chkShowGridSquaresLabel.TabIndex = 9;
            this.chkShowGridSquaresLabel.Text = "Show Squares. (EG: IO91)";
            this.chkShowGridSquaresLabel.UseVisualStyleBackColor = true;
            this.chkShowGridSquaresLabel.CheckedChanged += new System.EventHandler(this.chkShowGridLabel_CheckedChanged);
            // 
            // chkShowGridFieldsLabel
            // 
            this.chkShowGridFieldsLabel.AutoSize = true;
            this.chkShowGridFieldsLabel.Location = new System.Drawing.Point(28, 160);
            this.chkShowGridFieldsLabel.Name = "chkShowGridFieldsLabel";
            this.chkShowGridFieldsLabel.Size = new System.Drawing.Size(127, 17);
            this.chkShowGridFieldsLabel.TabIndex = 8;
            this.chkShowGridFieldsLabel.Text = "Show Fields. (EG: IO)";
            this.chkShowGridFieldsLabel.UseVisualStyleBackColor = true;
            this.chkShowGridFieldsLabel.CheckedChanged += new System.EventHandler(this.chkShowGridLabel_CheckedChanged);
            // 
            // chkShowGridLabelsMaster
            // 
            this.chkShowGridLabelsMaster.AutoSize = true;
            this.chkShowGridLabelsMaster.Location = new System.Drawing.Point(15, 137);
            this.chkShowGridLabelsMaster.Name = "chkShowGridLabelsMaster";
            this.chkShowGridLabelsMaster.Size = new System.Drawing.Size(87, 17);
            this.chkShowGridLabelsMaster.TabIndex = 7;
            this.chkShowGridLabelsMaster.Text = "Show Labels";
            this.chkShowGridLabelsMaster.UseVisualStyleBackColor = true;
            this.chkShowGridLabelsMaster.Click += new System.EventHandler(this.chkShowGridLabelsMaster_Click);
            // 
            // chkColourWorkedGridSquares
            // 
            this.chkColourWorkedGridSquares.AutoSize = true;
            this.chkColourWorkedGridSquares.Location = new System.Drawing.Point(15, 22);
            this.chkColourWorkedGridSquares.Name = "chkColourWorkedGridSquares";
            this.chkColourWorkedGridSquares.Size = new System.Drawing.Size(217, 17);
            this.chkColourWorkedGridSquares.TabIndex = 6;
            this.chkColourWorkedGridSquares.Text = "Colour Worked Grid Squares: (EG: IO91)";
            this.chkColourWorkedGridSquares.UseVisualStyleBackColor = true;
            // 
            // chkDisplayContacts
            // 
            this.chkDisplayContacts.AutoSize = true;
            this.chkDisplayContacts.Location = new System.Drawing.Point(15, 252);
            this.chkDisplayContacts.Name = "chkDisplayContacts";
            this.chkDisplayContacts.Size = new System.Drawing.Size(105, 17);
            this.chkDisplayContacts.TabIndex = 5;
            this.chkDisplayContacts.Text = "Display Contacts";
            this.chkDisplayContacts.UseVisualStyleBackColor = true;
            // 
            // chkDisplaySpots
            // 
            this.chkDisplaySpots.AutoSize = true;
            this.chkDisplaySpots.Location = new System.Drawing.Point(15, 229);
            this.chkDisplaySpots.Name = "chkDisplaySpots";
            this.chkDisplaySpots.Size = new System.Drawing.Size(90, 17);
            this.chkDisplaySpots.TabIndex = 4;
            this.chkDisplaySpots.Text = "Display Spots";
            this.chkDisplaySpots.UseVisualStyleBackColor = true;
            // 
            // chkShowGridSubsquares
            // 
            this.chkShowGridSubsquares.AutoSize = true;
            this.chkShowGridSubsquares.Location = new System.Drawing.Point(28, 114);
            this.chkShowGridSubsquares.Name = "chkShowGridSubsquares";
            this.chkShowGridSubsquares.Size = new System.Drawing.Size(181, 17);
            this.chkShowGridSubsquares.TabIndex = 3;
            this.chkShowGridSubsquares.Text = "Show Subsquares. (EG: IO91CL)";
            this.chkShowGridSubsquares.UseVisualStyleBackColor = true;
            this.chkShowGridSubsquares.CheckedChanged += new System.EventHandler(this.chkShowGrid_CheckedChanged);
            // 
            // chkShowGridSquares
            // 
            this.chkShowGridSquares.AutoSize = true;
            this.chkShowGridSquares.Location = new System.Drawing.Point(28, 91);
            this.chkShowGridSquares.Name = "chkShowGridSquares";
            this.chkShowGridSquares.Size = new System.Drawing.Size(151, 17);
            this.chkShowGridSquares.TabIndex = 2;
            this.chkShowGridSquares.Text = "Show Squares. (EG: IO91)";
            this.chkShowGridSquares.UseVisualStyleBackColor = true;
            this.chkShowGridSquares.CheckedChanged += new System.EventHandler(this.chkShowGrid_CheckedChanged);
            // 
            // chkShowGridFields
            // 
            this.chkShowGridFields.AutoSize = true;
            this.chkShowGridFields.Location = new System.Drawing.Point(28, 68);
            this.chkShowGridFields.Name = "chkShowGridFields";
            this.chkShowGridFields.Size = new System.Drawing.Size(127, 17);
            this.chkShowGridFields.TabIndex = 1;
            this.chkShowGridFields.Text = "Show Fields. (EG: IO)";
            this.chkShowGridFields.UseVisualStyleBackColor = true;
            this.chkShowGridFields.CheckedChanged += new System.EventHandler(this.chkShowGrid_CheckedChanged);
            // 
            // chkShowGridMaster
            // 
            this.chkShowGridMaster.AutoSize = true;
            this.chkShowGridMaster.Location = new System.Drawing.Point(15, 45);
            this.chkShowGridMaster.Name = "chkShowGridMaster";
            this.chkShowGridMaster.Size = new System.Drawing.Size(117, 17);
            this.chkShowGridMaster.TabIndex = 0;
            this.chkShowGridMaster.Text = "Show Grid Squares";
            this.chkShowGridMaster.UseVisualStyleBackColor = true;
            this.chkShowGridMaster.Click += new System.EventHandler(this.chkShowGridMaster_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cboStartZoom);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cboMaxZoom);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cboMinZoom);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cboMapProvider);
            this.groupBox2.Controls.Add(this.chkCentreMapOnQth);
            this.groupBox2.Controls.Add(this.txtCentreMapOnLocator);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(281, 304);
            this.groupBox2.TabIndex = 93;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Map Configuration";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 153);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Start Zoom:";
            // 
            // cboStartZoom
            // 
            this.cboStartZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStartZoom.FormattingEnabled = true;
            this.cboStartZoom.Location = new System.Drawing.Point(75, 150);
            this.cboStartZoom.Name = "cboStartZoom";
            this.cboStartZoom.Size = new System.Drawing.Size(63, 21);
            this.cboStartZoom.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Max Zoom:";
            // 
            // cboMaxZoom
            // 
            this.cboMaxZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMaxZoom.FormattingEnabled = true;
            this.cboMaxZoom.Location = new System.Drawing.Point(75, 123);
            this.cboMaxZoom.Name = "cboMaxZoom";
            this.cboMaxZoom.Size = new System.Drawing.Size(63, 21);
            this.cboMaxZoom.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Min Zoom:";
            // 
            // cboMinZoom
            // 
            this.cboMinZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMinZoom.FormattingEnabled = true;
            this.cboMinZoom.Location = new System.Drawing.Point(75, 96);
            this.cboMinZoom.Name = "cboMinZoom";
            this.cboMinZoom.Size = new System.Drawing.Size(63, 21);
            this.cboMinZoom.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 271);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(217, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "* colours are configured in the standard way.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Source:";
            // 
            // cboMapProvider
            // 
            this.cboMapProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMapProvider.FormattingEnabled = true;
            this.cboMapProvider.Location = new System.Drawing.Point(75, 67);
            this.cboMapProvider.Name = "cboMapProvider";
            this.cboMapProvider.Size = new System.Drawing.Size(184, 21);
            this.cboMapProvider.TabIndex = 3;
            // 
            // chkCentreMapOnQth
            // 
            this.chkCentreMapOnQth.AutoSize = true;
            this.chkCentreMapOnQth.Location = new System.Drawing.Point(16, 46);
            this.chkCentreMapOnQth.Name = "chkCentreMapOnQth";
            this.chkCentreMapOnQth.Size = new System.Drawing.Size(198, 17);
            this.chkCentreMapOnQth.TabIndex = 2;
            this.chkCentreMapOnQth.Text = "Always centre on the operating QTH";
            this.chkCentreMapOnQth.UseVisualStyleBackColor = true;
            // 
            // txtCentreMapOnLocator
            // 
            this.txtCentreMapOnLocator.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCentreMapOnLocator.Location = new System.Drawing.Point(169, 20);
            this.txtCentreMapOnLocator.MaxLength = 6;
            this.txtCentreMapOnLocator.Name = "txtCentreMapOnLocator";
            this.txtCentreMapOnLocator.Size = new System.Drawing.Size(53, 20);
            this.txtCentreMapOnLocator.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Centre the map on the locator:";
            // 
            // GridSquareProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(603, 354);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDefaults);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GridSquareProperties";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Grid Square Properties";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDefaults;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkColourWorkedGridSquares;
        private System.Windows.Forms.CheckBox chkDisplayContacts;
        private System.Windows.Forms.CheckBox chkDisplaySpots;
        private System.Windows.Forms.CheckBox chkShowGridSubsquares;
        private System.Windows.Forms.CheckBox chkShowGridSquares;
        private System.Windows.Forms.CheckBox chkShowGridFields;
        private System.Windows.Forms.CheckBox chkShowGridMaster;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkCentreMapOnQth;
        private System.Windows.Forms.TextBox txtCentreMapOnLocator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkShowGridSubsquaresLabel;
        private System.Windows.Forms.CheckBox chkShowGridSquaresLabel;
        private System.Windows.Forms.CheckBox chkShowGridFieldsLabel;
        private System.Windows.Forms.CheckBox chkShowGridLabelsMaster;
        private System.Windows.Forms.CheckBox chkZoomToQsos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboMapProvider;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboMaxZoom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboMinZoom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboStartZoom;
    }
}