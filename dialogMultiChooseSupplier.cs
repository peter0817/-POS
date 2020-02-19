using POS_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogMultiChooseSupplier : Form
{
	public List<string> returnSupplierNo = new List<string>();

	public List<string> returnSupplierName = new List<string>();

	private IContainer components;

	private Button btn_cancel;

	public Label l_title;

	private TextBox tb_keyword;

	private Button btn_quickSearch;

	public Label label1;

	private DataGridView dataGridView1;

	private Button btn_SelectedCallBack;

	private DataGridViewCheckBoxColumn select;

	private DataGridViewTextBoxColumn Column2;

	private DataGridViewTextBoxColumn Column3;

	private DataGridViewTextBoxColumn hidden_SupplierNo;

	public dialogMultiChooseSupplier()
	{
		InitializeComponent();
	}

	private void dialogChooseSupplier_Load(object sender, EventArgs e)
	{
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "SupplierNo,SupplierName,SupplierIdNo,Status", "hypos_Supplier", " Status = 0", " CreateDate Desc", null, null, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count > 0)
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				dataGridView1.Rows.Add(false, dataTable.Rows[i]["SupplierIdNo"].ToString(), dataTable.Rows[i]["SupplierName"].ToString(), dataTable.Rows[i]["SupplierNo"].ToString());
			}
		}
		if (dataGridView1.CurrentCell != null)
		{
			dataGridView1.ClearSelection();
			dataGridView1.CurrentRow.Selected = false;
		}
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
				dataGridView1.Rows.Add(false, dataTable.Rows[i]["SupplierIdNo"].ToString(), dataTable.Rows[i]["SupplierName"].ToString(), dataTable.Rows[i]["SupplierNo"].ToString());
			}
		}
		if (dataGridView1.CurrentCell != null)
		{
			dataGridView1.ClearSelection();
			dataGridView1.CurrentRow.Selected = false;
		}
	}

	private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
	{
		if (e.RowIndex >= 0 && !bool.Parse(dataGridView1[0, e.RowIndex].Value.ToString()))
		{
			dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(252, 252, 237);
		}
	}

	private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex >= 0 && !bool.Parse(dataGridView1[0, e.RowIndex].Value.ToString()))
		{
			dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
		}
	}

	private void tb_keyword_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			btn_quickSearch_Click(sender, e);
		}
	}

	private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex >= 0)
		{
			bool flag = bool.Parse(dataGridView1[0, e.RowIndex].Value.ToString());
			if (flag)
			{
				dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
			}
			else
			{
				dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 192);
			}
			dataGridView1[0, e.RowIndex].Value = !flag;
			dataGridView1.Refresh();
			dataGridView1.RefreshEdit();
		}
	}

	private void dataGridView1_SelectionChanged(object sender, EventArgs e)
	{
		dataGridView1.ClearSelection();
	}

	private void btn_SelectedCallBack_Click(object sender, EventArgs e)
	{
		if (dataGridView1.Rows.Count > 0)
		{
			foreach (DataGridViewRow item in (IEnumerable)dataGridView1.Rows)
			{
				if (bool.Parse(item.Cells[0].Value.ToString()))
				{
					returnSupplierNo.Add(item.Cells[3].Value.ToString());
					returnSupplierName.Add(item.Cells[2].Value.ToString());
				}
			}
			base.DialogResult = DialogResult.OK;
		}
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		btn_cancel = new System.Windows.Forms.Button();
		l_title = new System.Windows.Forms.Label();
		btn_quickSearch = new System.Windows.Forms.Button();
		tb_keyword = new System.Windows.Forms.TextBox();
		label1 = new System.Windows.Forms.Label();
		dataGridView1 = new System.Windows.Forms.DataGridView();
		btn_SelectedCallBack = new System.Windows.Forms.Button();
		select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		hidden_SupplierNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
		SuspendLayout();
		btn_cancel.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_cancel.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_cancel.ForeColor = System.Drawing.Color.White;
		btn_cancel.Location = new System.Drawing.Point(472, 489);
		btn_cancel.Name = "btn_cancel";
		btn_cancel.Size = new System.Drawing.Size(124, 34);
		btn_cancel.TabIndex = 46;
		btn_cancel.TabStop = false;
		btn_cancel.Text = "取消選擇";
		btn_cancel.UseVisualStyleBackColor = false;
		btn_cancel.Click += new System.EventHandler(btn_cancel_Click);
		l_title.AutoSize = true;
		l_title.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_title.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		l_title.Location = new System.Drawing.Point(180, 37);
		l_title.Name = "l_title";
		l_title.Size = new System.Drawing.Size(105, 24);
		l_title.TabIndex = 52;
		l_title.Text = "選擇供應商";
		l_title.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		btn_quickSearch.BackColor = System.Drawing.Color.FromArgb(34, 159, 208);
		btn_quickSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_quickSearch.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_quickSearch.ForeColor = System.Drawing.Color.White;
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
		tb_keyword.Text = "請輸入供應商代碼或關鍵字";
		tb_keyword.KeyDown += new System.Windows.Forms.KeyEventHandler(tb_keyword_KeyDown);
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		label1.Location = new System.Drawing.Point(299, 39);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(125, 20);
		label1.TabIndex = 52;
		label1.Text = "供應商快速篩選 ";
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
		dataGridView1.Columns.AddRange(select, Column2, Column3, hidden_SupplierNo);
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(6);
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
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
		dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dataGridView1_CellClick);
		dataGridView1.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(dataGridView1_CellMouseLeave);
		dataGridView1.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dataGridView1_CellMouseMove);
		dataGridView1.SelectionChanged += new System.EventHandler(dataGridView1_SelectionChanged);
		btn_SelectedCallBack.BackColor = System.Drawing.Color.FromArgb(125, 156, 35);
		btn_SelectedCallBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_SelectedCallBack.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_SelectedCallBack.ForeColor = System.Drawing.Color.White;
		btn_SelectedCallBack.Location = new System.Drawing.Point(333, 489);
		btn_SelectedCallBack.Name = "btn_SelectedCallBack";
		btn_SelectedCallBack.Size = new System.Drawing.Size(124, 34);
		btn_SelectedCallBack.TabIndex = 46;
		btn_SelectedCallBack.TabStop = false;
		btn_SelectedCallBack.Text = "選擇勾選";
		btn_SelectedCallBack.UseVisualStyleBackColor = false;
		btn_SelectedCallBack.Click += new System.EventHandler(btn_SelectedCallBack_Click);
		select.HeaderText = "勾選";
		select.Name = "select";
		select.ReadOnly = true;
		Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
		Column2.HeaderText = "統一編號";
		Column2.Name = "Column2";
		Column2.ReadOnly = true;
		Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column2.Width = 150;
		Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		Column3.HeaderText = "供應商名稱";
		Column3.Name = "Column3";
		Column3.ReadOnly = true;
		Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		hidden_SupplierNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
		hidden_SupplierNo.HeaderText = "系統供應商編號";
		hidden_SupplierNo.Name = "hidden_SupplierNo";
		hidden_SupplierNo.ReadOnly = true;
		hidden_SupplierNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		hidden_SupplierNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		hidden_SupplierNo.Visible = false;
		hidden_SupplierNo.Width = 150;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		AutoScroll = true;
		BackColor = System.Drawing.Color.Silver;
		base.ClientSize = new System.Drawing.Size(904, 576);
		base.ControlBox = false;
		base.Controls.Add(dataGridView1);
		base.Controls.Add(btn_quickSearch);
		base.Controls.Add(tb_keyword);
		base.Controls.Add(label1);
		base.Controls.Add(l_title);
		base.Controls.Add(btn_SelectedCallBack);
		base.Controls.Add(btn_cancel);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "dialogMultiChooseSupplier";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "frmSearchMember";
		base.Load += new System.EventHandler(dialogChooseSupplier_Load);
		((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}
}
