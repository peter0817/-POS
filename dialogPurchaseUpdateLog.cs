using POS_Client;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogPurchaseUpdateLog : Form
{
	private string _PurchaseNo;

	private IContainer components;

	private Button btn_cancel;

	public Label l_title;

	private DataGridView dataGridView1;

	private DataGridViewTextBoxColumn Column1;

	private DataGridViewTextBoxColumn Column2;

	private DataGridViewTextBoxColumn Column3;

	public dialogPurchaseUpdateLog(string PurchaseNo)
	{
		InitializeComponent();
		_PurchaseNo = PurchaseNo;
	}

	private void dialogPurchaseUpdateLog_Load(object sender, EventArgs e)
	{
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "*", "hypos_PurchaseGoods_Detail_Log l, hypos_GOODSLST g", " l.GDSNO = g.GDSNO and PurchaseNo = {0}", " updateDate Desc", null, new string[1]
		{
			_PurchaseNo
		}, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
		string text = "";
		string text2 = "";
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			if (string.IsNullOrEmpty(text))
			{
				text = dataTable.Rows[i]["updateDate"].ToString();
			}
			if (!text.Equals(dataTable.Rows[i]["updateDate"].ToString()))
			{
				dataGridView1.Rows.Add(dataGridView1.RowCount + 1, text, text2);
				text2 = "";
				text = dataTable.Rows[i]["updateDate"].ToString();
			}
			string text3 = (Math.Sign(int.Parse(dataTable.Rows[i]["adjustQuantity"].ToString())) >= 0) ? ("+" + dataTable.Rows[i]["adjustQuantity"].ToString()) : dataTable.Rows[i]["adjustQuantity"].ToString();
			string text4 = (Math.Sign(int.Parse(dataTable.Rows[i]["adjustMoney"].ToString())) >= 0) ? ("+" + dataTable.Rows[i]["adjustMoney"].ToString()) : dataTable.Rows[i]["adjustMoney"].ToString();
			string text5 = "";
			string a = dataTable.Rows[i]["adjustType"].ToString();
			if (!(a == "E"))
			{
				if (a == "F")
				{
					text5 = "(過期退回)";
				}
			}
			else
			{
				text5 = "(原廠退貨)";
			}
			text2 = text2 + dataTable.Rows[i]["GDSNO"].ToString() + "\n" + getCommodityName(dataTable.Rows[i]) + "(" + text3 + "/" + text4 + ")" + text5 + "\n";
		}
		if (!string.IsNullOrEmpty(text2))
		{
			dataGridView1.Rows.Add(dataGridView1.RowCount + 1, text, text2);
			text2 = "";
		}
	}

	private string getCommodityName(DataRow row)
	{
		string text = row["GDName"].ToString();
		string text2 = row["formCode"].ToString();
		string text3 = row["CName"].ToString();
		string str = row["spec"].ToString();
		string str2 = row["capacity"].ToString();
		string[] array = new string[2]
		{
			text3,
			text2
		};
		if (!string.IsNullOrEmpty(text2) || !string.IsNullOrEmpty(text3))
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
				text = text.Substring(0, text.LastIndexOf("-")) + "]";
			}
		}
		text = text + " " + str;
		return text + " " + str2;
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
		dataGridView1 = new System.Windows.Forms.DataGridView();
		Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
		SuspendLayout();
		btn_cancel.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_cancel.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_cancel.ForeColor = System.Drawing.Color.White;
		btn_cancel.Location = new System.Drawing.Point(390, 514);
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
		l_title.Location = new System.Drawing.Point(381, 26);
		l_title.Name = "l_title";
		l_title.Size = new System.Drawing.Size(143, 24);
		l_title.TabIndex = 52;
		l_title.Text = "進貨單變更記錄";
		l_title.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		dataGridView1.AllowUserToAddRows = false;
		dataGridView1.AllowUserToDeleteRows = false;
		dataGridView1.AllowUserToResizeColumns = false;
		dataGridView1.AllowUserToResizeRows = false;
		dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
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
		dataGridView1.Columns.AddRange(Column1, Column2, Column3);
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(6);
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
		dataGridView1.EnableHeadersVisualStyles = false;
		dataGridView1.Location = new System.Drawing.Point(47, 63);
		dataGridView1.Name = "dataGridView1";
		dataGridView1.RowHeadersVisible = false;
		dataGridView1.RowTemplate.Height = 35;
		dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		dataGridView1.Size = new System.Drawing.Size(811, 431);
		dataGridView1.TabIndex = 53;
		Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
		Column1.HeaderText = "項次";
		Column1.MinimumWidth = 60;
		Column1.Name = "Column1";
		Column1.ReadOnly = true;
		Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column1.Width = 60;
		Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
		Column2.HeaderText = "編修時間";
		Column2.MinimumWidth = 200;
		Column2.Name = "Column2";
		Column2.ReadOnly = true;
		Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column2.Width = 200;
		Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		Column3.HeaderText = "編修內容";
		Column3.Name = "Column3";
		Column3.ReadOnly = true;
		Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		AutoScroll = true;
		BackColor = System.Drawing.Color.Silver;
		base.ClientSize = new System.Drawing.Size(904, 576);
		base.ControlBox = false;
		base.Controls.Add(dataGridView1);
		base.Controls.Add(l_title);
		base.Controls.Add(btn_cancel);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "dialogPurchaseUpdateLog";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "dialogPurchaseUpdateLog";
		base.Load += new System.EventHandler(dialogPurchaseUpdateLog_Load);
		((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}
}
