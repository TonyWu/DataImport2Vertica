namespace BIETLTool
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
            this.components = new System.ComponentModel.Container();
            this.startButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.originUsername = new System.Windows.Forms.TextBox();
            this.originPassword = new System.Windows.Forms.TextBox();
            this.destinationPassword = new System.Windows.Forms.TextBox();
            this.destinationUsername = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.folderName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.maxRecordCounter = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.loadedCount = new System.Windows.Forms.Label();
            this.originServer = new System.Windows.Forms.ComboBox();
            this.originDatabase = new System.Windows.Forms.ComboBox();
            this.originTable = new System.Windows.Forms.ComboBox();
            this.destinationTable = new System.Windows.Forms.ComboBox();
            this.destinationServer = new System.Windows.Forms.ComboBox();
            this.destinationDatabase = new System.Windows.Forms.TextBox();
            this.truncateDestination = new System.Windows.Forms.CheckBox();
            this.originSQL = new System.Windows.Forms.TextBox();
            this.enableSQL = new System.Windows.Forms.CheckBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.originDeltas = new System.Windows.Forms.CheckBox();
            this.originDeltaID = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(309, 415);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(112, 23);
            this.startButton.TabIndex = 12;
            this.startButton.Text = "Start Streaming";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Origin Server:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Origin Database:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "User Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Password:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Origin Table:";
            // 
            // originUsername
            // 
            this.originUsername.Location = new System.Drawing.Point(184, 146);
            this.originUsername.Name = "originUsername";
            this.originUsername.Size = new System.Drawing.Size(237, 20);
            this.originUsername.TabIndex = 3;
            // 
            // originPassword
            // 
            this.originPassword.Location = new System.Drawing.Point(184, 172);
            this.originPassword.Name = "originPassword";
            this.originPassword.Size = new System.Drawing.Size(237, 20);
            this.originPassword.TabIndex = 4;
            this.originPassword.UseSystemPasswordChar = true;
            // 
            // destinationPassword
            // 
            this.destinationPassword.Location = new System.Drawing.Point(184, 304);
            this.destinationPassword.Name = "destinationPassword";
            this.destinationPassword.Size = new System.Drawing.Size(237, 20);
            this.destinationPassword.TabIndex = 9;
            this.destinationPassword.Text = "birocks992";
            this.destinationPassword.UseSystemPasswordChar = true;
            // 
            // destinationUsername
            // 
            this.destinationUsername.Location = new System.Drawing.Point(184, 278);
            this.destinationUsername.Name = "destinationUsername";
            this.destinationUsername.Size = new System.Drawing.Size(237, 20);
            this.destinationUsername.TabIndex = 8;
            this.destinationUsername.Text = "dbadmin";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 259);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Destination Table:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 311);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Password:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 285);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "User Name:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 233);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Destination Database:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 205);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Destination Server:";
            // 
            // folderName
            // 
            this.folderName.Location = new System.Drawing.Point(184, 330);
            this.folderName.Name = "folderName";
            this.folderName.Size = new System.Drawing.Size(237, 20);
            this.folderName.TabIndex = 10;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 337);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 13);
            this.label11.TabIndex = 26;
            this.label11.Text = "Folder:";
            // 
            // maxRecordCounter
            // 
            this.maxRecordCounter.Location = new System.Drawing.Point(184, 357);
            this.maxRecordCounter.Name = "maxRecordCounter";
            this.maxRecordCounter.Size = new System.Drawing.Size(237, 20);
            this.maxRecordCounter.TabIndex = 11;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 364);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(111, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "Max Records Per File:";
            // 
            // loadedCount
            // 
            this.loadedCount.AutoSize = true;
            this.loadedCount.ForeColor = System.Drawing.Color.Crimson;
            this.loadedCount.Location = new System.Drawing.Point(12, 396);
            this.loadedCount.Name = "loadedCount";
            this.loadedCount.Size = new System.Drawing.Size(54, 13);
            this.loadedCount.TabIndex = 30;
            this.loadedCount.Text = "Loading...";
            // 
            // originServer
            // 
            this.originServer.FormattingEnabled = true;
            this.originServer.Location = new System.Drawing.Point(184, 7);
            this.originServer.Name = "originServer";
            this.originServer.Size = new System.Drawing.Size(237, 21);
            this.originServer.TabIndex = 32;
            this.originServer.DropDown += new System.EventHandler(this.originServer_DropDown);
            // 
            // originDatabase
            // 
            this.originDatabase.FormattingEnabled = true;
            this.originDatabase.Location = new System.Drawing.Point(184, 34);
            this.originDatabase.Name = "originDatabase";
            this.originDatabase.Size = new System.Drawing.Size(237, 21);
            this.originDatabase.TabIndex = 33;
            this.originDatabase.DropDown += new System.EventHandler(this.originDatabase_DropDown);
            // 
            // originTable
            // 
            this.originTable.FormattingEnabled = true;
            this.originTable.Location = new System.Drawing.Point(184, 61);
            this.originTable.Name = "originTable";
            this.originTable.Size = new System.Drawing.Size(237, 21);
            this.originTable.TabIndex = 34;
            this.originTable.DropDown += new System.EventHandler(this.originTable_DropDown);
            // 
            // destinationTable
            // 
            this.destinationTable.FormattingEnabled = true;
            this.destinationTable.Location = new System.Drawing.Point(184, 252);
            this.destinationTable.Name = "destinationTable";
            this.destinationTable.Size = new System.Drawing.Size(237, 21);
            this.destinationTable.TabIndex = 37;
            this.destinationTable.DropDown += new System.EventHandler(this.destinationTable_DropDown);
            // 
            // destinationServer
            // 
            this.destinationServer.FormattingEnabled = true;
            this.destinationServer.Location = new System.Drawing.Point(184, 198);
            this.destinationServer.Name = "destinationServer";
            this.destinationServer.Size = new System.Drawing.Size(237, 21);
            this.destinationServer.TabIndex = 35;
            this.destinationServer.DropDown += new System.EventHandler(this.destinationServer_DropDown);
            // 
            // destinationDatabase
            // 
            this.destinationDatabase.Location = new System.Drawing.Point(184, 225);
            this.destinationDatabase.Name = "destinationDatabase";
            this.destinationDatabase.Size = new System.Drawing.Size(237, 20);
            this.destinationDatabase.TabIndex = 38;
            this.destinationDatabase.Text = "Reporting";
            // 
            // truncateDestination
            // 
            this.truncateDestination.AutoSize = true;
            this.truncateDestination.Location = new System.Drawing.Point(111, 257);
            this.truncateDestination.Name = "truncateDestination";
            this.truncateDestination.Size = new System.Drawing.Size(69, 17);
            this.truncateDestination.TabIndex = 39;
            this.truncateDestination.Text = "Truncate";
            this.truncateDestination.UseVisualStyleBackColor = true;
            // 
            // originSQL
            // 
            this.originSQL.Enabled = false;
            this.originSQL.Location = new System.Drawing.Point(184, 116);
            this.originSQL.Multiline = true;
            this.originSQL.Name = "originSQL";
            this.originSQL.Size = new System.Drawing.Size(237, 20);
            this.originSQL.TabIndex = 41;
            this.originSQL.Enter += new System.EventHandler(this.originSQL_Enter);
            this.originSQL.Leave += new System.EventHandler(this.originSQL_Leave);
            // 
            // enableSQL
            // 
            this.enableSQL.AutoSize = true;
            this.enableSQL.Location = new System.Drawing.Point(12, 118);
            this.enableSQL.Name = "enableSQL";
            this.enableSQL.Size = new System.Drawing.Size(83, 17);
            this.enableSQL.TabIndex = 42;
            this.enableSQL.Text = "Enable SQL";
            this.enableSQL.UseVisualStyleBackColor = true;
            this.enableSQL.CheckedChanged += new System.EventHandler(this.originUpdate_CheckedChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // originDeltas
            // 
            this.originDeltas.AutoSize = true;
            this.originDeltas.Location = new System.Drawing.Point(12, 90);
            this.originDeltas.Name = "originDeltas";
            this.originDeltas.Size = new System.Drawing.Size(83, 17);
            this.originDeltas.TabIndex = 43;
            this.originDeltas.Text = "Load Deltas";
            this.originDeltas.UseVisualStyleBackColor = true;
            this.originDeltas.CheckedChanged += new System.EventHandler(this.originDeltas_CheckedChanged);
            // 
            // originDeltaID
            // 
            this.originDeltaID.Enabled = false;
            this.originDeltaID.Location = new System.Drawing.Point(184, 90);
            this.originDeltaID.Multiline = true;
            this.originDeltaID.Name = "originDeltaID";
            this.originDeltaID.Size = new System.Drawing.Size(237, 20);
            this.originDeltaID.TabIndex = 44;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 470);
            this.Controls.Add(this.originDeltaID);
            this.Controls.Add(this.originDeltas);
            this.Controls.Add(this.enableSQL);
            this.Controls.Add(this.originSQL);
            this.Controls.Add(this.truncateDestination);
            this.Controls.Add(this.destinationDatabase);
            this.Controls.Add(this.destinationTable);
            this.Controls.Add(this.destinationServer);
            this.Controls.Add(this.originTable);
            this.Controls.Add(this.originDatabase);
            this.Controls.Add(this.originServer);
            this.Controls.Add(this.loadedCount);
            this.Controls.Add(this.maxRecordCounter);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.folderName);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.destinationPassword);
            this.Controls.Add(this.destinationUsername);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.originPassword);
            this.Controls.Add(this.originUsername);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startButton);
            this.Name = "Form1";
            this.Text = "SQL Stream";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox originUsername;
        private System.Windows.Forms.TextBox originPassword;
        private System.Windows.Forms.TextBox destinationPassword;
        private System.Windows.Forms.TextBox destinationUsername;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox folderName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox maxRecordCounter;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label loadedCount;
        private System.Windows.Forms.ComboBox originServer;
        private System.Windows.Forms.ComboBox originDatabase;
        private System.Windows.Forms.ComboBox originTable;
        private System.Windows.Forms.ComboBox destinationTable;
        private System.Windows.Forms.ComboBox destinationServer;
        private System.Windows.Forms.TextBox destinationDatabase;
        private System.Windows.Forms.CheckBox truncateDestination;
        private System.Windows.Forms.TextBox originSQL;
        private System.Windows.Forms.CheckBox enableSQL;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.CheckBox originDeltas;
        private System.Windows.Forms.TextBox originDeltaID;
    }
}

