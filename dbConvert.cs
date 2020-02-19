using DbAccess;
using POS_Client;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class dbConvert : Form
{
	private bool _shouldExit;

	private IContainer components;

	private ProgressBar pbrProgress;

	private Label lblMessage;

	public dbConvert()
	{
		InitializeComponent();
	}

	private void dbConvert_Load(object sender, EventArgs e)
	{
		string text = "";
		string appSettings = ConfigOperation.GetAppSettings("OLD_POS_DATABASE_NAME");
		text = ((!bool.Parse(ConfigOperation.GetAppSettings("OLD_POS_DATABASE_SSPI"))) ? $"Data Source=(local)\\SQLExpress;Initial Catalog={appSettings};User ID=sa;Password=1031" : $"Data Source=(local)\\SQLExpress;Initial Catalog={appSettings};Integrated Security=SSPI;");
		string sqlitePath = Program.DataPath + "\\Old_db.db3";
		Cursor = Cursors.WaitCursor;
		SqlConversionHandler handler = delegate(bool done, bool success, int percent, string msg)
		{
			dbConvert dbConvert = this;
			Invoke((MethodInvoker)delegate
			{
				dbConvert.lblMessage.Text = msg;
				dbConvert.pbrProgress.Value = percent;
				if (done)
				{
					dbConvert.Cursor = Cursors.Default;
					if (success)
					{
						MessageBox.Show(dbConvert, msg, "資料移轉成功", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						dbConvert.Close();
					}
					else
					{
						MessageBox.Show(dbConvert, msg, "資料移轉失敗", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						dbConvert.pbrProgress.Value = 0;
						dbConvert.lblMessage.Text = string.Empty;
						Application.Exit();
					}
				}
			});
		};
		SqlTableSelectionHandler selectionHandler = null;
		FailedViewDefinitionHandler viewFailureHandler = delegate(ViewSchema vs)
		{
			dbConvert owner = this;
			string updated = null;
			Invoke((MethodInvoker)delegate
			{
				ViewFailureDialog viewFailureDialog = new ViewFailureDialog
				{
					View = vs
				};
				if (viewFailureDialog.ShowDialog(owner) == DialogResult.OK)
				{
					updated = viewFailureDialog.ViewSQL;
				}
				else
				{
					updated = null;
				}
			});
			return updated;
		};
		string password = "1031";
		bool createViews = false;
		bool createTriggers = false;
		SqlServerToSQLite.ConvertSqlServerToSQLiteDatabase(text, sqlitePath, password, handler, selectionHandler, viewFailureHandler, createTriggers, createViews);
	}

	private void dbConvert_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (SqlServerToSQLite.IsActive)
		{
			SqlServerToSQLite.CancelConversion();
			_shouldExit = true;
			e.Cancel = true;
		}
		else
		{
			e.Cancel = false;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		pbrProgress = new System.Windows.Forms.ProgressBar();
		lblMessage = new System.Windows.Forms.Label();
		SuspendLayout();
		pbrProgress.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
		pbrProgress.Location = new System.Drawing.Point(14, 75);
		pbrProgress.Name = "pbrProgress";
		pbrProgress.Size = new System.Drawing.Size(506, 30);
		pbrProgress.TabIndex = 16;
		lblMessage.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
		lblMessage.Location = new System.Drawing.Point(12, 18);
		lblMessage.Name = "lblMessage";
		lblMessage.Size = new System.Drawing.Size(508, 43);
		lblMessage.TabIndex = 15;
		lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(540, 128);
		base.Controls.Add(lblMessage);
		base.Controls.Add(pbrProgress);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.Name = "MainForm";
		Text = "防檢局資料移轉";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(dbConvert_FormClosing);
		base.Load += new System.EventHandler(dbConvert_Load);
		ResumeLayout(false);
	}
}
