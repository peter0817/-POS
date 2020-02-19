using KeyboardClassLibrary;
using POS_Client;
using POS_Client.Properties;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogSetBatchNoAndMFGdate : Form
{
	private string _GDSNO;

	private string _LicenseCode;

	private DataTable dt;

	private frmNewInventory _fni;

	private IContainer components;

	private Button btn_back;

	private TableLayoutPanel tableLayoutPanel1;

	private Panel panel3;

	private Label label6;

	private Panel panel5;

	private Label label10;

	private Panel panel13;

	private PictureBox pictureBox1;

	private Panel panel17;

	private Button btn_down;

	private Button btn_top;

	private PictureBox pictureBox2;

	private Keyboardcontrol keyboardcontrol1;

	private Button btn_Save;

	private TextBox textBox1;

	private TextBox tb_BatchNo;

	private Panel panel1;

	private Label l_GDName;

	private DateTimePicker dateTimePicker0;

	private Label label1;

	public dialogSetBatchNoAndMFGdate(frmNewInventory fni, string GDSNO)
	{
		InitializeComponent();
		_fni = fni;
		_GDSNO = GDSNO;
	}

	private void dialogSetBatchNoAndMFGdate_Load(object sender, EventArgs e)
	{
		dateTimePicker0.Value = DateTime.Today;
		dateTimePicker0.Checked = false;
		string strTableName = "hypos_GOODSLST as hg left outer join HyLicence as hl on hg.licType =hl.licType and hg.domManufId =hl.licNo";
		string strWhereClause = "hg.GDSNO ={0} AND ((hg.ISWS ='Y' and hg.CLA1NO ='0302' and hg.licType = hl.licType and hg.domManufId = hl.licNo) OR (hg.ISWS ='N' and hg.CLA1NO ='0302') OR hg.CLA1NO ='0303' OR hg.CLA1NO ='0305' OR hg.CLA1NO ='0308') AND (hl.isDelete='N' or hl.isDelete is null) ";
		dt = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "hg.inventory,hg.GDSNO,hg.spec,hg.capacity,hg.GDName,hg.formCode,hg.CName,hg.contents,hg.brandName,hg.CLA1NO,hg.ISWS", strTableName, strWhereClause, "", null, new string[1]
		{
			_GDSNO
		}, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dt.Rows.Count > 0)
		{
			l_GDName.Text = "【" + dt.Rows[0]["GDName"].ToString() + "-" + dt.Rows[0]["CName"].ToString() + "】批號與製造日期設定";
		}
		else
		{
			AutoClosingMessageBox.Show("店內碼錯誤!");
			Close();
		}
		tb_BatchNo.Select();
	}

	private void btn_back_Click(object sender, EventArgs e)
	{
		string[] data = new string[2]
		{
			"",
			""
		};
		_fni.infolistInfoSetting(data);
		base.DialogResult = DialogResult.Yes;
		AutoClosingMessageBox.Show("商品已選入");
		Close();
	}

	private void btn_KeyboardLocation_Click(object sender, EventArgs e)
	{
		if (panel17.Location.Y > 300)
		{
			panel17.Location = new Point(panel17.Location.X, 0);
		}
		else
		{
			panel17.Location = new Point(panel17.Location.X, 367);
		}
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
		panel17.BringToFront();
		if (panel17.Location.X > 900)
		{
			panel17.Location = new Point(0, panel17.Location.Y);
		}
		else
		{
			panel17.Location = new Point(997, panel17.Location.Y);
		}
	}

	private void keyboardcontrol1_UserKeyPressed(object sender, KeyboardEventArgs e)
	{
		SendKeys.Send(e.KeyboardKeyPressed);
	}

	private void btn_SaveMemberDataAndSelect_Click(object sender, EventArgs e)
	{
		string text = "";
		if (!tb_BatchNo.Text.Trim().Equals(""))
		{
			if (!dateTimePicker0.Checked)
			{
				text += "請填寫製造日期\n";
			}
		}
		else if (dateTimePicker0.Checked && tb_BatchNo.Text.Trim().Equals(""))
		{
			text += "請填寫批號\n";
		}
		if (!text.Equals(""))
		{
			AutoClosingMessageBox.Show(text);
			return;
		}
		string text2 = "";
		if (dateTimePicker0.Checked)
		{
			text2 = dateTimePicker0.Value.ToString("yyyy-MM-dd");
		}
		string[] data = new string[2]
		{
			tb_BatchNo.Text,
			text2
		};
		_fni.infolistInfoSetting(data);
		base.DialogResult = DialogResult.Yes;
		AutoClosingMessageBox.Show("設定完成，商品已選入");
		Close();
	}

	private void dateTimePicker0_ValueChanged(object sender, EventArgs e)
	{
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
		btn_back = new System.Windows.Forms.Button();
		tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		panel1 = new System.Windows.Forms.Panel();
		dateTimePicker0 = new System.Windows.Forms.DateTimePicker();
		panel13 = new System.Windows.Forms.Panel();
		tb_BatchNo = new System.Windows.Forms.TextBox();
		panel3 = new System.Windows.Forms.Panel();
		label6 = new System.Windows.Forms.Label();
		panel5 = new System.Windows.Forms.Panel();
		label10 = new System.Windows.Forms.Label();
		textBox1 = new System.Windows.Forms.TextBox();
		pictureBox1 = new System.Windows.Forms.PictureBox();
		panel17 = new System.Windows.Forms.Panel();
		btn_down = new System.Windows.Forms.Button();
		btn_top = new System.Windows.Forms.Button();
		pictureBox2 = new System.Windows.Forms.PictureBox();
		keyboardcontrol1 = new KeyboardClassLibrary.Keyboardcontrol();
		btn_Save = new System.Windows.Forms.Button();
		l_GDName = new System.Windows.Forms.Label();
		label1 = new System.Windows.Forms.Label();
		tableLayoutPanel1.SuspendLayout();
		panel1.SuspendLayout();
		panel13.SuspendLayout();
		panel3.SuspendLayout();
		panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
		panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
		SuspendLayout();
		btn_back.BackColor = System.Drawing.Color.FromArgb(153, 153, 153);
		btn_back.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_back.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_back.ForeColor = System.Drawing.Color.White;
		btn_back.Location = new System.Drawing.Point(521, 248);
		btn_back.Name = "btn_back";
		btn_back.Size = new System.Drawing.Size(92, 40);
		btn_back.TabIndex = 0;
		btn_back.Text = "放棄設定";
		btn_back.UseVisualStyleBackColor = false;
		btn_back.Click += new System.EventHandler(btn_back_Click);
		tableLayoutPanel1.BackColor = System.Drawing.Color.White;
		tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		tableLayoutPanel1.ColumnCount = 4;
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 162f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 162f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		tableLayoutPanel1.Controls.Add(panel1, 1, 1);
		tableLayoutPanel1.Controls.Add(panel13, 1, 0);
		tableLayoutPanel1.Controls.Add(panel3, 0, 0);
		tableLayoutPanel1.Controls.Add(panel5, 0, 1);
		tableLayoutPanel1.Location = new System.Drawing.Point(51, 129);
		tableLayoutPanel1.Name = "tableLayoutPanel1";
		tableLayoutPanel1.RowCount = 2;
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		tableLayoutPanel1.Size = new System.Drawing.Size(850, 104);
		tableLayoutPanel1.TabIndex = 41;
		tableLayoutPanel1.SetColumnSpan(panel1, 3);
		panel1.Controls.Add(dateTimePicker0);
		panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		panel1.Location = new System.Drawing.Point(164, 52);
		panel1.Margin = new System.Windows.Forms.Padding(0);
		panel1.Name = "panel1";
		panel1.Size = new System.Drawing.Size(685, 51);
		panel1.TabIndex = 55;
		dateTimePicker0.Anchor = System.Windows.Forms.AnchorStyles.Left;
		dateTimePicker0.Checked = false;
		dateTimePicker0.CustomFormat = "yyyy-MM-dd";
		dateTimePicker0.Font = new System.Drawing.Font("微軟正黑體", 14.25f);
		dateTimePicker0.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		dateTimePicker0.Location = new System.Drawing.Point(10, 9);
		dateTimePicker0.Margin = new System.Windows.Forms.Padding(10, 13, 3, 3);
		dateTimePicker0.Name = "dateTimePicker0";
		dateTimePicker0.ShowCheckBox = true;
		dateTimePicker0.Size = new System.Drawing.Size(181, 33);
		dateTimePicker0.TabIndex = 5;
		dateTimePicker0.Value = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
		dateTimePicker0.ValueChanged += new System.EventHandler(dateTimePicker0_ValueChanged);
		tableLayoutPanel1.SetColumnSpan(panel13, 3);
		panel13.Controls.Add(tb_BatchNo);
		panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		panel13.Location = new System.Drawing.Point(164, 1);
		panel13.Margin = new System.Windows.Forms.Padding(0);
		panel13.Name = "panel13";
		panel13.Size = new System.Drawing.Size(685, 50);
		panel13.TabIndex = 54;
		tb_BatchNo.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_BatchNo.ForeColor = System.Drawing.Color.DarkGray;
		tb_BatchNo.ImeMode = System.Windows.Forms.ImeMode.Disable;
		tb_BatchNo.Location = new System.Drawing.Point(9, 11);
		tb_BatchNo.Margin = new System.Windows.Forms.Padding(10);
		tb_BatchNo.MaxLength = 20;
		tb_BatchNo.Name = "tb_BatchNo";
		tb_BatchNo.Size = new System.Drawing.Size(666, 29);
		tb_BatchNo.TabIndex = 43;
		panel3.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel3.Controls.Add(label6);
		panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		panel3.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		panel3.ForeColor = System.Drawing.Color.White;
		panel3.Location = new System.Drawing.Point(1, 1);
		panel3.Margin = new System.Windows.Forms.Padding(0);
		panel3.Name = "panel3";
		panel3.Size = new System.Drawing.Size(162, 50);
		panel3.TabIndex = 21;
		label6.AutoSize = true;
		label6.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label6.ForeColor = System.Drawing.Color.White;
		label6.Location = new System.Drawing.Point(103, 14);
		label6.Name = "label6";
		label6.Size = new System.Drawing.Size(42, 21);
		label6.TabIndex = 0;
		label6.Text = "批號";
		panel5.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel5.Controls.Add(label10);
		panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		panel5.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		panel5.ForeColor = System.Drawing.Color.White;
		panel5.Location = new System.Drawing.Point(1, 52);
		panel5.Margin = new System.Windows.Forms.Padding(0);
		panel5.Name = "panel5";
		panel5.Size = new System.Drawing.Size(162, 51);
		panel5.TabIndex = 23;
		label10.AutoSize = true;
		label10.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label10.ForeColor = System.Drawing.Color.White;
		label10.Location = new System.Drawing.Point(71, 15);
		label10.Name = "label10";
		label10.Size = new System.Drawing.Size(74, 21);
		label10.TabIndex = 0;
		label10.Text = "製造日期";
		textBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
		textBox1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		textBox1.ForeColor = System.Drawing.Color.DarkGray;
		textBox1.Location = new System.Drawing.Point(174, 175);
		textBox1.Margin = new System.Windows.Forms.Padding(10);
		textBox1.Name = "textBox1";
		textBox1.Size = new System.Drawing.Size(239, 29);
		textBox1.TabIndex = 58;
		pictureBox1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom);
		pictureBox1.BackColor = System.Drawing.Color.Silver;
		pictureBox1.Image = POS_Client.Properties.Resources.keyboard;
		pictureBox1.Location = new System.Drawing.Point(878, 493);
		pictureBox1.Name = "pictureBox1";
		pictureBox1.Size = new System.Drawing.Size(70, 0);
		pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		pictureBox1.TabIndex = 52;
		pictureBox1.TabStop = false;
		pictureBox1.Click += new System.EventHandler(pictureBox1_Click);
		panel17.BackColor = System.Drawing.Color.FromArgb(51, 51, 51);
		panel17.Controls.Add(btn_down);
		panel17.Controls.Add(btn_top);
		panel17.Controls.Add(pictureBox2);
		panel17.Controls.Add(keyboardcontrol1);
		panel17.Location = new System.Drawing.Point(953, 367);
		panel17.Margin = new System.Windows.Forms.Padding(0);
		panel17.Name = "panel17";
		panel17.Size = new System.Drawing.Size(949, 269);
		panel17.TabIndex = 53;
		btn_down.Location = new System.Drawing.Point(862, 112);
		btn_down.Name = "btn_down";
		btn_down.Size = new System.Drawing.Size(58, 40);
		btn_down.TabIndex = 52;
		btn_down.Text = "Down";
		btn_down.UseVisualStyleBackColor = true;
		btn_down.Click += new System.EventHandler(btn_KeyboardLocation_Click);
		btn_top.Location = new System.Drawing.Point(862, 55);
		btn_top.Name = "btn_top";
		btn_top.Size = new System.Drawing.Size(58, 40);
		btn_top.TabIndex = 51;
		btn_top.Text = "Top";
		btn_top.UseVisualStyleBackColor = true;
		btn_top.Click += new System.EventHandler(btn_KeyboardLocation_Click);
		pictureBox2.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom);
		pictureBox2.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
		pictureBox2.Image = POS_Client.Properties.Resources.keyboard_close;
		pictureBox2.Location = new System.Drawing.Point(842, 7);
		pictureBox2.Name = "pictureBox2";
		pictureBox2.Size = new System.Drawing.Size(59, 34);
		pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		pictureBox2.TabIndex = 50;
		pictureBox2.TabStop = false;
		pictureBox2.Click += new System.EventHandler(pictureBox1_Click);
		keyboardcontrol1.KeyboardType = KeyboardClassLibrary.BoW.Standard;
		keyboardcontrol1.Location = new System.Drawing.Point(12, 6);
		keyboardcontrol1.Name = "keyboardcontrol1";
		keyboardcontrol1.Size = new System.Drawing.Size(816, 260);
		keyboardcontrol1.TabIndex = 0;
		keyboardcontrol1.UserKeyPressed += new KeyboardClassLibrary.KeyboardDelegate(keyboardcontrol1_UserKeyPressed);
		btn_Save.BackColor = System.Drawing.Color.DarkCyan;
		btn_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_Save.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_Save.ForeColor = System.Drawing.Color.White;
		btn_Save.Location = new System.Drawing.Point(367, 248);
		btn_Save.Name = "btn_Save";
		btn_Save.Size = new System.Drawing.Size(92, 40);
		btn_Save.TabIndex = 54;
		btn_Save.Text = "儲存設定";
		btn_Save.UseVisualStyleBackColor = false;
		btn_Save.Click += new System.EventHandler(btn_SaveMemberDataAndSelect_Click);
		l_GDName.Font = new System.Drawing.Font("微軟正黑體", 14f, System.Drawing.FontStyle.Bold);
		l_GDName.Location = new System.Drawing.Point(215, 37);
		l_GDName.Name = "l_GDName";
		l_GDName.Size = new System.Drawing.Size(553, 24);
		l_GDName.TabIndex = 55;
		l_GDName.Text = "label1";
		l_GDName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label1.Location = new System.Drawing.Point(211, 77);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(553, 40);
		label1.TabIndex = 57;
		label1.Text = "批發農藥出貨必須設定批號與製造日期，預先設定可於出貨時協助快速選入。\r\n若進貨商品有不同批號數量，請分為兩筆設定。";
		label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		BackColor = System.Drawing.Color.Silver;
		base.ClientSize = new System.Drawing.Size(949, 311);
		base.Controls.Add(label1);
		base.Controls.Add(l_GDName);
		base.Controls.Add(btn_Save);
		base.Controls.Add(panel17);
		base.Controls.Add(pictureBox1);
		base.Controls.Add(tableLayoutPanel1);
		base.Controls.Add(btn_back);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "dialogSetBatchNoAndMFGdate";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "選擇會員 / 會員編修";
		base.Load += new System.EventHandler(dialogSetBatchNoAndMFGdate_Load);
		tableLayoutPanel1.ResumeLayout(false);
		panel1.ResumeLayout(false);
		panel13.ResumeLayout(false);
		panel13.PerformLayout();
		panel3.ResumeLayout(false);
		panel3.PerformLayout();
		panel5.ResumeLayout(false);
		panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
		panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}
}
