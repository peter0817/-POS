using POS_Client;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogSyncShipDetailLog : Form
{
	private IContainer components;

	private Label label1;

	private Label label2;

	private DataGridView dataGridView1;

	private Button btn_cancel;

	private DataGridViewTextBoxColumn Column1;

	private DataGridViewTextBoxColumn Column2;

	private DataGridViewTextBoxColumn Column3;

	private DataGridViewTextBoxColumn Column4;

	private DataGridViewTextBoxColumn Column5;

	private DataGridViewTextBoxColumn Column6;

	private DataGridViewTextBoxColumn Column7;

	private DataGridViewTextBoxColumn Column8;

	private DataGridViewTextBoxColumn Column9;

	private DataGridViewTextBoxColumn Column10;

	private DataGridViewTextBoxColumn Column11;

	private DataGridViewTextBoxColumn Column12;

	private DataGridViewTextBoxColumn Column13;

	public dialogSyncShipDetailLog(string time)
	{
		InitializeComponent();
		dataGridView1.ClearSelection();
		label2.Text = time;
		string a = "";
		string a2 = "even_cellstyle";
		int num = 1;
		string sql = "SELECT mainLogId FROM hypos_Synchronize_main_log where status = '0' and updateType = '9' and updateDate = '" + time + "'";
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteReaderReturnDataTable);
		string text = "";
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
		text = dataTable.Rows[0]["mainLogId"].ToString();
		string sql2 = "SELECT ssdl.*,gl.ISWS,gl.CLA1NO FROM hypos_Sync_ship_detail_log as ssdl LEFT JOIN hypos_GOODSLST as gl on ssdl.barcode = gl.GDSNO where ssdl.mainLogId = " + text + " and gl.ISWS = 'Y' and gl.CLA1NO = '0302'";
		DataTable dataTable2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql2, null, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable2.Rows.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < dataTable2.Rows.Count; i++)
		{
			string text2 = "";
			string text3 = "";
			string str = string.IsNullOrEmpty(dataTable2.Rows[i]["DeliveryNo"].ToString()) ? "" : dataTable2.Rows[i]["DeliveryNo"].ToString();
			string text4 = DataBaseUtilities.DBOperation(Program.ConnectionString, "SELECT CreateDate FROM hypos_DeliveryGoods_Master where DeliveryNo = '" + str + "'", null, CommandOperationType.ExecuteScalar).ToString();
			string text5 = DataBaseUtilities.DBOperation(Program.ConnectionString, "SELECT SupplierName FROM hypos_Supplier where SupplierNo = '" + dataTable2.Rows[i]["vendorNO"].ToString() + "'", null, CommandOperationType.ExecuteScalar).ToString();
			string str2 = string.IsNullOrEmpty(dataTable2.Rows[i]["barcode"].ToString()) ? "" : dataTable2.Rows[i]["barcode"].ToString();
			string sql3 = "SELECT GDName,CName,formCode,contents,brandName,spec,capacity FROM hypos_GOODSLST where GDSNO = '" + str2 + "'";
			DataTable dataTable3 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql3, null, CommandOperationType.ExecuteReaderReturnDataTable);
			string text6 = "";
			if (dataTable3.Rows.Count > 0)
			{
				text6 = dataTable3.Rows[0]["GDName"].ToString() + "[" + dataTable3.Rows[0]["CName"].ToString() + "-" + dataTable3.Rows[0]["formCode"].ToString() + "．" + dataTable3.Rows[0]["contents"].ToString() + "-" + dataTable3.Rows[0]["brandName"].ToString() + "]" + dataTable3.Rows[0]["spec"].ToString() + dataTable3.Rows[0]["capacity"].ToString();
			}
			switch (string.IsNullOrEmpty(dataTable2.Rows[i]["status"].ToString()) ? "0" : dataTable2.Rows[i]["status"].ToString())
			{
			case "0":
				text2 = "正常";
				break;
			case "1":
				text2 = "取消";
				break;
			case "2":
				text2 = "變更";
				break;
			}
			switch (string.IsNullOrEmpty(dataTable2.Rows[i]["itemType"].ToString()) ? "0" : dataTable2.Rows[i]["itemType"].ToString())
			{
			case "0302":
				text3 = "管制農藥";
				break;
			case "0303":
				text3 = "補助肥料";
				break;
			case "0305":
				text3 = "資材";
				break;
			case "0308":
				text3 = "其他";
				break;
			}
			if (a != dataTable2.Rows[i]["DeliveryNo"].ToString())
			{
				a = dataTable2.Rows[i]["DeliveryNo"].ToString();
				a2 = ((!(a2 == "odd_cellstyle")) ? "odd_cellstyle" : "even_cellstyle");
				num = 1;
				dataGridView1.Rows.Add(text2, string.IsNullOrEmpty(dataTable2.Rows[i]["DeliveryNo"].ToString()) ? "" : dataTable2.Rows[i]["DeliveryNo"].ToString(), text4, text5, string.IsNullOrEmpty(dataTable2.Rows[i]["vendorId"].ToString()) ? "" : dataTable2.Rows[i]["vendorId"].ToString(), string.IsNullOrEmpty(dataTable2.Rows[i]["vendorName"].ToString()) ? "" : dataTable2.Rows[i]["vendorName"].ToString(), num, string.IsNullOrEmpty(dataTable2.Rows[i]["barcode"].ToString()) ? "" : dataTable2.Rows[i]["barcode"].ToString(), text6, string.IsNullOrEmpty(dataTable2.Rows[i]["batchNO"].ToString()) ? "" : dataTable2.Rows[i]["batchNO"].ToString(), string.IsNullOrEmpty(dataTable2.Rows[i]["MFD"].ToString()) ? "" : dataTable2.Rows[i]["MFD"].ToString(), string.IsNullOrEmpty(dataTable2.Rows[i]["shipQTY"].ToString()) ? "" : dataTable2.Rows[i]["shipQTY"].ToString(), text3);
				if (a2 == "odd_cellstyle")
				{
					dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 150);
				}
				else if (a2 == "even_cellstyle")
				{
					dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 228, 181);
				}
			}
			else
			{
				a = dataTable2.Rows[i]["DeliveryNo"].ToString();
				num++;
				dataGridView1.Rows.Add("", "", "", "", "", "", num, string.IsNullOrEmpty(dataTable2.Rows[i]["barcode"].ToString()) ? "0" : dataTable2.Rows[i]["barcode"].ToString(), text6, string.IsNullOrEmpty(dataTable2.Rows[i]["batchNO"].ToString()) ? "" : dataTable2.Rows[i]["batchNO"].ToString(), string.IsNullOrEmpty(dataTable2.Rows[i]["MFD"].ToString()) ? "" : dataTable2.Rows[i]["MFD"].ToString(), string.IsNullOrEmpty(dataTable2.Rows[i]["shipQTY"].ToString()) ? "" : dataTable2.Rows[i]["shipQTY"].ToString(), text3);
				if (a2 == "odd_cellstyle")
				{
					dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 150);
				}
				else if (a2 == "even_cellstyle")
				{
					dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 228, 181);
				}
			}
		}
	}

	private void btn_cancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void dataGridView1_SelectionChanged(object sender, EventArgs e)
	{
		if (dataGridView1.SelectedCells.Count > 0)
		{
			dataGridView1.ClearSelection();
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
		label1 = new System.Windows.Forms.Label();
		label2 = new System.Windows.Forms.Label();
		dataGridView1 = new System.Windows.Forms.DataGridView();
		btn_cancel = new System.Windows.Forms.Button();
		Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
		SuspendLayout();
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label1.Location = new System.Drawing.Point(421, 21);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(181, 24);
		label1.TabIndex = 0;
		label1.Text = "農藥出貨紀錄檔上傳";
		label2.AutoSize = true;
		label2.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label2.Location = new System.Drawing.Point(29, 40);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(128, 24);
		label2.TabIndex = 1;
		label2.Text = "yyyy-MM-dd";
		dataGridView1.AllowUserToAddRows = false;
		dataGridView1.AllowUserToDeleteRows = false;
		dataGridView1.AllowUserToResizeColumns = false;
		dataGridView1.AllowUserToResizeRows = false;
		dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
		dataGridView1.BackgroundColor = System.Drawing.Color.Silver;
		dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
		dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		dataGridViewCellStyle.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle.ForeColor = System.Drawing.Color.White;
		dataGridViewCellStyle.Padding = new System.Windows.Forms.Padding(6);
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
		dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		dataGridView1.Columns.AddRange(Column1, Column2, Column3, Column4, Column5, Column6, Column7, Column8, Column9, Column10, Column11, Column12, Column13);
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(255, 240, 150);
		dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(6);
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(255, 240, 150);
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
		dataGridView1.EnableHeadersVisualStyles = false;
		dataGridView1.Location = new System.Drawing.Point(24, 67);
		dataGridView1.Name = "dataGridView1";
		dataGridView1.ReadOnly = true;
		dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridView1.RowHeadersVisible = false;
		dataGridView1.RowTemplate.Height = 40;
		dataGridView1.Size = new System.Drawing.Size(945, 564);
		dataGridView1.TabIndex = 2;
		dataGridView1.SelectionChanged += new System.EventHandler(dataGridView1_SelectionChanged);
		btn_cancel.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_cancel.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_cancel.ForeColor = System.Drawing.Color.White;
		btn_cancel.Location = new System.Drawing.Point(431, 654);
		btn_cancel.Name = "btn_cancel";
		btn_cancel.Size = new System.Drawing.Size(124, 34);
		btn_cancel.TabIndex = 64;
		btn_cancel.TabStop = false;
		btn_cancel.Text = "關閉";
		btn_cancel.UseVisualStyleBackColor = false;
		btn_cancel.Click += new System.EventHandler(btn_cancel_Click);
		Column1.HeaderText = "狀態";
		Column1.Name = "Column1";
		Column1.ReadOnly = true;
		Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column1.Width = 58;
		Column2.HeaderText = "出貨單號";
		Column2.Name = "Column2";
		Column2.ReadOnly = true;
		Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column2.Width = 90;
		Column3.HeaderText = "建立時間";
		Column3.Name = "Column3";
		Column3.ReadOnly = true;
		Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column3.Width = 90;
		Column4.HeaderText = "出貨對象";
		Column4.Name = "Column4";
		Column4.ReadOnly = true;
		Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column4.Width = 90;
		Column5.HeaderText = "證號";
		Column5.Name = "Column5";
		Column5.ReadOnly = true;
		Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column5.Width = 58;
		Column6.HeaderText = "商業名稱";
		Column6.Name = "Column6";
		Column6.ReadOnly = true;
		Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column6.Width = 90;
		Column7.HeaderText = "項次";
		Column7.Name = "Column7";
		Column7.ReadOnly = true;
		Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column7.Width = 58;
		Column8.HeaderText = "商品條碼";
		Column8.Name = "Column8";
		Column8.ReadOnly = true;
		Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column8.Width = 90;
		Column9.HeaderText = "商品名稱";
		Column9.Name = "Column9";
		Column9.ReadOnly = true;
		Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column9.Width = 90;
		Column10.HeaderText = "批號";
		Column10.Name = "Column10";
		Column10.ReadOnly = true;
		Column10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column10.Width = 58;
		Column11.HeaderText = "製造日期";
		Column11.Name = "Column11";
		Column11.ReadOnly = true;
		Column11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column11.Width = 90;
		Column12.HeaderText = "數量";
		Column12.Name = "Column12";
		Column12.ReadOnly = true;
		Column12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column12.Width = 58;
		Column13.HeaderText = "類型";
		Column13.Name = "Column13";
		Column13.ReadOnly = true;
		Column13.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column13.Width = 58;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		AutoScroll = true;
		BackColor = System.Drawing.Color.Silver;
		base.ClientSize = new System.Drawing.Size(997, 700);
		base.ControlBox = false;
		base.Controls.Add(btn_cancel);
		base.Controls.Add(dataGridView1);
		base.Controls.Add(label2);
		base.Controls.Add(label1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "dialogSyncShipDetailLog";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "農委會防檢局POS系統";
		((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}
}
