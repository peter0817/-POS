using POS_Client;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogSellPriceLog : Form
{
	private string _GDSNO = "";

	private IContainer components;

	private Button btn_cancel;

	public Label l_title;

	private TableLayoutPanel tableLayoutPanel2;

	private Panel panel22;

	private Label label33;

	private TableLayoutPanel tableLayoutPanel1;

	private Panel panel1;

	private TextBox tb_newPrice;

	private Button button1;

	public dialogSellPriceLog(string GDSNO)
	{
		InitializeComponent();
		_GDSNO = GDSNO;
	}

	private void dialogSellPriceLog_Load(object sender, EventArgs e)
	{
		if (tableLayoutPanel1.HasChildren)
		{
			tableLayoutPanel1.Controls.Clear();
		}
		Label label = new Label();
		label.Text = "編修日期時間";
		label.ForeColor = Color.White;
		label.BackColor = Color.Transparent;
		label.Dock = DockStyle.Fill;
		label.Anchor = AnchorStyles.None;
		label.AutoSize = true;
		Label label2 = new Label();
		label2.Text = "售價";
		label2.ForeColor = Color.White;
		label2.BackColor = Color.Transparent;
		label2.Dock = DockStyle.Fill;
		label2.Anchor = AnchorStyles.None;
		label2.AutoSize = true;
		Label label3 = new Label();
		label3.Text = "編修帳號";
		label3.ForeColor = Color.White;
		label3.BackColor = Color.Transparent;
		label3.Dock = DockStyle.Fill;
		label3.Anchor = AnchorStyles.None;
		label3.AutoSize = true;
		Label label4 = new Label();
		label4.Text = "狀態";
		label4.ForeColor = Color.White;
		label4.BackColor = Color.Transparent;
		label4.Dock = DockStyle.Fill;
		label4.Anchor = AnchorStyles.None;
		label4.AutoSize = true;
		tableLayoutPanel1.Controls.Add(label, 0, 0);
		tableLayoutPanel1.Controls.Add(label2, 1, 0);
		tableLayoutPanel1.Controls.Add(label3, 2, 0);
		tableLayoutPanel1.Controls.Add(label4, 3, 0);
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "*", "hypos_SellPrice_log", "GDSNO = {0}", " editDate desc", null, new string[1]
		{
			_GDSNO
		}, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count > 0)
		{
			tableLayoutPanel1.RowCount = dataTable.Rows.Count + 1;
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				Label label5 = new Label();
				label5.Text = dataTable.Rows[i]["editDate"].ToString();
				label5.BackColor = Color.Transparent;
				label5.Anchor = AnchorStyles.Left;
				label5.AutoSize = true;
				Label label6 = new Label();
				label6.Text = dataTable.Rows[i]["price"].ToString();
				label6.BackColor = Color.Transparent;
				label6.Anchor = AnchorStyles.Right;
				label6.AutoSize = true;
				Label label7 = new Label();
				label7.Text = dataTable.Rows[i]["Account"].ToString();
				label7.BackColor = Color.Transparent;
				label7.Anchor = AnchorStyles.Left;
				label7.AutoSize = true;
				Label label8 = new Label();
				label8.Text = dataTable.Rows[i]["status"].ToString();
				label8.BackColor = Color.Transparent;
				label8.Anchor = AnchorStyles.Left;
				label8.AutoSize = true;
				tableLayoutPanel1.Controls.Add(label5, 0, i + 1);
				tableLayoutPanel1.Controls.Add(label6, 1, i + 1);
				tableLayoutPanel1.Controls.Add(label7, 2, i + 1);
				tableLayoutPanel1.Controls.Add(label8, 3, i + 1);
				tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 38f));
			}
			if (dataTable.Rows.Count > 7)
			{
				tableLayoutPanel1.AutoScroll = true;
				tableLayoutPanel1.Padding = new Padding(0, 0, 13, 0);
			}
			else
			{
				tableLayoutPanel1.AutoScroll = false;
				tableLayoutPanel1.Padding = new Padding(0, 0, 0, 0);
			}
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
			DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_SellPrice_log set status = 'N' where GDSNO = {0}", new string[1]
			{
				_GDSNO
			}, CommandOperationType.ExecuteNonQuery);
			DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_GOODSLST set Price = {1} where GDSNO = {0}", new string[2]
			{
				_GDSNO,
				text
			}, CommandOperationType.ExecuteNonQuery);
			string[,] strFieldArray = new string[4, 2]
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
				}
			};
			DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_SellPrice_log", null, null, strFieldArray, null, CommandOperationType.ExecuteNonQuery);
			if (base.Owner != null)
			{
				(base.Owner as frmEditCommodity).tb_Price.Text = text;
				(base.Owner as frmEditCommodity)._Price = text;
			}
			dialogSellPriceLog_Load(sender, e);
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
		if (tb_newPrice.Text == "請輸入新售價")
		{
			tb_newPrice.Text = "";
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
		btn_cancel = new System.Windows.Forms.Button();
		l_title = new System.Windows.Forms.Label();
		tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		panel22 = new System.Windows.Forms.Panel();
		label33 = new System.Windows.Forms.Label();
		panel1 = new System.Windows.Forms.Panel();
		button1 = new System.Windows.Forms.Button();
		tb_newPrice = new System.Windows.Forms.TextBox();
		tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		tableLayoutPanel2.SuspendLayout();
		panel22.SuspendLayout();
		panel1.SuspendLayout();
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
		tableLayoutPanel1.AutoScroll = true;
		tableLayoutPanel1.AutoSize = true;
		tableLayoutPanel1.BackColor = System.Drawing.Color.White;
		tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		tableLayoutPanel1.ColumnCount = 4;
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.08112f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.51838f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.3891f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.01141f));
		tableLayoutPanel1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tableLayoutPanel1.Location = new System.Drawing.Point(50, 144);
		tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		tableLayoutPanel1.MaximumSize = new System.Drawing.Size(973, 310);
		tableLayoutPanel1.Name = "tableLayoutPanel1";
		tableLayoutPanel1.RowCount = 1;
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42f));
		tableLayoutPanel1.Size = new System.Drawing.Size(792, 44);
		tableLayoutPanel1.TabIndex = 54;
		tableLayoutPanel1.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(tableLayoutPanel1_CellPaint);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		AutoScroll = true;
		BackColor = System.Drawing.Color.Silver;
		base.ClientSize = new System.Drawing.Size(904, 576);
		base.ControlBox = false;
		base.Controls.Add(tableLayoutPanel1);
		base.Controls.Add(tableLayoutPanel2);
		base.Controls.Add(l_title);
		base.Controls.Add(btn_cancel);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "dialogSellPriceLog";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "frmSearchMember";
		base.Load += new System.EventHandler(dialogSellPriceLog_Load);
		tableLayoutPanel2.ResumeLayout(false);
		panel22.ResumeLayout(false);
		panel22.PerformLayout();
		panel1.ResumeLayout(false);
		panel1.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}
}
