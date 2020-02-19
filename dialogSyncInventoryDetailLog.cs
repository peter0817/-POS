using POS_Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogSyncInventoryDetailLog : Form
{
	public string p_time = "";

	private IContainer components;

	private Button btn_cancel;

	private DataGridView dataGridView1;

	private Label label2;

	private Label label1;

	private DataGridViewTextBoxColumn Column1;

	private DataGridViewTextBoxColumn Column2;

	private DataGridViewTextBoxColumn Column3;

	private DataGridViewTextBoxColumn Column5;

	private DataGridViewTextBoxColumn Column4;

	private DataGridViewTextBoxColumn Column8;

	private DataGridViewTextBoxColumn Column6;

	private DataGridViewTextBoxColumn Column7;

	private DataGridViewTextBoxColumn Column9;

	public dialogSyncInventoryDetailLog(string time)
	{
		InitializeComponent();
		dataGridView1.ClearSelection();
		label2.Text = time;
		p_time = time;
		List<string> list = new List<string>();
		string sql = "SELECT sidl.adjustType as adjustType, sidl.AdjustNo as AdjustNo, sidl.updateDate as updateDate, sidl.GDSNO as GDSNO, sidl.adjustCount as adjustCount, sidl.GDName as GDName, sidl.CName as CName, sidl.formCode as formCode, sidl.contents as contents, sidl.brandName as brandName, sidl.spec as spec, sidl.capacity as capacity FROM ( SELECT * FROM hypos_Synchronize_main_log as sml WHERE sml.updateDate = {0} AND sml.status = '0' AND sml.updateType = '1' ) as sml2 INNER JOIN ( SELECT sidl.*, gl.GDName as GDName, gl.CName as CName, gl.formCode as formCode, gl.contents as contents, gl.brandName as brandName, gl.spec as spec, gl.capacity as capacity FROM hypos_Sync_inventory_detail_log as sidl INNER JOIN hypos_GOODSLST as gl on sidl.GDSNO = gl.barcode ) as sidl on sml2.mainLogId = sidl.mainLogId";
		list.Add(time);
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, list.ToArray(), CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			string text = "";
			switch (string.IsNullOrEmpty(dataTable.Rows[i]["adjustType"].ToString()) ? "0" : dataTable.Rows[i]["adjustType"].ToString())
			{
			case "A":
				text = "破損";
				break;
			case "B":
				text = "盤減";
				break;
			case "C":
				text = "盤增";
				break;
			case "E":
				text = "原廠退回";
				break;
			case "F":
				text = "過期退貨";
				break;
			case "G":
				text = "過期銷毀";
				break;
			case "H":
				text = "資料異常";
				break;
			case "X":
				text = "其他";
				break;
			}
			dataGridView1.Rows.Add(text, string.IsNullOrEmpty(dataTable.Rows[i]["AdjustNo"].ToString()) ? "0" : dataTable.Rows[i]["AdjustNo"].ToString(), string.IsNullOrEmpty(dataTable.Rows[i]["updateDate"].ToString()) ? "0" : dataTable.Rows[i]["updateDate"].ToString(), string.IsNullOrEmpty(dataTable.Rows[i]["GDSNO"].ToString()) ? "0" : dataTable.Rows[i]["GDSNO"].ToString(), Get_PDate(dataTable.Rows[i]["GDSNO"].ToString(), dataTable.Rows[i]["AdjustNo"].ToString()), Get_BatchNo(dataTable.Rows[i]["GDSNO"].ToString(), dataTable.Rows[i]["AdjustNo"].ToString()), dataTable.Rows[i]["GDName"].ToString() + "[" + dataTable.Rows[i]["CName"].ToString() + "-" + dataTable.Rows[i]["formCode"].ToString() + "．" + dataTable.Rows[i]["contents"].ToString() + "-" + dataTable.Rows[i]["brandName"].ToString() + "]" + dataTable.Rows[i]["spec"].ToString() + dataTable.Rows[i]["capacity"].ToString(), string.IsNullOrEmpty(dataTable.Rows[i]["adjustCount"].ToString()) ? "0" : dataTable.Rows[i]["adjustCount"].ToString(), Get_VendorName(dataTable.Rows[i]["GDSNO"].ToString(), dataTable.Rows[i]["AdjustNo"].ToString()));
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

	public string Get_PDate(string GDSNO, string AdjustNo)
	{
		string text = "";
		List<string> list = new List<string>();
		string text2 = p_time.Substring(0, 14).Insert(14, "%");
		list.Add(p_time);
		string sql = "SELECT sidl.adjustType as adjustType, sidl.AdjustNo as AdjustNo, sidl.updateDate as updateDate, sidl.GDSNO as GDSNO, sidl.adjustCount as adjustCount, sidl.GDName as GDName, sidl.CName as CName, sidl.formCode as formCode, sidl.contents as contents, sidl.brandName as brandName, sidl.spec as spec, sidl.capacity as capacity ,GDST.MFGDate as 'PDate', GDST.BatchNo as 'PBatchNo' FROM ( SELECT * FROM hypos_Synchronize_main_log as sml WHERE sml.updateDate = {0} AND sml.status = '0' AND sml.updateType = '1' ) as sml2 INNER JOIN ( SELECT sidl.*, gl.GDName as GDName, gl.CName as CName, gl.formCode as formCode, gl.contents as contents, gl.brandName as brandName, gl.spec as spec, gl.capacity as capacity FROM hypos_Sync_inventory_detail_log as sidl INNER JOIN hypos_GOODSLST as gl on sidl.GDSNO = gl.barcode ) as sidl on sml2.mainLogId = sidl.mainLogId INNER JOIN hypos_GOODSLST as 'GD1' on sidl.GDSNO = GD1.GDSNO INNER JOIN hypos_PurchaseGoods_Detail as 'GDST' on  GDST.GDSNO = GD1.GDSNO inner  join hypos_PurchaseGoods_Master AS 'mmm' on mmm.PurchaseNo = GDST.PurchaseNo where mmm.UpdateDate like '" + text2 + "' AND sidl.GDSNO = '" + GDSNO + "' ";
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, list.ToArray(), CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count > 0)
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				text = dataTable.Rows[0]["PDate"].ToString();
				if (!(text != ""))
				{
					text = "";
				}
			}
		}
		return text;
	}

	public string Get_BatchNo(string GDSNO, string AdjustNo)
	{
		string text = "";
		List<string> list = new List<string>();
		string text2 = p_time.Substring(0, 14).Insert(14, "%");
		list.Add(p_time);
		string sql = "SELECT sidl.adjustType as adjustType, sidl.AdjustNo as AdjustNo, sidl.updateDate as updateDate, sidl.GDSNO as GDSNO, sidl.adjustCount as adjustCount, sidl.GDName as GDName, sidl.CName as CName, sidl.formCode as formCode, sidl.contents as contents, sidl.brandName as brandName, sidl.spec as spec, sidl.capacity as capacity ,GDST.MFGDate as 'PDate', GDST.BatchNo as 'PBatchNo' FROM ( SELECT * FROM hypos_Synchronize_main_log as sml WHERE sml.updateDate = {0} AND sml.status = '0' AND sml.updateType = '1' ) as sml2 INNER JOIN ( SELECT sidl.*, gl.GDName as GDName, gl.CName as CName, gl.formCode as formCode, gl.contents as contents, gl.brandName as brandName, gl.spec as spec, gl.capacity as capacity FROM hypos_Sync_inventory_detail_log as sidl INNER JOIN hypos_GOODSLST as gl on sidl.GDSNO = gl.barcode ) as sidl on sml2.mainLogId = sidl.mainLogId INNER JOIN hypos_GOODSLST as 'GD1' on sidl.GDSNO = GD1.GDSNO INNER JOIN hypos_PurchaseGoods_Detail as 'GDST' on  GDST.GDSNO = GD1.GDSNO inner  join hypos_PurchaseGoods_Master AS 'mmm' on mmm.PurchaseNo = GDST.PurchaseNo where mmm.UpdateDate like '" + text2 + "' AND sidl.GDSNO = '" + GDSNO + "' ";
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, list.ToArray(), CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count > 0)
		{
			for (int i = 0; i < 1; i++)
			{
				text = dataTable.Rows[0]["PBatchNo"].ToString();
				if (!(text != ""))
				{
					text = "";
				}
			}
		}
		return text;
	}

	public string Get_VendorName(string GDSNO, string AdjustNo)
	{
		string text = "";
		List<string> list = new List<string>();
		string sql = "SELECT sidl.adjustType as adjustType, sidl.AdjustNo as AdjustNo, sidl.updateDate as updateDate, sidl.GDSNO as GDSNO, sidl.adjustCount as adjustCount, sidl.GDName as GDName, sidl.CName as CName, sidl.formCode as formCode, sidl.contents as contents, sidl.brandName as brandName, sidl.spec as spec, sidl.capacity as capacity ,dd1.MFGDate as 'PDate', dd1.BatchNo as 'PBatchNo' , ss1.vendorId as 'VenderID', ss1.vendorName as 'VenderName' FROM ( SELECT * FROM hypos_Synchronize_main_log as sml WHERE sml.updateDate = {0} AND sml.status = '0' AND sml.updateType = '1' ) as sml2 INNER JOIN ( SELECT sidl.*, gl.GDName as GDName, gl.CName as CName, gl.formCode as formCode, gl.contents as contents, gl.brandName as brandName, gl.spec as spec, gl.capacity as capacity FROM hypos_Sync_inventory_detail_log as sidl INNER JOIN hypos_GOODSLST as gl on sidl.GDSNO = gl.barcode ) as sidl on sml2.mainLogId = sidl.mainLogId INNER JOIN hypos_PurchaseGoods_Detail as dd1 on sidl.GDSNO = dd1.GDSNO INNER JOIN hypos_PurchaseGoods_Master as mm1 on mm1.PurchaseNo = dd1.PurchaseNo INNER JOIN hypos_Supplier as ss1 on ss1.SupplierNo = mm1.SupplierNo where sidl.GDSNO = '" + GDSNO + "' AND AdjustNo = '" + AdjustNo + "' ";
		list.Add(p_time);
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, list.ToArray(), CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count > 0)
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				text = dataTable.Rows[0]["VenderName"].ToString() + "(" + dataTable.Rows[0]["VenderID"].ToString() + ")";
				if (!(text != ""))
				{
					text = "";
				}
			}
		}
		return text;
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
		btn_cancel = new System.Windows.Forms.Button();
		dataGridView1 = new System.Windows.Forms.DataGridView();
		label2 = new System.Windows.Forms.Label();
		label1 = new System.Windows.Forms.Label();
		Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
		SuspendLayout();
		btn_cancel.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_cancel.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_cancel.ForeColor = System.Drawing.Color.White;
		btn_cancel.Location = new System.Drawing.Point(431, 643);
		btn_cancel.Name = "btn_cancel";
		btn_cancel.Size = new System.Drawing.Size(124, 34);
		btn_cancel.TabIndex = 68;
		btn_cancel.TabStop = false;
		btn_cancel.Text = "關閉";
		btn_cancel.UseVisualStyleBackColor = false;
		btn_cancel.Click += new System.EventHandler(btn_cancel_Click);
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
		dataGridView1.Columns.AddRange(Column1, Column2, Column3, Column5, Column4, Column8, Column6, Column7, Column9);
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
		dataGridView1.Location = new System.Drawing.Point(25, 70);
		dataGridView1.Name = "dataGridView1";
		dataGridView1.ReadOnly = true;
		dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridView1.RowHeadersVisible = false;
		dataGridView1.RowTemplate.Height = 40;
		dataGridView1.Size = new System.Drawing.Size(942, 554);
		dataGridView1.TabIndex = 67;
		dataGridView1.SelectionChanged += new System.EventHandler(dataGridView1_SelectionChanged);
		label2.AutoSize = true;
		label2.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label2.Location = new System.Drawing.Point(30, 43);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(128, 24);
		label2.TabIndex = 66;
		label2.Text = "yyyy-MM-dd";
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label1.Location = new System.Drawing.Point(409, 29);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(181, 24);
		label1.TabIndex = 65;
		label1.Text = "庫存調整紀錄檔上傳";
		Column1.HeaderText = "盤點原因";
		Column1.Name = "Column1";
		Column1.ReadOnly = true;
		Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column1.Width = 90;
		Column2.HeaderText = "盤點代碼";
		Column2.Name = "Column2";
		Column2.ReadOnly = true;
		Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column2.Width = 90;
		Column3.HeaderText = "盤點日期時間";
		Column3.Name = "Column3";
		Column3.ReadOnly = true;
		Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column3.Width = 122;
		Column5.HeaderText = "商品條碼";
		Column5.Name = "Column5";
		Column5.ReadOnly = true;
		Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column5.Width = 90;
		Column4.HeaderText = "製造日期";
		Column4.Name = "Column4";
		Column4.ReadOnly = true;
		Column4.Width = 109;
		Column8.HeaderText = "批號";
		Column8.Name = "Column8";
		Column8.ReadOnly = true;
		Column8.Width = 77;
		Column6.HeaderText = "商品名稱";
		Column6.Name = "Column6";
		Column6.ReadOnly = true;
		Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column6.Width = 90;
		Column7.HeaderText = "數量調整";
		Column7.Name = "Column7";
		Column7.ReadOnly = true;
		Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column7.Width = 90;
		Column9.HeaderText = "退貨對象";
		Column9.Name = "Column9";
		Column9.ReadOnly = true;
		Column9.Width = 109;
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
		base.Name = "dialogSyncInventoryDetailLog";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "農委會防檢局POS系統";
		((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}
}
