namespace BIETLTool
{
    partial class MainForm
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
            this.listBoxTransforms = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxTransform = new System.Windows.Forms.GroupBox();
            this.panelDelta = new System.Windows.Forms.Panel();
            this.txtPrimaryKey = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDeltaId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.checkBoxSetTotal = new System.Windows.Forms.CheckBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.checkBoxTruncateDestTable = new System.Windows.Forms.CheckBox();
            this.txtMaxRecordCounter = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtWorkingFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSaveTransform = new System.Windows.Forms.Button();
            this.checkBoxLoadDeltas = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnExecute = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.btnEditOutput = new System.Windows.Forms.Button();
            this.btnNewOutputStep = new System.Windows.Forms.Button();
            this.txtOutputStep = new System.Windows.Forms.TextBox();
            this.btnEditInputStep = new System.Windows.Forms.Button();
            this.btnNewInputStep = new System.Windows.Forms.Button();
            this.txtInputStep = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smtpSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxTransform.SuspendLayout();
            this.panelDelta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxTransforms
            // 
            this.listBoxTransforms.FormattingEnabled = true;
            this.listBoxTransforms.Location = new System.Drawing.Point(12, 59);
            this.listBoxTransforms.Name = "listBoxTransforms";
            this.listBoxTransforms.Size = new System.Drawing.Size(148, 511);
            this.listBoxTransforms.TabIndex = 0;
            this.listBoxTransforms.SelectedIndexChanged += new System.EventHandler(this.listBoxTransforms_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Transforms: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "SQL Server Setting:";
            // 
            // groupBoxTransform
            // 
            this.groupBoxTransform.Controls.Add(this.panelDelta);
            this.groupBoxTransform.Controls.Add(this.txtTotal);
            this.groupBoxTransform.Controls.Add(this.checkBoxSetTotal);
            this.groupBoxTransform.Controls.Add(this.lblMessage);
            this.groupBoxTransform.Controls.Add(this.checkBoxTruncateDestTable);
            this.groupBoxTransform.Controls.Add(this.txtMaxRecordCounter);
            this.groupBoxTransform.Controls.Add(this.label5);
            this.groupBoxTransform.Controls.Add(this.txtWorkingFolder);
            this.groupBoxTransform.Controls.Add(this.label3);
            this.groupBoxTransform.Controls.Add(this.btnSaveTransform);
            this.groupBoxTransform.Controls.Add(this.checkBoxLoadDeltas);
            this.groupBoxTransform.Controls.Add(this.label4);
            this.groupBoxTransform.Controls.Add(this.btnExecute);
            this.groupBoxTransform.Controls.Add(this.dataGridView2);
            this.groupBoxTransform.Controls.Add(this.btnEditOutput);
            this.groupBoxTransform.Controls.Add(this.btnNewOutputStep);
            this.groupBoxTransform.Controls.Add(this.txtOutputStep);
            this.groupBoxTransform.Controls.Add(this.btnEditInputStep);
            this.groupBoxTransform.Controls.Add(this.btnNewInputStep);
            this.groupBoxTransform.Controls.Add(this.txtInputStep);
            this.groupBoxTransform.Controls.Add(this.label2);
            this.groupBoxTransform.Location = new System.Drawing.Point(179, 35);
            this.groupBoxTransform.Name = "groupBoxTransform";
            this.groupBoxTransform.Size = new System.Drawing.Size(726, 535);
            this.groupBoxTransform.TabIndex = 13;
            this.groupBoxTransform.TabStop = false;
            this.groupBoxTransform.Text = "Transform";
            // 
            // panelDelta
            // 
            this.panelDelta.Controls.Add(this.txtPrimaryKey);
            this.panelDelta.Controls.Add(this.label7);
            this.panelDelta.Controls.Add(this.txtDeltaId);
            this.panelDelta.Controls.Add(this.label6);
            this.panelDelta.Location = new System.Drawing.Point(118, 213);
            this.panelDelta.Name = "panelDelta";
            this.panelDelta.Size = new System.Drawing.Size(598, 31);
            this.panelDelta.TabIndex = 36;
            // 
            // txtPrimaryKey
            // 
            this.txtPrimaryKey.Location = new System.Drawing.Point(286, 7);
            this.txtPrimaryKey.Name = "txtPrimaryKey";
            this.txtPrimaryKey.Size = new System.Drawing.Size(309, 20);
            this.txtPrimaryKey.TabIndex = 37;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(215, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 36;
            this.label7.Text = "Primary Key:";
            // 
            // txtDeltaId
            // 
            this.txtDeltaId.Location = new System.Drawing.Point(84, 6);
            this.txtDeltaId.Name = "txtDeltaId";
            this.txtDeltaId.Size = new System.Drawing.Size(108, 20);
            this.txtDeltaId.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "Delta Column:";
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(544, 135);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(172, 20);
            this.txtTotal.TabIndex = 34;
            // 
            // checkBoxSetTotal
            // 
            this.checkBoxSetTotal.AutoSize = true;
            this.checkBoxSetTotal.Location = new System.Drawing.Point(469, 138);
            this.checkBoxSetTotal.Name = "checkBoxSetTotal";
            this.checkBoxSetTotal.Size = new System.Drawing.Size(69, 17);
            this.checkBoxSetTotal.TabIndex = 33;
            this.checkBoxSetTotal.Text = "Set Total";
            this.checkBoxSetTotal.UseVisualStyleBackColor = true;
            this.checkBoxSetTotal.CheckedChanged += new System.EventHandler(this.checkBoxSetTotal_CheckedChanged);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(398, 257);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 13);
            this.lblMessage.TabIndex = 32;
            // 
            // checkBoxTruncateDestTable
            // 
            this.checkBoxTruncateDestTable.AutoSize = true;
            this.checkBoxTruncateDestTable.Location = new System.Drawing.Point(16, 254);
            this.checkBoxTruncateDestTable.Name = "checkBoxTruncateDestTable";
            this.checkBoxTruncateDestTable.Size = new System.Drawing.Size(198, 17);
            this.checkBoxTruncateDestTable.TabIndex = 31;
            this.checkBoxTruncateDestTable.Text = "Truncate Destination table in Vertica";
            this.checkBoxTruncateDestTable.UseVisualStyleBackColor = true;
            this.checkBoxTruncateDestTable.CheckedChanged += new System.EventHandler(this.checkBoxTruncateDestTable_CheckedChanged);
            // 
            // txtMaxRecordCounter
            // 
            this.txtMaxRecordCounter.Location = new System.Drawing.Point(148, 173);
            this.txtMaxRecordCounter.Name = "txtMaxRecordCounter";
            this.txtMaxRecordCounter.Size = new System.Drawing.Size(286, 20);
            this.txtMaxRecordCounter.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Max Records Per File:";
            // 
            // txtWorkingFolder
            // 
            this.txtWorkingFolder.Location = new System.Drawing.Point(148, 135);
            this.txtWorkingFolder.Name = "txtWorkingFolder";
            this.txtWorkingFolder.Size = new System.Drawing.Size(286, 20);
            this.txtWorkingFolder.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Working Folder: ";
            // 
            // btnSaveTransform
            // 
            this.btnSaveTransform.Location = new System.Drawing.Point(645, 506);
            this.btnSaveTransform.Name = "btnSaveTransform";
            this.btnSaveTransform.Size = new System.Drawing.Size(75, 23);
            this.btnSaveTransform.TabIndex = 25;
            this.btnSaveTransform.Text = "Save";
            this.btnSaveTransform.UseVisualStyleBackColor = true;
            this.btnSaveTransform.Click += new System.EventHandler(this.btnSaveTransform_Click);
            // 
            // checkBoxLoadDeltas
            // 
            this.checkBoxLoadDeltas.AutoSize = true;
            this.checkBoxLoadDeltas.Location = new System.Drawing.Point(19, 222);
            this.checkBoxLoadDeltas.Name = "checkBoxLoadDeltas";
            this.checkBoxLoadDeltas.Size = new System.Drawing.Size(83, 17);
            this.checkBoxLoadDeltas.TabIndex = 23;
            this.checkBoxLoadDeltas.Text = "Load Deltas";
            this.checkBoxLoadDeltas.UseVisualStyleBackColor = true;
            this.checkBoxLoadDeltas.CheckedChanged += new System.EventHandler(this.checkBoxLoadDeltas_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Vertica Setting:";
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(645, 258);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 21;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(14, 287);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(706, 213);
            this.dataGridView2.TabIndex = 20;
            // 
            // btnEditOutput
            // 
            this.btnEditOutput.Location = new System.Drawing.Point(359, 92);
            this.btnEditOutput.Name = "btnEditOutput";
            this.btnEditOutput.Size = new System.Drawing.Size(75, 23);
            this.btnEditOutput.TabIndex = 19;
            this.btnEditOutput.Text = "Edit...";
            this.btnEditOutput.UseVisualStyleBackColor = true;
            this.btnEditOutput.Click += new System.EventHandler(this.btnEditOutput_Click);
            // 
            // btnNewOutputStep
            // 
            this.btnNewOutputStep.Location = new System.Drawing.Point(265, 92);
            this.btnNewOutputStep.Name = "btnNewOutputStep";
            this.btnNewOutputStep.Size = new System.Drawing.Size(75, 23);
            this.btnNewOutputStep.TabIndex = 18;
            this.btnNewOutputStep.Text = "New...";
            this.btnNewOutputStep.UseVisualStyleBackColor = true;
            this.btnNewOutputStep.Click += new System.EventHandler(this.btnNewOutputStep_Click);
            // 
            // txtOutputStep
            // 
            this.txtOutputStep.Location = new System.Drawing.Point(14, 94);
            this.txtOutputStep.Name = "txtOutputStep";
            this.txtOutputStep.ReadOnly = true;
            this.txtOutputStep.Size = new System.Drawing.Size(234, 20);
            this.txtOutputStep.TabIndex = 17;
            // 
            // btnEditInputStep
            // 
            this.btnEditInputStep.Location = new System.Drawing.Point(359, 41);
            this.btnEditInputStep.Name = "btnEditInputStep";
            this.btnEditInputStep.Size = new System.Drawing.Size(75, 23);
            this.btnEditInputStep.TabIndex = 16;
            this.btnEditInputStep.Text = "Edit...";
            this.btnEditInputStep.UseVisualStyleBackColor = true;
            this.btnEditInputStep.Click += new System.EventHandler(this.btnEditInputStep_Click);
            // 
            // btnNewInputStep
            // 
            this.btnNewInputStep.Location = new System.Drawing.Point(265, 41);
            this.btnNewInputStep.Name = "btnNewInputStep";
            this.btnNewInputStep.Size = new System.Drawing.Size(75, 23);
            this.btnNewInputStep.TabIndex = 15;
            this.btnNewInputStep.Text = "New...";
            this.btnNewInputStep.UseVisualStyleBackColor = true;
            this.btnNewInputStep.Click += new System.EventHandler(this.btnNewInputStep_Click);
            // 
            // txtInputStep
            // 
            this.txtInputStep.Location = new System.Drawing.Point(14, 43);
            this.txtInputStep.Name = "txtInputStep";
            this.txtInputStep.ReadOnly = true;
            this.txtInputStep.Size = new System.Drawing.Size(234, 20);
            this.txtInputStep.TabIndex = 14;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(924, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.smtpSettingToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // smtpSettingToolStripMenuItem
            // 
            this.smtpSettingToolStripMenuItem.Name = "smtpSettingToolStripMenuItem";
            this.smtpSettingToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.smtpSettingToolStripMenuItem.Text = "Smtp Setting";
            this.smtpSettingToolStripMenuItem.Click += new System.EventHandler(this.smtpSettingToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(142, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 589);
            this.Controls.Add(this.groupBoxTransform);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxTransforms);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBoxTransform.ResumeLayout(false);
            this.groupBoxTransform.PerformLayout();
            this.panelDelta.ResumeLayout(false);
            this.panelDelta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxTransforms;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBoxTransform;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button btnEditOutput;
        private System.Windows.Forms.Button btnNewOutputStep;
        private System.Windows.Forms.TextBox txtOutputStep;
        private System.Windows.Forms.Button btnEditInputStep;
        private System.Windows.Forms.Button btnNewInputStep;
        private System.Windows.Forms.TextBox txtInputStep;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smtpSettingToolStripMenuItem;
        private System.Windows.Forms.TextBox txtDeltaId;
        private System.Windows.Forms.CheckBox checkBoxLoadDeltas;
        private System.Windows.Forms.Button btnSaveTransform;
        private System.Windows.Forms.TextBox txtWorkingFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMaxRecordCounter;
        private System.Windows.Forms.CheckBox checkBoxTruncateDestTable;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.CheckBox checkBoxSetTotal;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.Panel panelDelta;
        private System.Windows.Forms.TextBox txtPrimaryKey;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}