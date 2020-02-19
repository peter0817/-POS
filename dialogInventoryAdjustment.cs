using POS_Client;
using POS_Client.Properties;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogInventoryAdjustment : Form
{
	private string _GDSNO = "";

	private string _strAdjustPlusOrMinus = "minus";

	private IContainer components;

	private Label l_GDSNO;

	private Label l_goodsInfo;

	private DataGridView dataGridView1;

	private FlowLayoutPanel flowLayoutPanel2;

	private Label l_goodsContent;

	private FlowLayoutPanel flowLayoutPanel1;

	private ComboBox cb_adjustType;

	private TextBox tb_adjustCount;

	private Button btn_enter;

	private Label label3;

	private Label label2;

	private Panel panel3;

	private Label label1;

	private Panel panel2;

	private Label label33;

	private Panel panel22;

	private TableLayoutPanel tableLayoutPanel2;

	public Label l_title;

	private Button btn_close;

	private DataGridViewTextBoxColumn Column1;

	private DataGridViewTextBoxColumn Column2;

	private DataGridViewTextBoxColumn Column3;

	public Label l_inventoryCountTotal;

	private Button btn_plus;

	private Button btn_minus;

	public dialogInventoryAdjustment(string GDSNO, string total)
	{
		InitializeComponent();
		_GDSNO = GDSNO;
		l_GDSNO.Text = GDSNO;
		l_inventoryCountTotal.Text = total;
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, "SELECT * FROM hypos_GOODSLST WHERE GDSNO = {0}", new string[1]
		{
			GDSNO
		}, CommandOperationType.ExecuteReaderReturnDataTable);
		l_goodsInfo.Text = setCommodityName(dataTable.Rows[0]);
		l_goodsContent.Text = string.Format("({0} {1})", dataTable.Rows[0]["spec"].ToString(), dataTable.Rows[0]["capacity"].ToString());
		DataTable dataTable2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, "SELECT * FROM hypos_InventoryAdjustment WHERE GDSNO = {0} ORDER BY updateDate DESC", new string[1]
		{
			GDSNO
		}, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable2.Rows.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < dataTable2.Rows.Count; i++)
		{
			string text = "";
			switch (dataTable2.Rows[i]["adjustType"].ToString())
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
				text = "原廠退貨";
				break;
			case "F":
				text = "過期退回";
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
			case "0":
				text = "店內盤點";
				break;
			}
			dataGridView1.Rows.Add(dataTable2.Rows[i]["updateDate"].ToString(), text, dataTable2.Rows[i]["adjustCount"].ToString());
		}
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
			if (text.LastIndexOf("-") > 0)
			{
				text = text.Substring(0, text.LastIndexOf("-")) + "]";
			}
		}
		return text;
	}

	private void dialogSellPriceLog_Load(object sender, EventArgs e)
	{
		ComboboxItem[] items = new ComboboxItem[8]
		{
			new ComboboxItem("請選擇調整理由", ""),
			new ComboboxItem("破損", "A"),
			new ComboboxItem("盤減", "B"),
			new ComboboxItem("盤增", "C"),
			new ComboboxItem("過期銷毀", "G"),
			new ComboboxItem("資料異常", "H"),
			new ComboboxItem("其他", "X"),
			new ComboboxItem("店內盤點", "0")
		};
		cb_adjustType.Items.AddRange(items);
		cb_adjustType.SelectedIndex = 0;
	}

	public static string getNewAdjustNo()
	{
		string sql = "SELECT AdjustNo FROM hypos_InventoryAdjustment order by AdjustNo desc limit 1";
		string text = Convert.ToString(DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteScalar));
		string text2 = DateTime.Now.Year.ToString().Substring(2, 2) + $"{DateTime.Now.Month:00}";
		if ("-1".Equals(text) || string.IsNullOrEmpty(text))
		{
			return $"{Program.SiteNo}{text2}0001";
		}
		string value = text.Substring(2, 4);
		if (!text2.Equals(value))
		{
			return $"{Program.SiteNo}{text2}0001";
		}
		string arg = $"{int.Parse(text.Substring(6, 4)) + 1:0000}";
		return $"{Program.SiteNo}{text2}{arg}";
	}

	private void btn_close_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btn_enter_Click(object sender, EventArgs e)
	{
		string text = "";
		string text2 = (cb_adjustType.SelectedItem as ComboboxItem).Value.ToString();
		string text3 = (cb_adjustType.SelectedItem as ComboboxItem).Text;
		if (string.IsNullOrEmpty(text2))
		{
			text += "請選擇調整理由\n";
		}
		if ("請輸入調整數字".Equals(tb_adjustCount.Text) || string.IsNullOrEmpty(tb_adjustCount.Text))
		{
			text += "請輸入調整數字\n";
		}
		if (!string.IsNullOrEmpty(text))
		{
			AutoClosingMessageBox.Show(text);
		}
		else
		{
			try
			{
				string text4 = tb_adjustCount.Text;
				if (_strAdjustPlusOrMinus.Equals("minus"))
				{
					text4 = "-" + tb_adjustCount.Text;
				}
				else if (_strAdjustPlusOrMinus.Equals("plus"))
				{
					text4 = "+" + tb_adjustCount.Text;
				}
				string text5 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				l_inventoryCountTotal.Text = (int.Parse(l_inventoryCountTotal.Text) + int.Parse(text4)).ToString();
				dataGridView1.Rows.Insert(0, text5, text3, text4);
				DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_GOODSLST set inventory = {1} WHERE GDSNO = {0}", new string[2]
				{
					_GDSNO,
					l_inventoryCountTotal.Text
				}, CommandOperationType.ExecuteNonQuery);
				string[,] strFieldArray = new string[6, 2]
				{
					{
						"AdjustNo",
						getNewAdjustNo()
					},
					{
						"GDSNO",
						_GDSNO
					},
					{
						"adjustType",
						text2
					},
					{
						"adjustCount",
						text4
					},
					{
						"updateDate",
						text5
					},
					{
						"GoodsTotalCountLog",
						l_inventoryCountTotal.Text
					}
				};
				DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_InventoryAdjustment", null, null, strFieldArray, null, CommandOperationType.ExecuteNonQuery);
				string[,] strFieldArray2 = new string[1, 2]
				{
					{
						"UpdateDate",
						DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
					}
				};
				DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Update, "", "hypos_GOODSLST", " GDSNO = {0} ", "", strFieldArray2, new string[1]
				{
					_GDSNO
				}, CommandOperationType.ExecuteNonQuery);
				if (text2.Equals("0"))
				{
					AutoClosingMessageBox.Show("此調整不會回傳防檢局，僅供店內管理使用");
				}
				cb_adjustType.SelectedIndex = 0;
				tb_adjustCount.Text = "";
			}
			catch (FormatException)
			{
				AutoClosingMessageBox.Show("金額格式錯誤");
			}
			catch (Exception)
			{
				MessageBox.Show("調整錯誤");
			}
		}
	}

	private void digitOnly_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = (!char.IsDigit(e.KeyChar) && !e.KeyChar.Equals('\b'));
	}

	private void tb_adjustCount_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.KeyValue == 13)
		{
			btn_enter_Click(sender, e);
		}
	}

	private void tb_adjustCount_Enter(object sender, EventArgs e)
	{
		if ("請輸入調整數字".Equals(tb_adjustCount.Text))
		{
			tb_adjustCount.Text = "";
		}
	}

	private void tb_adjustCount_Leave(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(tb_adjustCount.Text))
		{
			tb_adjustCount.Text = "請輸入調整數字";
		}
	}

	private void btn_minus_Click(object sender, EventArgs e)
	{
		FocusOnbtnMinus();
	}

	private void btn_plus_Click(object sender, EventArgs e)
	{
		FocusOnbtnPlus();
	}

	private void FocusOnbtnPlus()
	{
		if (_strAdjustPlusOrMinus.Equals("minus"))
		{
			_strAdjustPlusOrMinus = "plus";
			btn_minus.BackColor = Color.Gainsboro;
			btn_plus.BackColor = SystemColors.ButtonShadow;
		}
	}

	private void FocusOnbtnMinus()
	{
		if (_strAdjustPlusOrMinus.Equals("plus"))
		{
			_strAdjustPlusOrMinus = "minus";
			btn_minus.BackColor = SystemColors.ButtonShadow;
			btn_plus.BackColor = Color.Gainsboro;
		}
	}

	private void cb_adjustType_SelectedValueChanged(object sender, EventArgs e)
	{
		string text = (cb_adjustType.SelectedItem as ComboboxItem).Text;
		if (text.Equals("破損"))
		{
			FocusOnbtnMinus();
		}
		else if (text.Equals("盤減"))
		{
			FocusOnbtnMinus();
		}
		else if (text.Equals("盤增"))
		{
			FocusOnbtnPlus();
		}
		else if (text.Equals("過期銷毀"))
		{
			FocusOnbtnMinus();
		}
		else if (text.Equals("資料異常"))
		{
			FocusOnbtnMinus();
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
		l_GDSNO = new System.Windows.Forms.Label();
		l_goodsInfo = new System.Windows.Forms.Label();
		dataGridView1 = new System.Windows.Forms.DataGridView();
		Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
		l_goodsContent = new System.Windows.Forms.Label();
		flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		cb_adjustType = new System.Windows.Forms.ComboBox();
		tb_adjustCount = new System.Windows.Forms.TextBox();
		btn_enter = new System.Windows.Forms.Button();
		label3 = new System.Windows.Forms.Label();
		label2 = new System.Windows.Forms.Label();
		panel3 = new System.Windows.Forms.Panel();
		label1 = new System.Windows.Forms.Label();
		panel2 = new System.Windows.Forms.Panel();
		label33 = new System.Windows.Forms.Label();
		panel22 = new System.Windows.Forms.Panel();
		tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		l_inventoryCountTotal = new System.Windows.Forms.Label();
		l_title = new System.Windows.Forms.Label();
		btn_close = new System.Windows.Forms.Button();
		btn_minus = new System.Windows.Forms.Button();
		btn_plus = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
		flowLayoutPanel2.SuspendLayout();
		flowLayoutPanel1.SuspendLayout();
		panel3.SuspendLayout();
		panel2.SuspendLayout();
		panel22.SuspendLayout();
		tableLayoutPanel2.SuspendLayout();
		SuspendLayout();
		l_GDSNO.AutoSize = true;
		l_GDSNO.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		l_GDSNO.ForeColor = System.Drawing.Color.Gray;
		l_GDSNO.Location = new System.Drawing.Point(10, 5);
		l_GDSNO.Margin = new System.Windows.Forms.Padding(10, 5, 3, 0);
		l_GDSNO.Name = "l_GDSNO";
		l_GDSNO.Size = new System.Drawing.Size(28, 20);
		l_GDSNO.TabIndex = 55;
		l_GDSNO.Text = "{0}";
		l_goodsInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
		l_goodsInfo.AutoSize = true;
		l_goodsInfo.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		l_goodsInfo.Location = new System.Drawing.Point(10, 25);
		l_goodsInfo.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
		l_goodsInfo.Name = "l_goodsInfo";
		l_goodsInfo.Size = new System.Drawing.Size(28, 20);
		l_goodsInfo.TabIndex = 57;
		l_goodsInfo.Text = "{0}";
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
		dataGridView1.Columns.AddRange(Column1, Column2, Column3);
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
		dataGridView1.Location = new System.Drawing.Point(47, 235);
		dataGridView1.MultiSelect = false;
		dataGridView1.Name = "dataGridView1";
		dataGridView1.ReadOnly = true;
		dataGridView1.RowHeadersVisible = false;
		dataGridView1.RowTemplate.Height = 35;
		dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		dataGridView1.Size = new System.Drawing.Size(811, 260);
		dataGridView1.TabIndex = 58;
		Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
		Column1.HeaderText = "編修日期時間";
		Column1.MinimumWidth = 120;
		Column1.Name = "Column1";
		Column1.ReadOnly = true;
		Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column1.Width = 120;
		Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		Column2.HeaderText = "類型/理由";
		Column2.Name = "Column2";
		Column2.ReadOnly = true;
		Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
		Column3.HeaderText = "數量";
		Column3.Name = "Column3";
		Column3.ReadOnly = true;
		Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column3.Width = 56;
		flowLayoutPanel2.Controls.Add(l_GDSNO);
		flowLayoutPanel2.Controls.Add(l_goodsInfo);
		flowLayoutPanel2.Controls.Add(l_goodsContent);
		flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
		flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		flowLayoutPanel2.Location = new System.Drawing.Point(164, 1);
		flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
		flowLayoutPanel2.Name = "flowLayoutPanel2";
		flowLayoutPanel2.Size = new System.Drawing.Size(646, 71);
		flowLayoutPanel2.TabIndex = 23;
		l_goodsContent.AutoSize = true;
		l_goodsContent.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		l_goodsContent.ForeColor = System.Drawing.Color.Gray;
		l_goodsContent.Location = new System.Drawing.Point(10, 45);
		l_goodsContent.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
		l_goodsContent.Name = "l_goodsContent";
		l_goodsContent.Size = new System.Drawing.Size(28, 20);
		l_goodsContent.TabIndex = 56;
		l_goodsContent.Text = "{0}";
		flowLayoutPanel1.Controls.Add(cb_adjustType);
		flowLayoutPanel1.Controls.Add(btn_plus);
		flowLayoutPanel1.Controls.Add(btn_minus);
		flowLayoutPanel1.Controls.Add(tb_adjustCount);
		flowLayoutPanel1.Controls.Add(btn_enter);
		flowLayoutPanel1.Controls.Add(label3);
		flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		flowLayoutPanel1.Location = new System.Drawing.Point(164, 118);
		flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		flowLayoutPanel1.Name = "flowLayoutPanel1";
		flowLayoutPanel1.Size = new System.Drawing.Size(646, 63);
		flowLayoutPanel1.TabIndex = 22;
		cb_adjustType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cb_adjustType.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_adjustType.FormattingEnabled = true;
		cb_adjustType.Location = new System.Drawing.Point(10, 3);
		cb_adjustType.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
		cb_adjustType.Name = "cb_adjustType";
		cb_adjustType.Size = new System.Drawing.Size(166, 28);
		cb_adjustType.TabIndex = 0;
		cb_adjustType.SelectedValueChanged += new System.EventHandler(cb_adjustType_SelectedValueChanged);
		tb_adjustCount.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_adjustCount.Location = new System.Drawing.Point(274, 3);
		tb_adjustCount.Name = "tb_adjustCount";
		tb_adjustCount.Size = new System.Drawing.Size(311, 29);
		tb_adjustCount.TabIndex = 1;
		tb_adjustCount.Text = "請輸入調整數字";
		tb_adjustCount.Enter += new System.EventHandler(tb_adjustCount_Enter);
		tb_adjustCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(digitOnly_KeyPress);
		tb_adjustCount.KeyUp += new System.Windows.Forms.KeyEventHandler(tb_adjustCount_KeyUp);
		tb_adjustCount.Leave += new System.EventHandler(tb_adjustCount_Leave);
		btn_enter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_enter.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_enter.Image = POS_Client.Properties.Resources.ic_input_black_24dp_1x;
		btn_enter.Location = new System.Drawing.Point(591, 3);
		btn_enter.Name = "btn_enter";
		btn_enter.Size = new System.Drawing.Size(48, 29);
		btn_enter.TabIndex = 2;
		btn_enter.UseVisualStyleBackColor = true;
		btn_enter.Click += new System.EventHandler(btn_enter_Click);
		label3.AutoSize = true;
		label3.Font = new System.Drawing.Font("微軟正黑體", 9f);
		label3.ForeColor = System.Drawing.Color.Red;
		label3.Location = new System.Drawing.Point(10, 37);
		label3.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
		label3.Name = "label3";
		label3.Size = new System.Drawing.Size(600, 16);
		label3.TabIndex = 3;
		label3.Text = "*請輸入數字，正負數值請點選「＋ / －」按鈕，預設為「－」扣除。將依輸入正負數字增加或扣除目前庫存數字";
		label2.AutoSize = true;
		label2.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label2.ForeColor = System.Drawing.Color.White;
		label2.Location = new System.Drawing.Point(85, 21);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(74, 21);
		label2.TabIndex = 0;
		label2.Text = "庫存調整";
		panel3.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel3.Controls.Add(label2);
		panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		panel3.Location = new System.Drawing.Point(1, 118);
		panel3.Margin = new System.Windows.Forms.Padding(0);
		panel3.Name = "panel3";
		panel3.Size = new System.Drawing.Size(162, 63);
		panel3.TabIndex = 19;
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.Color.White;
		label1.Location = new System.Drawing.Point(85, 20);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(74, 21);
		label1.TabIndex = 0;
		label1.Text = "商品名稱";
		panel2.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel2.Controls.Add(label1);
		panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		panel2.Location = new System.Drawing.Point(1, 1);
		panel2.Margin = new System.Windows.Forms.Padding(0);
		panel2.Name = "panel2";
		panel2.Size = new System.Drawing.Size(162, 71);
		panel2.TabIndex = 19;
		label33.AutoSize = true;
		label33.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label33.ForeColor = System.Drawing.Color.White;
		label33.Location = new System.Drawing.Point(85, 13);
		label33.Name = "label33";
		label33.Size = new System.Drawing.Size(74, 21);
		label33.TabIndex = 0;
		label33.Text = "現品庫存";
		panel22.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel22.Controls.Add(label33);
		panel22.Dock = System.Windows.Forms.DockStyle.Fill;
		panel22.Location = new System.Drawing.Point(1, 73);
		panel22.Margin = new System.Windows.Forms.Padding(0);
		panel22.Name = "panel22";
		panel22.Size = new System.Drawing.Size(162, 44);
		panel22.TabIndex = 19;
		tableLayoutPanel2.BackColor = System.Drawing.Color.White;
		tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		tableLayoutPanel2.ColumnCount = 2;
		tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 162f));
		tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		tableLayoutPanel2.Controls.Add(panel22, 0, 1);
		tableLayoutPanel2.Controls.Add(panel2, 0, 0);
		tableLayoutPanel2.Controls.Add(panel3, 0, 2);
		tableLayoutPanel2.Controls.Add(l_inventoryCountTotal, 1, 1);
		tableLayoutPanel2.Controls.Add(flowLayoutPanel1, 1, 2);
		tableLayoutPanel2.Controls.Add(flowLayoutPanel2, 1, 0);
		tableLayoutPanel2.Location = new System.Drawing.Point(47, 17);
		tableLayoutPanel2.Name = "tableLayoutPanel2";
		tableLayoutPanel2.RowCount = 3;
		tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40f));
		tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
		tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35f));
		tableLayoutPanel2.Size = new System.Drawing.Size(811, 182);
		tableLayoutPanel2.TabIndex = 57;
		l_inventoryCountTotal.Anchor = System.Windows.Forms.AnchorStyles.Left;
		l_inventoryCountTotal.AutoSize = true;
		l_inventoryCountTotal.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		l_inventoryCountTotal.Location = new System.Drawing.Point(174, 85);
		l_inventoryCountTotal.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
		l_inventoryCountTotal.Name = "l_inventoryCountTotal";
		l_inventoryCountTotal.Size = new System.Drawing.Size(28, 20);
		l_inventoryCountTotal.TabIndex = 21;
		l_inventoryCountTotal.Text = "{0}";
		l_title.AutoSize = true;
		l_title.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_title.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		l_title.Location = new System.Drawing.Point(371, 208);
		l_title.Name = "l_title";
		l_title.Size = new System.Drawing.Size(162, 24);
		l_title.TabIndex = 56;
		l_title.Text = "近期庫存調整一覽";
		l_title.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		btn_close.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_close.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_close.ForeColor = System.Drawing.Color.White;
		btn_close.Location = new System.Drawing.Point(390, 507);
		btn_close.Name = "btn_close";
		btn_close.Size = new System.Drawing.Size(124, 34);
		btn_close.TabIndex = 55;
		btn_close.TabStop = false;
		btn_close.Text = "關閉";
		btn_close.UseVisualStyleBackColor = false;
		btn_close.Click += new System.EventHandler(btn_close_Click);
		btn_minus.BackColor = System.Drawing.SystemColors.ButtonShadow;
		btn_minus.Location = new System.Drawing.Point(228, 3);
		btn_minus.Name = "btn_minus";
		btn_minus.Size = new System.Drawing.Size(40, 31);
		btn_minus.TabIndex = 46;
		btn_minus.Text = "-";
		btn_minus.UseVisualStyleBackColor = false;
		btn_minus.Click += new System.EventHandler(btn_minus_Click);
		btn_plus.BackColor = System.Drawing.Color.Gainsboro;
		btn_plus.Location = new System.Drawing.Point(182, 3);
		btn_plus.Name = "btn_plus";
		btn_plus.Size = new System.Drawing.Size(40, 31);
		btn_plus.TabIndex = 45;
		btn_plus.Text = "+";
		btn_plus.UseVisualStyleBackColor = false;
		btn_plus.Click += new System.EventHandler(btn_plus_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		AutoScroll = true;
		BackColor = System.Drawing.Color.Silver;
		base.ClientSize = new System.Drawing.Size(904, 576);
		base.ControlBox = false;
		base.Controls.Add(dataGridView1);
		base.Controls.Add(tableLayoutPanel2);
		base.Controls.Add(l_title);
		base.Controls.Add(btn_close);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "dialogInventoryAdjustment";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "frmSearchMember";
		base.Load += new System.EventHandler(dialogSellPriceLog_Load);
		((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
		flowLayoutPanel2.ResumeLayout(false);
		flowLayoutPanel2.PerformLayout();
		flowLayoutPanel1.ResumeLayout(false);
		flowLayoutPanel1.PerformLayout();
		panel3.ResumeLayout(false);
		panel3.PerformLayout();
		panel2.ResumeLayout(false);
		panel2.PerformLayout();
		panel22.ResumeLayout(false);
		panel22.PerformLayout();
		tableLayoutPanel2.ResumeLayout(false);
		tableLayoutPanel2.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}
}
