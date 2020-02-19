using KeyboardClassLibrary;
using POS_Client;
using POS_Client.Properties;
using POS_Client.WebService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dialogAddNewVendor : Form
{
	private frmNewDeliveryOrder frmD;

	private frmChooseVendor frmC;

	private DataTable _dtVendorInfo;

	private string _Feature = "";

	private string _SupplierNo;

	private string _LicenseCode;

	private string vendorId = "";

	private IContainer components;

	private Button btn_back;

	private TableLayoutPanel tableLayoutPanel1;

	private Panel panel4;

	private Label label3;

	private Panel panel6;

	private Label label12;

	private Panel panel3;

	private Label label6;

	private Panel panel5;

	private Label label10;

	private TextBox tb_SupplierName;

	private Panel panel11;

	private Label l_BuyDate;

	private Panel panel14;

	private TextBox tb_zipcode;

	private ComboBox cb_area;

	private ComboBox cb_city;

	private TextBox tb_Address;

	private Panel panel15;

	private Panel panel13;

	private Button btn_checkvendorID;

	private PictureBox pictureBox1;

	private Panel panel17;

	private Button btn_down;

	private Button btn_top;

	private PictureBox pictureBox2;

	private Keyboardcontrol keyboardcontrol1;

	private Button btn_SaveVendorInfoAndSelect;

	private TextBox textBox1;

	private Panel panel8;

	private Label label4;

	private TextBox tb_SupplierIdNo;

	private Panel panel1;

	private Label label1;

	private Label label5;

	private TextBox tb_vendorName;

	private TextBox tb_vendorId;

	private Label label2;

	private Panel panel2;

	private TextBox tb_TelExt;

	private TextBox tb_TelNo;

	private TextBox tb_SupplierNo;

	private Label label8;

	private Label l_title;

	public dialogAddNewVendor(frmNewDeliveryOrder frmD, frmChooseVendor frmC, string SupplierNo)
	{
		InitializeComponent();
		this.frmD = frmD;
		this.frmC = frmC;
		_SupplierNo = SupplierNo;
		_Feature = "ChooseVendor";
		l_title.Text = "選擇廠商 / 廠商編修";
		btn_SaveVendorInfoAndSelect.Text = "確定選擇(並選入儲存變更)";
		btn_back.Text = "重新選擇";
	}

	public dialogAddNewVendor(frmNewDeliveryOrder frmD, frmChooseVendor frmC)
	{
		InitializeComponent();
		this.frmD = frmD;
		this.frmC = frmC;
		_Feature = "AddNewVendor";
		l_title.Text = "新增廠商";
		btn_SaveVendorInfoAndSelect.Text = "確定新增(並選入)";
		btn_back.Text = "取消";
		_SupplierNo = getSupplierNo();
		tb_SupplierNo.Text = _SupplierNo;
	}

	private void dialogAddNewVendor_Load(object sender, EventArgs e)
	{
		if ("ChooseVendor".Equals(_Feature))
		{
			DataTable dataSource = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "*", "ADDRCITY", "", "", null, null, CommandOperationType.ExecuteReaderReturnDataTable);
			cb_city.DisplayMember = "city";
			cb_city.ValueMember = "cityno";
			cb_city.DataSource = dataSource;
			string sql = "SELECT * FROM hypos_Supplier WHERE SupplierNo = {0} ";
			_dtVendorInfo = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, new string[1]
			{
				_SupplierNo
			}, CommandOperationType.ExecuteReaderReturnDataTable);
			if (_dtVendorInfo.Rows.Count > 0)
			{
				tb_SupplierNo.Text = _dtVendorInfo.Rows[0]["SupplierNo"].ToString();
				tb_SupplierName.Text = _dtVendorInfo.Rows[0]["SupplierName"].ToString();
				tb_SupplierIdNo.Text = _dtVendorInfo.Rows[0]["SupplierIdNo"].ToString();
				tb_vendorId.Text = _dtVendorInfo.Rows[0]["vendorId"].ToString();
				tb_vendorName.Text = _dtVendorInfo.Rows[0]["vendorName"].ToString();
				tb_TelNo.Text = _dtVendorInfo.Rows[0]["TelNo"].ToString();
				tb_TelExt.Text = _dtVendorInfo.Rows[0]["TelExt"].ToString();
				tb_zipcode.Text = _dtVendorInfo.Rows[0]["Zipcode"].ToString();
				tb_Address.Text = _dtVendorInfo.Rows[0]["Address"].ToString();
				if (!"".Equals(_dtVendorInfo.Rows[0]["CityNo"].ToString()))
				{
					cb_city.SelectedValue = _dtVendorInfo.Rows[0]["CityNo"].ToString();
				}
				else
				{
					cb_city.SelectedValue = 0;
				}
				if (!"".Equals(_dtVendorInfo.Rows[0]["Zipcode"].ToString()))
				{
					cb_area.SelectedValue = _dtVendorInfo.Rows[0]["Zipcode"].ToString();
				}
				else
				{
					cb_area.SelectedValue = 0;
				}
			}
			else
			{
				AutoClosingMessageBox.Show("廠商編號錯誤!");
				Close();
			}
		}
		else if ("AddNewVendor".Equals(_Feature))
		{
			DataTable dataSource2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "*", "ADDRCITY", "", "", null, null, CommandOperationType.ExecuteReaderReturnDataTable);
			cb_city.DisplayMember = "city";
			cb_city.ValueMember = "cityno";
			cb_city.DataSource = dataSource2;
		}
	}

	private void cb_city_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (cb_city.SelectedValue != null)
		{
			cb_area.DataSource = null;
			DataTable dataSource = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "*", "ADDRAREA", "cityno = {0}", "", null, new string[1]
			{
				cb_city.SelectedValue.ToString()
			}, CommandOperationType.ExecuteReaderReturnDataTable);
			cb_area.DisplayMember = "area";
			cb_area.ValueMember = "zipcode";
			cb_area.DataSource = dataSource;
		}
	}

	private void cb_area_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (cb_area.SelectedValue != null)
		{
			tb_zipcode.Text = cb_area.SelectedValue.ToString();
		}
	}

	private void btn_back_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btn_save_Click(object sender, EventArgs e)
	{
		string text = "";
		if (cb_city.SelectedValue == null)
		{
			text += "請輸入廠商所在城市\n";
		}
		if (cb_area.SelectedValue == null)
		{
			text += "請輸入廠商所在區域\n";
		}
		if (!text.Equals(""))
		{
			AutoClosingMessageBox.Show(text);
			return;
		}
		string[,] strFieldArray = new string[4, 2]
		{
			{
				"City",
				cb_city.SelectedValue.ToString()
			},
			{
				"Area",
				cb_area.SelectedValue.ToString()
			},
			{
				"Address",
				tb_Address.Text
			},
			{
				"UpdateDate",
				DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
			}
		};
		DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Update, "", "hypos_CUST_RTL", "VipNo = {0}", "", strFieldArray, new string[1]
		{
			_SupplierNo
		}, CommandOperationType.ExecuteNonQuery);
		base.DialogResult = DialogResult.Yes;
		AutoClosingMessageBox.Show("廠商已選入");
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

	private void btn_LowIncome_Click(object sender, EventArgs e)
	{
		vendorId = tb_vendorId.Text.Trim();
		if (vendorId.Equals("請輸入執照號碼後點選檢查"))
		{
			vendorId = "";
		}
		if (vendorId.Equals(""))
		{
			tb_vendorName.Text = "";
			AutoClosingMessageBox.Show("「販賣執照號碼」必填，請檢查");
			return;
		}
		if (!checkVendorID(vendorId))
		{
			tb_vendorName.Text = "";
			AutoClosingMessageBox.Show("請輸入廠商之新販賣業執照號碼，包含一碼英文+五碼數字");
			return;
		}
		VendorResultObject vendorResultObject = new VerifyVendorInfoWS().vendorData(tb_vendorId.Text);
		if (vendorResultObject.success == "Y")
		{
			if (MessageBox.Show("證號：" + vendorResultObject.vendorId + "、名稱：" + vendorResultObject.vendorName + " \n是否使用此廠商資訊？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
			{
				tb_vendorName.Text = vendorResultObject.vendorName;
			}
		}
		else if (vendorResultObject.success == "N")
		{
			string text = "";
			if (vendorResultObject.message.Equals("廠商販賣執照號碼不存在"))
			{
				text = "廠商營業執照號碼不存在，請檢查輸入值";
			}
			else if (vendorResultObject.message.Equals("廠商已歇業或停業"))
			{
				text = "此廠商目前已停用，請先確認廠商狀態";
			}
			MessageBox.Show(text);
		}
	}

	private bool checkVendorID(string id)
	{
		List<string> list = new List<string>
		{
			"A",
			"B",
			"C",
			"D",
			"E",
			"F",
			"G",
			"H",
			"J",
			"K",
			"L",
			"M",
			"N",
			"P",
			"Q",
			"R",
			"S",
			"T",
			"U",
			"V",
			"X",
			"Y",
			"W",
			"Z",
			"I",
			"O"
		};
		if (id.Trim().Length == 6)
		{
			for (int i = 1; i < 6; i++)
			{
				byte b = Convert.ToByte(id.Trim().Substring(i, 1));
				if (b > 9 || b < 0)
				{
					return false;
				}
			}
			id = id.ToUpper();
			int j;
			for (j = 0; j < list.Count && !(id.Substring(0, 1) == list[j]); j++)
			{
			}
			if (j > 25)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	private void btn_SaveVendorInfoAndSelect_Click(object sender, EventArgs e)
	{
		string text = "";
		if (tb_SupplierName.Text.Trim() == "" || tb_SupplierName.Text.Trim() == "請輸入廠商名稱")
		{
			text += "請輸入廠商名稱\n";
		}
		if (cb_city.SelectedValue == null)
		{
			text += "請輸入廠商所在城市\n";
		}
		if (cb_area.SelectedValue == null)
		{
			text += "請輸入廠商所在區域\n";
		}
		if (!text.Equals(""))
		{
			AutoClosingMessageBox.Show(text);
			return;
		}
		if (tb_SupplierIdNo.Text.Trim() == "請輸入廠商統一編號")
		{
			tb_SupplierIdNo.Text = "";
		}
		if (tb_TelNo.Text.Trim() == "請輸入電話號碼")
		{
			tb_TelNo.Text = "";
		}
		if (tb_TelExt.Text.Trim() == "分機號碼")
		{
			tb_TelExt.Text = "";
		}
		if (tb_Address.Text.Trim() == "請輸入地址")
		{
			tb_Address.Text = "";
		}
		if (_Feature == "ChooseVendor")
		{
			string text2 = tb_vendorId.Text;
			if (tb_vendorName.Text.Equals(""))
			{
				text2 = "";
			}
			string[,] strFieldArray = new string[9, 2]
			{
				{
					"SupplierName",
					tb_SupplierName.Text
				},
				{
					"SupplierIdNo",
					tb_SupplierIdNo.Text
				},
				{
					"vendorId",
					text2
				},
				{
					"vendorName",
					tb_vendorName.Text
				},
				{
					"TelNo",
					tb_TelNo.Text
				},
				{
					"TelExt",
					tb_TelExt.Text
				},
				{
					"Address",
					tb_Address.Text
				},
				{
					"CityNo",
					cb_city.SelectedValue.ToString()
				},
				{
					"Zipcode",
					cb_area.SelectedValue.ToString()
				}
			};
			DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Update, "", "hypos_Supplier", "SupplierNo = {0}", "", strFieldArray, new string[1]
			{
				_SupplierNo
			}, CommandOperationType.ExecuteNonQuery);
			if (tb_vendorName.Text == "")
			{
				AutoClosingMessageBox.Show("請注意，未檢查營業資訊的廠商，無法進行管制農藥的出貨");
			}
			AutoClosingMessageBox.Show("廠商資料已儲存，並選入廠商");
			frmD.addVendor(tb_SupplierNo.Text, tb_SupplierName.Text);
			frmD.Show();
			frmC.Close();
			Close();
		}
		else if (_Feature == "AddNewVendor")
		{
			string text3 = tb_vendorId.Text;
			if (tb_vendorName.Text.Equals(""))
			{
				text3 = "";
			}
			string[,] strFieldArray2 = new string[13, 2]
			{
				{
					"SupplierNo",
					tb_SupplierNo.Text
				},
				{
					"SupplierName",
					tb_SupplierName.Text
				},
				{
					"SupplierIdNo",
					tb_SupplierIdNo.Text
				},
				{
					"vendorId",
					text3
				},
				{
					"vendorName",
					tb_vendorName.Text
				},
				{
					"TelNo",
					tb_TelNo.Text
				},
				{
					"TelExt",
					tb_TelExt.Text
				},
				{
					"Address",
					tb_Address.Text
				},
				{
					"CityNo",
					cb_city.SelectedValue.ToString()
				},
				{
					"Zipcode",
					cb_area.SelectedValue.ToString()
				},
				{
					"Type",
					"0"
				},
				{
					"Status",
					"0"
				},
				{
					"vendorType",
					"0"
				}
			};
			DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_Supplier", "", "", strFieldArray2, null, CommandOperationType.ExecuteNonQuery);
			if (tb_vendorName.Text == "")
			{
				AutoClosingMessageBox.Show("請注意，未檢查營業資訊的廠商，無法進行管制農藥的出貨");
			}
			AutoClosingMessageBox.Show("廠商已選入");
			frmD.addVendor(tb_SupplierNo.Text, tb_SupplierName.Text);
			frmD.Show();
			frmC.Close();
			Close();
		}
	}

	public static string getSupplierNo()
	{
		string sql = "SELECT SupplierNo FROM hypos_Supplier where SupplierNo like 'S" + Program.SiteNo + "%' order by SupplierNo desc limit 1";
		string text = Convert.ToString(DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteScalar));
		string text2 = DateTime.Now.Year.ToString().Substring(2, 2);
		if ("-1".Equals(text))
		{
			return $"S{Program.SiteNo}{text2}00001";
		}
		string value = text.Substring(3, 2);
		if (!text2.Equals(value))
		{
			return $"S{Program.SiteNo}{text2}00001";
		}
		string arg = $"{int.Parse(text.Substring(5, 5)) + 1:00000}";
		return $"S{Program.SiteNo}{text2}{arg}";
	}

	private void tb_SupplierName_Enter(object sender, EventArgs e)
	{
		if ("請輸入廠商名稱".Equals(tb_SupplierName.Text))
		{
			tb_SupplierName.Text = "";
		}
	}

	private void tb_SupplierName_Leave(object sender, EventArgs e)
	{
		if ("".Equals(tb_SupplierName.Text))
		{
			tb_SupplierName.Text = "請輸入廠商名稱";
		}
	}

	private void tb_SupplierIdNo_Enter(object sender, EventArgs e)
	{
		if ("請輸入廠商統一編號".Equals(tb_SupplierIdNo.Text))
		{
			tb_SupplierIdNo.Text = "";
		}
	}

	private void tb_SupplierIdNo_Leave(object sender, EventArgs e)
	{
		if ("".Equals(tb_SupplierIdNo.Text))
		{
			tb_SupplierIdNo.Text = "請輸入廠商統一編號";
		}
	}

	private void tb_vendorId_Enter(object sender, EventArgs e)
	{
		if ("請輸入執照號碼後點選檢查".Equals(tb_vendorId.Text))
		{
			tb_vendorId.Text = "";
		}
	}

	private void tb_vendorId_Leave(object sender, EventArgs e)
	{
		if ("".Equals(tb_vendorId.Text))
		{
			tb_vendorId.Text = "請輸入執照號碼後點選檢查";
		}
	}

	private void tb_TelNo_Enter(object sender, EventArgs e)
	{
		if ("請輸入電話號碼".Equals(tb_TelNo.Text))
		{
			tb_TelNo.Text = "";
		}
	}

	private void tb_TelNo_Leave(object sender, EventArgs e)
	{
		if ("".Equals(tb_TelNo.Text))
		{
			tb_TelNo.Text = "請輸入電話號碼";
		}
	}

	private void tb_TelExt_Enter(object sender, EventArgs e)
	{
		if ("分機號碼".Equals(tb_TelExt.Text))
		{
			tb_TelExt.Text = "";
		}
	}

	private void tb_TelExt_Leave(object sender, EventArgs e)
	{
		if ("".Equals(tb_TelExt.Text))
		{
			tb_TelExt.Text = "分機號碼";
		}
	}

	private void tb_Address_Enter(object sender, EventArgs e)
	{
		if ("請輸入地址".Equals(tb_Address.Text))
		{
			tb_Address.Text = "";
		}
	}

	private void tb_Address_Leave(object sender, EventArgs e)
	{
		if ("".Equals(tb_Address.Text))
		{
			tb_Address.Text = "請輸入地址";
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_Client.dialogAddNewVendor));
		btn_back = new System.Windows.Forms.Button();
		tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		tb_SupplierIdNo = new System.Windows.Forms.TextBox();
		panel8 = new System.Windows.Forms.Panel();
		label4 = new System.Windows.Forms.Label();
		panel15 = new System.Windows.Forms.Panel();
		tb_SupplierName = new System.Windows.Forms.TextBox();
		panel13 = new System.Windows.Forms.Panel();
		tb_SupplierNo = new System.Windows.Forms.TextBox();
		panel6 = new System.Windows.Forms.Panel();
		label12 = new System.Windows.Forms.Label();
		panel3 = new System.Windows.Forms.Panel();
		label6 = new System.Windows.Forms.Label();
		panel11 = new System.Windows.Forms.Panel();
		l_BuyDate = new System.Windows.Forms.Label();
		panel5 = new System.Windows.Forms.Panel();
		label8 = new System.Windows.Forms.Label();
		label10 = new System.Windows.Forms.Label();
		panel4 = new System.Windows.Forms.Panel();
		label3 = new System.Windows.Forms.Label();
		panel1 = new System.Windows.Forms.Panel();
		label5 = new System.Windows.Forms.Label();
		tb_vendorName = new System.Windows.Forms.TextBox();
		tb_vendorId = new System.Windows.Forms.TextBox();
		label2 = new System.Windows.Forms.Label();
		label1 = new System.Windows.Forms.Label();
		btn_checkvendorID = new System.Windows.Forms.Button();
		panel14 = new System.Windows.Forms.Panel();
		tb_zipcode = new System.Windows.Forms.TextBox();
		cb_area = new System.Windows.Forms.ComboBox();
		cb_city = new System.Windows.Forms.ComboBox();
		tb_Address = new System.Windows.Forms.TextBox();
		panel2 = new System.Windows.Forms.Panel();
		tb_TelExt = new System.Windows.Forms.TextBox();
		tb_TelNo = new System.Windows.Forms.TextBox();
		textBox1 = new System.Windows.Forms.TextBox();
		pictureBox1 = new System.Windows.Forms.PictureBox();
		panel17 = new System.Windows.Forms.Panel();
		btn_down = new System.Windows.Forms.Button();
		btn_top = new System.Windows.Forms.Button();
		pictureBox2 = new System.Windows.Forms.PictureBox();
		keyboardcontrol1 = new KeyboardClassLibrary.Keyboardcontrol();
		btn_SaveVendorInfoAndSelect = new System.Windows.Forms.Button();
		l_title = new System.Windows.Forms.Label();
		tableLayoutPanel1.SuspendLayout();
		panel8.SuspendLayout();
		panel15.SuspendLayout();
		panel13.SuspendLayout();
		panel6.SuspendLayout();
		panel3.SuspendLayout();
		panel11.SuspendLayout();
		panel5.SuspendLayout();
		panel4.SuspendLayout();
		panel1.SuspendLayout();
		panel14.SuspendLayout();
		panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
		panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
		SuspendLayout();
		btn_back.BackColor = System.Drawing.Color.FromArgb(153, 153, 153);
		btn_back.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_back.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_back.ForeColor = System.Drawing.Color.White;
		btn_back.Location = new System.Drawing.Point(566, 493);
		btn_back.Name = "btn_back";
		btn_back.Size = new System.Drawing.Size(92, 40);
		btn_back.TabIndex = 0;
		btn_back.Text = "重新選擇";
		btn_back.UseVisualStyleBackColor = false;
		btn_back.Click += new System.EventHandler(btn_back_Click);
		tableLayoutPanel1.BackColor = System.Drawing.Color.White;
		tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		tableLayoutPanel1.ColumnCount = 4;
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 162f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 162f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		tableLayoutPanel1.Controls.Add(tb_SupplierIdNo, 3, 1);
		tableLayoutPanel1.Controls.Add(panel8, 2, 1);
		tableLayoutPanel1.Controls.Add(panel15, 1, 1);
		tableLayoutPanel1.Controls.Add(panel13, 1, 0);
		tableLayoutPanel1.Controls.Add(panel6, 0, 2);
		tableLayoutPanel1.Controls.Add(panel3, 0, 0);
		tableLayoutPanel1.Controls.Add(panel11, 0, 5);
		tableLayoutPanel1.Controls.Add(panel5, 0, 1);
		tableLayoutPanel1.Controls.Add(panel4, 0, 6);
		tableLayoutPanel1.Controls.Add(panel1, 1, 2);
		tableLayoutPanel1.Controls.Add(panel14, 1, 6);
		tableLayoutPanel1.Controls.Add(panel2, 1, 5);
		tableLayoutPanel1.Location = new System.Drawing.Point(51, 51);
		tableLayoutPanel1.Name = "tableLayoutPanel1";
		tableLayoutPanel1.RowCount = 8;
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.Size = new System.Drawing.Size(850, 411);
		tableLayoutPanel1.TabIndex = 41;
		tb_SupplierIdNo.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_SupplierIdNo.ForeColor = System.Drawing.Color.DarkGray;
		tb_SupplierIdNo.Location = new System.Drawing.Point(598, 62);
		tb_SupplierIdNo.Margin = new System.Windows.Forms.Padding(10);
		tb_SupplierIdNo.MaxLength = 10;
		tb_SupplierIdNo.Name = "tb_SupplierIdNo";
		tb_SupplierIdNo.Size = new System.Drawing.Size(239, 29);
		tb_SupplierIdNo.TabIndex = 59;
		tb_SupplierIdNo.Text = "請輸入廠商統一編號";
		tb_SupplierIdNo.Enter += new System.EventHandler(tb_SupplierIdNo_Enter);
		tb_SupplierIdNo.Leave += new System.EventHandler(tb_SupplierIdNo_Leave);
		panel8.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel8.Controls.Add(label4);
		panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		panel8.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		panel8.ForeColor = System.Drawing.Color.White;
		panel8.Location = new System.Drawing.Point(425, 52);
		panel8.Margin = new System.Windows.Forms.Padding(0);
		panel8.Name = "panel8";
		panel8.Size = new System.Drawing.Size(162, 50);
		panel8.TabIndex = 58;
		label4.AutoSize = true;
		label4.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label4.ForeColor = System.Drawing.Color.White;
		label4.Location = new System.Drawing.Point(71, 15);
		label4.Name = "label4";
		label4.Size = new System.Drawing.Size(74, 21);
		label4.TabIndex = 0;
		label4.Text = "統一編號";
		panel15.Controls.Add(tb_SupplierName);
		panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		panel15.Location = new System.Drawing.Point(164, 52);
		panel15.Margin = new System.Windows.Forms.Padding(0);
		panel15.Name = "panel15";
		panel15.Size = new System.Drawing.Size(260, 50);
		panel15.TabIndex = 55;
		tb_SupplierName.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_SupplierName.ForeColor = System.Drawing.Color.DarkGray;
		tb_SupplierName.Location = new System.Drawing.Point(10, 10);
		tb_SupplierName.Margin = new System.Windows.Forms.Padding(10);
		tb_SupplierName.MaxLength = 10;
		tb_SupplierName.Name = "tb_SupplierName";
		tb_SupplierName.Size = new System.Drawing.Size(239, 29);
		tb_SupplierName.TabIndex = 42;
		tb_SupplierName.Text = "請輸入廠商名稱";
		tb_SupplierName.Enter += new System.EventHandler(tb_SupplierName_Enter);
		tb_SupplierName.Leave += new System.EventHandler(tb_SupplierName_Leave);
		tableLayoutPanel1.SetColumnSpan(panel13, 3);
		panel13.Controls.Add(tb_SupplierNo);
		panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		panel13.Location = new System.Drawing.Point(164, 1);
		panel13.Margin = new System.Windows.Forms.Padding(0);
		panel13.Name = "panel13";
		panel13.Size = new System.Drawing.Size(685, 50);
		panel13.TabIndex = 54;
		tb_SupplierNo.Enabled = false;
		tb_SupplierNo.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_SupplierNo.ForeColor = System.Drawing.Color.DarkGray;
		tb_SupplierNo.Location = new System.Drawing.Point(9, 11);
		tb_SupplierNo.Margin = new System.Windows.Forms.Padding(10);
		tb_SupplierNo.MaxLength = 10;
		tb_SupplierNo.Name = "tb_SupplierNo";
		tb_SupplierNo.Size = new System.Drawing.Size(239, 29);
		tb_SupplierNo.TabIndex = 43;
		panel6.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel6.Controls.Add(label12);
		panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		panel6.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		panel6.ForeColor = System.Drawing.Color.White;
		panel6.Location = new System.Drawing.Point(1, 103);
		panel6.Margin = new System.Windows.Forms.Padding(0);
		panel6.Name = "panel6";
		tableLayoutPanel1.SetRowSpan(panel6, 3);
		panel6.Size = new System.Drawing.Size(162, 152);
		panel6.TabIndex = 20;
		label12.AutoSize = true;
		label12.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label12.ForeColor = System.Drawing.Color.White;
		label12.Location = new System.Drawing.Point(71, 74);
		label12.Name = "label12";
		label12.Size = new System.Drawing.Size(74, 21);
		label12.TabIndex = 0;
		label12.Text = "營業資訊";
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
		label6.Location = new System.Drawing.Point(71, 14);
		label6.Name = "label6";
		label6.Size = new System.Drawing.Size(74, 21);
		label6.TabIndex = 0;
		label6.Text = "廠商編號";
		panel11.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel11.Controls.Add(l_BuyDate);
		panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		panel11.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		panel11.ForeColor = System.Drawing.Color.White;
		panel11.Location = new System.Drawing.Point(1, 256);
		panel11.Margin = new System.Windows.Forms.Padding(0);
		panel11.Name = "panel11";
		panel11.Size = new System.Drawing.Size(162, 50);
		panel11.TabIndex = 50;
		l_BuyDate.AutoSize = true;
		l_BuyDate.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_BuyDate.ForeColor = System.Drawing.Color.White;
		l_BuyDate.Location = new System.Drawing.Point(71, 16);
		l_BuyDate.Name = "l_BuyDate";
		l_BuyDate.Size = new System.Drawing.Size(74, 21);
		l_BuyDate.TabIndex = 0;
		l_BuyDate.Text = "電話號碼";
		panel5.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel5.Controls.Add(label8);
		panel5.Controls.Add(label10);
		panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		panel5.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		panel5.ForeColor = System.Drawing.Color.White;
		panel5.Location = new System.Drawing.Point(1, 52);
		panel5.Margin = new System.Windows.Forms.Padding(0);
		panel5.Name = "panel5";
		panel5.Size = new System.Drawing.Size(162, 50);
		panel5.TabIndex = 23;
		label8.AutoSize = true;
		label8.Font = new System.Drawing.Font("微軟正黑體", 11f);
		label8.ForeColor = System.Drawing.Color.Red;
		label8.Location = new System.Drawing.Point(58, 16);
		label8.Name = "label8";
		label8.Size = new System.Drawing.Size(16, 19);
		label8.TabIndex = 49;
		label8.Text = "*";
		label10.AutoSize = true;
		label10.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label10.ForeColor = System.Drawing.Color.White;
		label10.Location = new System.Drawing.Point(71, 15);
		label10.Name = "label10";
		label10.Size = new System.Drawing.Size(74, 21);
		label10.TabIndex = 0;
		label10.Text = "廠商名稱";
		panel4.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel4.Controls.Add(label3);
		panel4.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		panel4.ForeColor = System.Drawing.Color.White;
		panel4.Location = new System.Drawing.Point(1, 307);
		panel4.Margin = new System.Windows.Forms.Padding(0);
		panel4.Name = "panel4";
		tableLayoutPanel1.SetRowSpan(panel4, 2);
		panel4.Size = new System.Drawing.Size(162, 103);
		panel4.TabIndex = 45;
		label3.AutoSize = true;
		label3.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label3.ForeColor = System.Drawing.Color.White;
		label3.Location = new System.Drawing.Point(103, 44);
		label3.Name = "label3";
		label3.Size = new System.Drawing.Size(42, 21);
		label3.TabIndex = 0;
		label3.Text = "地址";
		tableLayoutPanel1.SetColumnSpan(panel1, 3);
		panel1.Controls.Add(label5);
		panel1.Controls.Add(tb_vendorName);
		panel1.Controls.Add(tb_vendorId);
		panel1.Controls.Add(label2);
		panel1.Controls.Add(label1);
		panel1.Controls.Add(btn_checkvendorID);
		panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		panel1.Location = new System.Drawing.Point(167, 106);
		panel1.Name = "panel1";
		tableLayoutPanel1.SetRowSpan(panel1, 3);
		panel1.Size = new System.Drawing.Size(679, 146);
		panel1.TabIndex = 60;
		label5.AutoSize = true;
		label5.Font = new System.Drawing.Font("微軟正黑體", 11f);
		label5.Location = new System.Drawing.Point(19, 117);
		label5.Name = "label5";
		label5.Size = new System.Drawing.Size(421, 19);
		label5.TabIndex = 47;
		label5.Text = "*請注意，農藥商品的出貨廠商必需先驗證商業執照號碼與名稱";
		tb_vendorName.Enabled = false;
		tb_vendorName.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_vendorName.ForeColor = System.Drawing.Color.DarkGray;
		tb_vendorName.Location = new System.Drawing.Point(107, 69);
		tb_vendorName.Margin = new System.Windows.Forms.Padding(10);
		tb_vendorName.MaxLength = 10;
		tb_vendorName.Name = "tb_vendorName";
		tb_vendorName.Size = new System.Drawing.Size(262, 29);
		tb_vendorName.TabIndex = 46;
		tb_vendorId.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_vendorId.ForeColor = System.Drawing.Color.DarkGray;
		tb_vendorId.ImeMode = System.Windows.Forms.ImeMode.Disable;
		tb_vendorId.Location = new System.Drawing.Point(107, 20);
		tb_vendorId.Margin = new System.Windows.Forms.Padding(10);
		tb_vendorId.MaxLength = 6;
		tb_vendorId.Name = "tb_vendorId";
		tb_vendorId.Size = new System.Drawing.Size(262, 29);
		tb_vendorId.TabIndex = 43;
		tb_vendorId.Text = "請輸入執照號碼後點選檢查";
		tb_vendorId.Enter += new System.EventHandler(tb_vendorId_Enter);
		tb_vendorId.Leave += new System.EventHandler(tb_vendorId_Leave);
		label2.AutoSize = true;
		label2.Font = new System.Drawing.Font("微軟正黑體", 12f);
		label2.Location = new System.Drawing.Point(19, 72);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(89, 20);
		label2.TabIndex = 45;
		label2.Text = "商業名稱：";
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 12f);
		label1.Location = new System.Drawing.Point(19, 23);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(89, 20);
		label1.TabIndex = 44;
		label1.Text = "執照號碼：";
		btn_checkvendorID.BackColor = System.Drawing.Color.Gray;
		btn_checkvendorID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_checkvendorID.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_checkvendorID.ForeColor = System.Drawing.Color.White;
		btn_checkvendorID.Location = new System.Drawing.Point(382, 20);
		btn_checkvendorID.Name = "btn_checkvendorID";
		btn_checkvendorID.Size = new System.Drawing.Size(104, 78);
		btn_checkvendorID.TabIndex = 43;
		btn_checkvendorID.Text = "檢查";
		btn_checkvendorID.UseVisualStyleBackColor = false;
		btn_checkvendorID.Click += new System.EventHandler(btn_LowIncome_Click);
		tableLayoutPanel1.SetColumnSpan(panel14, 3);
		panel14.Controls.Add(tb_zipcode);
		panel14.Controls.Add(cb_area);
		panel14.Controls.Add(cb_city);
		panel14.Controls.Add(tb_Address);
		panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		panel14.Location = new System.Drawing.Point(164, 307);
		panel14.Margin = new System.Windows.Forms.Padding(0);
		panel14.Name = "panel14";
		tableLayoutPanel1.SetRowSpan(panel14, 2);
		panel14.Size = new System.Drawing.Size(685, 103);
		panel14.TabIndex = 53;
		tb_zipcode.Cursor = System.Windows.Forms.Cursors.No;
		tb_zipcode.Enabled = false;
		tb_zipcode.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_zipcode.Location = new System.Drawing.Point(288, 16);
		tb_zipcode.Name = "tb_zipcode";
		tb_zipcode.ReadOnly = true;
		tb_zipcode.Size = new System.Drawing.Size(100, 29);
		tb_zipcode.TabIndex = 6;
		cb_area.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cb_area.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_area.FormattingEnabled = true;
		cb_area.Location = new System.Drawing.Point(151, 16);
		cb_area.Name = "cb_area";
		cb_area.Size = new System.Drawing.Size(121, 28);
		cb_area.TabIndex = 5;
		cb_area.SelectedIndexChanged += new System.EventHandler(cb_area_SelectedIndexChanged);
		cb_city.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cb_city.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_city.FormattingEnabled = true;
		cb_city.Location = new System.Drawing.Point(14, 16);
		cb_city.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
		cb_city.Name = "cb_city";
		cb_city.Size = new System.Drawing.Size(121, 28);
		cb_city.TabIndex = 4;
		cb_city.SelectedIndexChanged += new System.EventHandler(cb_city_SelectedIndexChanged);
		tb_Address.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_Address.ForeColor = System.Drawing.Color.DarkGray;
		tb_Address.Location = new System.Drawing.Point(14, 57);
		tb_Address.Margin = new System.Windows.Forms.Padding(0);
		tb_Address.Name = "tb_Address";
		tb_Address.Size = new System.Drawing.Size(656, 29);
		tb_Address.TabIndex = 7;
		tb_Address.Text = "請輸入地址";
		tb_Address.Enter += new System.EventHandler(tb_Address_Enter);
		tb_Address.Leave += new System.EventHandler(tb_Address_Leave);
		tableLayoutPanel1.SetColumnSpan(panel2, 3);
		panel2.Controls.Add(tb_TelExt);
		panel2.Controls.Add(tb_TelNo);
		panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		panel2.Location = new System.Drawing.Point(167, 259);
		panel2.Name = "panel2";
		panel2.Size = new System.Drawing.Size(679, 44);
		panel2.TabIndex = 61;
		tb_TelExt.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_TelExt.ForeColor = System.Drawing.Color.DarkGray;
		tb_TelExt.Location = new System.Drawing.Point(285, 10);
		tb_TelExt.Margin = new System.Windows.Forms.Padding(10);
		tb_TelExt.MaxLength = 10;
		tb_TelExt.Name = "tb_TelExt";
		tb_TelExt.Size = new System.Drawing.Size(135, 29);
		tb_TelExt.TabIndex = 49;
		tb_TelExt.Text = "分機號碼";
		tb_TelExt.Enter += new System.EventHandler(tb_TelExt_Enter);
		tb_TelExt.Leave += new System.EventHandler(tb_TelExt_Leave);
		tb_TelNo.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_TelNo.ForeColor = System.Drawing.Color.DarkGray;
		tb_TelNo.Location = new System.Drawing.Point(10, 10);
		tb_TelNo.Margin = new System.Windows.Forms.Padding(10);
		tb_TelNo.MaxLength = 10;
		tb_TelNo.Name = "tb_TelNo";
		tb_TelNo.Size = new System.Drawing.Size(262, 29);
		tb_TelNo.TabIndex = 48;
		tb_TelNo.Text = "請輸入電話號碼";
		tb_TelNo.Enter += new System.EventHandler(tb_TelNo_Enter);
		tb_TelNo.Leave += new System.EventHandler(tb_TelNo_Leave);
		textBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
		textBox1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		textBox1.ForeColor = System.Drawing.Color.DarkGray;
		textBox1.Location = new System.Drawing.Point(174, 175);
		textBox1.Margin = new System.Windows.Forms.Padding(10);
		textBox1.Name = "textBox1";
		textBox1.Size = new System.Drawing.Size(239, 29);
		textBox1.TabIndex = 58;
		pictureBox1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom);
		pictureBox1.BackColor = System.Drawing.Color.Silver;
		pictureBox1.Image = POS_Client.Properties.Resources.keyboard;
		pictureBox1.Location = new System.Drawing.Point(878, 493);
		pictureBox1.Name = "pictureBox1";
		pictureBox1.Size = new System.Drawing.Size(70, 0);
		pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		pictureBox1.TabIndex = 52;
		pictureBox1.TabStop = false;
		pictureBox1.Click += new System.EventHandler(pictureBox1_Click);
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
		btn_SaveVendorInfoAndSelect.BackColor = System.Drawing.Color.Red;
		btn_SaveVendorInfoAndSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_SaveVendorInfoAndSelect.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_SaveVendorInfoAndSelect.ForeColor = System.Drawing.Color.White;
		btn_SaveVendorInfoAndSelect.Location = new System.Drawing.Point(301, 493);
		btn_SaveVendorInfoAndSelect.Name = "btn_SaveVendorInfoAndSelect";
		btn_SaveVendorInfoAndSelect.Size = new System.Drawing.Size(211, 40);
		btn_SaveVendorInfoAndSelect.TabIndex = 54;
		btn_SaveVendorInfoAndSelect.Text = "確定選擇(並儲存變更)";
		btn_SaveVendorInfoAndSelect.UseVisualStyleBackColor = false;
		btn_SaveVendorInfoAndSelect.Click += new System.EventHandler(btn_SaveVendorInfoAndSelect_Click);
		l_title.Anchor = System.Windows.Forms.AnchorStyles.None;
		l_title.Font = new System.Drawing.Font("微軟正黑體", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_title.Location = new System.Drawing.Point(389, 12);
		l_title.Name = "l_title";
		l_title.Size = new System.Drawing.Size(200, 26);
		l_title.TabIndex = 48;
		l_title.Text = "TITLE";
		l_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		BackColor = System.Drawing.Color.Silver;
		base.ClientSize = new System.Drawing.Size(949, 565);
		base.Controls.Add(l_title);
		base.Controls.Add(btn_SaveVendorInfoAndSelect);
		base.Controls.Add(panel17);
		base.Controls.Add(pictureBox1);
		base.Controls.Add(tableLayoutPanel1);
		base.Controls.Add(btn_back);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "dialogAddNewVendor";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "選擇會員 / 會員編修";
		base.Load += new System.EventHandler(dialogAddNewVendor_Load);
		tableLayoutPanel1.ResumeLayout(false);
		tableLayoutPanel1.PerformLayout();
		panel8.ResumeLayout(false);
		panel8.PerformLayout();
		panel15.ResumeLayout(false);
		panel15.PerformLayout();
		panel13.ResumeLayout(false);
		panel13.PerformLayout();
		panel6.ResumeLayout(false);
		panel6.PerformLayout();
		panel3.ResumeLayout(false);
		panel3.PerformLayout();
		panel11.ResumeLayout(false);
		panel11.PerformLayout();
		panel5.ResumeLayout(false);
		panel5.PerformLayout();
		panel4.ResumeLayout(false);
		panel4.PerformLayout();
		panel1.ResumeLayout(false);
		panel1.PerformLayout();
		panel14.ResumeLayout(false);
		panel14.PerformLayout();
		panel2.ResumeLayout(false);
		panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
		panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
		ResumeLayout(false);
	}
}
