using POS_Client;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogDeliveryPriceLog : Form
{
	private string _GDSNO = "";

	private IContainer components;

	private Button btn_cancel;

	public Label l_title;

	private TableLayoutPanel tableLayoutPanel2;

	private Panel panel22;

	private Label label33;

	private Panel panel1;

	private TextBox tb_newPrice;

	private Button button1;

	private DataGridView dataGridView2;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;

	public dialogDeliveryPriceLog(string GDSNO)
	{
		InitializeComponent();
		_GDSNO = GDSNO;
	}

	private void dialogDeliveryPriceLog_Load(object sender, EventArgs e)
	{
		dataGridView2.Rows.Clear();
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "*", "hypos_DeliveryPrice_log", "GDSNO = {0}", " editDate desc", null, new string[1]
		{
			_GDSNO
		}, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			string text = "";
			string a = dataTable.Rows[i]["status"].ToString();
			if (!(a == "Y"))
			{
				if (a == "N")
				{
					text = "";
				}
			}
			else
			{
				text = "使用中";
			}
			dataGridView2.Rows.Insert(i, dataTable.Rows[i]["editDate"].ToString(), dataTable.Rows[i]["price"].ToString(), dataTable.Rows[i]["Account"].ToString(), text);
		}
	}

	private void btn_cancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btn_UpdatePrice_Click(object sender, EventArgs e)
	{
		int result = 0;
		string text = tb_newPrice.Text.Trim();
		if (int.TryParse(tb_newPrice.Text.Trim(), out result))
		{
			DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_DeliveryPrice_log set status = 'N' where GDSNO = {0}", new string[1]
			{
				_GDSNO
			}, CommandOperationType.ExecuteNonQuery);
			DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_GOODSLST set DeliveryPrice = {1} where GDSNO = {0}", new string[2]
			{
				_GDSNO,
				text
			}, CommandOperationType.ExecuteNonQuery);
			string[,] strFieldArray = new string[5, 2]
			{
				{
					"GDSNO",
					_GDSNO
				},
				{
					"Account",
					Program.Casher
				},
				{
					"price",
					text
				},
				{
					"status",
					"Y"
				},
				{
					"editDate",
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
				}
			};
			DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_DeliveryPrice_log", null, null, strFieldArray, null, CommandOperationType.ExecuteNonQuery);
			if (base.Owner != null)
			{
				(base.Owner as frmEditCommodity).textBox1.Text = text;
				(base.Owner as frmEditCommodity)._DeliveryPrice = text;
			}
			dialogDeliveryPriceLog_Load(sender, e);
		}
	}

	private void digitOnly_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = (!char.IsDigit(e.KeyChar) && !e.KeyChar.Equals('\b'));
	}

	private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
	{
		if (e.Row == 0)
		{
			e.Graphics.FillRectangle(Brushes.DarkGray, e.CellBounds);
		}
	}

	private void tb_newPrice_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.KeyValue == 13)
		{
			btn_UpdatePrice_Click(sender, e);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		btn_UpdatePrice_Click(sender, e);
	}

	private void tb_newPrice_MouseClick(object sender, MouseEventArgs e)
	{
		tb_newPrice.Text = "";
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
		btn_cancel = new System.Windows.Forms.Button();
		l_title = new System.Windows.Forms.Label();
		tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		panel22 = new System.Windows.Forms.Panel();
		label33 = new System.Windows.Forms.Label();
		panel1 = new System.Windows.Forms.Panel();
		button1 = new System.Windows.Forms.Button();
		tb_newPrice = new System.Windows.Forms.TextBox();
		dataGridView2 = new System.Windows.Forms.DataGridView();
		dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		tableLayoutPanel2.SuspendLayout();
		panel22.SuspendLayout();
		panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
		SuspendLayout();
		btn_cancel.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_cancel.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_cancel.ForeColor = System.Drawing.Color.White;
		btn_cancel.Location = new System.Drawing.Point(390, 493);
		btn_cancel.Name = "btn_cancel";
		btn_cancel.Size = new System.Drawing.Size(124, 34);
		btn_cancel.TabIndex = 46;
		btn_cancel.TabStop = false;
		btn_cancel.Text = "關閉";
		btn_cancel.UseVisualStyleBackColor = false;
		btn_cancel.Click += new System.EventHandler(btn_cancel_Click);
		l_title.AutoSize = true;
		l_title.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_title.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		l_title.Location = new System.Drawing.Point(364, 110);
		l_title.Name = "l_title";
		l_title.Size = new System.Drawing.Size(162, 24);
		l_title.TabIndex = 52;
		l_title.Text = "近期調整售價一覽";
		l_title.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		tableLayoutPanel2.BackColor = System.Drawing.Color.White;
		tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		tableLayoutPanel2.ColumnCount = 2;
		tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 162f));
		tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		tableLayoutPanel2.Controls.Add(panel22, 0, 0);
		tableLayoutPanel2.Controls.Add(panel1, 1, 0);
		tableLayoutPanel2.Location = new System.Drawing.Point(120, 27);
		tableLayoutPanel2.Name = "tableLayoutPanel2";
		tableLayoutPanel2.RowCount = 1;
		tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.23292f));
		tableLayoutPanel2.Size = new System.Drawing.Size(607, 50);
		tableLayoutPanel2.TabIndex = 53;
		panel22.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel22.Controls.Add(label33);
		panel22.Dock = System.Windows.Forms.DockStyle.Fill;
		panel22.Location = new System.Drawing.Point(1, 1);
		panel22.Margin = new System.Windows.Forms.Padding(0);
		panel22.Name = "panel22";
		panel22.Size = new System.Drawing.Size(162, 48);
		panel22.TabIndex = 19;
		label33.AutoSize = true;
		label33.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label33.ForeColor = System.Drawing.Color.White;
		label33.Location = new System.Drawing.Point(53, 13);
		label33.Name = "label33";
		label33.Size = new System.Drawing.Size(106, 21);
		label33.TabIndex = 0;
		label33.Text = "設定新的售價";
		panel1.Controls.Add(button1);
		panel1.Controls.Add(tb_newPrice);
		panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		panel1.Location = new System.Drawing.Point(167, 4);
		panel1.Name = "panel1";
		panel1.Size = new System.Drawing.Size(436, 42);
		panel1.TabIndex = 20;
		button1.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		button1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		button1.ForeColor = System.Drawing.Color.White;
		button1.Location = new System.Drawing.Point(263, 5);
		button1.Name = "button1";
		button1.Size = new System.Drawing.Size(124, 34);
		button1.TabIndex = 55;
		button1.TabStop = false;
		button1.Text = "確認";
		button1.UseVisualStyleBackColor = false;
		button1.Click += new System.EventHandler(button1_Click);
		tb_newPrice.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_newPrice.ImeMode = System.Windows.Forms.ImeMode.Disable;
		tb_newPrice.Location = new System.Drawing.Point(12, 7);
		tb_newPrice.Name = "tb_newPrice";
		tb_newPrice.Size = new System.Drawing.Size(227, 29);
		tb_newPrice.TabIndex = 20;
		tb_newPrice.Text = "請輸入新售價";
		tb_newPrice.MouseClick += new System.Windows.Forms.MouseEventHandler(tb_newPrice_MouseClick);
		tb_newPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(digitOnly_KeyPress);
		tb_newPrice.KeyUp += new System.Windows.Forms.KeyEventHandler(tb_newPrice_KeyUp);
		dataGridView2.AllowUserToAddRows = false;
		dataGridView2.AllowUserToDeleteRows = false;
		dataGridView2.AllowUserToResizeColumns = false;
		dataGridView2.AllowUserToResizeRows = false;
		dataGridView2.BackgroundColor = System.Drawing.Color.White;
		dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
		dataGridView2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		dataGridViewCellStyle.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle.ForeColor = System.Drawing.Color.White;
		dataGridViewCellStyle.Padding = new System.Windows.Forms.Padding(3);
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
		dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		dataGridView2.Columns.AddRange(dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5);
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		dataGridView2.DefaultCellStyle = dataGridViewCellStyle2;
		dataGridView2.EnableHeadersVisualStyles = false;
		dataGridView2.Location = new System.Drawing.Point(26, 150);
		dataGridView2.Name = "dataGridView2";
		dataGridView2.ReadOnly = true;
		dataGridView2.RowHeadersVisible = false;
		dataGridView2.RowTemplate.Height = 35;
		dataGridView2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		dataGridView2.Size = new System.Drawing.Size(855, 321);
		dataGridView2.TabIndex = 61;
		dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
		dataGridViewTextBoxColumn1.HeaderText = "編修日期時間";
		dataGridViewTextBoxColumn1.MinimumWidth = 150;
		dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
		dataGridViewTextBoxColumn1.ReadOnly = true;
		dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		dataGridViewTextBoxColumn1.Width = 230;
		dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle3;
		dataGridViewTextBoxColumn3.HeaderText = "售價";
		dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
		dataGridViewTextBoxColumn3.ReadOnly = true;
		dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		dataGridViewTextBoxColumn3.Width = 325;
		dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle4;
		dataGridViewTextBoxColumn4.HeaderText = "編修帳號";
		dataGridViewTextBoxColumn4.MinimumWidth = 100;
		dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
		dataGridViewTextBoxColumn4.ReadOnly = true;
		dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		dataGridViewTextBoxColumn4.Width = 160;
		dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle5;
		dataGridViewTextBoxColumn5.HeaderText = "狀態";
		dataGridViewTextBoxColumn5.MinimumWidth = 60;
		dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
		dataGridViewTextBoxColumn5.ReadOnly = true;
		dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		dataGridViewTextBoxColumn5.Width = 140;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		AutoScroll = true;
		BackColor = System.Drawing.Color.Silver;
		base.ClientSize = new System.Drawing.Size(904, 576);
		base.ControlBox = false;
		base.Controls.Add(dataGridView2);
		base.Controls.Add(tableLayoutPanel2);
		base.Controls.Add(l_title);
		base.Controls.Add(btn_cancel);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "dialogDeliveryPriceLog";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "frmSearchMember";
		base.Load += new System.EventHandler(dialogDeliveryPriceLog_Load);
		tableLayoutPanel2.ResumeLayout(false);
		panel22.ResumeLayout(false);
		panel22.PerformLayout();
		panel1.ResumeLayout(false);
		panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}
}
