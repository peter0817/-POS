using BarcodeLib;
using Microsoft.Win32;
using POS_Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class Commodity_barcode : Form
{
	public static class myPrinters
	{
		[DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetDefaultPrinter(string Name);
	}

	private static string _ExePath;

	private int _pageRecordCount;

	private string _pageType = "";

	private List<string> _BarcodeList;

	private string _printerType;

	private string _printerName;

	private string _barCode;

	private List<string> _cropList;

	private List<string> _blightList;

	private Dictionary<string, string> _cropMap;

	private Dictionary<string, string> _blightMap;

	private IContainer components;

	private WebBrowser webBrowser1;

	private Button btnPrint;

	private Button btnPrintView;

	private Button button1;

	public Commodity_barcode(List<string> BarcodeList, int pageRecordCount)
	{
		InitializeComponent();
		if (!Directory.Exists("TempBarCode"))
		{
			Directory.CreateDirectory("TempBarCode");
		}
		_pageRecordCount = pageRecordCount;
		_BarcodeList = BarcodeList;
		_ExePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		string sql = "SELECT BarcodeListType, BarcodeListPrinterName FROM hypos_PrinterManage ";
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteReaderReturnDataTable);
		_printerType = dataTable.Rows[0]["BarcodeListType"].ToString();
		_printerName = dataTable.Rows[0]["BarcodeListPrinterName"].ToString();
		for (int i = 0; i < _BarcodeList.Count; i++)
		{
			string text = _BarcodeList[i].Trim();
			string str = text + ".gif";
			string filename = _ExePath + "\\TempBarCode\\" + str;
			Barcode barcode = new Barcode();
			barcode.IncludeLabel = true;
			barcode.LabelFont = new Font("Verdana", 8f);
			barcode.Width = 156;
			barcode.Height = 63;
			barcode.Encode(TYPE.CODE128, text, barcode.Width, barcode.Height).Save(filename, ImageFormat.Gif);
		}
	}

	public Commodity_barcode(string barCode, List<string> cropList, List<string> blightList)
	{
		InitializeComponent();
		if (!Directory.Exists("TempBarCode"))
		{
			Directory.CreateDirectory("TempBarCode");
		}
		_pageType = "WithPair";
		_barCode = barCode;
		_cropList = cropList;
		_blightList = blightList;
		_ExePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		string sql = "SELECT BarcodeListType, BarcodeListPrinterName FROM hypos_PrinterManage ";
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteReaderReturnDataTable);
		_printerType = dataTable.Rows[0]["BarcodeListType"].ToString();
		_printerName = dataTable.Rows[0]["BarcodeListPrinterName"].ToString();
		string text = "code in (";
		for (int i = 0; i < _cropList.Count; i++)
		{
			text = text + "{" + i + "},";
		}
		text = text.Substring(0, text.Length - 1) + ")";
		DataTable dataTable2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "code,name", "HyCrop", text, "", null, _cropList.ToArray(), CommandOperationType.ExecuteReaderReturnDataTable);
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		for (int j = 0; j < dataTable2.Rows.Count; j++)
		{
			dictionary.Add(dataTable2.Rows[j]["code"].ToString(), dataTable2.Rows[j]["name"].ToString());
		}
		_cropMap = dictionary;
		string text2 = "code in (";
		for (int k = 0; k < _blightList.Count; k++)
		{
			text2 = text2 + "{" + k + "},";
		}
		text2 = text2.Substring(0, text2.Length - 1) + ")";
		DataTable dataTable3 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "code,name", "HyBlight", text2, "", null, _blightList.ToArray(), CommandOperationType.ExecuteReaderReturnDataTable);
		Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
		for (int l = 0; l < dataTable3.Rows.Count; l++)
		{
			dictionary2.Add(dataTable3.Rows[l]["code"].ToString(), dataTable3.Rows[l]["name"].ToString());
		}
		_blightMap = dictionary2;
		for (int m = 0; m < _cropList.Count; m++)
		{
			string text3 = _barCode + "-" + _cropList[m] + "-" + _blightList[m];
			string filename = _ExePath + "\\TempBarCode\\" + text3 + ".gif";
			Barcode barcode = new Barcode();
			barcode.IncludeLabel = true;
			barcode.LabelFont = new Font("Verdana", 8f);
			barcode.Width = 266;
			barcode.Height = 101;
			barcode.Encode(TYPE.CODE128, text3, barcode.Width, barcode.Height).Save(filename, ImageFormat.Gif);
		}
	}

	private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
	{
		string text = "width:170mm;";
		string text2 = "min-height:244mm;";
		string text3 = "font-size: 1.15em;";
		if (_pageRecordCount == 12)
		{
			text3 = "";
		}
		string text4 = "";
		text4 = ((!"WithPair".Equals(_pageType)) ? divPage() : divPagePRO());
		string documentText = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta content=\"text/html;charset=utf-8\" http-equiv=\"Content-Type\"/><style>body{width:100%;height:100%;margin:0;padding:0;background-color:#FAFAFA;font-family:\"微軟正黑體\", Microsoft JhengHei;} *{box-sizing: border-box;-moz-box-sizing: border-box;} .page{" + text + text2 + "padding:10mm;margin:10mm auto;border:1px #D3D3D3 solid;border-radius:5px;background:white;box-shadow:0 0 5px rgba(0, 0, 0, 0.1);} .subpage{padding: 1cm;border: 5px red solid;height: 257mm;outline: 2cm #FFEAEA solid;} table{border-collapse:collapse;border-spacing:0;border:1px dotted #666;background-color:#FFF;" + text3 + "margin:0 0 20px 0;width:100%;} th{text-align:right;border:1px dotted #666;padding:5px;} td{text-align:left;border:1px dotted #666;padding:5px;} @page{size:A4;margin:0;} @media print{html,body{width:210mm;height:297mm;} .page{border:initial;border-radius:initial;width:initial;min-height:initial;box-shadow:initial;background:initial;}}</style></head><body style=\"margin:0;padding:0;overflow:auto;\"><div class=\"book\">" + text4 + "</body></html>";
		webBrowser1.DocumentText = documentText;
		webBrowser1.DocumentCompleted -= webBrowser1_DocumentCompleted;
	}

	private string divPage()
	{
		string text = "";
		string text2 = "GDSNO in (";
		for (int i = 0; i < _BarcodeList.Count; i++)
		{
			text2 = text2 + "{" + i + "},";
		}
		text2 = text2.Substring(0, text2.Length - 1) + ")";
		string strSelectField = "domManufId,GDName,brandName,CName,contents,spec,capacity,GDSNO,formCode";
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, strSelectField, "hypos_GOODSLST", text2, "", null, _BarcodeList.ToArray(), CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count > 0 && (_pageRecordCount == 3 || _pageRecordCount == 6 || _pageRecordCount == 12))
		{
			int num = _BarcodeList.Count / _pageRecordCount;
			num = ((_BarcodeList.Count <= _pageRecordCount) ? 1 : (num + 1));
			for (int j = 0; j < dataTable.Rows.Count; j++)
			{
				int num2 = (j + 1) / _pageRecordCount;
				if ((j + 1) % _pageRecordCount != 0 || j < _pageRecordCount - 1)
				{
					num2++;
				}
				text = tableConent(text, _pageRecordCount, dataTable, j, num2, num);
			}
			text += "</div>";
		}
		return text;
	}

	private string divPagePRO()
	{
		string text = "";
		int count = _cropList.Count;
		int num = 3;
		string sql = "SELECT GDSNO,GDName,domManufId,brandName,CName,contents,spec,capacity FROM hypos_GOODSLST where GDSNO='" + _barCode + "'";
		DataTable dt = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteReaderReturnDataTable);
		if (count > 0)
		{
			int num2 = count / num;
			num2 = ((count <= num) ? 1 : (num2 + 1));
			for (int i = 0; i < count; i++)
			{
				int num3 = (i + 1) / num;
				if ((i + 1) % num != 0 || i < num - 1)
				{
					num3++;
				}
				text = tableConentPRO(dt, text, num, i, num3, num2);
			}
			text += "</div>";
		}
		return text;
	}

	private string tableConent(string result, int pageRecordCount, DataTable dt, int i, int up, int un)
	{
		if (pageRecordCount == 3)
		{
			if (i == (up - 1) * pageRecordCount)
			{
				result = result + "<div class=\"page\">商品清冊．Page " + up + "/" + un;
			}
			result = result + "<table summary=\"資料表格\">" + content3(dt, i) + "</table>";
			if (i + 1 == dt.Rows.Count || i == up * pageRecordCount - 1)
			{
				result += "</div>";
			}
		}
		if (pageRecordCount == 6)
		{
			if (i == (up - 1) * pageRecordCount)
			{
				result = result + "<div class=\"page\">商品清冊．Page " + up + "/" + un + "<table style=\"font-size:.9em;\">";
			}
			if ((i + 1) % 2 == 1)
			{
				result += "<tr>";
			}
			result = result + "<td style=\"width:50%;\"><table summary=\"資料表格\">" + content6(dt, i) + "</table></td>";
			if (i + 1 == dt.Rows.Count && dt.Rows.Count % 2 == 1)
			{
				result += "<td style=\"width:50%;\"></td>";
			}
			if ((i + 1) % 2 == 0 || i + 1 == dt.Rows.Count)
			{
				result += "</tr>";
			}
			if (i + 1 == dt.Rows.Count || i == up * pageRecordCount - 1)
			{
				result += "</table></div>";
			}
		}
		if (pageRecordCount == 12)
		{
			if (i == (up - 1) * pageRecordCount)
			{
				result = result + "<div class=\"page\">商品清冊．Page " + up + "/" + un + "<table style=\"font-size:.75em;\">";
			}
			string text = "";
			if ((i + 1) % 3 == 1)
			{
				text = " style=\"width:33%;vertical-align:top;\"";
				result += "<tr>";
			}
			if ((i + 1) % 3 == 2)
			{
				text = " style=\"width:33%;\"";
			}
			result = result + "<td" + text + "><table summary=\"一組商品條碼\">" + content12(dt, i) + "</table></td>";
			if (i + 1 == dt.Rows.Count && (dt.Rows.Count % 3 == 1 || dt.Rows.Count % 3 == 2))
			{
				result += "<td style=\"width:33%;vertical-align:top;\"></td><td style=\"width:33%;\"></td><td></td>";
			}
			if ((i + 1) % 3 == 0 || i + 1 == dt.Rows.Count)
			{
				result += "</tr>";
			}
			if (i + 1 == dt.Rows.Count || i == up * pageRecordCount - 1)
			{
				result += "</table></div>";
			}
		}
		return result;
	}

	private string tableConentPRO(DataTable dt, string result, int pageRecordCount, int i, int up, int un)
	{
		if (i == (up - 1) * pageRecordCount)
		{
			result = result + "<div class=\"page\">商品清冊．Page " + up + "/" + un;
		}
		result = result + "<table summary=\"資料表格\" style=\"font-size:.9em;\">" + contentPRO(dt, i) + "</table>";
		if (i + 1 == _cropList.Count || i == up * pageRecordCount - 1)
		{
			result += "</div>";
		}
		return result;
	}

	private string contentPRO(DataTable dt, int i)
	{
		string text = "";
		if (dt.Rows.Count > 0)
		{
			string text2 = _barCode + "-" + _cropList[i] + "-" + _blightList[i];
			string text3 = dt.Rows[0]["GDSNO"].ToString();
			string text4 = dt.Rows[0]["GDName"].ToString();
			string text5 = dt.Rows[0]["domManufId"].ToString();
			string text6 = dt.Rows[0]["brandName"].ToString();
			string text7 = dt.Rows[0]["CName"].ToString();
			string text8 = dt.Rows[0]["contents"].ToString();
			string text9 = dt.Rows[0]["spec"].ToString();
			string text10 = dt.Rows[0]["capacity"].ToString();
			string[] array = new string[7]
			{
				"廠牌(商品)名稱",
				"許可證號",
				"廠商名稱",
				"中文普通名稱",
				"含量",
				"容器",
				"容量"
			};
			string[] array2 = new string[7]
			{
				text4,
				text5,
				text6,
				text7,
				text8,
				text9,
				text10
			};
			string text11 = _ExePath + "\\TempBarCode\\" + text2 + ".gif";
			text = string.Concat("<tr><td rowspan=\"8\" style=\"width:13%;vertical-align:top;text-align:center;\"><img src=\"" + text11 + "\" style=\"width:200px;\"/><table style=\"border:none;font-size:85%\"><tr><td style=\"border:none;\">" + text2 + "</td></tr><tr><td style=\"border:none;\">" + text4, "</td></tr><tr><td style=\"border: none;\">", _cropMap[_cropList[i]], " x ", _blightMap[_blightList[i]], "</td></tr></table></td><th style=\"width:22%;\">商品條碼</th><td>", text3, "</td></tr>");
			for (int j = 0; j < array.Length; j++)
			{
				text = text + "<tr><th>" + array[j] + "</th><td>" + array2[j] + "</td></tr>";
			}
		}
		return text;
	}

	private string content3(DataTable dt, int i)
	{
		string text = dt.Rows[i]["domManufId"].ToString();
		string str = dt.Rows[i]["GDSNO"].ToString() + ".gif";
		string text2 = dt.Rows[i]["CName"].ToString();
		string text3 = dt.Rows[i]["contents"].ToString();
		string text4 = dt.Rows[i]["spec"].ToString();
		string text5 = dt.Rows[i]["capacity"].ToString();
		string[] array = new string[5]
		{
			"廠牌(商品)名稱",
			"中文普通名稱",
			"含量",
			"容器",
			"容量"
		};
		string[] array2 = new string[5]
		{
			commodityName(dt, i),
			text2,
			text3,
			text4,
			text5
		};
		string text6 = _ExePath + "\\TempBarCode\\" + str;
		string text7 = "<tr><td rowspan=\"7\" style=\"width:15%;\"><img src=\"" + text6 + "\" style=\"width:200px;\"/></td><th style=\"width:25%;\">許可證號</th><td>" + text + "</td></tr>";
		for (int j = 0; j < array.Length; j++)
		{
			text7 = text7 + "<tr><th>" + array[j] + "</th><td>" + array2[j] + "</td></tr>";
		}
		return text7;
	}

	private string content6(DataTable dt, int i)
	{
		string str = dt.Rows[i]["GDSNO"].ToString() + ".gif";
		string text = dt.Rows[i]["spec"].ToString();
		string text2 = dt.Rows[i]["capacity"].ToString();
		string[] array = new string[2]
		{
			"包裝容器",
			"容量單位"
		};
		string[] array2 = new string[2]
		{
			text,
			text2
		};
		string text3 = _ExePath + "\\TempBarCode\\" + str;
		string text4 = "<tr><td colspan=\"2\" style=\"text-align:center;height:100px;\"><img src=\"" + text3 + "\" style=\"width:185px;\"/></td></tr><tr><td colspan=\"2\" style=\"height:65px;vertical-align:top;\">" + commodityName(dt, i) + "</td></tr>";
		for (int j = 0; j < array.Length; j++)
		{
			text4 = text4 + "<tr><th>" + array[j] + "</th><td>" + array2[j] + "</td></tr>";
		}
		return text4;
	}

	private string content12(DataTable dt, int i)
	{
		string str = dt.Rows[i]["GDSNO"].ToString() + ".gif";
		string text = dt.Rows[i]["spec"].ToString();
		string text2 = dt.Rows[i]["capacity"].ToString();
		string text3 = _ExePath + "\\TempBarCode\\" + str;
		return "<tr><td colspan=\"2\" style=\"text-align:center;height:115px;\"><img src=\"" + text3 + "\" style=\"width:135px;\"/></td></tr><tr><td style=\"height:45px;\">" + commodityName(dt, i) + "</td></tr><tr><td style=\"font-size:.9em;\">" + text + ((!"".Equals(text2.Trim())) ? " / " : "") + text2 + "</td></tr>";
	}

	private string commodityName(DataTable dt, int i)
	{
		string text = dt.Rows[i]["GDName"].ToString();
		string text2 = dt.Rows[i]["CName"].ToString();
		string text3 = dt.Rows[i]["contents"].ToString();
		string text4 = dt.Rows[i]["brandName"].ToString();
		if (!string.IsNullOrEmpty(text2) || !string.IsNullOrEmpty(text3) || !string.IsNullOrEmpty(text4))
		{
			text += "[";
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + text2 + "-";
			}
			if (!string.IsNullOrEmpty(text3))
			{
				text = text + text3 + "-";
			}
			if (!string.IsNullOrEmpty(text4))
			{
				text = text + text4 + "-";
			}
			int length = text.LastIndexOf("-");
			text = text.Substring(0, length) + "]";
		}
		return text;
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		string text = null;
		try
		{
			text = new PrintDocument().PrinterSettings.PrinterName;
			if (!string.IsNullOrEmpty(_printerName))
			{
				myPrinters.SetDefaultPrinter(_printerName);
			}
			IEPageSetup();
			webBrowser1.Print();
			AutoClosingMessageBox.Show("列印完成", 1000);
		}
		catch (Exception ex)
		{
			Console.WriteLine("列印錯誤 ::: " + ex.Message);
		}
		finally
		{
			if (!string.IsNullOrEmpty(text))
			{
				myPrinters.SetDefaultPrinter(text);
			}
		}
	}

	private void btnPrintView_Click(object sender, EventArgs e)
	{
		string text = null;
		try
		{
			text = new PrintDocument().PrinterSettings.PrinterName;
			if (!string.IsNullOrEmpty(_printerName))
			{
				myPrinters.SetDefaultPrinter(_printerName);
			}
			IEPageSetup();
			webBrowser1.ShowPrintPreviewDialog();
			AutoClosingMessageBox.Show("列印預覽完成", 1000);
		}
		catch (Exception ex)
		{
			Console.WriteLine("列印預覽錯誤 ::: " + ex.Message);
		}
		finally
		{
			if (!string.IsNullOrEmpty(text))
			{
				myPrinters.SetDefaultPrinter(text);
			}
		}
	}

	private void IEPageSetup()
	{
		string name = "Software\\Microsoft\\Internet Explorer\\PageSetup";
		bool writable = true;
		object obj = "";
		object obj2 = "0.395670";
		object obj3 = "0.166540";
		string[] array = new string[6]
		{
			"footer",
			"header",
			"margin_bottom",
			"margin_left",
			"margin_right",
			"margin_top"
		};
		object[] array2 = new object[6]
		{
			obj,
			obj,
			obj2,
			obj3,
			obj3,
			obj3
		};
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(name, writable);
		for (int i = 0; i < array.Length; i++)
		{
			registryKey.SetValue(array[i], array2[i]);
		}
		registryKey.Close();
	}

	private void button1_Click(object sender, EventArgs e)
	{
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
		webBrowser1 = new System.Windows.Forms.WebBrowser();
		btnPrint = new System.Windows.Forms.Button();
		btnPrintView = new System.Windows.Forms.Button();
		button1 = new System.Windows.Forms.Button();
		SuspendLayout();
		webBrowser1.Location = new System.Drawing.Point(22, 56);
		webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
		webBrowser1.Name = "webBrowser1";
		webBrowser1.Size = new System.Drawing.Size(974, 698);
		webBrowser1.TabIndex = 2;
		webBrowser1.Url = new System.Uri("", System.UriKind.Relative);
		webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
		btnPrint.Location = new System.Drawing.Point(22, 12);
		btnPrint.Name = "btnPrint";
		btnPrint.Size = new System.Drawing.Size(75, 38);
		btnPrint.TabIndex = 3;
		btnPrint.Text = "立即列印";
		btnPrint.UseVisualStyleBackColor = true;
		btnPrint.Click += new System.EventHandler(btnPrint_Click);
		btnPrintView.Location = new System.Drawing.Point(103, 12);
		btnPrintView.Name = "btnPrintView";
		btnPrintView.Size = new System.Drawing.Size(75, 38);
		btnPrintView.TabIndex = 4;
		btnPrintView.Text = "預覽列印";
		btnPrintView.UseVisualStyleBackColor = true;
		btnPrintView.Click += new System.EventHandler(btnPrintView_Click);
		button1.Location = new System.Drawing.Point(220, 12);
		button1.Name = "button1";
		button1.Size = new System.Drawing.Size(75, 38);
		button1.TabIndex = 5;
		button1.Text = "關閉視窗";
		button1.UseVisualStyleBackColor = true;
		button1.Click += new System.EventHandler(button1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1017, 766);
		base.Controls.Add(button1);
		base.Controls.Add(btnPrintView);
		base.Controls.Add(btnPrint);
		base.Controls.Add(webBrowser1);
		base.Name = "Commodity_barcode";
		Text = "Commodity_BarCode";
		ResumeLayout(false);
	}
}
