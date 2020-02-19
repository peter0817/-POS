using POS_Client;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogMedDescription : Form
{
	private string _barcode = "";

	private string _pesticideId;

	private string _cropId;

	private string _pestId;

	private string _formCode;

	private string _content;

	private IContainer components;

	private Button btn_cancel;

	public Label l_title;

	private Panel panel22;

	private Label label33;

	private TableLayoutPanel tableLayoutPanel2;

	private FlowLayoutPanel flowLayoutPanel1;

	private Panel panel1;

	private Label label1;

	private Panel panel2;

	private Label label2;

	private Panel panel3;

	private Label label3;

	private Panel panel4;

	private Label label4;

	private Panel panel5;

	private Label label5;

	private Panel panel6;

	private Label label6;

	private Panel panel8;

	private Label label8;

	private Panel panel9;

	private Label label9;

	private Panel panel10;

	private Label label10;

	private Panel panel11;

	private Label label11;

	private FlowLayoutPanel flowLayoutPanel2;

	private FlowLayoutPanel flowLayoutPanel3;

	private Label label_pesticideName;

	private Label label_formCode;

	private Label label_usages;

	private Label label_recovery;

	private Label label_intervals;

	private Label label_dilute;

	private Label label_contents;

	private Label label_direction;

	private Label label_notes;

	private Label label_period;

	private Label label_frequency;

	public dialogMedDescription(string barcode, string cropId, string pestId)
	{
		InitializeComponent();
		_barcode = barcode;
		_cropId = cropId;
		_pestId = pestId;
	}

	public dialogMedDescription(string pesticideId, string cropId, string pestId, string formCode, string content)
	{
		InitializeComponent();
		_pesticideId = pesticideId;
		_cropId = cropId;
		_pestId = pestId;
		_formCode = formCode;
		_content = content;
	}

	private void dialogMedDescription_Load(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(_barcode))
		{
			string[] strWhereParameterArray = new string[1]
			{
				_barcode
			};
			DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "pesticideId,formCode,contents", "hypos_GOODSLST", "GDSNO={0}", "", null, strWhereParameterArray, CommandOperationType.ExecuteReaderReturnDataTable);
			string strSelectField = "pesticideName,formCode,usages,period,frequency,contents,dilute,intervals,recovery,direction,notes";
			string strWhereClause = "pesticideId={0} and cropId={1} and pestId={2} and formCode={3} and contents={4} ";
			string[] strWhereParameterArray2 = new string[5]
			{
				dataTable.Rows[0]["pesticideId"].ToString(),
				_cropId,
				_pestId,
				dataTable.Rows[0]["formCode"].ToString(),
				dataTable.Rows[0]["contents"].ToString()
			};
			DataTable dataTable2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, strSelectField, "HyScope", strWhereClause, "", null, strWhereParameterArray2, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable2.Rows.Count > 0)
			{
				label_pesticideName.Text = dataTable2.Rows[0]["pesticideName"].ToString();
				label_formCode.Text = dataTable2.Rows[0]["formCode"].ToString();
				label_usages.Text = dataTable2.Rows[0]["usages"].ToString();
				label_period.Text = dataTable2.Rows[0]["period"].ToString();
				label_frequency.Text = dataTable2.Rows[0]["frequency"].ToString();
				label_contents.Text = dataTable2.Rows[0]["contents"].ToString();
				label_dilute.Text = dataTable2.Rows[0]["dilute"].ToString();
				label_intervals.Text = dataTable2.Rows[0]["intervals"].ToString();
				label_recovery.Text = dataTable2.Rows[0]["recovery"].ToString();
				label_direction.Text = dataTable2.Rows[0]["direction"].ToString();
				label_notes.Text = dataTable2.Rows[0]["notes"].ToString();
			}
		}
		else
		{
			string strSelectField2 = "pesticideName,formCode,usages,period,frequency,contents,dilute,intervals,recovery,direction,notes";
			string strWhereClause2 = "pesticideId={0} and cropId={1} and pestId={2} and formCode={3} and contents={4} ";
			string[] strWhereParameterArray3 = new string[5]
			{
				_pesticideId,
				_cropId,
				_pestId,
				_formCode,
				_content
			};
			DataTable dataTable3 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, strSelectField2, "HyScope", strWhereClause2, "", null, strWhereParameterArray3, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable3.Rows.Count > 0)
			{
				label_pesticideName.Text = dataTable3.Rows[0]["pesticideName"].ToString();
				label_formCode.Text = dataTable3.Rows[0]["formCode"].ToString();
				label_usages.Text = dataTable3.Rows[0]["usages"].ToString();
				label_period.Text = dataTable3.Rows[0]["period"].ToString();
				label_frequency.Text = dataTable3.Rows[0]["frequency"].ToString();
				label_contents.Text = dataTable3.Rows[0]["contents"].ToString();
				label_dilute.Text = dataTable3.Rows[0]["dilute"].ToString();
				label_intervals.Text = dataTable3.Rows[0]["intervals"].ToString();
				label_recovery.Text = dataTable3.Rows[0]["recovery"].ToString();
				label_direction.Text = dataTable3.Rows[0]["direction"].ToString();
				label_notes.Text = dataTable3.Rows[0]["notes"].ToString();
			}
		}
	}

	private void btn_cancel_Click(object sender, EventArgs e)
	{
		Close();
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
		btn_cancel = new System.Windows.Forms.Button();
		l_title = new System.Windows.Forms.Label();
		panel22 = new System.Windows.Forms.Panel();
		label33 = new System.Windows.Forms.Label();
		tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		label_recovery = new System.Windows.Forms.Label();
		label_intervals = new System.Windows.Forms.Label();
		label_dilute = new System.Windows.Forms.Label();
		label_contents = new System.Windows.Forms.Label();
		label_formCode = new System.Windows.Forms.Label();
		flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		label_pesticideName = new System.Windows.Forms.Label();
		panel1 = new System.Windows.Forms.Panel();
		label1 = new System.Windows.Forms.Label();
		panel2 = new System.Windows.Forms.Panel();
		label2 = new System.Windows.Forms.Label();
		panel3 = new System.Windows.Forms.Panel();
		label3 = new System.Windows.Forms.Label();
		panel4 = new System.Windows.Forms.Panel();
		label4 = new System.Windows.Forms.Label();
		panel5 = new System.Windows.Forms.Panel();
		label5 = new System.Windows.Forms.Label();
		panel6 = new System.Windows.Forms.Panel();
		label6 = new System.Windows.Forms.Label();
		panel8 = new System.Windows.Forms.Panel();
		label8 = new System.Windows.Forms.Label();
		panel9 = new System.Windows.Forms.Panel();
		label9 = new System.Windows.Forms.Label();
		panel10 = new System.Windows.Forms.Panel();
		label10 = new System.Windows.Forms.Label();
		panel11 = new System.Windows.Forms.Panel();
		label11 = new System.Windows.Forms.Label();
		flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
		label_direction = new System.Windows.Forms.Label();
		flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
		label_notes = new System.Windows.Forms.Label();
		label_usages = new System.Windows.Forms.Label();
		label_period = new System.Windows.Forms.Label();
		label_frequency = new System.Windows.Forms.Label();
		panel22.SuspendLayout();
		tableLayoutPanel2.SuspendLayout();
		flowLayoutPanel1.SuspendLayout();
		panel1.SuspendLayout();
		panel2.SuspendLayout();
		panel3.SuspendLayout();
		panel4.SuspendLayout();
		panel5.SuspendLayout();
		panel6.SuspendLayout();
		panel8.SuspendLayout();
		panel9.SuspendLayout();
		panel10.SuspendLayout();
		panel11.SuspendLayout();
		flowLayoutPanel2.SuspendLayout();
		flowLayoutPanel3.SuspendLayout();
		SuspendLayout();
		btn_cancel.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_cancel.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_cancel.ForeColor = System.Drawing.Color.White;
		btn_cancel.Location = new System.Drawing.Point(397, 510);
		btn_cancel.Name = "btn_cancel";
		btn_cancel.Size = new System.Drawing.Size(124, 34);
		btn_cancel.TabIndex = 46;
		btn_cancel.TabStop = false;
		btn_cancel.Text = "關閉";
		btn_cancel.UseVisualStyleBackColor = false;
		btn_cancel.Click += new System.EventHandler(btn_cancel_Click);
		l_title.AutoSize = true;
		l_title.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_title.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		l_title.Location = new System.Drawing.Point(409, 20);
		l_title.Name = "l_title";
		l_title.Size = new System.Drawing.Size(86, 24);
		l_title.TabIndex = 52;
		l_title.Text = "用藥說明";
		l_title.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		panel22.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel22.Controls.Add(label33);
		panel22.Dock = System.Windows.Forms.DockStyle.Fill;
		panel22.Location = new System.Drawing.Point(1, 1);
		panel22.Margin = new System.Windows.Forms.Padding(0);
		panel22.Name = "panel22";
		panel22.Size = new System.Drawing.Size(162, 58);
		panel22.TabIndex = 19;
		label33.AutoSize = true;
		label33.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label33.ForeColor = System.Drawing.Color.White;
		label33.Location = new System.Drawing.Point(101, 19);
		label33.Name = "label33";
		label33.Size = new System.Drawing.Size(58, 21);
		label33.TabIndex = 0;
		label33.Text = "農藥名";
		tableLayoutPanel2.BackColor = System.Drawing.Color.White;
		tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		tableLayoutPanel2.ColumnCount = 4;
		tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 162f));
		tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 162f));
		tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		tableLayoutPanel2.Controls.Add(label_recovery, 3, 4);
		tableLayoutPanel2.Controls.Add(label_intervals, 3, 3);
		tableLayoutPanel2.Controls.Add(label_dilute, 3, 2);
		tableLayoutPanel2.Controls.Add(label_contents, 3, 1);
		tableLayoutPanel2.Controls.Add(label_formCode, 1, 1);
		tableLayoutPanel2.Controls.Add(panel22, 0, 0);
		tableLayoutPanel2.Controls.Add(flowLayoutPanel1, 1, 0);
		tableLayoutPanel2.Controls.Add(panel1, 0, 1);
		tableLayoutPanel2.Controls.Add(panel2, 0, 2);
		tableLayoutPanel2.Controls.Add(panel3, 0, 3);
		tableLayoutPanel2.Controls.Add(panel4, 0, 4);
		tableLayoutPanel2.Controls.Add(panel5, 0, 5);
		tableLayoutPanel2.Controls.Add(panel6, 0, 6);
		tableLayoutPanel2.Controls.Add(panel8, 2, 1);
		tableLayoutPanel2.Controls.Add(panel9, 2, 2);
		tableLayoutPanel2.Controls.Add(panel10, 2, 3);
		tableLayoutPanel2.Controls.Add(panel11, 2, 4);
		tableLayoutPanel2.Controls.Add(flowLayoutPanel2, 1, 5);
		tableLayoutPanel2.Controls.Add(flowLayoutPanel3, 1, 6);
		tableLayoutPanel2.Controls.Add(label_usages, 1, 2);
		tableLayoutPanel2.Controls.Add(label_period, 1, 3);
		tableLayoutPanel2.Controls.Add(label_frequency, 1, 4);
		tableLayoutPanel2.Location = new System.Drawing.Point(12, 65);
		tableLayoutPanel2.Name = "tableLayoutPanel2";
		tableLayoutPanel2.RowCount = 7;
		tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571f));
		tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571f));
		tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571f));
		tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571f));
		tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571f));
		tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571f));
		tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571f));
		tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel2.Size = new System.Drawing.Size(880, 419);
		tableLayoutPanel2.TabIndex = 53;
		label_recovery.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label_recovery.Location = new System.Drawing.Point(606, 237);
		label_recovery.Name = "label_recovery";
		label_recovery.Size = new System.Drawing.Size(254, 51);
		label_recovery.TabIndex = 62;
		label_recovery.Text = "{8}";
		label_recovery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		label_intervals.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label_intervals.Location = new System.Drawing.Point(606, 178);
		label_intervals.Name = "label_intervals";
		label_intervals.Size = new System.Drawing.Size(254, 51);
		label_intervals.TabIndex = 61;
		label_intervals.Text = "{7}";
		label_intervals.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		label_dilute.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label_dilute.Location = new System.Drawing.Point(606, 119);
		label_dilute.Name = "label_dilute";
		label_dilute.Size = new System.Drawing.Size(254, 51);
		label_dilute.TabIndex = 60;
		label_dilute.Text = "{6}";
		label_dilute.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		label_contents.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label_contents.Location = new System.Drawing.Point(606, 60);
		label_contents.Name = "label_contents";
		label_contents.Size = new System.Drawing.Size(254, 51);
		label_contents.TabIndex = 59;
		label_contents.Text = "{5}";
		label_contents.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		label_formCode.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label_formCode.Location = new System.Drawing.Point(167, 60);
		label_formCode.Name = "label_formCode";
		label_formCode.Size = new System.Drawing.Size(254, 51);
		label_formCode.TabIndex = 55;
		label_formCode.Text = "{1}";
		label_formCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		tableLayoutPanel2.SetColumnSpan(flowLayoutPanel1, 3);
		flowLayoutPanel1.Controls.Add(label_pesticideName);
		flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		flowLayoutPanel1.Location = new System.Drawing.Point(164, 1);
		flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		flowLayoutPanel1.Name = "flowLayoutPanel1";
		flowLayoutPanel1.Size = new System.Drawing.Size(715, 58);
		flowLayoutPanel1.TabIndex = 20;
		label_pesticideName.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label_pesticideName.Location = new System.Drawing.Point(3, 0);
		label_pesticideName.Name = "label_pesticideName";
		label_pesticideName.Size = new System.Drawing.Size(342, 51);
		label_pesticideName.TabIndex = 54;
		label_pesticideName.Text = "{0}";
		label_pesticideName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		panel1.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel1.Controls.Add(label1);
		panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		panel1.Location = new System.Drawing.Point(1, 60);
		panel1.Margin = new System.Windows.Forms.Padding(0);
		panel1.Name = "panel1";
		panel1.Size = new System.Drawing.Size(162, 58);
		panel1.TabIndex = 19;
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.Color.White;
		label1.Location = new System.Drawing.Point(117, 18);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(42, 21);
		label1.TabIndex = 0;
		label1.Text = "劑型";
		panel2.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel2.Controls.Add(label2);
		panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		panel2.Location = new System.Drawing.Point(1, 119);
		panel2.Margin = new System.Windows.Forms.Padding(0);
		panel2.Name = "panel2";
		panel2.Size = new System.Drawing.Size(162, 58);
		panel2.TabIndex = 19;
		label2.AutoSize = true;
		label2.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label2.ForeColor = System.Drawing.Color.White;
		label2.Location = new System.Drawing.Point(69, 18);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(90, 21);
		label2.TabIndex = 0;
		label2.Text = "公頃用藥量";
		panel3.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel3.Controls.Add(label3);
		panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		panel3.Location = new System.Drawing.Point(1, 178);
		panel3.Margin = new System.Windows.Forms.Padding(0);
		panel3.Name = "panel3";
		panel3.Size = new System.Drawing.Size(162, 58);
		panel3.TabIndex = 19;
		label3.AutoSize = true;
		label3.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label3.ForeColor = System.Drawing.Color.White;
		label3.Location = new System.Drawing.Point(85, 18);
		label3.Name = "label3";
		label3.Size = new System.Drawing.Size(74, 21);
		label3.TabIndex = 0;
		label3.Text = "使用時期";
		panel4.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel4.Controls.Add(label4);
		panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		panel4.Location = new System.Drawing.Point(1, 237);
		panel4.Margin = new System.Windows.Forms.Padding(0);
		panel4.Name = "panel4";
		panel4.Size = new System.Drawing.Size(162, 58);
		panel4.TabIndex = 19;
		label4.AutoSize = true;
		label4.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label4.ForeColor = System.Drawing.Color.White;
		label4.Location = new System.Drawing.Point(85, 18);
		label4.Name = "label4";
		label4.Size = new System.Drawing.Size(74, 21);
		label4.TabIndex = 0;
		label4.Text = "施藥次數";
		panel5.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel5.Controls.Add(label5);
		panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		panel5.Location = new System.Drawing.Point(1, 296);
		panel5.Margin = new System.Windows.Forms.Padding(0);
		panel5.Name = "panel5";
		panel5.Size = new System.Drawing.Size(162, 58);
		panel5.TabIndex = 19;
		label5.AutoSize = true;
		label5.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label5.ForeColor = System.Drawing.Color.White;
		label5.Location = new System.Drawing.Point(85, 18);
		label5.Name = "label5";
		label5.Size = new System.Drawing.Size(74, 21);
		label5.TabIndex = 0;
		label5.Text = "施藥方法";
		panel6.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel6.Controls.Add(label6);
		panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		panel6.Location = new System.Drawing.Point(1, 355);
		panel6.Margin = new System.Windows.Forms.Padding(0);
		panel6.Name = "panel6";
		panel6.Size = new System.Drawing.Size(162, 63);
		panel6.TabIndex = 19;
		label6.AutoSize = true;
		label6.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label6.ForeColor = System.Drawing.Color.White;
		label6.Location = new System.Drawing.Point(85, 18);
		label6.Name = "label6";
		label6.Size = new System.Drawing.Size(74, 21);
		label6.TabIndex = 0;
		label6.Text = "注意事項";
		panel8.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel8.Controls.Add(label8);
		panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		panel8.Location = new System.Drawing.Point(440, 60);
		panel8.Margin = new System.Windows.Forms.Padding(0);
		panel8.Name = "panel8";
		panel8.Size = new System.Drawing.Size(162, 58);
		panel8.TabIndex = 19;
		label8.AutoSize = true;
		label8.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label8.ForeColor = System.Drawing.Color.White;
		label8.Location = new System.Drawing.Point(117, 18);
		label8.Name = "label8";
		label8.Size = new System.Drawing.Size(42, 21);
		label8.TabIndex = 0;
		label8.Text = "含量";
		panel9.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel9.Controls.Add(label9);
		panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		panel9.Location = new System.Drawing.Point(440, 119);
		panel9.Margin = new System.Windows.Forms.Padding(0);
		panel9.Name = "panel9";
		panel9.Size = new System.Drawing.Size(162, 58);
		panel9.TabIndex = 19;
		label9.AutoSize = true;
		label9.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label9.ForeColor = System.Drawing.Color.White;
		label9.Location = new System.Drawing.Point(85, 18);
		label9.Name = "label9";
		label9.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
		label9.Size = new System.Drawing.Size(74, 21);
		label9.TabIndex = 0;
		label9.Text = "稀釋倍數";
		panel10.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel10.Controls.Add(label10);
		panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		panel10.Location = new System.Drawing.Point(440, 178);
		panel10.Margin = new System.Windows.Forms.Padding(0);
		panel10.Name = "panel10";
		panel10.Size = new System.Drawing.Size(162, 58);
		panel10.TabIndex = 19;
		label10.AutoSize = true;
		label10.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label10.ForeColor = System.Drawing.Color.White;
		label10.Location = new System.Drawing.Point(85, 18);
		label10.Name = "label10";
		label10.Size = new System.Drawing.Size(74, 21);
		label10.TabIndex = 0;
		label10.Text = "施藥間隔";
		panel11.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel11.Controls.Add(label11);
		panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		panel11.Location = new System.Drawing.Point(440, 237);
		panel11.Margin = new System.Windows.Forms.Padding(0);
		panel11.Name = "panel11";
		panel11.Size = new System.Drawing.Size(162, 58);
		panel11.TabIndex = 19;
		label11.AutoSize = true;
		label11.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label11.ForeColor = System.Drawing.Color.White;
		label11.Location = new System.Drawing.Point(69, 18);
		label11.Name = "label11";
		label11.Size = new System.Drawing.Size(90, 21);
		label11.TabIndex = 0;
		label11.Text = "安全採收期";
		tableLayoutPanel2.SetColumnSpan(flowLayoutPanel2, 3);
		flowLayoutPanel2.Controls.Add(label_direction);
		flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
		flowLayoutPanel2.Location = new System.Drawing.Point(164, 296);
		flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
		flowLayoutPanel2.Name = "flowLayoutPanel2";
		flowLayoutPanel2.Size = new System.Drawing.Size(715, 58);
		flowLayoutPanel2.TabIndex = 20;
		label_direction.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label_direction.Location = new System.Drawing.Point(3, 0);
		label_direction.Name = "label_direction";
		label_direction.Size = new System.Drawing.Size(693, 51);
		label_direction.TabIndex = 63;
		label_direction.Text = "{9}";
		label_direction.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		tableLayoutPanel2.SetColumnSpan(flowLayoutPanel3, 3);
		flowLayoutPanel3.Controls.Add(label_notes);
		flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
		flowLayoutPanel3.Location = new System.Drawing.Point(164, 355);
		flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
		flowLayoutPanel3.Name = "flowLayoutPanel3";
		flowLayoutPanel3.Size = new System.Drawing.Size(715, 63);
		flowLayoutPanel3.TabIndex = 20;
		label_notes.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label_notes.Location = new System.Drawing.Point(3, 0);
		label_notes.Name = "label_notes";
		label_notes.Size = new System.Drawing.Size(693, 51);
		label_notes.TabIndex = 64;
		label_notes.Text = "{10}";
		label_notes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		label_usages.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label_usages.Location = new System.Drawing.Point(167, 119);
		label_usages.Name = "label_usages";
		label_usages.Size = new System.Drawing.Size(254, 51);
		label_usages.TabIndex = 56;
		label_usages.Text = "{2}";
		label_usages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		label_period.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label_period.Location = new System.Drawing.Point(167, 178);
		label_period.Name = "label_period";
		label_period.Size = new System.Drawing.Size(254, 51);
		label_period.TabIndex = 57;
		label_period.Text = "{3}";
		label_period.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		label_frequency.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label_frequency.Location = new System.Drawing.Point(167, 237);
		label_frequency.Name = "label_frequency";
		label_frequency.Size = new System.Drawing.Size(254, 51);
		label_frequency.TabIndex = 58;
		label_frequency.Text = "{4}";
		label_frequency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		AutoScroll = true;
		BackColor = System.Drawing.Color.Silver;
		base.ClientSize = new System.Drawing.Size(904, 576);
		base.ControlBox = false;
		base.Controls.Add(tableLayoutPanel2);
		base.Controls.Add(l_title);
		base.Controls.Add(btn_cancel);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "dialogMedDescription";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "frmSearchMember";
		base.Load += new System.EventHandler(dialogMedDescription_Load);
		panel22.ResumeLayout(false);
		panel22.PerformLayout();
		tableLayoutPanel2.ResumeLayout(false);
		flowLayoutPanel1.ResumeLayout(false);
		panel1.ResumeLayout(false);
		panel1.PerformLayout();
		panel2.ResumeLayout(false);
		panel2.PerformLayout();
		panel3.ResumeLayout(false);
		panel3.PerformLayout();
		panel4.ResumeLayout(false);
		panel4.PerformLayout();
		panel5.ResumeLayout(false);
		panel5.PerformLayout();
		panel6.ResumeLayout(false);
		panel6.PerformLayout();
		panel8.ResumeLayout(false);
		panel8.PerformLayout();
		panel9.ResumeLayout(false);
		panel9.PerformLayout();
		panel10.ResumeLayout(false);
		panel10.PerformLayout();
		panel11.ResumeLayout(false);
		panel11.PerformLayout();
		flowLayoutPanel2.ResumeLayout(false);
		flowLayoutPanel3.ResumeLayout(false);
		ResumeLayout(false);
		PerformLayout();
	}
}
