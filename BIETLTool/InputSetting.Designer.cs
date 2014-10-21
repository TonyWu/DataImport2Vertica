namespace BIETLTool
{
    partial class InputSetting
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.comboBoxConnection = new System.Windows.Forms.ComboBox();
            this.btnEditConnection = new System.Windows.Forms.Button();
            this.btnNewConnection = new System.Windows.Forms.Button();
            this.txtSql = new System.Windows.Forms.RichTextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.checkBoxEnableSQL = new System.Windows.Forms.CheckBox();
            this.btnNewVertica = new System.Windows.Forms.Button();
            this.txtTargetTable = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Step Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Connection";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(109, 13);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(375, 20);
            this.txtName.TabIndex = 2;
            // 
            // comboBoxConnection
            // 
            this.comboBoxConnection.FormattingEnabled = true;
            this.comboBoxConnection.Location = new System.Drawing.Point(109, 46);
            this.comboBoxConnection.Name = "comboBoxConnection";
            this.comboBoxConnection.Size = new System.Drawing.Size(206, 21);
            this.comboBoxConnection.TabIndex = 3;
            // 
            // btnEditConnection
            // 
            this.btnEditConnection.Location = new System.Drawing.Point(418, 46);
            this.btnEditConnection.Name = "btnEditConnection";
            this.btnEditConnection.Size = new System.Drawing.Size(66, 23);
            this.btnEditConnection.TabIndex = 4;
            this.btnEditConnection.Text = "Edit...";
            this.btnEditConnection.UseVisualStyleBackColor = true;
            this.btnEditConnection.Click += new System.EventHandler(this.btnEditConnection_Click);
            // 
            // btnNewConnection
            // 
            this.btnNewConnection.Location = new System.Drawing.Point(334, 46);
            this.btnNewConnection.Name = "btnNewConnection";
            this.btnNewConnection.Size = new System.Drawing.Size(64, 23);
            this.btnNewConnection.TabIndex = 5;
            this.btnNewConnection.Text = "New...";
            this.btnNewConnection.UseVisualStyleBackColor = true;
            this.btnNewConnection.Click += new System.EventHandler(this.btnNewConnection_Click);
            // 
            // txtSql
            // 
            this.txtSql.Location = new System.Drawing.Point(13, 158);
            this.txtSql.Name = "txtSql";
            this.txtSql.Size = new System.Drawing.Size(575, 303);
            this.txtSql.TabIndex = 8;
            this.txtSql.Text = "";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(224, 469);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(344, 469);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 10;
            this.button5.Text = "Preview";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(458, 469);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // checkBoxEnableSQL
            // 
            this.checkBoxEnableSQL.AutoSize = true;
            this.checkBoxEnableSQL.Location = new System.Drawing.Point(13, 124);
            this.checkBoxEnableSQL.Name = "checkBoxEnableSQL";
            this.checkBoxEnableSQL.Size = new System.Drawing.Size(83, 17);
            this.checkBoxEnableSQL.TabIndex = 12;
            this.checkBoxEnableSQL.Text = "Enable SQL";
            this.checkBoxEnableSQL.UseVisualStyleBackColor = true;
            this.checkBoxEnableSQL.CheckedChanged += new System.EventHandler(this.checkBoxEnableSQL_CheckedChanged);
            // 
            // btnNewVertica
            // 
            this.btnNewVertica.Location = new System.Drawing.Point(507, 49);
            this.btnNewVertica.Name = "btnNewVertica";
            this.btnNewVertica.Size = new System.Drawing.Size(81, 23);
            this.btnNewVertica.TabIndex = 13;
            this.btnNewVertica.Text = "New Vertica..";
            this.btnNewVertica.UseVisualStyleBackColor = true;
            this.btnNewVertica.Visible = false;
            this.btnNewVertica.Click += new System.EventHandler(this.btnNewVertica_Click);
            // 
            // txtTargetTable
            // 
            this.txtTargetTable.Location = new System.Drawing.Point(109, 93);
            this.txtTargetTable.Name = "txtTargetTable";
            this.txtTargetTable.Size = new System.Drawing.Size(375, 20);
            this.txtTargetTable.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Target Table";
            // 
            // InputSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 504);
            this.Controls.Add(this.txtTargetTable);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnNewVertica);
            this.Controls.Add(this.checkBoxEnableSQL);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtSql);
            this.Controls.Add(this.btnNewConnection);
            this.Controls.Add(this.btnEditConnection);
            this.Controls.Add(this.comboBoxConnection);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "InputSetting";
            this.Text = "SQL Server Setting";
            this.Load += new System.EventHandler(this.InputSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox comboBoxConnection;
        private System.Windows.Forms.Button btnEditConnection;
        private System.Windows.Forms.Button btnNewConnection;
        private System.Windows.Forms.RichTextBox txtSql;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox checkBoxEnableSQL;
        private System.Windows.Forms.Button btnNewVertica;
        private System.Windows.Forms.TextBox txtTargetTable;
        private System.Windows.Forms.Label label3;
    }
}