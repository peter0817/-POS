using POS_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogChooseVendors : Form
{
	private List<string> lst_vendorlist = new List<string>();

	private IContainer components;

	private Button btn_cancel;

	public Label l_title;

	private TextBox tb_keyword;

	private Button btn_quickSearch;

	public Label label1;

	private DataGridView dataGridView1;

	private Button btn_OK;

	private DataGridViewCheckBoxColumn Column1;

	private DataGridViewTextBoxColumn Column2;

	private DataGridViewTextBoxColumn Column3;

	private DataGridViewButtonColumn Column4;

	private DataGridViewTextBoxColumn hidden_SupplierNo;

	public string returnSupplierNo
	{
		get;
		set;
	}

	public string returnSupplierName
	{
		get;
		set;
	}

	public dialogChooseVendors(List<string> lst)
	{
		InitializeComponent();
		lst_vendorlist.Clear();
		lst_vendorlist = lst;
	}

	private void dialogChooseVendors_Load(object sender, EventArgs e)
	{
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "SupplierNo,SupplierName,SupplierIdNo,Status", "hypos_Supplier", " Status = 0", " CreateDate Desc", null, null, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count > 0)
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				dataGridView1.Rows.Insert(i, false, dataTable.Rows[i]["SupplierIdNo"].ToString(), dataTable.Rows[i]["SupplierName"].ToString(), "選擇", dataTable.Rows[i]["SupplierNo"].ToString());
			}
		}
		tb_keyword.Focus();
		tb_keyword.SelectionStart = tb_keyword.Text.Length;
	}

	private void btn_cancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
	{
		if (e.Row == 0)
		{
			e.Graphics.FillRectangle(Brushes.DarkGray, e.CellBounds);
		}
	}

	private void btn_quickSearch_Click(object sender, EventArgs e)
	{
		dataGridView1.Rows.Clear();
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "SupplierNo,SupplierName,SupplierIdNo,Status", "hypos_Supplier", " Status = 0 and (SupplierIdNo like {0} or SupplierName like {0})", " CreateDate Desc", null, new string[1]
		{
			"%" + tb_keyword.Text + "%"
		}, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count > 0)
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				dataGridView1.Rows.Insert(i, false, dataTable.Rows[i]["SupplierIdNo"].ToString(), dataTable.Rows[i]["SupplierName"].ToString(), "選擇", dataTable.Rows[i]["SupplierNo"].ToString());
			}
		}
	}

	private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		DataGridView dataGridView = (DataGridView)sender;
		if (dataGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
		{
			string item = dataGridView[4, e.RowIndex].Value.ToString() + "," + dataGridView[2, e.RowIndex].Value.ToString();
			lst_vendorlist.Add(item);
			base.DialogResult = DialogResult.OK;
			Close();
		}
	}

	private void tb_keyword_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			btn_quickSearch_Click(sender, e);
		}
	}

	private void btn_OK_Click(object sender, EventArgs e)
	{
		bool flag = false;
		foreach (DataGridViewRow item2 in (IEnumerable)dataGridView1.Rows)
		{
			if ("True".Equals(item2.Cells[0].Value.ToString()))
			{
				flag = true;
			}
		}
		if (flag)
		{
			foreach (DataGridViewRow item3 in (IEnumerable)dataGridView1.Rows)
			{
				if ("True".Equals(item3.Cells[0].Value.ToString()))
				{
					string item = item3.Cells[4].Value.ToString() + "," + item3.Cells[2].Value.ToString();
					lst_vendorlist.Add(item);
				}
			}
			base.DialogResult = DialogResult.OK;
			Close();
		}
		else
		{
			AutoClosingMessageBox.Show("請選擇廠商");
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
		btn_cancel = new System.Windows.Forms.Button();
		l_title = new System.Windows.Forms.Label();
		btn_quickSearch = new System.Windows.Forms.Button();
		tb_keyword = new System.Windows.Forms.TextBox();
		label1 = new System.Windows.Forms.Label();
		dataGridView1 = new System.Windows.Forms.DataGridView();
		Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column4 = new System.Windows.Forms.DataGridViewButtonColumn();
		hidden_SupplierNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		btn_OK = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
		SuspendLayout();
		btn_cancel.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_cancel.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_cancel.ForeColor = System.Drawing.Color.White;
		btn_cancel.Location = new System.Drawing.Point(464, 493);
		btn_cancel.Name = "btn_cancel";
		btn_cancel.Size = new System.Drawing.Size(113, 35);
		btn_cancel.TabIndex = 46;
		btn_cancel.TabStop = false;
		btn_cancel.Text = "取消";
		btn_cancel.UseVisualStyleBackColor = false;
		btn_cancel.Click += new System.EventHandler(btn_cancel_Click);
		l_title.AutoSize = true;
		l_title.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_title.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		l_title.Location = new System.Drawing.Point(180, 37);
		l_title.Name = "l_title";
		l_title.Size = new System.Drawing.Size(86, 24);
		l_title.TabIndex = 52;
		l_title.Text = "選擇廠商";
		l_title.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		btn_quickSearch.BackColor = System.Drawing.Color.White;
		btn_quickSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_quickSearch.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_quickSearch.ForeColor = System.Drawing.Color.Black;
		btn_quickSearch.Location = new System.Drawing.Point(667, 35);
		btn_quickSearch.Margin = new System.Windows.Forms.Padding(10, 10, 3, 3);
		btn_quickSearch.Name = "btn_quickSearch";
		btn_quickSearch.Size = new System.Drawing.Size(56, 29);
		btn_quickSearch.TabIndex = 21;
		btn_quickSearch.Text = "GO";
		btn_quickSearch.UseVisualStyleBackColor = false;
		btn_quickSearch.Click += new System.EventHandler(btn_quickSearch_Click);
		tb_keyword.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_keyword.ForeColor = System.Drawing.Color.Gray;
		tb_keyword.Location = new System.Drawing.Point(430, 35);
		tb_keyword.Name = "tb_keyword";
		tb_keyword.Size = new System.Drawing.Size(227, 29);
		tb_keyword.TabIndex = 20;
		tb_keyword.Text = "請輸入廠商代碼或關鍵字";
		tb_keyword.KeyDown += new System.Windows.Forms.KeyEventHandler(tb_keyword_KeyDown);
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		label1.Location = new System.Drawing.Point(315, 38);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(109, 20);
		label1.TabIndex = 52;
		label1.Text = "廠商快速篩選 ";
		label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		dataGridView1.AllowUserToAddRows = false;
		dataGridView1.AllowUserToDeleteRows = false;
		dataGridView1.AllowUserToResizeColumns = false;
		dataGridView1.AllowUserToResizeRows = false;
		dataGridView1.BackgroundColor = System.Drawing.Color.White;
		dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
		dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		dataGridViewCellStyle.Font = new System.Drawing.Font("微軟正黑體", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		dataGridViewCellStyle.ForeColor = System.Drawing.Color.White;
		dataGridViewCellStyle.Padding = new System.Windows.Forms.Padding(6);
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
		dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		dataGridView1.Columns.AddRange(Column1, Column2, Column3, Column4, hidden_SupplierNo);
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(6);
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
		dataGridView1.EnableHeadersVisualStyles = false;
		dataGridView1.Location = new System.Drawing.Point(47, 81);
		dataGridView1.Name = "dataGridView1";
		dataGridView1.RowHeadersVisible = false;
		dataGridView1.RowTemplate.Height = 35;
		dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		dataGridView1.Size = new System.Drawing.Size(811, 392);
		dataGridView1.TabIndex = 53;
		dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dataGridView1_CellContentClick);
		Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
		Column1.HeaderText = "請勾選";
		Column1.Name = "Column1";
		Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		Column1.Width = 150;
		Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
		Column2.HeaderText = "統一編號";
		Column2.Name = "Column2";
		Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column2.Width = 150;
		Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		Column3.HeaderText = "廠商名稱";
		Column3.Name = "Column3";
		Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column4.HeaderText = "選擇";
		Column4.Name = "Column4";
		Column4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		hidden_SupplierNo.HeaderText = "hidden_SupplierNo";
		hidden_SupplierNo.Name = "hidden_SupplierNo";
		hidden_SupplierNo.Visible = false;
		btn_OK.BackColor = System.Drawing.Color.FromArgb(157, 189, 59);
		btn_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_OK.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_OK.ForeColor = System.Drawing.Color.White;
		btn_OK.Location = new System.Drawing.Point(334, 492);
		btn_OK.Name = "btn_OK";
		btn_OK.Size = new System.Drawing.Size(113, 35);
		btn_OK.TabIndex = 54;
		btn_OK.TabStop = false;
		btn_OK.Text = "選擇勾選";
		btn_OK.UseVisualStyleBackColor = false;
		btn_OK.Click += new System.EventHandler(btn_OK_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		AutoScroll = true;
		BackColor = System.Drawing.Color.Silver;
		base.ClientSize = new System.Drawing.Size(904, 576);
		base.ControlBox = false;
		base.Controls.Add(btn_OK);
		base.Controls.Add(dataGridView1);
		base.Controls.Add(btn_quickSearch);
		base.Controls.Add(tb_keyword);
		base.Controls.Add(label1);
		base.Controls.Add(l_title);
		base.Controls.Add(btn_cancel);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "dialogChooseVendors";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "frmSearchMember";
		base.Load += new System.EventHandler(dialogChooseVendors_Load);
		((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}
}
