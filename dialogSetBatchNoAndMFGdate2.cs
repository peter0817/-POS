using KeyboardClassLibrary;
using POS_Client;
using POS_Client.Properties;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogSetBatchNoAndMFGdate2 : Form
{
	private string _GDSNO;

	private string _LicenseCode;

	private DataTable dt;

	private frmNewDeliveryOrder _fndo;

	public CommodityInfo uc;

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

	private DataGridView infolist;

	private Label label1;

	private DateTimePicker dateTimePicker0;

	private Label l_GDName;

	private Label label2;

	private frmMainShopSimple.CustomColumn DeliveryInfo;

	private DataGridViewTextBoxColumn quantity;

	private DataGridViewButtonColumn select;

	private Label label3;

	public dialogSetBatchNoAndMFGdate2(frmNewDeliveryOrder fndo, string GDSNO)
	{
		InitializeComponent();
		_GDSNO = GDSNO;
		_fndo = fndo;
	}

	private void dialogSetBatchNoAndMFGdate2_Load(object sender, EventArgs e)
	{
		try
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
			string sql = "select p.POSBatchNo, p.BatchNo, p.MFGDate, p.barcode, p.num, p.PurchaseNo, (select b.backlogQuantity from hypos_BatchNo_log as b where p.POSBatchNo = b.POSBatchNo order by b.createDate desc limit 1) as backlogQuantity from hypos_PurchaseGoodsBatchNo_log as p where p.barcode = {0} and  backlogQuantity <> 0 and  p.BatchNo <> ''and p.PurchaseNo in (select PurchaseNo from hypos_PurchaseGoods_Master)";
			DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, new string[1]
			{
				_GDSNO
			}, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					string text = DataBaseUtilities.DBOperation(Program.ConnectionString, "SELECT SupplierNo FROM hypos_PurchaseGoods_Master where PurchaseNo = {0}", new string[1]
					{
						row["PurchaseNo"].ToString()
					}, CommandOperationType.ExecuteScalar).ToString();
					string str = DataBaseUtilities.DBOperation(Program.ConnectionString, "SELECT SupplierName FROM hypos_Supplier where SupplierNo = {0}", new string[1]
					{
						text
					}, CommandOperationType.ExecuteScalar).ToString();
					string text2 = string.IsNullOrEmpty(row["backlogQuantity"].ToString()) ? "0" : row["backlogQuantity"].ToString();
					uc = new CommodityInfo();
					uc.setMemberIdNo("");
					uc.setHiddenGDSNO(_GDSNO);
					uc.setMemberVipNo("進貨單號:" + row["PurchaseNo"].ToString());
					uc.setCommodityName("進貨廠商:" + str);
					uc.setCommodityClass("批號:" + row["BatchNo"].ToString().PadRight(20, ' ') + " 製造日期:" + row["MFGDate"].ToString());
					uc.setHiddenBatchNo(row["BatchNo"].ToString());
					uc.setHiddenMFGDate(row["MFGDate"].ToString());
					uc.setHiddenPOSBatchNo(row["POSBatchNo"].ToString());
					uc.setlabe1("");
					infolist.Rows.Add(uc, text2, "選擇");
					infolist.CurrentCell = infolist.Rows[infolist.RowCount - 1].Cells[0];
				}
				foreach (DataGridViewRow item in (IEnumerable)infolist.Rows)
				{
					item.Height = 100;
				}
			}
			else
			{
				AutoClosingMessageBox.Show("目前沒有任何此商品的進貨批號設定紀錄");
			}
		}
		catch (Exception)
		{
		}
		tb_BatchNo.Select();
	}

	private void btn_back_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		AutoClosingMessageBox.Show("不設定批號或製造日期無法選入商品");
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
		if (dt.Rows.Count > 0 && dt.Rows[0]["CLA1NO"].ToString().Equals("0302") && dt.Rows[0]["ISWS"].ToString().Equals("Y"))
		{
			if (tb_BatchNo.Text.Trim().Equals(""))
			{
				text = "管制農藥商品出貨請輸入批號或製造日期";
			}
			if (!dateTimePicker0.Checked)
			{
				text = "管制農藥商品出貨請輸入批號或製造日期";
			}
		}
		if (!text.Equals(""))
		{
			AutoClosingMessageBox.Show(text);
			return;
		}
		string text2 = "N";
		string text3 = "";
		if (dateTimePicker0.Checked)
		{
			text3 = dateTimePicker0.Value.ToString("yyyy-MM-dd");
		}
		string[] data = new string[4]
		{
			tb_BatchNo.Text,
			text3,
			"",
			text2
		};
		_fndo.infolistInfoSetting(data);
		base.DialogResult = DialogResult.Yes;
		AutoClosingMessageBox.Show("設定完成，商品已選入");
		Close();
	}

	private void infolist_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		DataGridView dataGridView = (DataGridView)sender;
		if (dataGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
		{
			string hiddenBatchNo = (dataGridView[0, e.RowIndex].Value as CommodityInfo).getHiddenBatchNo();
			string hiddenMFGDate = (dataGridView[0, e.RowIndex].Value as CommodityInfo).getHiddenMFGDate();
			string hiddenPOSBatchNo = (dataGridView[0, e.RowIndex].Value as CommodityInfo).getHiddenPOSBatchNo();
			string text = "";
			text = ((dt.Rows.Count <= 0 || !dt.Rows[0]["CLA1NO"].ToString().Equals("0302") || !dt.Rows[0]["ISWS"].ToString().Equals("Y")) ? "N" : "Y");
			string[] data = new string[4]
			{
				hiddenBatchNo,
				hiddenMFGDate,
				hiddenPOSBatchNo,
				text
			};
			_fndo.infolistInfoSetting(data);
			base.DialogResult = DialogResult.Yes;
			Close();
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
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
		panel17 = new System.Windows.Forms.Panel();
		btn_down = new System.Windows.Forms.Button();
		btn_top = new System.Windows.Forms.Button();
		pictureBox2 = new System.Windows.Forms.PictureBox();
		keyboardcontrol1 = new KeyboardClassLibrary.Keyboardcontrol();
		btn_Save = new System.Windows.Forms.Button();
		pictureBox1 = new System.Windows.Forms.PictureBox();
		infolist = new System.Windows.Forms.DataGridView();
		DeliveryInfo = new POS_Client.frmMainShopSimple.CustomColumn();
		quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
		select = new System.Windows.Forms.DataGridViewButtonColumn();
		label1 = new System.Windows.Forms.Label();
		l_GDName = new System.Windows.Forms.Label();
		label2 = new System.Windows.Forms.Label();
		label3 = new System.Windows.Forms.Label();
		tableLayoutPanel1.SuspendLayout();
		panel1.SuspendLayout();
		panel13.SuspendLayout();
		panel3.SuspendLayout();
		panel5.SuspendLayout();
		panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
		((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)infolist).BeginInit();
		SuspendLayout();
		btn_back.BackColor = System.Drawing.Color.FromArgb(153, 153, 153);
		btn_back.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_back.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_back.ForeColor = System.Drawing.Color.White;
		btn_back.Location = new System.Drawing.Point(516, 248);
		btn_back.Name = "btn_back";
		btn_back.Size = new System.Drawing.Size(176, 40);
		btn_back.TabIndex = 0;
		btn_back.Text = "放棄設定不選入商品";
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
		dateTimePicker0.TabIndex = 6;
		dateTimePicker0.Value = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
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
		btn_Save.Location = new System.Drawing.Point(285, 248);
		btn_Save.Name = "btn_Save";
		btn_Save.Size = new System.Drawing.Size(176, 40);
		btn_Save.TabIndex = 54;
		btn_Save.Text = "儲存設定並選入商品";
		btn_Save.UseVisualStyleBackColor = false;
		btn_Save.Click += new System.EventHandler(btn_SaveMemberDataAndSelect_Click);
		pictureBox1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom);
		pictureBox1.BackColor = System.Drawing.Color.Silver;
		pictureBox1.Image = POS_Client.Properties.Resources.keyboard;
		pictureBox1.Location = new System.Drawing.Point(879, 690);
		pictureBox1.Name = "pictureBox1";
		pictureBox1.Size = new System.Drawing.Size(70, 173);
		pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		pictureBox1.TabIndex = 52;
		pictureBox1.TabStop = false;
		pictureBox1.Click += new System.EventHandler(pictureBox1_Click);
		infolist.AllowUserToAddRows = false;
		infolist.AllowUserToDeleteRows = false;
		infolist.AllowUserToResizeColumns = false;
		infolist.AllowUserToResizeRows = false;
		infolist.Anchor = System.Windows.Forms.AnchorStyles.None;
		infolist.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
		infolist.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		infolist.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
		infolist.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle.BackColor = System.Drawing.Color.FromArgb(157, 157, 157);
		dataGridViewCellStyle.Font = new System.Drawing.Font("微軟正黑體", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		dataGridViewCellStyle.ForeColor = System.Drawing.Color.White;
		dataGridViewCellStyle.Padding = new System.Windows.Forms.Padding(6);
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.Color.White;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		infolist.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
		infolist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		infolist.Columns.AddRange(DeliveryInfo, quantity, select);
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		infolist.DefaultCellStyle = dataGridViewCellStyle2;
		infolist.EnableHeadersVisualStyles = false;
		infolist.GridColor = System.Drawing.SystemColors.ActiveBorder;
		infolist.Location = new System.Drawing.Point(52, 321);
		infolist.MultiSelect = false;
		infolist.Name = "infolist";
		infolist.ReadOnly = true;
		infolist.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
		dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
		dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		infolist.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
		infolist.RowHeadersVisible = false;
		infolist.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		infolist.RowTemplate.Height = 102;
		infolist.RowTemplate.ReadOnly = true;
		infolist.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		infolist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		infolist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		infolist.Size = new System.Drawing.Size(848, 347);
		infolist.TabIndex = 57;
		infolist.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(infolist_CellContentClick);
		dataGridViewCellStyle4.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		DeliveryInfo.DefaultCellStyle = dataGridViewCellStyle4;
		DeliveryInfo.HeaderText = "進貨單資訊";
		DeliveryInfo.Name = "DeliveryInfo";
		DeliveryInfo.ReadOnly = true;
		DeliveryInfo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		DeliveryInfo.Width = 500;
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle5.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		quantity.DefaultCellStyle = dataGridViewCellStyle5;
		quantity.HeaderText = "剩餘數量";
		quantity.Name = "quantity";
		quantity.ReadOnly = true;
		quantity.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		quantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		quantity.Width = 150;
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
		select.DefaultCellStyle = dataGridViewCellStyle6;
		select.FillWeight = 80f;
		select.HeaderText = "選取";
		select.Name = "select";
		select.ReadOnly = true;
		select.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		select.Text = "選擇";
		select.Width = 200;
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label1.Location = new System.Drawing.Point(48, 295);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(185, 20);
		label1.TabIndex = 58;
		label1.Text = "自進貨單設定批號中選擇";
		label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		l_GDName.Font = new System.Drawing.Font("微軟正黑體", 14f, System.Drawing.FontStyle.Bold);
		l_GDName.Location = new System.Drawing.Point(211, 45);
		l_GDName.Name = "l_GDName";
		l_GDName.Size = new System.Drawing.Size(553, 24);
		l_GDName.TabIndex = 59;
		l_GDName.Text = "label1";
		l_GDName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		label2.AutoSize = true;
		label2.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label2.Location = new System.Drawing.Point(211, 92);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(553, 20);
		label2.TabIndex = 60;
		label2.Text = "批發農藥出貨必須設定批號與製造日期，若有不同批號數量需分為兩筆設定。";
		label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		label3.AutoSize = true;
		label3.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label3.ForeColor = System.Drawing.Color.Red;
		label3.Location = new System.Drawing.Point(239, 295);
		label3.Name = "label3";
		label3.Size = new System.Drawing.Size(227, 20);
		label3.TabIndex = 61;
		label3.Text = "(＊出貨數量不可高於剩餘數量)";
		label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		BackColor = System.Drawing.Color.Silver;
		base.ClientSize = new System.Drawing.Size(950, 681);
		base.Controls.Add(label3);
		base.Controls.Add(label2);
		base.Controls.Add(l_GDName);
		base.Controls.Add(label1);
		base.Controls.Add(infolist);
		base.Controls.Add(btn_Save);
		base.Controls.Add(panel17);
		base.Controls.Add(pictureBox1);
		base.Controls.Add(tableLayoutPanel1);
		base.Controls.Add(btn_back);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "dialogSetBatchNoAndMFGdate2";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "選擇會員 / 會員編修";
		base.Load += new System.EventHandler(dialogSetBatchNoAndMFGdate2_Load);
		tableLayoutPanel1.ResumeLayout(false);
		panel1.ResumeLayout(false);
		panel13.ResumeLayout(false);
		panel13.PerformLayout();
		panel3.ResumeLayout(false);
		panel3.PerformLayout();
		panel5.ResumeLayout(false);
		panel5.PerformLayout();
		panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
		((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
		((System.ComponentModel.ISupportInitialize)infolist).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}
}
