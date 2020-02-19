using POS_Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogSyncSellDetailLog : Form
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

	public dialogSyncSellDetailLog(string time)
	{
		InitializeComponent();
		dataGridView1.ClearSelection();
		label2.Text = time;
		string a = "";
		string a2 = "even_cellstyle";
		int num = 1;
		List<string> list = new List<string>();
		string str = "SELECT ssdl.status as status, ssdl.sellNo as sellNo, ssdl.BuyDate as BuyDate, ssdl.VipNo as VipNo, ssdl.barcode as barcode, ssdl.PRNO as PRNO, ssdl.BLNO as BLNO, ssdl.count as count, ssdl.CLA1NO as CLA1NO, ssdl.IdNo as IdNo, ssdl.Name as Name, ssdl.GDName as GDName, ssdl.CName as CName, ssdl.formCode as formCode, ssdl.contents as contents, ssdl.brandName as brandName, ssdl.spec as spec, ssdl.capacity as capacity FROM ( SELECT * FROM hypos_Synchronize_main_log as sml WHERE sml.updateDate = {0";
		list.Add(time);
		str += "} AND sml.status = '0' AND sml.updateType = '0' ) as sml2 INNER JOIN ( SELECT ssdl.*, cr.Name as Name, cr.IdNo as IdNo, gl.GDName as GDName, gl.CName as CName, gl.formCode as formCode, gl.contents as contents, gl.brandName as brandName, gl.spec as spec, gl.capacity as capacity FROM hypos_Sync_sell_detail_log as ssdl INNER JOIN  hypos_CUST_RTL as cr on ssdl.VipNo = cr.VipNo INNER JOIN hypos_GOODSLST as gl on ssdl.barcode = gl.barcode where gl.ISWS = 'Y' ) as ssdl on sml2.mainLogId = ssdl.mainLogId";
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, str, list.ToArray(), CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			string text = "";
			string text2 = "";
			switch (string.IsNullOrEmpty(dataTable.Rows[i]["status"].ToString()) ? "0" : dataTable.Rows[i]["status"].ToString())
			{
			case "0":
				text = "正常";
				break;
			case "1":
				text = "取消";
				break;
			case "2":
				text = "變更";
				break;
			}
			switch (string.IsNullOrEmpty(dataTable.Rows[i]["CLA1NO"].ToString()) ? "0" : dataTable.Rows[i]["CLA1NO"].ToString())
			{
			case "0302":
				text2 = "管制農藥";
				break;
			case "0303":
				text2 = "補助肥料";
				break;
			case "0305":
				text2 = "資材";
				break;
			case "0308":
				text2 = "其他";
				break;
			}
			if (a != dataTable.Rows[i]["sellNo"].ToString())
			{
				a = dataTable.Rows[i]["sellNo"].ToString();
				a2 = ((!(a2 == "odd_cellstyle")) ? "odd_cellstyle" : "even_cellstyle");
				string sql = "SELECT * FROM HyCrop WHERE code = {0} ";
				string[] strParameterArray = new string[1]
				{
					dataTable.Rows[i]["PRNO"].ToString()
				};
				DataTable dataTable2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, strParameterArray, CommandOperationType.ExecuteReaderReturnDataTable);
				string text3 = "";
				if (dataTable2.Rows.Count > 0)
				{
					text3 = dataTable2.Rows[0]["name"].ToString();
				}
				string sql2 = "SELECT * FROM HyBlight WHERE code = {0} ";
				string[] strParameterArray2 = new string[1]
				{
					dataTable.Rows[i]["BLNO"].ToString()
				};
				DataTable dataTable3 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql2, strParameterArray2, CommandOperationType.ExecuteReaderReturnDataTable);
				string text4 = "";
				if (dataTable3.Rows.Count > 0)
				{
					text4 = dataTable3.Rows[0]["name"].ToString();
				}
				num = 1;
				dataGridView1.Rows.Add(text, string.IsNullOrEmpty(dataTable.Rows[i]["sellNo"].ToString()) ? "0" : dataTable.Rows[i]["sellNo"].ToString(), string.IsNullOrEmpty(dataTable.Rows[i]["BuyDate"].ToString()) ? "0" : dataTable.Rows[i]["BuyDate"].ToString(), string.IsNullOrEmpty(dataTable.Rows[i]["Name"].ToString()) ? "0" : dataTable.Rows[i]["Name"].ToString(), string.IsNullOrEmpty(dataTable.Rows[i]["VipNo"].ToString()) ? "0" : dataTable.Rows[i]["VipNo"].ToString(), string.IsNullOrEmpty(dataTable.Rows[i]["IdNo"].ToString()) ? "0" : dataTable.Rows[i]["IdNo"].ToString(), num, string.IsNullOrEmpty(dataTable.Rows[i]["barcode"].ToString()) ? "0" : dataTable.Rows[i]["barcode"].ToString(), dataTable.Rows[i]["GDName"].ToString() + "[" + dataTable.Rows[i]["CName"].ToString() + "-" + dataTable.Rows[i]["formCode"].ToString() + "．" + dataTable.Rows[i]["contents"].ToString() + "-" + dataTable.Rows[i]["brandName"].ToString() + "]" + dataTable.Rows[i]["spec"].ToString() + dataTable.Rows[i]["capacity"].ToString(), text3, text4, string.IsNullOrEmpty(dataTable.Rows[i]["count"].ToString()) ? "0" : dataTable.Rows[i]["count"].ToString(), text2);
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
				a = dataTable.Rows[i]["sellNo"].ToString();
				string sql3 = "SELECT * FROM HyCrop WHERE code = {0} ";
				string[] strParameterArray3 = new string[1]
				{
					dataTable.Rows[i]["PRNO"].ToString()
				};
				DataTable dataTable4 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql3, strParameterArray3, CommandOperationType.ExecuteReaderReturnDataTable);
				string text5 = "";
				if (dataTable4.Rows.Count > 0)
				{
					text5 = dataTable4.Rows[0]["name"].ToString();
				}
				string sql4 = "SELECT * FROM HyBlight WHERE code = {0} ";
				string[] strParameterArray4 = new string[1]
				{
					dataTable.Rows[i]["BLNO"].ToString()
				};
				DataTable dataTable5 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql4, strParameterArray4, CommandOperationType.ExecuteReaderReturnDataTable);
				string text6 = "";
				if (dataTable5.Rows.Count > 0)
				{
					text6 = dataTable5.Rows[0]["name"].ToString();
				}
				num++;
				dataGridView1.Rows.Add("", "", "", "", "", "", num, string.IsNullOrEmpty(dataTable.Rows[i]["barcode"].ToString()) ? "0" : dataTable.Rows[i]["barcode"].ToString(), dataTable.Rows[i]["GDName"].ToString() + "[" + dataTable.Rows[i]["CName"].ToString() + "-" + dataTable.Rows[i]["formCode"].ToString() + "．" + dataTable.Rows[i]["contents"].ToString() + "-" + dataTable.Rows[i]["brandName"].ToString() + "]" + dataTable.Rows[i]["spec"].ToString() + dataTable.Rows[i]["capacity"].ToString(), text5, text6, string.IsNullOrEmpty(dataTable.Rows[i]["count"].ToString()) ? "0" : dataTable.Rows[i]["count"].ToString(), text2);
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
		btn_cancel = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
		SuspendLayout();
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label1.Location = new System.Drawing.Point(427, 21);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(143, 24);
		label1.TabIndex = 0;
		label1.Text = "銷售紀錄檔上傳";
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
		Column1.HeaderText = "狀態";
		Column1.Name = "Column1";
		Column1.ReadOnly = true;
		Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column1.Width = 58;
		Column2.HeaderText = "銷售單號";
		Column2.Name = "Column2";
		Column2.ReadOnly = true;
		Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column2.Width = 90;
		Column3.HeaderText = "購買時間";
		Column3.Name = "Column3";
		Column3.ReadOnly = true;
		Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column3.Width = 90;
		Column4.HeaderText = "購買會員";
		Column4.Name = "Column4";
		Column4.ReadOnly = true;
		Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column4.Width = 90;
		Column5.HeaderText = "會員編號";
		Column5.Name = "Column5";
		Column5.ReadOnly = true;
		Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column5.Width = 90;
		Column6.HeaderText = "身分證字號";
		Column6.Name = "Column6";
		Column6.ReadOnly = true;
		Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column6.Width = 106;
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
		Column10.HeaderText = "作物";
		Column10.Name = "Column10";
		Column10.ReadOnly = true;
		Column10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column10.Width = 58;
		Column11.HeaderText = "病蟲害";
		Column11.Name = "Column11";
		Column11.ReadOnly = true;
		Column11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column11.Width = 74;
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
		base.Name = "dialogSyncSellDetailLog";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "農委會防檢局POS系統";
		((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}
}
