using POS_Client;
using POS_Client.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogChooseVendor : Form
{
	private UC_Member[] ucMembers;

	private List<string> lst_vendorlist = new List<string>();

	private int pageNow = 1;

	public int pageTotal = 1;

	public DataTable dt;

	private IContainer components;

	private TableLayoutPanel tableLayoutPanel1;

	private Panel panel2;

	private Label label2;

	private Panel panel1;

	private Label label1;

	private TextBox tb_vendorName;

	private TextBox tb_vendorID;

	private Panel panel3;

	private Panel panel4;

	private Label label3;

	private Label label4;

	private TextBox tb_SupplierNo;

	private TextBox tb_vendorIdNo;

	private Label label5;

	private Button btn_enter;

	private Button btn_reset;

	private UC_Member uC_Member5;

	private UC_Member uC_Member6;

	private Button btn_ChooseEnter;

	private Button btn_resetCheck;

	private Label l_pageInfo;

	private Button btn_pageLeft;

	private Button btn_pageRight;

	private Button btn_cancel;

	private UC_Member uC_Member3;

	private UC_Member uC_Member4;

	private UC_Member uC_Member2;

	private UC_Member uC_Member1;

	public dialogChooseVendor(List<string> lst)
	{
		lst_vendorlist.Clear();
		lst_vendorlist = lst;
		InitializeComponent();
		ucMembers = new UC_Member[6]
		{
			uC_Member1,
			uC_Member2,
			uC_Member3,
			uC_Member4,
			uC_Member5,
			uC_Member6
		};
		UC_Member[] array = ucMembers;
		foreach (UC_Member obj in array)
		{
			obj.OnClickMember += MemberChecked;
			obj.checkMember(check: false);
			obj.showCheckBox(isShow: true);
		}
		dt = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "*", "hypos_Supplier", " status = 0 ", "EditDate DESC limit 6", null, null, CommandOperationType.ExecuteReaderReturnDataTable);
		changePage(1);
	}

	public void changePage(int page)
	{
		int num = 0;
		pageNow = page;
		pageTotal = (int)Math.Ceiling((double)dt.Rows.Count / 6.0);
		for (int i = (pageNow - 1) * 6; i < pageNow * 6; i++)
		{
			if (i < dt.Rows.Count)
			{
				ucMembers[num].setVendorInfo(dt.Rows[i]["SupplierName"].ToString(), dt.Rows[i]["SupplierNo"].ToString(), dt.Rows[i]["SupplierIdNo"].ToString(), dt.Rows[i]["vendorId"].ToString());
				ucMembers[num].Visible = true;
			}
			else
			{
				ucMembers[num].Visible = false;
			}
			ucMembers[num].checkMember(check: false);
			ucMembers[num].BackColor = Color.White;
			num++;
		}
		l_pageInfo.Text = $"共{dt.Rows.Count}筆．{pageTotal}頁｜目前在第{pageNow}頁";
	}

	public void MemberChecked(object vipNo, EventArgs s)
	{
	}

	private bool hasSelectedMember()
	{
		UC_Member[] array = ucMembers;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].isChecked())
			{
				if (MessageBox.Show("勾選的資料尚未放入暫存清單，是否放棄將勾選放入暫存？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					return false;
				}
				return true;
			}
		}
		return false;
	}

	private void btn_cancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btn_ChooseEnter_Click(object sender, EventArgs e)
	{
		string text = sender as string;
		UC_Member[] array = ucMembers;
		foreach (UC_Member uC_Member in array)
		{
			if (uC_Member.isChecked())
			{
				text = uC_Member.getVendorSupplierNo() + "," + uC_Member.getVendorSupplierName();
				lst_vendorlist.Add(text);
			}
		}
		Close();
	}

	private void btn_enter_Click(object sender, EventArgs e)
	{
		if (tb_vendorIdNo.Text == "請輸入統一編號" && tb_vendorID.Text == "請輸入營業執照號碼" && tb_vendorName.Text == "請輸入廠商名稱關鍵字" && tb_SupplierNo.Text == "請輸入廠商系統編號")
		{
			AutoClosingMessageBox.Show("必須輸入查詢條件");
			return;
		}
		btn_pageLeft.Visible = true;
		btn_pageRight.Visible = true;
		l_pageInfo.Visible = true;
		label1.Text = "會員搜尋結果";
		int num = 0;
		List<string> list = new List<string>();
		string text = "SELECT * FROM hypos_Supplier WHERE status = 0 ";
		if (tb_vendorIdNo.Text != "請輸入統一編號")
		{
			text = text + " AND SupplierIdNo like {" + num + "}";
			list.Add("%" + tb_vendorIdNo.Text + "%");
			num++;
		}
		if (tb_vendorID.Text != "請輸入營業執照號碼")
		{
			text = text + " AND vendorId like {" + num + "}";
			list.Add("%" + tb_vendorID.Text + "%");
			num++;
		}
		if (tb_vendorName.Text != "請輸入廠商名稱關鍵字")
		{
			text = text + " AND SupplierName like {" + num + "}";
			list.Add("%" + tb_vendorName.Text + "%");
			num++;
		}
		if (tb_SupplierNo.Text != "請輸入廠商系統編號")
		{
			text = text + " AND SupplierNo like {" + num + "}";
			list.Add("%" + tb_SupplierNo.Text + "%");
		}
		dt = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, text, list.ToArray(), CommandOperationType.ExecuteReaderReturnDataTable);
		changePage(1);
	}

	private void btn_resetCheck_Click(object sender, EventArgs e)
	{
		UC_Member[] array = ucMembers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].checkMember(check: false);
		}
	}

	private void btn_reset_Click(object sender, EventArgs e)
	{
		tb_vendorName.Text = "請輸入廠商名稱關鍵字";
		tb_vendorIdNo.Text = "請輸入統一編號";
		tb_vendorID.Text = "請輸入營業執照號碼";
		tb_SupplierNo.Text = "請輸入廠商系統編號";
	}

	private void tb_vipTelNo_Enter(object sender, EventArgs e)
	{
		if (tb_vendorName.Text == "請輸入廠商名稱關鍵字")
		{
			tb_vendorName.Text = "";
		}
	}

	private void tb_vipTelNo_Leave(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(tb_vendorName.Text))
		{
			tb_vendorName.Text = "請輸入廠商名稱關鍵字";
		}
	}

	private void tb_vipNo_Enter(object sender, EventArgs e)
	{
		if (tb_vendorIdNo.Text == "請輸入統一編號")
		{
			tb_vendorIdNo.Text = "";
		}
	}

	private void tb_vipNo_Leave(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(tb_vendorIdNo.Text))
		{
			tb_vendorIdNo.Text = "請輸入統一編號";
		}
	}

	private void tb_vipName_Enter(object sender, EventArgs e)
	{
		if (tb_vendorID.Text == "請輸入營業執照號碼")
		{
			tb_vendorID.Text = "";
		}
	}

	private void tb_vipName_Leave(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(tb_vendorID.Text))
		{
			tb_vendorID.Text = "請輸入營業執照號碼";
		}
	}

	private void tb_IdNo_Enter(object sender, EventArgs e)
	{
		if (tb_SupplierNo.Text == "請輸入廠商系統編號")
		{
			tb_SupplierNo.Text = "";
		}
	}

	private void tb_IdNo_Leave(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(tb_SupplierNo.Text))
		{
			tb_SupplierNo.Text = "請輸入廠商系統編號";
		}
	}

	private void tb_IdNo_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = (!char.IsDigit(e.KeyChar) && !e.KeyChar.Equals('\b') && !char.IsUpper(e.KeyChar));
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
		tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		tb_SupplierNo = new System.Windows.Forms.TextBox();
		panel2 = new System.Windows.Forms.Panel();
		label2 = new System.Windows.Forms.Label();
		panel1 = new System.Windows.Forms.Panel();
		label1 = new System.Windows.Forms.Label();
		panel3 = new System.Windows.Forms.Panel();
		label3 = new System.Windows.Forms.Label();
		panel4 = new System.Windows.Forms.Panel();
		label4 = new System.Windows.Forms.Label();
		tb_vendorName = new System.Windows.Forms.TextBox();
		tb_vendorID = new System.Windows.Forms.TextBox();
		tb_vendorIdNo = new System.Windows.Forms.TextBox();
		label5 = new System.Windows.Forms.Label();
		btn_enter = new System.Windows.Forms.Button();
		btn_reset = new System.Windows.Forms.Button();
		btn_ChooseEnter = new System.Windows.Forms.Button();
		btn_resetCheck = new System.Windows.Forms.Button();
		l_pageInfo = new System.Windows.Forms.Label();
		btn_cancel = new System.Windows.Forms.Button();
		uC_Member6 = new POS_Client.UC_Member();
		uC_Member5 = new POS_Client.UC_Member();
		btn_pageRight = new System.Windows.Forms.Button();
		btn_pageLeft = new System.Windows.Forms.Button();
		uC_Member3 = new POS_Client.UC_Member();
		uC_Member4 = new POS_Client.UC_Member();
		uC_Member2 = new POS_Client.UC_Member();
		uC_Member1 = new POS_Client.UC_Member();
		tableLayoutPanel1.SuspendLayout();
		panel2.SuspendLayout();
		panel1.SuspendLayout();
		panel3.SuspendLayout();
		panel4.SuspendLayout();
		SuspendLayout();
		tableLayoutPanel1.BackColor = System.Drawing.Color.White;
		tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		tableLayoutPanel1.ColumnCount = 4;
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30f));
		tableLayoutPanel1.Controls.Add(tb_SupplierNo, 3, 1);
		tableLayoutPanel1.Controls.Add(panel2, 0, 1);
		tableLayoutPanel1.Controls.Add(panel1, 0, 0);
		tableLayoutPanel1.Controls.Add(panel3, 2, 0);
		tableLayoutPanel1.Controls.Add(panel4, 2, 1);
		tableLayoutPanel1.Controls.Add(tb_vendorName, 1, 0);
		tableLayoutPanel1.Controls.Add(tb_vendorID, 1, 1);
		tableLayoutPanel1.Controls.Add(tb_vendorIdNo, 3, 0);
		tableLayoutPanel1.Location = new System.Drawing.Point(99, 53);
		tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
		tableLayoutPanel1.Name = "tableLayoutPanel1";
		tableLayoutPanel1.RowCount = 2;
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		tableLayoutPanel1.Size = new System.Drawing.Size(796, 104);
		tableLayoutPanel1.TabIndex = 40;
		tb_SupplierNo.BackColor = System.Drawing.Color.White;
		tb_SupplierNo.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_SupplierNo.ForeColor = System.Drawing.Color.DarkGray;
		tb_SupplierNo.Location = new System.Drawing.Point(567, 62);
		tb_SupplierNo.Margin = new System.Windows.Forms.Padding(10);
		tb_SupplierNo.MaxLength = 10;
		tb_SupplierNo.Name = "tb_SupplierNo";
		tb_SupplierNo.Size = new System.Drawing.Size(217, 29);
		tb_SupplierNo.TabIndex = 44;
		tb_SupplierNo.Text = "請輸入廠商系統編號";
		tb_SupplierNo.Enter += new System.EventHandler(tb_IdNo_Enter);
		tb_SupplierNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(tb_IdNo_KeyPress);
		tb_SupplierNo.Leave += new System.EventHandler(tb_IdNo_Leave);
		panel2.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel2.Controls.Add(label2);
		panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		panel2.Location = new System.Drawing.Point(1, 52);
		panel2.Margin = new System.Windows.Forms.Padding(0);
		panel2.Name = "panel2";
		panel2.Size = new System.Drawing.Size(158, 51);
		panel2.TabIndex = 23;
		label2.AutoSize = true;
		label2.BackColor = System.Drawing.Color.Transparent;
		label2.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label2.ForeColor = System.Drawing.Color.White;
		label2.Location = new System.Drawing.Point(31, 15);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(106, 21);
		label2.TabIndex = 0;
		label2.Text = "營業執照號碼";
		panel1.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel1.Controls.Add(label1);
		panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		panel1.Location = new System.Drawing.Point(1, 1);
		panel1.Margin = new System.Windows.Forms.Padding(0);
		panel1.Name = "panel1";
		panel1.Size = new System.Drawing.Size(158, 50);
		panel1.TabIndex = 22;
		label1.AutoSize = true;
		label1.BackColor = System.Drawing.Color.Transparent;
		label1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.Color.White;
		label1.Location = new System.Drawing.Point(62, 18);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(74, 21);
		label1.TabIndex = 0;
		label1.Text = "廠商名稱";
		panel3.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel3.Controls.Add(label3);
		panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		panel3.Location = new System.Drawing.Point(398, 1);
		panel3.Margin = new System.Windows.Forms.Padding(0);
		panel3.Name = "panel3";
		panel3.Size = new System.Drawing.Size(158, 50);
		panel3.TabIndex = 41;
		label3.AutoSize = true;
		label3.BackColor = System.Drawing.Color.Transparent;
		label3.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label3.ForeColor = System.Drawing.Color.White;
		label3.Location = new System.Drawing.Point(30, 18);
		label3.Name = "label3";
		label3.Size = new System.Drawing.Size(106, 21);
		label3.TabIndex = 43;
		label3.Text = "廠商統一編號";
		panel4.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel4.Controls.Add(label4);
		panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		panel4.Location = new System.Drawing.Point(398, 52);
		panel4.Margin = new System.Windows.Forms.Padding(0);
		panel4.Name = "panel4";
		panel4.Size = new System.Drawing.Size(158, 51);
		panel4.TabIndex = 42;
		label4.AutoSize = true;
		label4.BackColor = System.Drawing.Color.Transparent;
		label4.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label4.ForeColor = System.Drawing.Color.White;
		label4.Location = new System.Drawing.Point(30, 15);
		label4.Name = "label4";
		label4.Size = new System.Drawing.Size(106, 21);
		label4.TabIndex = 44;
		label4.Text = "廠商系統編號";
		tb_vendorName.BackColor = System.Drawing.Color.White;
		tb_vendorName.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_vendorName.ForeColor = System.Drawing.Color.DarkGray;
		tb_vendorName.Location = new System.Drawing.Point(170, 11);
		tb_vendorName.Margin = new System.Windows.Forms.Padding(10);
		tb_vendorName.MaxLength = 12;
		tb_vendorName.Name = "tb_vendorName";
		tb_vendorName.Size = new System.Drawing.Size(217, 29);
		tb_vendorName.TabIndex = 24;
		tb_vendorName.Text = "請輸入廠商名稱關鍵字";
		tb_vendorName.Enter += new System.EventHandler(tb_vipTelNo_Enter);
		tb_vendorName.Leave += new System.EventHandler(tb_vipTelNo_Leave);
		tb_vendorID.BackColor = System.Drawing.Color.White;
		tb_vendorID.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_vendorID.ForeColor = System.Drawing.Color.DarkGray;
		tb_vendorID.Location = new System.Drawing.Point(170, 62);
		tb_vendorID.Margin = new System.Windows.Forms.Padding(10);
		tb_vendorID.MaxLength = 10;
		tb_vendorID.Name = "tb_vendorID";
		tb_vendorID.Size = new System.Drawing.Size(217, 29);
		tb_vendorID.TabIndex = 25;
		tb_vendorID.Text = "請輸入營業執照號碼";
		tb_vendorID.Enter += new System.EventHandler(tb_vipName_Enter);
		tb_vendorID.Leave += new System.EventHandler(tb_vipName_Leave);
		tb_vendorIdNo.BackColor = System.Drawing.Color.White;
		tb_vendorIdNo.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_vendorIdNo.ForeColor = System.Drawing.Color.DarkGray;
		tb_vendorIdNo.ImeMode = System.Windows.Forms.ImeMode.Disable;
		tb_vendorIdNo.Location = new System.Drawing.Point(567, 11);
		tb_vendorIdNo.Margin = new System.Windows.Forms.Padding(10);
		tb_vendorIdNo.MaxLength = 100;
		tb_vendorIdNo.Name = "tb_vendorIdNo";
		tb_vendorIdNo.Size = new System.Drawing.Size(217, 29);
		tb_vendorIdNo.TabIndex = 43;
		tb_vendorIdNo.Text = "請輸入統一編號";
		tb_vendorIdNo.Enter += new System.EventHandler(tb_vipNo_Enter);
		tb_vendorIdNo.Leave += new System.EventHandler(tb_vipNo_Leave);
		label5.AutoSize = true;
		label5.Font = new System.Drawing.Font("微軟正黑體", 14f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label5.ForeColor = System.Drawing.Color.Black;
		label5.Location = new System.Drawing.Point(454, 19);
		label5.Name = "label5";
		label5.Size = new System.Drawing.Size(86, 24);
		label5.TabIndex = 41;
		label5.Text = "選擇廠商";
		btn_enter.BackColor = System.Drawing.Color.FromArgb(157, 189, 59);
		btn_enter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_enter.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_enter.ForeColor = System.Drawing.Color.White;
		btn_enter.Location = new System.Drawing.Point(371, 171);
		btn_enter.Name = "btn_enter";
		btn_enter.Size = new System.Drawing.Size(113, 35);
		btn_enter.TabIndex = 42;
		btn_enter.TabStop = false;
		btn_enter.Text = "查詢";
		btn_enter.UseVisualStyleBackColor = false;
		btn_enter.Click += new System.EventHandler(btn_enter_Click);
		btn_reset.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_reset.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_reset.ForeColor = System.Drawing.Color.White;
		btn_reset.Location = new System.Drawing.Point(508, 171);
		btn_reset.Name = "btn_reset";
		btn_reset.Size = new System.Drawing.Size(113, 35);
		btn_reset.TabIndex = 43;
		btn_reset.TabStop = false;
		btn_reset.Text = "清除重設";
		btn_reset.UseVisualStyleBackColor = false;
		btn_reset.Click += new System.EventHandler(btn_reset_Click);
		btn_ChooseEnter.BackColor = System.Drawing.Color.FromArgb(157, 189, 59);
		btn_ChooseEnter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_ChooseEnter.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_ChooseEnter.ForeColor = System.Drawing.Color.White;
		btn_ChooseEnter.Location = new System.Drawing.Point(371, 540);
		btn_ChooseEnter.Name = "btn_ChooseEnter";
		btn_ChooseEnter.Size = new System.Drawing.Size(113, 35);
		btn_ChooseEnter.TabIndex = 50;
		btn_ChooseEnter.TabStop = false;
		btn_ChooseEnter.Text = "選入勾選";
		btn_ChooseEnter.UseVisualStyleBackColor = false;
		btn_ChooseEnter.Click += new System.EventHandler(btn_ChooseEnter_Click);
		btn_resetCheck.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_resetCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_resetCheck.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_resetCheck.ForeColor = System.Drawing.Color.White;
		btn_resetCheck.Location = new System.Drawing.Point(508, 540);
		btn_resetCheck.Name = "btn_resetCheck";
		btn_resetCheck.Size = new System.Drawing.Size(113, 35);
		btn_resetCheck.TabIndex = 51;
		btn_resetCheck.TabStop = false;
		btn_resetCheck.Text = "清除重設";
		btn_resetCheck.UseVisualStyleBackColor = false;
		btn_resetCheck.Click += new System.EventHandler(btn_resetCheck_Click);
		l_pageInfo.AutoSize = true;
		l_pageInfo.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		l_pageInfo.Location = new System.Drawing.Point(394, 593);
		l_pageInfo.Name = "l_pageInfo";
		l_pageInfo.Size = new System.Drawing.Size(216, 20);
		l_pageInfo.TabIndex = 60;
		l_pageInfo.Text = "共{0}筆．{1}頁｜目前在第1頁\r\n";
		l_pageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		l_pageInfo.Visible = false;
		btn_cancel.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_cancel.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_cancel.ForeColor = System.Drawing.Color.White;
		btn_cancel.Location = new System.Drawing.Point(771, 540);
		btn_cancel.Name = "btn_cancel";
		btn_cancel.Size = new System.Drawing.Size(124, 34);
		btn_cancel.TabIndex = 63;
		btn_cancel.TabStop = false;
		btn_cancel.Text = "關閉";
		btn_cancel.UseVisualStyleBackColor = false;
		btn_cancel.Click += new System.EventHandler(btn_cancel_Click);
		uC_Member6.AutoSize = true;
		uC_Member6.BackColor = System.Drawing.Color.White;
		uC_Member6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		uC_Member6.Cursor = System.Windows.Forms.Cursors.Hand;
		uC_Member6.Location = new System.Drawing.Point(498, 419);
		uC_Member6.Margin = new System.Windows.Forms.Padding(0);
		uC_Member6.Name = "uC_Member6";
		uC_Member6.Size = new System.Drawing.Size(398, 102);
		uC_Member6.TabIndex = 49;
		uC_Member6.Visible = false;
		uC_Member5.AutoSize = true;
		uC_Member5.BackColor = System.Drawing.Color.White;
		uC_Member5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		uC_Member5.Cursor = System.Windows.Forms.Cursors.Hand;
		uC_Member5.Location = new System.Drawing.Point(100, 419);
		uC_Member5.Margin = new System.Windows.Forms.Padding(0);
		uC_Member5.Name = "uC_Member5";
		uC_Member5.Size = new System.Drawing.Size(398, 102);
		uC_Member5.TabIndex = 48;
		uC_Member5.Visible = false;
		btn_pageRight.FlatAppearance.BorderSize = 0;
		btn_pageRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_pageRight.Image = POS_Client.Properties.Resources.right;
		btn_pageRight.Location = new System.Drawing.Point(924, 217);
		btn_pageRight.Name = "btn_pageRight";
		btn_pageRight.Size = new System.Drawing.Size(48, 306);
		btn_pageRight.TabIndex = 62;
		btn_pageRight.UseVisualStyleBackColor = true;
		btn_pageRight.Visible = false;
		btn_pageLeft.FlatAppearance.BorderSize = 0;
		btn_pageLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_pageLeft.Image = POS_Client.Properties.Resources.left;
		btn_pageLeft.Location = new System.Drawing.Point(24, 217);
		btn_pageLeft.Name = "btn_pageLeft";
		btn_pageLeft.Size = new System.Drawing.Size(48, 306);
		btn_pageLeft.TabIndex = 61;
		btn_pageLeft.UseVisualStyleBackColor = true;
		btn_pageLeft.Visible = false;
		uC_Member3.AutoSize = true;
		uC_Member3.BackColor = System.Drawing.Color.White;
		uC_Member3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		uC_Member3.Cursor = System.Windows.Forms.Cursors.Hand;
		uC_Member3.Location = new System.Drawing.Point(100, 319);
		uC_Member3.Margin = new System.Windows.Forms.Padding(0);
		uC_Member3.Name = "uC_Member3";
		uC_Member3.Size = new System.Drawing.Size(398, 102);
		uC_Member3.TabIndex = 46;
		uC_Member3.Visible = false;
		uC_Member4.AutoSize = true;
		uC_Member4.BackColor = System.Drawing.Color.White;
		uC_Member4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		uC_Member4.Cursor = System.Windows.Forms.Cursors.Hand;
		uC_Member4.Location = new System.Drawing.Point(498, 319);
		uC_Member4.Margin = new System.Windows.Forms.Padding(0);
		uC_Member4.Name = "uC_Member4";
		uC_Member4.Size = new System.Drawing.Size(398, 102);
		uC_Member4.TabIndex = 47;
		uC_Member4.Visible = false;
		uC_Member2.AutoSize = true;
		uC_Member2.BackColor = System.Drawing.Color.White;
		uC_Member2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		uC_Member2.Cursor = System.Windows.Forms.Cursors.Hand;
		uC_Member2.Location = new System.Drawing.Point(498, 217);
		uC_Member2.Margin = new System.Windows.Forms.Padding(0);
		uC_Member2.Name = "uC_Member2";
		uC_Member2.Size = new System.Drawing.Size(398, 102);
		uC_Member2.TabIndex = 45;
		uC_Member2.Visible = false;
		uC_Member1.AutoSize = true;
		uC_Member1.BackColor = System.Drawing.Color.White;
		uC_Member1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		uC_Member1.Cursor = System.Windows.Forms.Cursors.Hand;
		uC_Member1.Location = new System.Drawing.Point(100, 217);
		uC_Member1.Margin = new System.Windows.Forms.Padding(0);
		uC_Member1.Name = "uC_Member1";
		uC_Member1.Size = new System.Drawing.Size(398, 102);
		uC_Member1.TabIndex = 44;
		uC_Member1.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		AutoScroll = true;
		BackColor = System.Drawing.Color.Silver;
		base.ClientSize = new System.Drawing.Size(997, 631);
		base.ControlBox = false;
		base.Controls.Add(btn_cancel);
		base.Controls.Add(btn_pageRight);
		base.Controls.Add(btn_pageLeft);
		base.Controls.Add(l_pageInfo);
		base.Controls.Add(btn_resetCheck);
		base.Controls.Add(btn_ChooseEnter);
		base.Controls.Add(uC_Member6);
		base.Controls.Add(uC_Member5);
		base.Controls.Add(uC_Member4);
		base.Controls.Add(uC_Member3);
		base.Controls.Add(uC_Member2);
		base.Controls.Add(uC_Member1);
		base.Controls.Add(btn_reset);
		base.Controls.Add(btn_enter);
		base.Controls.Add(label5);
		base.Controls.Add(tableLayoutPanel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "dialogChooseVendor";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "農委會防檢局POS系統";
		tableLayoutPanel1.ResumeLayout(false);
		tableLayoutPanel1.PerformLayout();
		panel2.ResumeLayout(false);
		panel2.PerformLayout();
		panel1.ResumeLayout(false);
		panel1.PerformLayout();
		panel3.ResumeLayout(false);
		panel3.PerformLayout();
		panel4.ResumeLayout(false);
		panel4.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}
}
