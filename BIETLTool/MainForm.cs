using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BIETLUtility;
using BIETLUtility.Configuration;
using BIETLUtility.Log;
using BIETLUtility.Mail;

namespace BIETLTool
{
    public partial class MainForm : Form
    {
        private Transformation _currentTransform;
        private List<LogMessage> _LogMessageList = new List<LogMessage>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.groupBoxTransform.Visible = false;
            this.listBoxTransforms.DataSource = Transformations.Instance().GetTransformList();

            if (this.listBoxTransforms.Items.Count > 0)
                this.listBoxTransforms.SelectedIndex = 0;
        }

        private void listBoxTransforms_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxTransforms.SelectedIndex > -1)
                {

                    this._currentTransform = Transformations.Instance().GetTransformation(this.listBoxTransforms.SelectedItem.ToString());
                    Transformations.Instance().CurrentTranformation = this._currentTransform;

                    this.groupBoxTransform.Text = this._currentTransform.Name;
                    this.txtInputStep.Text = this._currentTransform.InputStep == null ? string.Empty : this._currentTransform.InputStep.Name;
                    this.txtOutputStep.Text = this._currentTransform.OutputStep == null ? string.Empty : this._currentTransform.OutputStep.Name;
                    this.txtWorkingFolder.Text = this._currentTransform.WorkingFolder;
                    this.checkBoxLoadDeltas.Checked = this._currentTransform.OriginDeltasChecked;
                    this.txtMaxRecordCounter.Text = this._currentTransform.MaxRecordCounter.ToString();
                    this.txtDeltaId.Text = this._currentTransform.DeltaId;
                    this.txtPrimaryKey.Text = this._currentTransform.PrimaryKey.NameString;

                    this.txtTotal.Enabled = false;
                    this.checkBoxSetTotal.Checked = false;

                    if (this.checkBoxLoadDeltas.Checked)
                    {
                        //this.txtDeltaId.Enabled = true;
                        this.panelDelta.Visible = true;
                    }
                    else
                    {
                        //this.txtDeltaId.Enabled = false;
                        this.panelDelta.Visible = false;
                    }

                    this.checkBoxTruncateDestTable.Checked = this._currentTransform.IsTruncateDestination;

                    this.groupBoxTransform.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                btnExecute.Enabled = false;
                if (this.CurrentTransformationSave())
                {
                    SqlStreamClient client = new SqlStreamClient(this._currentTransform, EtlLog.NewLogger(this._currentTransform.Name));
                    this._currentTransform.IsTotalSetted = this.checkBoxSetTotal.Checked;
                    if (this._currentTransform.IsTotalSetted)
                        this._currentTransform.TotalCount = int.Parse(this.txtTotal.Text);

                    client.MessageLog += new EventHandler<LogMessageArg>(LogEventHandler);

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    await Task.Run(() => client.ExecuteTransformAsync());

                    stopwatch.Stop();
                }
            }
            catch (Exception ex)
            {
                var smtp = new SmtpClientAdaptor(Transformations.Instance().SmtpServer);
                string error;
                smtp.ExceptionNotify(string.Format("Execute transformation {0} failed",this._currentTransform.Name), ex.Message + "\n" + ex.StackTrace, out error);

                MessageBox.Show(ex.Message);
                btnExecute.Enabled = true;
            }
        }

        private void LogEventHandler(object sender, LogMessageArg e)
        {
            this._LogMessageList.Add(e.LogMessage);

            this.dataGridView2.Invoke((MethodInvoker)(() =>
                 {
                     this.dataGridView2.DataSource = null;
                    this.dataGridView2.DataSource = this._LogMessageList;

                    if (this.dataGridView2.Columns.Count == 4)
                    {
                        this.dataGridView2.Columns[2].Width = 400;
                        this.dataGridView2.Columns[3].Visible = false;
                    }

                    this.dataGridView2.CurrentCell = this.dataGridView2.Rows[this.dataGridView2.Rows.Count - 1].Cells[0];
                    var percent = (double)e.LogMessage.Value;

                    if (percent > 0)
                    {
                        this.lblMessage.Text = string.Format("Proccess {0:P2} completed!", percent);
                        if (percent >= 1 || e.LogMessage.Message.Contains("Complete"))
                            btnExecute.Enabled = true;
                    }
                 }));
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TransformNewForm newForm = new TransformNewForm();

                if (newForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Transformation trans = new Transformation(newForm.Name, newForm.Location);

                    Transformations.Instance().AddTransformation(trans);

                    this.listBoxTransforms.DataSource = Transformations.Instance().GetTransformList();
                    this.listBoxTransforms.SelectedItem = trans.Name;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "XML Files (*.xml)|*.xml";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string file = dialog.FileName;

                    Transformation trans = new Transformation(file);

                    Transformations.Instance().AddTransformation(trans);

                    this.listBoxTransforms.DataSource = Transformations.Instance().GetTransformList();
                    this.listBoxTransforms.SelectedItem = trans.Name;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNewInputStep_Click(object sender, EventArgs e)
        {
            InputSetting inputForm = new InputSetting();

            if (inputForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this._currentTransform.InputStep = inputForm.InputStep;
                this.txtInputStep.Text = this._currentTransform.InputStep.Name;
            }
        }

        private void btnNewOutputStep_Click(object sender, EventArgs e)
        {
            OutputSetting inputForm = new OutputSetting();

            if (inputForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this._currentTransform.OutputStep = inputForm.OutputStep;
                this.txtOutputStep.Text = this._currentTransform.OutputStep.Name;
            }
        }

        private void btnSaveTransform_Click(object sender, EventArgs e)
        {
            if (this.CurrentTransformationSave())
            {
                MessageBox.Show(string.Format("Transformation {0} saved!", this._currentTransform.Name));
            }
        }

        private void checkBoxLoadDeltas_CheckedChanged(object sender, EventArgs e)
        {
            //this.txtDeltaId.Enabled = this.checkBoxLoadDeltas.Checked;
            this.panelDelta.Visible = this.checkBoxLoadDeltas.Checked;
        }

        private void btnEditInputStep_Click(object sender, EventArgs e)
        {
            InputSetting inputForm = new InputSetting();
            inputForm.InputStep = this._currentTransform.InputStep;

            if (inputForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //this._currentTransform.InputStep = inputForm.InputStep;
                this.txtInputStep.Text = this._currentTransform.InputStep.Name;
            }
        }

        private void btnEditOutput_Click(object sender, EventArgs e)
        {
            OutputSetting inputForm = new OutputSetting();
            inputForm.OutputStep = this._currentTransform.OutputStep;

            if (inputForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //this._currentTransform.OutputStep = inputForm.OutputStep;
                this.txtOutputStep.Text = this._currentTransform.OutputStep.Name;
            }
        }

        private bool CurrentTransformationSave()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtWorkingFolder.Text) || string.IsNullOrWhiteSpace(this.txtMaxRecordCounter.Text))
                {
                    MessageBox.Show("Pls Set a working forlder or MaxRecordCounter!");
                    return false;
                }

                if (this.checkBoxLoadDeltas.Checked && string.IsNullOrWhiteSpace(this.txtDeltaId.Text))
                {
                    MessageBox.Show("DeltaId needed!");
                    return false;
                }

                int MaxRCounter;
                if (!int.TryParse(this.txtMaxRecordCounter.Text, out MaxRCounter))
                {
                    MessageBox.Show("Pls input a number for MaxRecordCounter!");
                    return false;
                }

                if (this.txtWorkingFolder.Text.Trim().EndsWith("\\"))
                    this._currentTransform.WorkingFolder = this.txtWorkingFolder.Text.Trim();
                else
                    this._currentTransform.WorkingFolder = this.txtWorkingFolder.Text.Trim() + "\\";

                this._currentTransform.OriginDeltasChecked = this.checkBoxLoadDeltas.Checked;
                this._currentTransform.DeltaId = this.txtDeltaId.Text;
                this._currentTransform.PrimaryKey.NameList = string.IsNullOrWhiteSpace(this.txtPrimaryKey.Text) ? new List<string>() : this.txtPrimaryKey.Text.Trim().Split(",".ToArray()).ToList();
                this._currentTransform.MaxRecordCounter = MaxRCounter;

                this._currentTransform.Save();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void checkBoxTruncateDestTable_CheckedChanged(object sender, EventArgs e)
        {
            this._currentTransform.IsTruncateDestination = this.checkBoxTruncateDestTable.Checked;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Transformations.Instance().Save();
        }

        private void checkBoxSetTotal_CheckedChanged(object sender, EventArgs e)
        {
            this.txtTotal.Enabled = this.checkBoxSetTotal.Checked;
        }

        private void smtpSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SmtpSettings smtpSettings = new SmtpSettings();
            smtpSettings.SmtpServer = Transformations.Instance().SmtpServer;

            if (smtpSettings.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Transformations.Instance().Save();
            }
        }

      
    }
}
