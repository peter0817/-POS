using POS_Client;
using POS_Client.WebService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogCheckVendorInfo : Form
{
	private frmEditInventory _fEI;

	private IContainer components;

	private Button btn_cancel;

	public Label l_title;

	private TableLayoutPanel tableLayoutPanel1;

	private Panel panel1;

	private Label label1;

	private Panel panel2;

	private Panel panel8;

	private TextBox tb_vendorId;

	private Panel panel7;

	private Label label4;

	private Panel panel6;

	private TextBox tb_SupplierIdNo;

	private Panel panel5;

	private Label label3;

	private Panel panel4;

	private TextBox tb_SupplierName;

	private Panel panel3;

	private Label label2;

	private TextBox tb_supplierNo;

	private TextBox tb_vendorName;

	private Button btn_checkVendorID;

	private Label label22;

	private Label label24;

	private Label label21;

	private Button btn_complete;

	private Label label6;

	private Label label5;

	public dialogCheckVendorInfo(frmEditInventory fEI)
	{
		InitializeComponent();
		_fEI = fEI;
		tb_supplierNo.Text = _fEI.get_SupplierNo();
		tb_SupplierName.Text = _fEI.get_SupplierName();
		tb_SupplierIdNo.Text = _fEI.get_SupplierIdNo();
		tb_vendorId.Text = _fEI.get_vendorId();
		tb_vendorName.Text = _fEI.get_vendorName();
	}

	private void btn_cancel_Click(object sender, EventArgs e)
	{
		if ("".Equals(tb_vendorName.Text) || "".Equals(tb_vendorId.Text))
		{
			AutoClosingMessageBox.Show("請注意，未驗證進貨廠商營業資訊的廠商無法進行管制農藥的原廠退回/過期退貨功能。");
		}
		Close();
	}

	private void btn_checkVendorID_Click(object sender, EventArgs e)
	{
		string text = tb_vendorId.Text.Trim();
		if (text.Equals("請輸入執照號碼後點選檢查"))
		{
			text = "";
		}
		if (text.Equals(""))
		{
			tb_vendorName.Text = "";
			AutoClosingMessageBox.Show("「販賣執照號碼」必填，請檢查");
			return;
		}
		if (!checkVendorID(text))
		{
			tb_vendorName.Text = "";
			AutoClosingMessageBox.Show("請輸入廠商之新販賣業執照號碼，包含一碼英文+五碼數字");
			return;
		}
		VendorResultObject vendorResultObject = new VerifyVendorInfoWS().vendorData(tb_vendorId.Text);
		if (vendorResultObject.success == "Y")
		{
			if (MessageBox.Show("證號：" + vendorResultObject.vendorId + "、名稱：" + vendorResultObject.vendorName + " \n是否使用此廠商資訊？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
			{
				tb_vendorName.Text = vendorResultObject.vendorName;
			}
		}
		else if (vendorResultObject.success == "N")
		{
			string text2 = "";
			if (vendorResultObject.message.Equals("廠商販賣執照號碼不存在"))
			{
				text2 = "廠商營業執照號碼不存在，請檢查輸入值";
			}
			else if (vendorResultObject.message.Equals("廠商已歇業或停業"))
			{
				text2 = "此廠商目前已停用，請先確認廠商狀態";
			}
			tb_vendorId.Text = "";
			tb_vendorName.Text = "";
			MessageBox.Show(text2);
		}
	}

	private bool checkVendorID(string id)
	{
		List<string> list = new List<string>
		{
			"A",
			"B",
			"C",
			"D",
			"E",
			"F",
			"G",
			"H",
			"J",
			"K",
			"L",
			"M",
			"N",
			"P",
			"Q",
			"R",
			"S",
			"T",
			"U",
			"V",
			"X",
			"Y",
			"W",
			"Z",
			"I",
			"O"
		};
		if (id.Trim().Length == 6)
		{
			for (int i = 1; i < 6; i++)
			{
				byte b = Convert.ToByte(id.Trim().Substring(i, 1));
				if (b > 9 || b < 0)
				{
					return false;
				}
			}
			id = id.ToUpper();
			int j;
			for (j = 0; j < list.Count && !(id.Substring(0, 1) == list[j]); j++)
			{
			}
			if (j > 25)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	private void btn_complete_Click(object sender, EventArgs e)
	{
		if ("".Equals(tb_vendorName.Text) || "".Equals(tb_vendorId.Text))
		{
			AutoClosingMessageBox.Show("請先驗證廠商營業資訊");
			return;
		}
		string[,] strFieldArray = new string[3, 2]
		{
			{
				"vendorId",
				tb_vendorId.Text
			},
			{
				"vendorName",
				tb_vendorName.Text
			},
			{
				"EditDate",
				DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
			}
		};
		DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Update, "", "hypos_Supplier", "SupplierNo ={0}", "", strFieldArray, new string[1]
		{
			tb_supplierNo.Text
		}, CommandOperationType.ExecuteNonQuery);
		_fEI.set_vendorINFO(tb_vendorId.Text, tb_vendorName.Text);
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
		tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		panel8 = new System.Windows.Forms.Panel();
		label21 = new System.Windows.Forms.Label();
		tb_vendorName = new System.Windows.Forms.TextBox();
		btn_checkVendorID = new System.Windows.Forms.Button();
		label22 = new System.Windows.Forms.Label();
		label24 = new System.Windows.Forms.Label();
		tb_vendorId = new System.Windows.Forms.TextBox();
		panel7 = new System.Windows.Forms.Panel();
		label4 = new System.Windows.Forms.Label();
		panel6 = new System.Windows.Forms.Panel();
		tb_SupplierIdNo = new System.Windows.Forms.TextBox();
		panel5 = new System.Windows.Forms.Panel();
		label3 = new System.Windows.Forms.Label();
		panel4 = new System.Windows.Forms.Panel();
		tb_SupplierName = new System.Windows.Forms.TextBox();
		panel3 = new System.Windows.Forms.Panel();
		label2 = new System.Windows.Forms.Label();
		panel1 = new System.Windows.Forms.Panel();
		label1 = new System.Windows.Forms.Label();
		panel2 = new System.Windows.Forms.Panel();
		tb_supplierNo = new System.Windows.Forms.TextBox();
		btn_complete = new System.Windows.Forms.Button();
		label5 = new System.Windows.Forms.Label();
		label6 = new System.Windows.Forms.Label();
		tableLayoutPanel1.SuspendLayout();
		panel8.SuspendLayout();
		panel7.SuspendLayout();
		panel6.SuspendLayout();
		panel5.SuspendLayout();
		panel4.SuspendLayout();
		panel3.SuspendLayout();
		panel1.SuspendLayout();
		panel2.SuspendLayout();
		SuspendLayout();
		btn_cancel.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_cancel.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_cancel.ForeColor = System.Drawing.Color.White;
		btn_cancel.Location = new System.Drawing.Point(519, 374);
		btn_cancel.Name = "btn_cancel";
		btn_cancel.Size = new System.Drawing.Size(124, 34);
		btn_cancel.TabIndex = 46;
		btn_cancel.TabStop = false;
		btn_cancel.Text = "取消";
		btn_cancel.UseVisualStyleBackColor = false;
		btn_cancel.Click += new System.EventHandler(btn_cancel_Click);
		l_title.AutoSize = true;
		l_title.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_title.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		l_title.Location = new System.Drawing.Point(41, 28);
		l_title.Name = "l_title";
		l_title.Size = new System.Drawing.Size(200, 24);
		l_title.TabIndex = 52;
		l_title.Text = "農藥廠商營業資訊驗證";
		l_title.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		tableLayoutPanel1.ColumnCount = 4;
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35f));
		tableLayoutPanel1.Controls.Add(panel8, 1, 2);
		tableLayoutPanel1.Controls.Add(panel7, 0, 2);
		tableLayoutPanel1.Controls.Add(panel6, 3, 1);
		tableLayoutPanel1.Controls.Add(panel5, 2, 1);
		tableLayoutPanel1.Controls.Add(panel4, 1, 1);
		tableLayoutPanel1.Controls.Add(panel3, 0, 1);
		tableLayoutPanel1.Controls.Add(panel1, 0, 0);
		tableLayoutPanel1.Controls.Add(panel2, 1, 0);
		tableLayoutPanel1.Location = new System.Drawing.Point(47, 73);
		tableLayoutPanel1.Name = "tableLayoutPanel1";
		tableLayoutPanel1.RowCount = 3;
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		tableLayoutPanel1.Size = new System.Drawing.Size(805, 276);
		tableLayoutPanel1.TabIndex = 53;
		tableLayoutPanel1.SetColumnSpan(panel8, 3);
		panel8.Controls.Add(label21);
		panel8.Controls.Add(tb_vendorName);
		panel8.Controls.Add(btn_checkVendorID);
		panel8.Controls.Add(label22);
		panel8.Controls.Add(label24);
		panel8.Controls.Add(tb_vendorId);
		panel8.Location = new System.Drawing.Point(123, 141);
		panel8.Name = "panel8";
		panel8.Size = new System.Drawing.Size(679, 132);
		panel8.TabIndex = 27;
		label21.AutoSize = true;
		label21.Font = new System.Drawing.Font("微軟正黑體", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label21.Location = new System.Drawing.Point(23, 101);
		label21.Name = "label21";
		label21.Size = new System.Drawing.Size(421, 19);
		label21.TabIndex = 96;
		label21.Text = "*請注意，使用原廠退回功能，必須先驗證商業執照號碼與名稱";
		tb_vendorName.Cursor = System.Windows.Forms.Cursors.No;
		tb_vendorName.Enabled = false;
		tb_vendorName.Font = new System.Drawing.Font("微軟正黑體", 14.25f);
		tb_vendorName.Location = new System.Drawing.Point(130, 59);
		tb_vendorName.Margin = new System.Windows.Forms.Padding(10, 10, 3, 10);
		tb_vendorName.Name = "tb_vendorName";
		tb_vendorName.ReadOnly = true;
		tb_vendorName.Size = new System.Drawing.Size(297, 33);
		tb_vendorName.TabIndex = 95;
		btn_checkVendorID.Font = new System.Drawing.Font("微軟正黑體", 14.25f);
		btn_checkVendorID.Location = new System.Drawing.Point(446, 20);
		btn_checkVendorID.Name = "btn_checkVendorID";
		btn_checkVendorID.Size = new System.Drawing.Size(75, 72);
		btn_checkVendorID.TabIndex = 94;
		btn_checkVendorID.Text = "檢查";
		btn_checkVendorID.UseVisualStyleBackColor = true;
		btn_checkVendorID.Click += new System.EventHandler(btn_checkVendorID_Click);
		label22.AutoSize = true;
		label22.Font = new System.Drawing.Font("微軟正黑體", 14.25f);
		label22.Location = new System.Drawing.Point(23, 60);
		label22.Name = "label22";
		label22.Size = new System.Drawing.Size(105, 24);
		label22.TabIndex = 93;
		label22.Text = "商業名稱：";
		label24.AutoSize = true;
		label24.Font = new System.Drawing.Font("微軟正黑體", 14.25f);
		label24.Location = new System.Drawing.Point(23, 23);
		label24.Name = "label24";
		label24.Size = new System.Drawing.Size(105, 24);
		label24.TabIndex = 92;
		label24.Text = "執照號碼：";
		tb_vendorId.Cursor = System.Windows.Forms.Cursors.Arrow;
		tb_vendorId.Font = new System.Drawing.Font("微軟正黑體", 14.25f);
		tb_vendorId.ImeMode = System.Windows.Forms.ImeMode.Disable;
		tb_vendorId.Location = new System.Drawing.Point(130, 20);
		tb_vendorId.Margin = new System.Windows.Forms.Padding(10, 10, 3, 10);
		tb_vendorId.MaxLength = 6;
		tb_vendorId.Name = "tb_vendorId";
		tb_vendorId.Size = new System.Drawing.Size(297, 33);
		tb_vendorId.TabIndex = 91;
		panel7.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel7.Controls.Add(label6);
		panel7.Controls.Add(label4);
		panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		panel7.Location = new System.Drawing.Point(0, 138);
		panel7.Margin = new System.Windows.Forms.Padding(0);
		panel7.Name = "panel7";
		panel7.Size = new System.Drawing.Size(120, 138);
		panel7.TabIndex = 26;
		label4.AutoSize = true;
		label4.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label4.ForeColor = System.Drawing.Color.White;
		label4.Location = new System.Drawing.Point(26, 24);
		label4.Name = "label4";
		label4.Size = new System.Drawing.Size(74, 21);
		label4.TabIndex = 90;
		label4.Text = "營業資訊";
		panel6.Controls.Add(tb_SupplierIdNo);
		panel6.Location = new System.Drawing.Point(524, 72);
		panel6.Name = "panel6";
		panel6.Size = new System.Drawing.Size(278, 63);
		panel6.TabIndex = 25;
		tb_SupplierIdNo.Cursor = System.Windows.Forms.Cursors.No;
		tb_SupplierIdNo.Enabled = false;
		tb_SupplierIdNo.Font = new System.Drawing.Font("微軟正黑體", 14.25f);
		tb_SupplierIdNo.Location = new System.Drawing.Point(10, 20);
		tb_SupplierIdNo.Margin = new System.Windows.Forms.Padding(10, 10, 3, 10);
		tb_SupplierIdNo.Name = "tb_SupplierIdNo";
		tb_SupplierIdNo.ReadOnly = true;
		tb_SupplierIdNo.Size = new System.Drawing.Size(256, 33);
		tb_SupplierIdNo.TabIndex = 91;
		panel5.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel5.Controls.Add(label3);
		panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		panel5.Location = new System.Drawing.Point(401, 69);
		panel5.Margin = new System.Windows.Forms.Padding(0);
		panel5.Name = "panel5";
		panel5.Size = new System.Drawing.Size(120, 69);
		panel5.TabIndex = 24;
		label3.AutoSize = true;
		label3.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label3.ForeColor = System.Drawing.Color.White;
		label3.Location = new System.Drawing.Point(26, 24);
		label3.Name = "label3";
		label3.Size = new System.Drawing.Size(74, 21);
		label3.TabIndex = 90;
		label3.Text = "統一編號";
		panel4.Controls.Add(tb_SupplierName);
		panel4.Location = new System.Drawing.Point(123, 72);
		panel4.Name = "panel4";
		panel4.Size = new System.Drawing.Size(275, 63);
		panel4.TabIndex = 23;
		tb_SupplierName.Cursor = System.Windows.Forms.Cursors.No;
		tb_SupplierName.Enabled = false;
		tb_SupplierName.Font = new System.Drawing.Font("微軟正黑體", 14.25f);
		tb_SupplierName.Location = new System.Drawing.Point(10, 20);
		tb_SupplierName.Margin = new System.Windows.Forms.Padding(10, 10, 3, 10);
		tb_SupplierName.Name = "tb_SupplierName";
		tb_SupplierName.ReadOnly = true;
		tb_SupplierName.Size = new System.Drawing.Size(253, 33);
		tb_SupplierName.TabIndex = 91;
		panel3.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel3.Controls.Add(label5);
		panel3.Controls.Add(label2);
		panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		panel3.Location = new System.Drawing.Point(0, 69);
		panel3.Margin = new System.Windows.Forms.Padding(0);
		panel3.Name = "panel3";
		panel3.Size = new System.Drawing.Size(120, 69);
		panel3.TabIndex = 22;
		label2.AutoSize = true;
		label2.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label2.ForeColor = System.Drawing.Color.White;
		label2.Location = new System.Drawing.Point(26, 24);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(74, 21);
		label2.TabIndex = 90;
		label2.Text = "廠商名稱";
		panel1.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel1.Controls.Add(label1);
		panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		panel1.Location = new System.Drawing.Point(0, 0);
		panel1.Margin = new System.Windows.Forms.Padding(0);
		panel1.Name = "panel1";
		panel1.Size = new System.Drawing.Size(120, 69);
		panel1.TabIndex = 20;
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.Color.White;
		label1.Location = new System.Drawing.Point(26, 24);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(74, 21);
		label1.TabIndex = 90;
		label1.Text = "廠商編號";
		tableLayoutPanel1.SetColumnSpan(panel2, 3);
		panel2.Controls.Add(tb_supplierNo);
		panel2.Location = new System.Drawing.Point(123, 3);
		panel2.Name = "panel2";
		panel2.Size = new System.Drawing.Size(679, 63);
		panel2.TabIndex = 21;
		tb_supplierNo.Cursor = System.Windows.Forms.Cursors.No;
		tb_supplierNo.Enabled = false;
		tb_supplierNo.Font = new System.Drawing.Font("微軟正黑體", 14.25f);
		tb_supplierNo.Location = new System.Drawing.Point(10, 20);
		tb_supplierNo.Margin = new System.Windows.Forms.Padding(10, 10, 3, 10);
		tb_supplierNo.Name = "tb_supplierNo";
		tb_supplierNo.ReadOnly = true;
		tb_supplierNo.Size = new System.Drawing.Size(657, 33);
		tb_supplierNo.TabIndex = 91;
		btn_complete.BackColor = System.Drawing.Color.Red;
		btn_complete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_complete.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_complete.ForeColor = System.Drawing.Color.White;
		btn_complete.Location = new System.Drawing.Point(249, 374);
		btn_complete.Name = "btn_complete";
		btn_complete.Size = new System.Drawing.Size(239, 34);
		btn_complete.TabIndex = 54;
		btn_complete.TabStop = false;
		btn_complete.Text = "完成(並儲存變更)";
		btn_complete.UseVisualStyleBackColor = false;
		btn_complete.Click += new System.EventHandler(btn_complete_Click);
		label5.AutoSize = true;
		label5.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label5.ForeColor = System.Drawing.Color.Red;
		label5.Location = new System.Drawing.Point(13, 23);
		label5.Name = "label5";
		label5.Size = new System.Drawing.Size(17, 21);
		label5.TabIndex = 91;
		label5.Text = "*";
		label6.AutoSize = true;
		label6.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label6.ForeColor = System.Drawing.Color.Red;
		label6.Location = new System.Drawing.Point(13, 23);
		label6.Name = "label6";
		label6.Size = new System.Drawing.Size(17, 21);
		label6.TabIndex = 92;
		label6.Text = "*";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		AutoScroll = true;
		BackColor = System.Drawing.Color.Silver;
		base.ClientSize = new System.Drawing.Size(904, 431);
		base.ControlBox = false;
		base.Controls.Add(btn_complete);
		base.Controls.Add(tableLayoutPanel1);
		base.Controls.Add(l_title);
		base.Controls.Add(btn_cancel);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "dialogCheckVendorInfo";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "frmSearchMember";
		tableLayoutPanel1.ResumeLayout(false);
		panel8.ResumeLayout(false);
		panel8.PerformLayout();
		panel7.ResumeLayout(false);
		panel7.PerformLayout();
		panel6.ResumeLayout(false);
		panel6.PerformLayout();
		panel5.ResumeLayout(false);
		panel5.PerformLayout();
		panel4.ResumeLayout(false);
		panel4.PerformLayout();
		panel3.ResumeLayout(false);
		panel3.PerformLayout();
		panel1.ResumeLayout(false);
		panel1.PerformLayout();
		panel2.ResumeLayout(false);
		panel2.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}
}
