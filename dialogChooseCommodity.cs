using POS_Client;
using POS_Client.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogChooseCommodity : Form
{
	private ucCommodityInfo[] ucCommoditys;

	private int pageNow = 1;

	public int pageTotal = 1;

	public DataTable dt;

	private string status = "";

	private string type = "";

	private string fromTable = "";

	private string selectString = "hg.GDSNO,hg.barcode,hg.CLA1NO,hg.GDNAME,hg.Price,hg.spec,hg.contents,hg.capacity,hg.brandName,hg.CName,hg.formCode,hg.status";

	private string orderByString = "hg.GDSNO,hg.barcode,hg.CreateDate,hg.GDName";

	private List<string> lst_commoditylist = new List<string>();

	private IContainer components;

	private Button btn_cancel;

	private Button btn_pageRight;

	private Button btn_pageLeft;

	private Label l_pageInfo;

	private Button btn_resetCheck;

	private Button btn_ChooseEnter;

	private Button btn_reset;

	private Button btn_enter;

	private Label label5;

	private TextBox tb_GDNAME;

	private Label label4;

	private Panel panel4;

	private Label label2;

	private TextBox tb_domManufName;

	private Panel panel2;

	private TableLayoutPanel tableLayoutPanel1;

	private TextBox tb_barcode;

	private Label label1;

	private Panel panel1;

	private TableLayoutPanel tableLayoutPanel2;

	private ucCommodityInfo uC_Commodity1;

	private ucCommodityInfo uC_Commodity2;

	private ucCommodityInfo uC_Commodity3;

	private ucCommodityInfo uC_Commodity4;

	private ucCommodityInfo uC_Commodity5;

	private ucCommodityInfo uC_Commodity6;

	private ucCommodityInfo uC_Commodity7;

	private ucCommodityInfo uC_Commodity8;

	public dialogChooseCommodity(List<string> lst)
	{
		lst_commoditylist.Clear();
		lst_commoditylist = lst;
		InitializeComponent();
		tb_barcode.Select();
		ucCommoditys = new ucCommodityInfo[8]
		{
			uC_Commodity1,
			uC_Commodity2,
			uC_Commodity3,
			uC_Commodity4,
			uC_Commodity5,
			uC_Commodity6,
			uC_Commodity7,
			uC_Commodity8
		};
		ucCommodityInfo[] array = ucCommoditys;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnClickCommodity += CommodityChecked;
		}
		fromTable = "hypos_GOODSLST hg, HyLicence as hl";
		type = "hg.CLA1NO ='0302' and hg.ISWS ='Y' and hg.licType = hl.licType and hg.domManufId = hl.licNo";
		if (!hasSelectedCommodity())
		{
			btn_pageLeft.Visible = true;
			btn_pageRight.Visible = true;
			status = "hg.status='U'";
			string strWhereClause = "".Equals(type) ? status : (type + " and " + status);
			dt = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, selectString, fromTable, strWhereClause, orderByString, null, null, CommandOperationType.ExecuteReaderReturnDataTable);
			changePage(1);
		}
	}

	private bool hasSelectedCommodity()
	{
		ucCommodityInfo[] array = ucCommoditys;
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

	public void CommodityChecked(object CommodityName, EventArgs s)
	{
	}

	public void changePage(int page)
	{
		int num = 0;
		pageNow = page;
		for (int i = (pageNow - 1) * 8; i < pageNow * 8; i++)
		{
			if (i < dt.Rows.Count)
			{
				ucCommoditys[num].setGDSNO(dt.Rows[i]["GDSNO"].ToString());
				ucCommoditys[num].setBarcode(dt.Rows[i]["barcode"].ToString());
				ucCommoditys[num].setPrice(dt.Rows[i]["Price"].ToString());
				ucCommoditys[num].setCommodityName(setCommodityName(dt.Rows[i]));
				if (dt.Rows[num]["CLA1NO"].ToString() == "0302")
				{
					ucCommoditys[num].setCommodityClass(string.Concat("農藥: " + dt.Rows[i]["spec"].ToString(), dt.Rows[i]["contents"].ToString(), dt.Rows[i]["capacity"].ToString()));
				}
				else if (dt.Rows[num]["CLA1NO"].ToString() == "0303")
				{
					ucCommoditys[num].setCommodityClass(string.Concat("肥料: " + dt.Rows[i]["spec"].ToString(), dt.Rows[i]["contents"].ToString(), dt.Rows[i]["capacity"].ToString()));
				}
				else if (dt.Rows[num]["CLA1NO"].ToString() == "0305")
				{
					ucCommoditys[num].setCommodityClass(string.Concat("資材: " + dt.Rows[i]["spec"].ToString(), dt.Rows[i]["contents"].ToString(), dt.Rows[i]["capacity"].ToString()));
				}
				else if (dt.Rows[i]["CLA1NO"].ToString() == "0308")
				{
					ucCommoditys[num].setCommodityClass(string.Concat("其他: " + dt.Rows[i]["spec"].ToString(), dt.Rows[i]["contents"].ToString(), dt.Rows[i]["capacity"].ToString()));
				}
				else
				{
					ucCommoditys[num].setCommodityClass(dt.Rows[i]["spec"].ToString() + dt.Rows[i]["contents"].ToString() + dt.Rows[i]["capacity"].ToString());
				}
				ucCommoditys[num].Visible = true;
			}
			else
			{
				ucCommoditys[num].Visible = false;
			}
			ucCommoditys[num].checkCommodity(check: false);
			ucCommoditys[num].BackColor = Color.White;
			num++;
		}
		pageTotal = (int)Math.Ceiling((double)dt.Rows.Count / 8.0);
		l_pageInfo.Text = $"共{dt.Rows.Count}筆．{Math.Ceiling((double)dt.Rows.Count / 8.0)}頁｜目前在第{pageNow}頁";
	}

	private string setCommodityName(DataRow row)
	{
		string text = row["GDName"].ToString();
		string text2 = row["formCode"].ToString();
		string text3 = row["CName"].ToString();
		string text4 = row["contents"].ToString();
		string text5 = row["brandName"].ToString();
		string[] array = new string[2]
		{
			text3,
			text2
		};
		string[] array2 = new string[2]
		{
			text4,
			text5
		};
		if (!string.IsNullOrEmpty(text2) || !string.IsNullOrEmpty(text3) || !string.IsNullOrEmpty(text4) || !string.IsNullOrEmpty(text5))
		{
			text += "[";
			for (int i = 0; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]))
				{
					text = text + array[i] + "-";
				}
			}
			if (text.LastIndexOf("-") > 0)
			{
				text = text.Substring(0, text.LastIndexOf("-")) + " . ";
			}
			for (int j = 0; j < array2.Length; j++)
			{
				if (!string.IsNullOrEmpty(array2[j]))
				{
					text = text + array2[j] + "-";
				}
			}
			text = ((text.LastIndexOf("-") <= 0) ? (text + "]") : (text.Substring(0, text.LastIndexOf("-")) + "]"));
		}
		return text;
	}

	private void btn_cancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btn_ChooseEnter_Click(object sender, EventArgs e)
	{
		string text = "";
		ucCommodityInfo[] array = ucCommoditys;
		foreach (ucCommodityInfo ucCommodityInfo in array)
		{
			if (ucCommodityInfo.isChecked())
			{
				text = ucCommodityInfo.getGDSNO() + "," + ucCommodityInfo.getCommodityName();
				lst_commoditylist.Add(text);
			}
		}
		Close();
	}

	private void btn_enter_Click(object sender, EventArgs e)
	{
		if (tb_barcode.Text == "請輸入商品條碼" && tb_GDNAME.Text == "請輸入商品名稱" && tb_domManufName.Text == "請輸入廠商名稱")
		{
			AutoClosingMessageBox.Show("必須輸入查詢條件");
			return;
		}
		fromTable = "hypos_GOODSLST hg, HyLicence as hl";
		type = "";
		if (!hasSelectedCommodity())
		{
			btn_pageLeft.Visible = true;
			btn_pageRight.Visible = true;
			status = "hg.status='U'";
			string text = "".Equals(type) ? status : (type + " and " + status);
			if (!(tb_barcode.Text == "請輸入商品條碼") && !tb_barcode.Text.Equals(""))
			{
				text = text + " and hg.barcode = '" + tb_barcode.Text + "'";
			}
			if (!(tb_GDNAME.Text == "請輸入商品名稱") && !tb_GDNAME.Text.Equals(""))
			{
				text = text + " and hg.GDNAME LIKE '%" + tb_GDNAME.Text + "%'";
			}
			if (!(tb_domManufName.Text == "請輸入廠商名稱") && !tb_domManufName.Text.Equals(""))
			{
				text = text + " and hl.domManufName LIKE '%" + tb_domManufName.Text + "%'";
			}
			dt = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, selectString, fromTable, text, orderByString, null, null, CommandOperationType.ExecuteReaderReturnDataTable);
			changePage(1);
		}
	}

	private void btn_pageLeft_Click(object sender, EventArgs e)
	{
		if (!hasSelectedCommodity() && pageNow > 1)
		{
			changePage(pageNow - 1);
		}
	}

	private void btn_pageRight_Click(object sender, EventArgs e)
	{
		if (!hasSelectedCommodity() && pageNow < pageTotal)
		{
			changePage(pageNow + 1);
		}
	}

	private void btn_reset_Click(object sender, EventArgs e)
	{
		tb_barcode.Text = "請輸入商品條碼";
		tb_GDNAME.Text = "請輸入商品名稱";
		tb_domManufName.Text = "請輸入廠商名稱";
	}

	private void btn_resetCheck_Click(object sender, EventArgs e)
	{
		ucCommodityInfo[] array = ucCommoditys;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].checkCommodity(check: false);
		}
	}

	private void tb_barcode_Enter(object sender, EventArgs e)
	{
		if (tb_barcode.Text == "請輸入商品條碼")
		{
			tb_barcode.Text = "";
		}
	}

	private void tb_barcode_Leave(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(tb_barcode.Text))
		{
			tb_barcode.Text = "請輸入商品條碼";
		}
	}

	private void tb_GDNAME_Enter(object sender, EventArgs e)
	{
		if (tb_GDNAME.Text == "請輸入商品名稱")
		{
			tb_GDNAME.Text = "";
		}
	}

	private void tb_GDNAME_Leave(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(tb_GDNAME.Text))
		{
			tb_GDNAME.Text = "請輸入商品名稱";
		}
	}

	private void tb_domManufName_Enter(object sender, EventArgs e)
	{
		if (tb_domManufName.Text == "請輸入廠商名稱")
		{
			tb_domManufName.Text = "";
		}
	}

	private void tb_domManufName_Leave(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(tb_domManufName.Text))
		{
			tb_domManufName.Text = "請輸入廠商名稱";
		}
	}

	private void tb_barcode_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = (!char.IsDigit(e.KeyChar) && !e.KeyChar.Equals('\b') && !char.IsLetter(e.KeyChar));
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
		btn_pageRight = new System.Windows.Forms.Button();
		btn_pageLeft = new System.Windows.Forms.Button();
		l_pageInfo = new System.Windows.Forms.Label();
		btn_resetCheck = new System.Windows.Forms.Button();
		btn_ChooseEnter = new System.Windows.Forms.Button();
		btn_reset = new System.Windows.Forms.Button();
		btn_enter = new System.Windows.Forms.Button();
		label5 = new System.Windows.Forms.Label();
		tb_GDNAME = new System.Windows.Forms.TextBox();
		label4 = new System.Windows.Forms.Label();
		panel4 = new System.Windows.Forms.Panel();
		label2 = new System.Windows.Forms.Label();
		tb_domManufName = new System.Windows.Forms.TextBox();
		panel2 = new System.Windows.Forms.Panel();
		tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		tb_barcode = new System.Windows.Forms.TextBox();
		label1 = new System.Windows.Forms.Label();
		panel1 = new System.Windows.Forms.Panel();
		tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		uC_Commodity8 = new POS_Client.ucCommodityInfo();
		uC_Commodity7 = new POS_Client.ucCommodityInfo();
		uC_Commodity6 = new POS_Client.ucCommodityInfo();
		uC_Commodity5 = new POS_Client.ucCommodityInfo();
		uC_Commodity4 = new POS_Client.ucCommodityInfo();
		uC_Commodity3 = new POS_Client.ucCommodityInfo();
		uC_Commodity2 = new POS_Client.ucCommodityInfo();
		uC_Commodity1 = new POS_Client.ucCommodityInfo();
		panel4.SuspendLayout();
		panel2.SuspendLayout();
		tableLayoutPanel1.SuspendLayout();
		panel1.SuspendLayout();
		tableLayoutPanel2.SuspendLayout();
		SuspendLayout();
		btn_cancel.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_cancel.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_cancel.ForeColor = System.Drawing.Color.White;
		btn_cancel.Location = new System.Drawing.Point(725, 622);
		btn_cancel.Name = "btn_cancel";
		btn_cancel.Size = new System.Drawing.Size(124, 34);
		btn_cancel.TabIndex = 79;
		btn_cancel.TabStop = false;
		btn_cancel.Text = "關閉";
		btn_cancel.UseVisualStyleBackColor = false;
		btn_cancel.Click += new System.EventHandler(btn_cancel_Click);
		btn_pageRight.FlatAppearance.BorderSize = 0;
		btn_pageRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_pageRight.Image = POS_Client.Properties.Resources.right;
		btn_pageRight.Location = new System.Drawing.Point(926, 212);
		btn_pageRight.Name = "btn_pageRight";
		btn_pageRight.Size = new System.Drawing.Size(48, 306);
		btn_pageRight.TabIndex = 78;
		btn_pageRight.UseVisualStyleBackColor = true;
		btn_pageRight.Visible = false;
		btn_pageRight.Click += new System.EventHandler(btn_pageRight_Click);
		btn_pageLeft.FlatAppearance.BorderSize = 0;
		btn_pageLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_pageLeft.Image = POS_Client.Properties.Resources.left;
		btn_pageLeft.Location = new System.Drawing.Point(20, 212);
		btn_pageLeft.Name = "btn_pageLeft";
		btn_pageLeft.Size = new System.Drawing.Size(48, 306);
		btn_pageLeft.TabIndex = 77;
		btn_pageLeft.UseVisualStyleBackColor = true;
		btn_pageLeft.Visible = false;
		btn_pageLeft.Click += new System.EventHandler(btn_pageLeft_Click);
		l_pageInfo.AutoSize = true;
		l_pageInfo.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		l_pageInfo.Location = new System.Drawing.Point(395, 670);
		l_pageInfo.Name = "l_pageInfo";
		l_pageInfo.Size = new System.Drawing.Size(216, 20);
		l_pageInfo.TabIndex = 76;
		l_pageInfo.Text = "共{0}筆．{1}頁｜目前在第1頁\r\n";
		l_pageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		l_pageInfo.Visible = false;
		btn_resetCheck.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_resetCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_resetCheck.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_resetCheck.ForeColor = System.Drawing.Color.White;
		btn_resetCheck.Location = new System.Drawing.Point(509, 623);
		btn_resetCheck.Name = "btn_resetCheck";
		btn_resetCheck.Size = new System.Drawing.Size(113, 35);
		btn_resetCheck.TabIndex = 75;
		btn_resetCheck.TabStop = false;
		btn_resetCheck.Text = "清除重設";
		btn_resetCheck.UseVisualStyleBackColor = false;
		btn_resetCheck.Click += new System.EventHandler(btn_resetCheck_Click);
		btn_ChooseEnter.BackColor = System.Drawing.Color.FromArgb(157, 189, 59);
		btn_ChooseEnter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_ChooseEnter.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_ChooseEnter.ForeColor = System.Drawing.Color.White;
		btn_ChooseEnter.Location = new System.Drawing.Point(372, 623);
		btn_ChooseEnter.Name = "btn_ChooseEnter";
		btn_ChooseEnter.Size = new System.Drawing.Size(113, 35);
		btn_ChooseEnter.TabIndex = 74;
		btn_ChooseEnter.TabStop = false;
		btn_ChooseEnter.Text = "選入勾選";
		btn_ChooseEnter.UseVisualStyleBackColor = false;
		btn_ChooseEnter.Click += new System.EventHandler(btn_ChooseEnter_Click);
		btn_reset.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_reset.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_reset.ForeColor = System.Drawing.Color.White;
		btn_reset.Location = new System.Drawing.Point(510, 161);
		btn_reset.Name = "btn_reset";
		btn_reset.Size = new System.Drawing.Size(113, 35);
		btn_reset.TabIndex = 67;
		btn_reset.TabStop = false;
		btn_reset.Text = "清除重設";
		btn_reset.UseVisualStyleBackColor = false;
		btn_reset.Click += new System.EventHandler(btn_reset_Click);
		btn_enter.BackColor = System.Drawing.Color.FromArgb(157, 189, 59);
		btn_enter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_enter.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_enter.ForeColor = System.Drawing.Color.White;
		btn_enter.Location = new System.Drawing.Point(373, 161);
		btn_enter.Name = "btn_enter";
		btn_enter.Size = new System.Drawing.Size(113, 35);
		btn_enter.TabIndex = 66;
		btn_enter.TabStop = false;
		btn_enter.Text = "查詢";
		btn_enter.UseVisualStyleBackColor = false;
		btn_enter.Click += new System.EventHandler(btn_enter_Click);
		label5.AutoSize = true;
		label5.Font = new System.Drawing.Font("微軟正黑體", 14f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label5.ForeColor = System.Drawing.Color.Black;
		label5.Location = new System.Drawing.Point(456, 14);
		label5.Name = "label5";
		label5.Size = new System.Drawing.Size(86, 24);
		label5.TabIndex = 65;
		label5.Text = "選擇商品";
		tb_GDNAME.BackColor = System.Drawing.Color.White;
		tb_GDNAME.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_GDNAME.ForeColor = System.Drawing.Color.DarkGray;
		tb_GDNAME.Location = new System.Drawing.Point(170, 11);
		tb_GDNAME.Margin = new System.Windows.Forms.Padding(10);
		tb_GDNAME.MaxLength = 100;
		tb_GDNAME.Name = "tb_GDNAME";
		tb_GDNAME.Size = new System.Drawing.Size(217, 29);
		tb_GDNAME.TabIndex = 25;
		tb_GDNAME.Text = "請輸入商品名稱";
		tb_GDNAME.Enter += new System.EventHandler(tb_GDNAME_Enter);
		tb_GDNAME.Leave += new System.EventHandler(tb_GDNAME_Leave);
		label4.AutoSize = true;
		label4.BackColor = System.Drawing.Color.Transparent;
		label4.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label4.ForeColor = System.Drawing.Color.White;
		label4.Location = new System.Drawing.Point(71, 16);
		label4.Name = "label4";
		label4.Size = new System.Drawing.Size(74, 21);
		label4.TabIndex = 44;
		label4.Text = "廠商名稱";
		panel4.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel4.Controls.Add(label4);
		panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		panel4.Location = new System.Drawing.Point(398, 1);
		panel4.Margin = new System.Windows.Forms.Padding(0);
		panel4.Name = "panel4";
		panel4.Size = new System.Drawing.Size(158, 50);
		panel4.TabIndex = 42;
		label2.AutoSize = true;
		label2.BackColor = System.Drawing.Color.Transparent;
		label2.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label2.ForeColor = System.Drawing.Color.White;
		label2.Location = new System.Drawing.Point(69, 13);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(74, 21);
		label2.TabIndex = 0;
		label2.Text = "商品名稱";
		tb_domManufName.BackColor = System.Drawing.Color.White;
		tb_domManufName.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_domManufName.ForeColor = System.Drawing.Color.DarkGray;
		tb_domManufName.Location = new System.Drawing.Point(567, 11);
		tb_domManufName.Margin = new System.Windows.Forms.Padding(10);
		tb_domManufName.MaxLength = 100;
		tb_domManufName.Name = "tb_domManufName";
		tb_domManufName.Size = new System.Drawing.Size(217, 29);
		tb_domManufName.TabIndex = 44;
		tb_domManufName.Text = "請輸入廠商名稱";
		tb_domManufName.Enter += new System.EventHandler(tb_domManufName_Enter);
		tb_domManufName.Leave += new System.EventHandler(tb_domManufName_Leave);
		panel2.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel2.Controls.Add(label2);
		panel2.Location = new System.Drawing.Point(1, 1);
		panel2.Margin = new System.Windows.Forms.Padding(0);
		panel2.Name = "panel2";
		panel2.Size = new System.Drawing.Size(158, 50);
		panel2.TabIndex = 23;
		tableLayoutPanel1.BackColor = System.Drawing.Color.White;
		tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		tableLayoutPanel1.ColumnCount = 4;
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30f));
		tableLayoutPanel1.Controls.Add(tb_domManufName, 3, 0);
		tableLayoutPanel1.Controls.Add(panel2, 0, 0);
		tableLayoutPanel1.Controls.Add(panel4, 2, 0);
		tableLayoutPanel1.Controls.Add(tb_GDNAME, 1, 0);
		tableLayoutPanel1.Location = new System.Drawing.Point(102, 99);
		tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
		tableLayoutPanel1.Name = "tableLayoutPanel1";
		tableLayoutPanel1.RowCount = 1;
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51f));
		tableLayoutPanel1.Size = new System.Drawing.Size(796, 52);
		tableLayoutPanel1.TabIndex = 64;
		tb_barcode.BackColor = System.Drawing.Color.White;
		tb_barcode.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_barcode.ForeColor = System.Drawing.Color.DarkGray;
		tb_barcode.ImeMode = System.Windows.Forms.ImeMode.Disable;
		tb_barcode.Location = new System.Drawing.Point(170, 11);
		tb_barcode.Margin = new System.Windows.Forms.Padding(10);
		tb_barcode.MaxLength = 100;
		tb_barcode.Name = "tb_barcode";
		tb_barcode.Size = new System.Drawing.Size(613, 29);
		tb_barcode.TabIndex = 81;
		tb_barcode.Text = "請輸入商品條碼";
		tb_barcode.Enter += new System.EventHandler(tb_barcode_Enter);
		tb_barcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(tb_barcode_KeyPress);
		tb_barcode.Leave += new System.EventHandler(tb_barcode_Leave);
		label1.AutoSize = true;
		label1.BackColor = System.Drawing.Color.Transparent;
		label1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.Color.White;
		label1.Location = new System.Drawing.Point(69, 15);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(74, 21);
		label1.TabIndex = 0;
		label1.Text = "商品條碼";
		panel1.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel1.Controls.Add(label1);
		panel1.Location = new System.Drawing.Point(1, 1);
		panel1.Margin = new System.Windows.Forms.Padding(0);
		panel1.Name = "panel1";
		panel1.Size = new System.Drawing.Size(158, 50);
		panel1.TabIndex = 80;
		tableLayoutPanel2.BackColor = System.Drawing.Color.White;
		tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		tableLayoutPanel2.ColumnCount = 2;
		tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80f));
		tableLayoutPanel2.Controls.Add(panel1, 0, 0);
		tableLayoutPanel2.Controls.Add(tb_barcode, 1, 0);
		tableLayoutPanel2.Location = new System.Drawing.Point(102, 48);
		tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
		tableLayoutPanel2.Name = "tableLayoutPanel2";
		tableLayoutPanel2.RowCount = 1;
		tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		tableLayoutPanel2.Size = new System.Drawing.Size(796, 52);
		tableLayoutPanel2.TabIndex = 82;
		uC_Commodity8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		uC_Commodity8.Cursor = System.Windows.Forms.Cursors.Hand;
		uC_Commodity8.Location = new System.Drawing.Point(498, 508);
		uC_Commodity8.MaximumSize = new System.Drawing.Size(398, 102);
		uC_Commodity8.MinimumSize = new System.Drawing.Size(398, 102);
		uC_Commodity8.Name = "uC_Commodity8";
		uC_Commodity8.Size = new System.Drawing.Size(398, 102);
		uC_Commodity8.TabIndex = 94;
		uC_Commodity7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		uC_Commodity7.Cursor = System.Windows.Forms.Cursors.Hand;
		uC_Commodity7.Location = new System.Drawing.Point(100, 508);
		uC_Commodity7.MaximumSize = new System.Drawing.Size(398, 102);
		uC_Commodity7.MinimumSize = new System.Drawing.Size(398, 102);
		uC_Commodity7.Name = "uC_Commodity7";
		uC_Commodity7.Size = new System.Drawing.Size(398, 102);
		uC_Commodity7.TabIndex = 93;
		uC_Commodity6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		uC_Commodity6.Cursor = System.Windows.Forms.Cursors.Hand;
		uC_Commodity6.Location = new System.Drawing.Point(498, 407);
		uC_Commodity6.MaximumSize = new System.Drawing.Size(398, 102);
		uC_Commodity6.MinimumSize = new System.Drawing.Size(398, 102);
		uC_Commodity6.Name = "uC_Commodity6";
		uC_Commodity6.Size = new System.Drawing.Size(398, 102);
		uC_Commodity6.TabIndex = 92;
		uC_Commodity6.Visible = false;
		uC_Commodity5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		uC_Commodity5.Cursor = System.Windows.Forms.Cursors.Hand;
		uC_Commodity5.Location = new System.Drawing.Point(100, 407);
		uC_Commodity5.MaximumSize = new System.Drawing.Size(398, 102);
		uC_Commodity5.MinimumSize = new System.Drawing.Size(398, 102);
		uC_Commodity5.Name = "uC_Commodity5";
		uC_Commodity5.Size = new System.Drawing.Size(398, 102);
		uC_Commodity5.TabIndex = 91;
		uC_Commodity5.Visible = false;
		uC_Commodity4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		uC_Commodity4.Cursor = System.Windows.Forms.Cursors.Hand;
		uC_Commodity4.Location = new System.Drawing.Point(498, 306);
		uC_Commodity4.MaximumSize = new System.Drawing.Size(398, 102);
		uC_Commodity4.MinimumSize = new System.Drawing.Size(398, 102);
		uC_Commodity4.Name = "uC_Commodity4";
		uC_Commodity4.Size = new System.Drawing.Size(398, 102);
		uC_Commodity4.TabIndex = 90;
		uC_Commodity4.Visible = false;
		uC_Commodity3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		uC_Commodity3.Cursor = System.Windows.Forms.Cursors.Hand;
		uC_Commodity3.Location = new System.Drawing.Point(100, 306);
		uC_Commodity3.MaximumSize = new System.Drawing.Size(398, 102);
		uC_Commodity3.MinimumSize = new System.Drawing.Size(398, 102);
		uC_Commodity3.Name = "uC_Commodity3";
		uC_Commodity3.Size = new System.Drawing.Size(398, 102);
		uC_Commodity3.TabIndex = 89;
		uC_Commodity3.Visible = false;
		uC_Commodity2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		uC_Commodity2.Cursor = System.Windows.Forms.Cursors.Hand;
		uC_Commodity2.Location = new System.Drawing.Point(498, 205);
		uC_Commodity2.MaximumSize = new System.Drawing.Size(398, 102);
		uC_Commodity2.MinimumSize = new System.Drawing.Size(398, 102);
		uC_Commodity2.Name = "uC_Commodity2";
		uC_Commodity2.Size = new System.Drawing.Size(398, 102);
		uC_Commodity2.TabIndex = 88;
		uC_Commodity2.Visible = false;
		uC_Commodity1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		uC_Commodity1.Cursor = System.Windows.Forms.Cursors.Hand;
		uC_Commodity1.Location = new System.Drawing.Point(100, 205);
		uC_Commodity1.MaximumSize = new System.Drawing.Size(398, 102);
		uC_Commodity1.MinimumSize = new System.Drawing.Size(398, 102);
		uC_Commodity1.Name = "uC_Commodity1";
		uC_Commodity1.Size = new System.Drawing.Size(398, 102);
		uC_Commodity1.TabIndex = 87;
		uC_Commodity1.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		AutoScroll = true;
		BackColor = System.Drawing.Color.Silver;
		base.ClientSize = new System.Drawing.Size(997, 700);
		base.ControlBox = false;
		base.Controls.Add(uC_Commodity8);
		base.Controls.Add(uC_Commodity7);
		base.Controls.Add(uC_Commodity6);
		base.Controls.Add(uC_Commodity5);
		base.Controls.Add(uC_Commodity4);
		base.Controls.Add(uC_Commodity3);
		base.Controls.Add(uC_Commodity2);
		base.Controls.Add(uC_Commodity1);
		base.Controls.Add(tableLayoutPanel2);
		base.Controls.Add(btn_cancel);
		base.Controls.Add(btn_pageRight);
		base.Controls.Add(btn_pageLeft);
		base.Controls.Add(l_pageInfo);
		base.Controls.Add(btn_resetCheck);
		base.Controls.Add(btn_ChooseEnter);
		base.Controls.Add(btn_reset);
		base.Controls.Add(btn_enter);
		base.Controls.Add(label5);
		base.Controls.Add(tableLayoutPanel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "dialogChooseCommodity";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "農委會防檢局POS系統";
		panel4.ResumeLayout(false);
		panel4.PerformLayout();
		panel2.ResumeLayout(false);
		panel2.PerformLayout();
		tableLayoutPanel1.ResumeLayout(false);
		tableLayoutPanel1.PerformLayout();
		panel1.ResumeLayout(false);
		panel1.PerformLayout();
		tableLayoutPanel2.ResumeLayout(false);
		tableLayoutPanel2.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}
}
