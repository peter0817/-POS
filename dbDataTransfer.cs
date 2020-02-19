using POS_Client;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class dbDataTransfer : Form
{
	private IContainer components;

	private ProgressBar progressBar1;

	private Label label1;

	private BackgroundWorker backgroundWorker1;

	private Label label2;

	public dbDataTransfer()
	{
		InitializeComponent();
		label1.Show();
		label2.Show();
		progressBar1.Show();
		label1.Refresh();
		label2.Refresh();
		progressBar1.Refresh();
		progressBar1.Maximum = 100;
		progressBar1.Minimum = 0;
		backgroundWorker1.WorkerReportsProgress = true;
		backgroundWorker1.RunWorkerAsync();
	}

	private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
	{
		DataTransfer();
	}

	private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		progressBar1.Value = e.ProgressPercentage;
		label1.Text = "移轉進度: " + e.ProgressPercentage + "%";
		switch (e.ProgressPercentage)
		{
		case 0:
			label2.Text = "進度說明:轉入商品資訊中";
			break;
		case 35:
			label2.Text = "進度說明:轉入會員資訊中";
			break;
		case 40:
			label2.Text = "進度說明:轉入廠商（供應商）資訊中";
			break;
		case 45:
			label2.Text = "進度說明:銷售單資訊轉入中";
			break;
		case 75:
			label2.Text = "進度說明:會員銷售（賒帳）／賒帳還款紀錄轉入中";
			break;
		case 90:
			label2.Text = "進度說明:進貨單轉入中";
			break;
		case 100:
			label2.Text = "進度說明:資料移轉結束";
			break;
		}
	}

	private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		Close();
	}

	private void DataTransfer()
	{
		try
		{
			backgroundWorker1.ReportProgress(0);
			string sql = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; UPDATE hypos_GOODSLST SET oldGDSNO = (select a.GDSNO from old.GOODSLST as a where a.GDSNO = hypos_GOODSLST.GDSNO),  Cost = (select a.PRICE from old.GOODSLST as a where a.GDSNO = hypos_GOODSLST.GDSNO),  Price = (select a.PRICE_R0 from old.GOODSLST as a where a.GDSNO = hypos_GOODSLST.GDSNO),  SpecialPrice1 = (select a.PRICE_R1 from old.GOODSLST as a where a.GDSNO = hypos_GOODSLST.GDSNO), SpecialPrice2 = (select a.PRICE_R2 from old.GOODSLST as a where a.GDSNO = hypos_GOODSLST.GDSNO) WHERE GDSNO in (select GDSNO from old.GOODSLST where (PL_TYP1 = '1' or PL_TYP2 = '1'))";
			DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteNonQuery);
			string sql2 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; UPDATE hypos_GOODSLST SET OpenPrice = 1 WHERE GDSNO in (select GDSNO from old.GOODSLST where (PL_TYP1 = '1' or PL_TYP2 = '1') and(IS_NOOFF = 0 or IS_OPPRC = 1))";
			DataBaseUtilities.DBOperation(Program.ConnectionString, sql2, null, CommandOperationType.ExecuteNonQuery);
			string sql3 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; UPDATE hypos_GOODSLST SET status = 'U' WHERE GDSNO in (select GDSNO from old.GOODSLST where (PL_TYP1 = '1' or PL_TYP2 = '1') and(VENNO is not null and VENNO != ''))";
			DataBaseUtilities.DBOperation(Program.ConnectionString, sql3, null, CommandOperationType.ExecuteNonQuery);
			backgroundWorker1.ReportProgress(10);
			string sql4 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; select * from old.GOODSLST WHERE PL_TYP1='0' and GDSNO2 in (SELECT GDSNO FROM hypos_GOODSLST)";
			DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql4, null, CommandOperationType.ExecuteReaderReturnDataTable);
			string sql5 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; select * from old.GOODSLST WHERE GDSNO not in (select GDSNO from hypos_GOODSLST) and(PL_TYP1 = '1' or PL_TYP2 = '1') and GDSNO2 in ('')";
			DataTable dataTable2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql5, null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable2.Rows.Count > 0)
			{
				for (int i = 0; i < dataTable2.Rows.Count; i++)
				{
					string text = "";
					switch (dataTable2.Rows[i]["CLA1NO"].ToString())
					{
					case "0001":
						text = "0305";
						break;
					case "0002":
						text = "0302";
						break;
					case "0302":
						text = "0302";
						break;
					case "0303":
						text = "0303";
						break;
					case "0305":
						text = "0305";
						break;
					default:
						text = "0308";
						break;
					}
					string text2 = "";
					if ("False".Equals(dataTable2.Rows[i]["IS_NOOFF"].ToString()) || "True".Equals(dataTable2.Rows[i]["IS_OPPRC"].ToString()))
					{
						text2 = "1";
					}
					string[] strParameterArray = new string[11]
					{
						getNewGDSNO(),
						dataTable2.Rows[i]["GDSNO"].ToString(),
						dataTable2.Rows[i]["GDSNAME"].ToString(),
						dataTable2.Rows[i]["MKERNO"].ToString(),
						dataTable2.Rows[i]["BUNITNO"].ToString(),
						text,
						dataTable2.Rows[i]["PRICE"].ToString(),
						dataTable2.Rows[i]["PRICE_R0"].ToString(),
						dataTable2.Rows[i]["PRICE_R1"].ToString(),
						dataTable2.Rows[i]["PRICE_R2"].ToString(),
						text2
					};
					string sql6 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; INSERT INTO hypos_GOODSLST ( GDSNO, oldGDSNO, GDName, brandName, spec, ISWS, CLA1NO, Cost, Price, SpecialPrice1, SpecialPrice2, OpenPrice, status, CreateDate, UpdateDate)  VALUES({0},{1},{2},(select NAME from MAKER where MKNO = {3}),(select UNITNAME from UNIT where UNITNO = {4}),'N',{5} , {6}, {7}, {8}, {9}, {10}, 'U', DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME'), DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME') ); ";
					DataBaseUtilities.DBOperation(Program.ConnectionString, sql6, strParameterArray, CommandOperationType.ExecuteNonQuery);
				}
			}
			backgroundWorker1.ReportProgress(15);
			string sql7 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; INSERT INTO hypos_GOODSLST (GDSNO, oldGDSNO, barcode, GDName, brandName, spec, ISWS, CLA1NO, Cost, Price, SpecialPrice1, SpecialPrice2, OpenPrice, status, CreateDate, UpdateDate) SELECT GDSNO2, GDSNO, GDSNO2, GDSNAME, (select NAME from MAKER where MKNO = MKERNO), (select UNITNAME from UNIT where UNITNO = BUNITNO), 'N', CASE CLA1NO    WHEN '0001' THEN '0305'   WHEN '0002' THEN '0302'    WHEN '0302' THEN '0302'    WHEN '0303' THEN '0303'    WHEN '0305' THEN '0305'    ELSE '0308' END, PRICE, PRICE_R0, PRICE_R1,  PRICE_R2, CASE(select a.GDSNO from old.GOODSLST as a where a.IS_NOOFF = 0 or a.IS_OPPRC = 1)    WHEN GDSNO THEN 1    ELSE null END, 'U', DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME'), DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME') FROM old.GOODSLST WHERE GDSNO not in (select GDSNO from hypos_GOODSLST) and (PL_TYP1='1' or PL_TYP2='1') and GDSNO2 not in ('')";
			DataBaseUtilities.DBOperation(Program.ConnectionString, sql7, null, CommandOperationType.ExecuteNonQuery);
			backgroundWorker1.ReportProgress(20);
			string sql8 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; select * from old.GOODSLST WHERE GDSNO2 in ('') and PL_TYP1='0' and GDSNO not in (select GDSNO from hypos_GOODSLST)";
			DataTable dataTable3 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql8, null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable3.Rows.Count > 0)
			{
				for (int j = 0; j < dataTable3.Rows.Count; j++)
				{
					string text3 = "";
					switch (dataTable3.Rows[j]["CLA1NO"].ToString())
					{
					case "0001":
						text3 = "0305";
						break;
					case "0002":
						text3 = "0302";
						break;
					case "0302":
						text3 = "0302";
						break;
					case "0303":
						text3 = "0303";
						break;
					case "0305":
						text3 = "0305";
						break;
					default:
						text3 = "0308";
						break;
					}
					string text4 = "";
					if ("False".Equals(dataTable3.Rows[j]["IS_NOOFF"].ToString()) || "True".Equals(dataTable3.Rows[j]["IS_OPPRC"].ToString()))
					{
						text4 = "1";
					}
					string[] strParameterArray2 = new string[11]
					{
						getNewGDSNO(),
						dataTable3.Rows[j]["GDSNO"].ToString(),
						dataTable3.Rows[j]["GDSNAME"].ToString(),
						dataTable3.Rows[j]["MKERNO"].ToString(),
						dataTable3.Rows[j]["BUNITNO"].ToString(),
						text3,
						dataTable3.Rows[j]["PRICE"].ToString(),
						dataTable3.Rows[j]["PRICE_R0"].ToString(),
						dataTable3.Rows[j]["PRICE_R1"].ToString(),
						dataTable3.Rows[j]["PRICE_R2"].ToString(),
						text4
					};
					string sql9 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; INSERT INTO hypos_GOODSLST ( GDSNO, oldGDSNO, GDName, brandName, spec, ISWS, CLA1NO, Cost, Price, SpecialPrice1, SpecialPrice2, OpenPrice, status, CreateDate, UpdateDate)  VALUES({0},{1},{2},(select NAME from MAKER where MKNO = {3}),(select UNITNAME from UNIT where UNITNO = {4}),'N',{5} , {6}, {7}, {8}, {9}, {10}, 'U', DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME'), DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME') ); ";
					DataBaseUtilities.DBOperation(Program.ConnectionString, sql9, strParameterArray2, CommandOperationType.ExecuteNonQuery);
				}
			}
			backgroundWorker1.ReportProgress(25);
			string sql10 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; INSERT INTO hypos_GOODSLST (GDSNO, oldGDSNO, barcode, GDName, brandName, spec, ISWS, CLA1NO, Cost, Price, SpecialPrice1, SpecialPrice2, OpenPrice, status, CreateDate, UpdateDate) SELECT GDSNO2, GDSNO, GDSNO2, GDSNAME, (select NAME from MAKER where MKNO = MKERNO), (select UNITNAME from UNIT where UNITNO = BUNITNO), 'N', CASE CLA1NO    WHEN '0001' THEN '0305'   WHEN '0002' THEN '0302'    WHEN '0302' THEN '0302'    WHEN '0303' THEN '0303'    WHEN '0305' THEN '0305'    ELSE '0308' END, PRICE, PRICE_R0, PRICE_R1,  PRICE_R2, CASE(select a.GDSNO from old.GOODSLST as a where a.IS_NOOFF = 0 or a.IS_OPPRC = 1)    WHEN GDSNO THEN 1    ELSE null END, 'U', DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME'), DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME') FROM old.GOODSLST WHERE GDSNO2 not in ('') and PL_TYP1='0' and GDSNO2 not in  (SELECT a.GDSNO FROM hypos_GOODSLST as a inner join (select GDSNO2 from old.GOODSLST WHERE GDSNO2 not in ('') and PL_TYP1 = '0') as b on a.GDSNO = b.GDSNO2)";
			DataBaseUtilities.DBOperation(Program.ConnectionString, sql10, null, CommandOperationType.ExecuteNonQuery);
			backgroundWorker1.ReportProgress(30);
			if (dataTable.Rows.Count > 0)
			{
				for (int k = 0; k < dataTable.Rows.Count; k++)
				{
					string text5 = "";
					switch (dataTable.Rows[k]["CLA1NO"].ToString())
					{
					case "0001":
						text5 = "0305";
						break;
					case "0002":
						text5 = "0302";
						break;
					case "0302":
						text5 = "0302";
						break;
					case "0303":
						text5 = "0303";
						break;
					case "0305":
						text5 = "0305";
						break;
					default:
						text5 = "0308";
						break;
					}
					string text6 = "";
					if ("False".Equals(dataTable.Rows[k]["IS_NOOFF"].ToString()) || "True".Equals(dataTable.Rows[k]["IS_OPPRC"].ToString()))
					{
						text6 = "1";
					}
					string[] strParameterArray3 = new string[11]
					{
						getNewGDSNO(),
						dataTable.Rows[k]["GDSNO"].ToString(),
						dataTable.Rows[k]["GDSNAME"].ToString(),
						dataTable.Rows[k]["MKERNO"].ToString(),
						dataTable.Rows[k]["BUNITNO"].ToString(),
						text5,
						dataTable.Rows[k]["PRICE"].ToString(),
						dataTable.Rows[k]["PRICE_R0"].ToString(),
						dataTable.Rows[k]["PRICE_R1"].ToString(),
						dataTable.Rows[k]["PRICE_R2"].ToString(),
						text6
					};
					string sql11 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; INSERT INTO hypos_GOODSLST ( GDSNO, oldGDSNO, GDName, brandName, spec, ISWS, CLA1NO, Cost, Price, SpecialPrice1, SpecialPrice2, OpenPrice, status, CreateDate, UpdateDate)  VALUES({0},{1},{2},(select NAME from MAKER where MKNO = {3}),(select UNITNAME from UNIT where UNITNO = {4}),'N',{5} , {6}, {7}, {8}, {9}, {10}, 'U', DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME'), DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME') ); ";
					DataBaseUtilities.DBOperation(Program.ConnectionString, sql11, strParameterArray3, CommandOperationType.ExecuteNonQuery);
				}
			}
			backgroundWorker1.ReportProgress(35);
			string sql12 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM old.CUST_RTL";
			DataTable dataTable4 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql12, null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable4.Rows.Count > 0)
			{
				for (int l = 0; l < dataTable4.Rows.Count; l++)
				{
					string text7 = "";
					string a = dataTable4.Rows[l]["SPECTYPE"].ToString();
					text7 = ((a == "1") ? "2" : ((!(a == "2")) ? "1" : "3"));
					string[,] strFieldArray = new string[19, 2]
					{
						{
							"LicenseCode",
							Program.LincenseCode
						},
						{
							"VipNo",
							getNewVipNo()
						},
						{
							"oldVIPNO",
							dataTable4.Rows[l]["VIPNO"].ToString()
						},
						{
							"Name",
							dataTable4.Rows[l]["NAME"].ToString()
						},
						{
							"IdNo",
							dataTable4.Rows[l]["IDNO"].ToString()
						},
						{
							"BirthDate",
							dataTable4.Rows[l]["BIRTHDAY"].ToString()
						},
						{
							"Telphone",
							dataTable4.Rows[l]["HTEL"].ToString()
						},
						{
							"Mobile",
							dataTable4.Rows[l]["MTEL"].ToString()
						},
						{
							"EMail",
							dataTable4.Rows[l]["EMAIL"].ToString()
						},
						{
							"CompanyIdNo",
							dataTable4.Rows[l]["COMPNA"].ToString()
						},
						{
							"CompanyName",
							dataTable4.Rows[l]["COMPID"].ToString()
						},
						{
							"CompanyTel",
							dataTable4.Rows[l]["CTEL"].ToString()
						},
						{
							"Fax",
							dataTable4.Rows[l]["CFAX"].ToString()
						},
						{
							"Address",
							dataTable4.Rows[l]["ADDR1"].ToString()
						},
						{
							"Type",
							text7
						},
						{
							"Status",
							"0"
						},
						{
							"BuyDate",
							dataTable4.Rows[l]["LSDATE"].ToString()
						},
						{
							"CreateDate",
							dataTable4.Rows[l]["UD_DATE"].ToString()
						},
						{
							"UpdateDate",
							DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd HH:mm:ss")
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_CUST_RTL", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
				}
			}
			backgroundWorker1.ReportProgress(40);
			string sql13 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM old.VENDOR";
			DataTable dataTable5 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql13, null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable5.Rows.Count > 0)
			{
				for (int m = 0; m < dataTable5.Rows.Count; m++)
				{
					string text8 = "";
					string a = dataTable5.Rows[m]["CLA1NO"].ToString();
					text8 = ((a == "V1") ? "0" : ((!(a == "V2")) ? "" : "1"));
					string text9 = "";
					a = dataTable5.Rows[m]["IS_TOUCH"].ToString();
					text9 = ((a == "1") ? "0" : ((!(a == "0")) ? "0" : "1"));
					string[,] strFieldArray2 = new string[15, 2]
					{
						{
							"SupplierNo",
							getSupplierNo()
						},
						{
							"oldVENDO",
							dataTable5.Rows[m]["VENNO"].ToString()
						},
						{
							"SupplierName",
							dataTable5.Rows[m]["NAME"].ToString()
						},
						{
							"SupplierIdNo",
							dataTable5.Rows[m]["COMPID"].ToString()
						},
						{
							"DutyName",
							dataTable5.Rows[m]["TOPMAN"].ToString()
						},
						{
							"TelNo",
							dataTable5.Rows[m]["TEL1"].ToString()
						},
						{
							"Fax",
							dataTable5.Rows[m]["FAX"].ToString()
						},
						{
							"Mobile",
							dataTable5.Rows[m]["TEL2"].ToString()
						},
						{
							"ContactName",
							dataTable5.Rows[m]["THMAN"].ToString()
						},
						{
							"ContactJob",
							dataTable5.Rows[m]["THMDUTY"].ToString()
						},
						{
							"Address",
							dataTable5.Rows[m]["ADDR1"].ToString()
						},
						{
							"Type",
							text8
						},
						{
							"Status",
							text9
						},
						{
							"CreateDate",
							DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd HH:mm:ss")
						},
						{
							"EditDate",
							DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd HH:mm:ss")
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_Supplier", "", "", strFieldArray2, null, CommandOperationType.ExecuteNonQuery);
				}
			}
			backgroundWorker1.ReportProgress(45);
			string sql14 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT distinct(SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO) as SNO FROM ECRHDHS order by HDATE, HTIME ";
			DataTable dataTable6 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql14, null, CommandOperationType.ExecuteReaderReturnDataTable);
			for (int n = 0; n < dataTable6.Rows.Count; n++)
			{
				string[] strParameterArray4 = new string[1]
				{
					dataTable6.Rows[n]["SNO"].ToString()
				};
				string sql15 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO) as SNO FROM old.ECRPYHS  where(SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) not in  ( SELECT SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO FROM old.ECRTRHS where HQTY < 0 ) and (SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) not in  ( SELECT SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO as SNO  FROM old.ECRTRHS where HGBCNO = 'REPAYMENT' and HGTYPE = 0 ) and  (SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) not in  ( SELECT(SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) as SNO FROM ECRHDHS where (SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) in (SELECT(SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) as SNO FROM ECRPYHS where HPYORD = 2) and(SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) not in (SELECT SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO as SNO FROM old.ECRTRHS where HGBCNO = 'REPAYMENT' and HGTYPE = 0) ) and {0} = (SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO)";
				if (((DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql15, strParameterArray4, CommandOperationType.ExecuteReaderReturnDataTable)).Rows.Count > 0)
				{
					string hseqNo = getHseqNo();
					string text10 = DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd HH:mm:ss");
					string[] strParameterArray5 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql16 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRHDHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					DataTable dataTable7 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql16, strParameterArray5, CommandOperationType.ExecuteReaderReturnDataTable);
					string[] strParameterArray6 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql17 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRPYHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					DataTable dataTable8 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql17, strParameterArray6, CommandOperationType.ExecuteReaderReturnDataTable);
					string[] strParameterArray7 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql18 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRTRHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					DataTable dataTable9 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql18, strParameterArray7, CommandOperationType.ExecuteReaderReturnDataTable);
					string text11 = "";
					string[] strParameterArray8 = new string[1]
					{
						dataTable7.Rows[0]["HCU_NO"].ToString()
					};
					string sql19 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT VipNo FROM hypos_CUST_RTL where oldVIPNO LIKE {0}";
					DataTable dataTable10 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql19, strParameterArray8, CommandOperationType.ExecuteReaderReturnDataTable);
					text11 = ((dataTable10.Rows.Count <= 0) ? "" : dataTable10.Rows[0]["VipNo"].ToString());
					string text12 = "";
					string[] strParameterArray9 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql20 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT sum(HQTY) as itemstotal FROM ECRTRHS where(SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) = {0}";
					DataTable dataTable11 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql20, strParameterArray9, CommandOperationType.ExecuteReaderReturnDataTable);
					text12 = ((dataTable11.Rows.Count <= 0) ? "" : Math.Abs(ConvertToInt(dataTable11.Rows[0]["itemstotal"].ToString())).ToString());
					string text13 = "";
					string a = dataTable7.Rows[0]["HHD_VLD"].ToString();
					if (a == null || a.Length != 0)
					{
						if (a == "D")
						{
							text13 = "1";
						}
					}
					else
					{
						text13 = "0";
					}
					string str = Convert.ToDateTime(dataTable7.Rows[0]["HDATE"].ToString()).ToString("yyyy-MM-dd");
					string text14 = "0";
					string text15 = "0";
					if (dataTable8.Rows.Count > 0)
					{
						if ("01".Equals(dataTable8.Rows[0]["HPYNO"].ToString()))
						{
							text14 = dataTable8.Rows[0]["HPYAMT"].ToString();
						}
						else if ("03".Equals(dataTable8.Rows[0]["HPYNO"].ToString()))
						{
							text15 = dataTable8.Rows[0]["HPYAMT"].ToString();
						}
					}
					string[,] strFieldArray3 = new string[15, 2]
					{
						{
							"sellNo",
							hseqNo
						},
						{
							"oldECRHDHSNo",
							dataTable6.Rows[n]["SNO"].ToString()
						},
						{
							"sellTime",
							str + " " + dataTable7.Rows[0]["HTIME"].ToString()
						},
						{
							"memberId",
							text11
						},
						{
							"sum",
							ConvertToInt(dataTable7.Rows[0]["HASTOTAL"].ToString()).ToString()
						},
						{
							"items",
							dataTable7.Rows[0]["HTR_NUM"].ToString()
						},
						{
							"itemstotal",
							text12
						},
						{
							"status",
							text13
						},
						{
							"sumDiscount",
							"0"
						},
						{
							"sumRebate",
							"0"
						},
						{
							"Refund",
							"0"
						},
						{
							"changcount",
							"1"
						},
						{
							"editDate",
							text10
						},
						{
							"Cash",
							text14
						},
						{
							"Credit",
							text15
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_main_sell", "", "", strFieldArray3, null, CommandOperationType.ExecuteNonQuery);
					string text16 = "";
					text16 = ((ConvertToInt(dataTable7.Rows[0]["HASTOTAL"].ToString()) >= 0) ? ConvertToInt(dataTable7.Rows[0]["HASTOTAL"].ToString()).ToString() : "0");
					string[,] strFieldArray4 = new string[3, 2]
					{
						{
							"sellNo",
							hseqNo
						},
						{
							"changeDate",
							str + " " + dataTable7.Rows[0]["HTIME"].ToString()
						},
						{
							"sum",
							text16
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_mainsell_log", "", "", strFieldArray4, null, CommandOperationType.ExecuteNonQuery);
					if (dataTable9.Rows.Count > 0)
					{
						for (int num = 0; num < dataTable9.Rows.Count; num++)
						{
							string text17 = "";
							string[] strParameterArray10 = new string[1]
							{
								dataTable9.Rows[num]["HGDSNO"].ToString()
							};
							string sql21 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT GDSNO FROM hypos_GOODSLST where oldGDSNO = {0}";
							DataTable dataTable12 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql21, strParameterArray10, CommandOperationType.ExecuteReaderReturnDataTable);
							text17 = ((dataTable12.Rows.Count <= 0) ? "" : dataTable12.Rows[0]["GDSNO"].ToString());
							string[,] strFieldArray5 = new string[10, 2]
							{
								{
									"sellNo",
									hseqNo
								},
								{
									"barcode",
									text17
								},
								{
									"fixedPrice",
									dataTable9.Rows[num]["HOPRICE"].ToString()
								},
								{
									"sellingPrice",
									dataTable9.Rows[num]["HSPRICE"].ToString()
								},
								{
									"num",
									dataTable9.Rows[num]["HQTY"].ToString()
								},
								{
									"subtotal",
									(ConvertToInt(dataTable9.Rows[num]["HQTY"].ToString()) * ConvertToInt(dataTable9.Rows[num]["HSPRICE"].ToString())).ToString()
								},
								{
									"discount",
									"0"
								},
								{
									"total",
									ConvertToInt(dataTable9.Rows[num]["HSLTOT"].ToString()).ToString()
								},
								{
									"PRNO",
									dataTable9.Rows[num]["CropNo"].ToString()
								},
								{
									"BLNO",
									dataTable9.Rows[num]["PestNo"].ToString()
								}
							};
							DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_detail_sell", "", "", strFieldArray5, null, CommandOperationType.ExecuteNonQuery);
						}
					}
				}
				string[] strParameterArray11 = new string[1]
				{
					dataTable6.Rows[n]["SNO"].ToString()
				};
				string sql22 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO) as SNO FROM ECRHDHS where (SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) in (SELECT(SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) as SNO FROM ECRPYHS where HPYORD = 2) and(SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) not in (SELECT SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO as SNO FROM old.ECRTRHS where HGBCNO = 'REPAYMENT' and HGTYPE = 0) and(SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) = {0}";
				if (((DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql22, strParameterArray11, CommandOperationType.ExecuteReaderReturnDataTable)).Rows.Count > 0)
				{
					string hseqNo2 = getHseqNo();
					string text18 = DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd HH:mm:ss");
					string[] strParameterArray12 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql23 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRHDHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					DataTable dataTable13 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql23, strParameterArray12, CommandOperationType.ExecuteReaderReturnDataTable);
					string[] strParameterArray13 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql24 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRPYHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					DataTable dataTable14 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql24, strParameterArray13, CommandOperationType.ExecuteReaderReturnDataTable);
					string[] strParameterArray14 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql25 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRTRHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					DataTable dataTable15 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql25, strParameterArray14, CommandOperationType.ExecuteReaderReturnDataTable);
					string text19 = "";
					string[] strParameterArray15 = new string[1]
					{
						dataTable13.Rows[0]["HCU_NO"].ToString()
					};
					string sql26 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT VipNo FROM hypos_CUST_RTL where oldVIPNO LIKE {0}";
					DataTable dataTable16 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql26, strParameterArray15, CommandOperationType.ExecuteReaderReturnDataTable);
					text19 = ((dataTable16.Rows.Count <= 0) ? "" : dataTable16.Rows[0]["VipNo"].ToString());
					string text20 = "";
					string[] strParameterArray16 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql27 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT sum(HQTY) as itemstotal FROM ECRTRHS where(SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) = {0}";
					DataTable dataTable17 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql27, strParameterArray16, CommandOperationType.ExecuteReaderReturnDataTable);
					text20 = ((dataTable17.Rows.Count <= 0) ? "" : Math.Abs(ConvertToInt(dataTable17.Rows[0]["itemstotal"].ToString())).ToString());
					string text21 = "";
					string a = dataTable13.Rows[0]["HHD_VLD"].ToString();
					if (a == null || a.Length != 0)
					{
						if (a == "D")
						{
							text21 = "1";
						}
					}
					else
					{
						text21 = "0";
					}
					string str2 = Convert.ToDateTime(dataTable13.Rows[0]["HDATE"].ToString()).ToString("yyyy-MM-dd");
					string text22 = "0";
					string text23 = "0";
					if (dataTable14.Rows.Count > 0)
					{
						for (int num2 = 0; num2 < dataTable14.Rows.Count; num2++)
						{
							if ("01".Equals(dataTable14.Rows[num2]["HPYNO"].ToString()))
							{
								text22 = dataTable14.Rows[num2]["HPYAMT"].ToString();
							}
							else if ("03".Equals(dataTable14.Rows[num2]["HPYNO"].ToString()))
							{
								text23 = dataTable14.Rows[num2]["HPYAMT"].ToString();
							}
						}
					}
					string[,] strFieldArray6 = new string[15, 2]
					{
						{
							"sellNo",
							hseqNo2
						},
						{
							"oldECRHDHSNo",
							dataTable6.Rows[n]["SNO"].ToString()
						},
						{
							"sellTime",
							str2 + " " + dataTable13.Rows[0]["HTIME"].ToString()
						},
						{
							"memberId",
							text19
						},
						{
							"sum",
							ConvertToInt(dataTable13.Rows[0]["HASTOTAL"].ToString()).ToString()
						},
						{
							"items",
							dataTable13.Rows[0]["HTR_NUM"].ToString()
						},
						{
							"itemstotal",
							text20
						},
						{
							"status",
							text21
						},
						{
							"sumDiscount",
							"0"
						},
						{
							"sumRebate",
							"0"
						},
						{
							"Refund",
							"0"
						},
						{
							"changcount",
							"1"
						},
						{
							"editDate",
							text18
						},
						{
							"Cash",
							text22
						},
						{
							"Credit",
							text23
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_main_sell", "", "", strFieldArray6, null, CommandOperationType.ExecuteNonQuery);
					string text24 = "";
					text24 = ((ConvertToInt(dataTable13.Rows[0]["HASTOTAL"].ToString()) >= 0) ? ConvertToInt(dataTable13.Rows[0]["HASTOTAL"].ToString()).ToString() : "0");
					string[,] strFieldArray7 = new string[3, 2]
					{
						{
							"sellNo",
							hseqNo2
						},
						{
							"changeDate",
							str2 + " " + dataTable13.Rows[0]["HTIME"].ToString()
						},
						{
							"sum",
							text24
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_mainsell_log", "", "", strFieldArray7, null, CommandOperationType.ExecuteNonQuery);
					if (dataTable15.Rows.Count > 0)
					{
						for (int num3 = 0; num3 < dataTable15.Rows.Count; num3++)
						{
							string text25 = "";
							string[] strParameterArray17 = new string[1]
							{
								dataTable15.Rows[num3]["HGDSNO"].ToString()
							};
							string sql28 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT GDSNO FROM hypos_GOODSLST where oldGDSNO = {0}";
							DataTable dataTable18 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql28, strParameterArray17, CommandOperationType.ExecuteReaderReturnDataTable);
							text25 = ((dataTable18.Rows.Count <= 0) ? "" : dataTable18.Rows[0]["GDSNO"].ToString());
							string[,] strFieldArray8 = new string[10, 2]
							{
								{
									"sellNo",
									hseqNo2
								},
								{
									"barcode",
									text25
								},
								{
									"fixedPrice",
									dataTable15.Rows[num3]["HOPRICE"].ToString()
								},
								{
									"sellingPrice",
									dataTable15.Rows[num3]["HSPRICE"].ToString()
								},
								{
									"num",
									dataTable15.Rows[num3]["HQTY"].ToString()
								},
								{
									"subtotal",
									(ConvertToInt(dataTable15.Rows[num3]["HQTY"].ToString()) * ConvertToInt(dataTable15.Rows[num3]["HSPRICE"].ToString())).ToString()
								},
								{
									"discount",
									"0"
								},
								{
									"total",
									ConvertToInt(dataTable15.Rows[num3]["HSLTOT"].ToString()).ToString()
								},
								{
									"PRNO",
									dataTable15.Rows[num3]["CropNo"].ToString()
								},
								{
									"BLNO",
									dataTable15.Rows[num3]["PestNo"].ToString()
								}
							};
							DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_detail_sell", "", "", strFieldArray8, null, CommandOperationType.ExecuteNonQuery);
						}
					}
				}
				string[] strParameterArray18 = new string[1]
				{
					dataTable6.Rows[n]["SNO"].ToString()
				};
				string sql29 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT (SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) as SNO FROM ECRHDHS where HPY_NUM = 0 and HASTOTAL = 0 and SNO not in (SELECT(SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) as SNO FROM ECRPYHS) and SNO not in (SELECT SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO as SNO FROM old.ECRTRHS where HQTY < 0) and (SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) = {0}";
				if (((DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql29, strParameterArray18, CommandOperationType.ExecuteReaderReturnDataTable)).Rows.Count > 0)
				{
					string hseqNo3 = getHseqNo();
					string text26 = DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd HH:mm:ss");
					string[] strParameterArray19 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql30 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRHDHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					DataTable dataTable19 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql30, strParameterArray19, CommandOperationType.ExecuteReaderReturnDataTable);
					string[] strParameterArray20 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql31 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRPYHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					_ = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql31, strParameterArray20, CommandOperationType.ExecuteReaderReturnDataTable);
					string[] strParameterArray21 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql32 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRTRHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					DataTable dataTable20 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql32, strParameterArray21, CommandOperationType.ExecuteReaderReturnDataTable);
					string text27 = "";
					string[] strParameterArray22 = new string[1]
					{
						dataTable19.Rows[0]["HCU_NO"].ToString()
					};
					string sql33 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT VipNo FROM hypos_CUST_RTL where oldVIPNO LIKE {0}";
					DataTable dataTable21 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql33, strParameterArray22, CommandOperationType.ExecuteReaderReturnDataTable);
					text27 = ((dataTable21.Rows.Count <= 0) ? "" : dataTable21.Rows[0]["VipNo"].ToString());
					string text28 = "";
					string[] strParameterArray23 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql34 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT sum(HQTY) as itemstotal FROM ECRTRHS where(SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) = {0}";
					DataTable dataTable22 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql34, strParameterArray23, CommandOperationType.ExecuteReaderReturnDataTable);
					text28 = ((dataTable22.Rows.Count <= 0) ? "" : Math.Abs(ConvertToInt(dataTable22.Rows[0]["itemstotal"].ToString())).ToString());
					string text29 = "";
					string a = dataTable19.Rows[0]["HHD_VLD"].ToString();
					if (a == null || a.Length != 0)
					{
						if (a == "D")
						{
							text29 = "1";
						}
					}
					else
					{
						text29 = "0";
					}
					string str3 = Convert.ToDateTime(dataTable19.Rows[0]["HDATE"].ToString()).ToString("yyyy-MM-dd");
					string text30 = "0";
					string text31 = "0";
					string[,] strFieldArray9 = new string[15, 2]
					{
						{
							"sellNo",
							hseqNo3
						},
						{
							"oldECRHDHSNo",
							dataTable6.Rows[n]["SNO"].ToString()
						},
						{
							"sellTime",
							str3 + " " + dataTable19.Rows[0]["HTIME"].ToString()
						},
						{
							"memberId",
							text27
						},
						{
							"sum",
							ConvertToInt(dataTable19.Rows[0]["HASTOTAL"].ToString()).ToString()
						},
						{
							"items",
							dataTable19.Rows[0]["HTR_NUM"].ToString()
						},
						{
							"itemstotal",
							text28
						},
						{
							"status",
							text29
						},
						{
							"sumDiscount",
							"0"
						},
						{
							"sumRebate",
							"0"
						},
						{
							"Refund",
							"0"
						},
						{
							"changcount",
							"1"
						},
						{
							"editDate",
							text26
						},
						{
							"Cash",
							text30
						},
						{
							"Credit",
							text31
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_main_sell", "", "", strFieldArray9, null, CommandOperationType.ExecuteNonQuery);
					string text32 = "";
					text32 = ((ConvertToInt(dataTable19.Rows[0]["HASTOTAL"].ToString()) >= 0) ? ConvertToInt(dataTable19.Rows[0]["HASTOTAL"].ToString()).ToString() : "0");
					string[,] strFieldArray10 = new string[3, 2]
					{
						{
							"sellNo",
							hseqNo3
						},
						{
							"changeDate",
							str3 + " " + dataTable19.Rows[0]["HTIME"].ToString()
						},
						{
							"sum",
							text32
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_mainsell_log", "", "", strFieldArray10, null, CommandOperationType.ExecuteNonQuery);
					if (dataTable20.Rows.Count > 0)
					{
						for (int num4 = 0; num4 < dataTable20.Rows.Count; num4++)
						{
							string text33 = "";
							string[] strParameterArray24 = new string[1]
							{
								dataTable20.Rows[num4]["HGDSNO"].ToString()
							};
							string sql35 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT GDSNO FROM hypos_GOODSLST where oldGDSNO = {0}";
							DataTable dataTable23 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql35, strParameterArray24, CommandOperationType.ExecuteReaderReturnDataTable);
							text33 = ((dataTable23.Rows.Count <= 0) ? "" : dataTable23.Rows[0]["GDSNO"].ToString());
							string[,] strFieldArray11 = new string[10, 2]
							{
								{
									"sellNo",
									hseqNo3
								},
								{
									"barcode",
									text33
								},
								{
									"fixedPrice",
									dataTable20.Rows[num4]["HOPRICE"].ToString()
								},
								{
									"sellingPrice",
									dataTable20.Rows[num4]["HSPRICE"].ToString()
								},
								{
									"num",
									dataTable20.Rows[num4]["HQTY"].ToString()
								},
								{
									"subtotal",
									(ConvertToInt(dataTable20.Rows[num4]["HQTY"].ToString()) * ConvertToInt(dataTable20.Rows[num4]["HSPRICE"].ToString())).ToString()
								},
								{
									"discount",
									"0"
								},
								{
									"total",
									ConvertToInt(dataTable20.Rows[num4]["HSLTOT"].ToString()).ToString()
								},
								{
									"PRNO",
									dataTable20.Rows[num4]["CropNo"].ToString()
								},
								{
									"BLNO",
									dataTable20.Rows[num4]["PestNo"].ToString()
								}
							};
							DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_detail_sell", "", "", strFieldArray11, null, CommandOperationType.ExecuteNonQuery);
						}
					}
				}
				string[] strParameterArray25 = new string[1]
				{
					dataTable6.Rows[n]["SNO"].ToString()
				};
				string sql36 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO as SNO FROM old.ECRTRHS where HQTY < 0 and(SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) = {0}";
				if (((DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql36, strParameterArray25, CommandOperationType.ExecuteReaderReturnDataTable)).Rows.Count > 0)
				{
					string hseqNo4 = getHseqNo();
					string text34 = DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd HH:mm:ss");
					string[] strParameterArray26 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql37 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRHDHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					DataTable dataTable24 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql37, strParameterArray26, CommandOperationType.ExecuteReaderReturnDataTable);
					string[] strParameterArray27 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql38 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRPYHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					DataTable dataTable25 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql38, strParameterArray27, CommandOperationType.ExecuteReaderReturnDataTable);
					string[] strParameterArray28 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql39 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRTRHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					DataTable dataTable26 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql39, strParameterArray28, CommandOperationType.ExecuteReaderReturnDataTable);
					string text35 = "";
					string text36 = "";
					if (ConvertToInt(dataTable24.Rows[0]["HASTOTAL"].ToString()) >= 0)
					{
						text35 = ConvertToInt(dataTable24.Rows[0]["HASTOTAL"].ToString()).ToString();
						text36 = "0";
					}
					else
					{
						text35 = "0";
						text36 = Math.Abs(ConvertToInt(dataTable24.Rows[0]["HASTOTAL"].ToString())).ToString();
					}
					string text37 = "";
					string[] strParameterArray29 = new string[1]
					{
						dataTable24.Rows[0]["HCU_NO"].ToString()
					};
					string sql40 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT VipNo FROM hypos_CUST_RTL where oldVIPNO LIKE {0}";
					DataTable dataTable27 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql40, strParameterArray29, CommandOperationType.ExecuteReaderReturnDataTable);
					text37 = ((dataTable27.Rows.Count <= 0) ? "" : dataTable27.Rows[0]["VipNo"].ToString());
					string text38 = "";
					string[] strParameterArray30 = new string[1]
					{
						dataTable6.Rows[n]["SNO"].ToString()
					};
					string sql41 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT sum(HQTY) as itemstotal FROM ECRTRHS where(SHOPNO || '_' || HWSNO || '_' || HDATE || '_' || HSEQNO) = {0}";
					DataTable dataTable28 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql41, strParameterArray30, CommandOperationType.ExecuteReaderReturnDataTable);
					text38 = ((dataTable28.Rows.Count <= 0) ? "" : Math.Abs(ConvertToInt(dataTable28.Rows[0]["itemstotal"].ToString())).ToString());
					DateTime dateTime = Convert.ToDateTime(dataTable24.Rows[0]["HDATE"].ToString());
					string str4 = dateTime.ToString("yyyy-MM-dd");
					string text39 = "0";
					string text40 = "0";
					if (dataTable25.Rows.Count > 0)
					{
						if (ConvertToInt(dataTable25.Rows[0]["HPYAMT"].ToString()) < 0)
						{
							text39 = "0";
							text40 = "0";
						}
						else if ("01".Equals(dataTable25.Rows[0]["HPYNO"].ToString()))
						{
							text39 = dataTable25.Rows[0]["HPYAMT"].ToString();
						}
						else
						{
							text40 = dataTable25.Rows[0]["HPYAMT"].ToString();
						}
					}
					string[,] strFieldArray12 = new string[15, 2]
					{
						{
							"sellNo",
							hseqNo4
						},
						{
							"oldECRHDHSNo",
							dataTable6.Rows[n]["SNO"].ToString()
						},
						{
							"sellTime",
							str4 + " " + dataTable24.Rows[0]["HTIME"].ToString()
						},
						{
							"memberId",
							text37
						},
						{
							"sum",
							text35
						},
						{
							"Refund",
							text36
						},
						{
							"items",
							dataTable24.Rows[0]["HTR_NUM"].ToString()
						},
						{
							"itemstotal",
							text38
						},
						{
							"status",
							"2"
						},
						{
							"sumDiscount",
							"0"
						},
						{
							"sumRebate",
							"0"
						},
						{
							"changcount",
							"2"
						},
						{
							"editDate",
							text34
						},
						{
							"Cash",
							text39
						},
						{
							"Credit",
							text40
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_main_sell", "", "", strFieldArray12, null, CommandOperationType.ExecuteNonQuery);
					string text41 = "";
					text41 = ((ConvertToInt(dataTable24.Rows[0]["HASTOTAL"].ToString()) >= 0) ? ConvertToInt(dataTable24.Rows[0]["HASTOTAL"].ToString()).ToString() : "0");
					string[,] strFieldArray13 = new string[3, 2]
					{
						{
							"sellNo",
							hseqNo4
						},
						{
							"changeDate",
							str4 + " " + dataTable24.Rows[0]["HTIME"].ToString()
						},
						{
							"sum",
							text41
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_mainsell_log", "", "", strFieldArray13, null, CommandOperationType.ExecuteNonQuery);
					string text42 = "";
					text42 = ((ConvertToInt(dataTable24.Rows[0]["HASTOTAL"].ToString()) >= 0) ? ConvertToInt(dataTable24.Rows[0]["HASTOTAL"].ToString()).ToString() : "0");
					Convert.ToDateTime(dataTable24.Rows[0]["HTIME"].ToString());
					string str5 = dateTime.AddSeconds(1.0).ToString("hh:mm:ss");
					string[,] strFieldArray14 = new string[6, 2]
					{
						{
							"sellNo",
							hseqNo4
						},
						{
							"changeDate",
							str4 + " " + str5
						},
						{
							"isprint",
							""
						},
						{
							"iscancel",
							""
						},
						{
							"ischange",
							"1"
						},
						{
							"sum",
							text42
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_mainsell_log", "", "", strFieldArray14, null, CommandOperationType.ExecuteNonQuery);
					if (dataTable26.Rows.Count > 0)
					{
						for (int num5 = 0; num5 < dataTable26.Rows.Count; num5++)
						{
							string text43 = "";
							string[] strParameterArray31 = new string[1]
							{
								dataTable26.Rows[num5]["HGDSNO"].ToString()
							};
							string sql42 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT GDSNO FROM hypos_GOODSLST where oldGDSNO = {0}";
							DataTable dataTable29 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql42, strParameterArray31, CommandOperationType.ExecuteReaderReturnDataTable);
							text43 = ((dataTable29.Rows.Count <= 0) ? "" : dataTable29.Rows[0]["GDSNO"].ToString());
							string text44 = "";
							text44 = ((ConvertToInt(dataTable26.Rows[num5]["HQTY"].ToString()) >= 0) ? dataTable26.Rows[num5]["HQTY"].ToString() : "0");
							string text45 = "";
							text45 = ((ConvertToInt(dataTable26.Rows[num5]["HSLTOT"].ToString()) >= 0) ? ConvertToInt(dataTable26.Rows[num5]["HSLTOT"].ToString()).ToString() : "0");
							string[,] strFieldArray15 = new string[10, 2]
							{
								{
									"sellNo",
									hseqNo4
								},
								{
									"barcode",
									text43
								},
								{
									"fixedPrice",
									dataTable26.Rows[num5]["HOPRICE"].ToString()
								},
								{
									"sellingPrice",
									dataTable26.Rows[num5]["HSPRICE"].ToString()
								},
								{
									"num",
									text44
								},
								{
									"subtotal",
									(ConvertToInt(text44) * ConvertToInt(dataTable26.Rows[num5]["HSPRICE"].ToString())).ToString()
								},
								{
									"discount",
									"0"
								},
								{
									"total",
									text45
								},
								{
									"PRNO",
									dataTable26.Rows[num5]["CropNo"].ToString()
								},
								{
									"BLNO",
									dataTable26.Rows[num5]["PestNo"].ToString()
								}
							};
							DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_detail_sell", "", "", strFieldArray15, null, CommandOperationType.ExecuteNonQuery);
							string sql43 = "select sellLogId from hypos_mainsell_log order by sellLogId desc LIMIT 0,1";
							string text46 = DataBaseUtilities.DBOperation(Program.ConnectionString, sql43, null, CommandOperationType.ExecuteScalar).ToString();
							string[,] strFieldArray16 = new string[11, 2]
							{
								{
									"sellLogId",
									text46
								},
								{
									"fixedPrice",
									dataTable26.Rows[num5]["HOPRICE"].ToString()
								},
								{
									"sellingPrice",
									dataTable26.Rows[num5]["HSPRICE"].ToString()
								},
								{
									"num",
									"0"
								},
								{
									"diffNum",
									Math.Abs(ConvertToInt(dataTable26.Rows[num5]["HQTY"].ToString())).ToString()
								},
								{
									"discount",
									"0"
								},
								{
									"subtotal",
									"0"
								},
								{
									"total",
									"0"
								},
								{
									"PRNO",
									dataTable26.Rows[num5]["CropNo"].ToString()
								},
								{
									"BLNO",
									dataTable26.Rows[num5]["PestNo"].ToString()
								},
								{
									"barcode",
									text43
								}
							};
							DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_detailsell_log", "", "", strFieldArray16, null, CommandOperationType.ExecuteNonQuery);
						}
					}
				}
			}
			backgroundWorker1.ReportProgress(75);
			string sql44 = "SELECT * FROM hypos_main_sell where oldECRHDHSNo is not null order by sellNo";
			DataTable dataTable30 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql44, null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable30.Rows.Count > 0)
			{
				for (int num6 = 0; num6 < dataTable30.Rows.Count; num6++)
				{
					string text47 = dataTable30.Rows[num6]["status"].ToString();
					string text48 = "";
					if (!(text47 == "0"))
					{
						if (text47 == "2")
						{
							text48 = "1";
						}
					}
					else
					{
						text48 = "0";
					}
					string[,] strFieldArray17 = new string[7, 2]
					{
						{
							"memberId",
							dataTable30.Rows[num6]["memberId"].ToString()
						},
						{
							"sellNo",
							dataTable30.Rows[num6]["sellNo"].ToString()
						},
						{
							"editdate",
							dataTable30.Rows[num6]["sellTime"].ToString()
						},
						{
							"sellType",
							text48
						},
						{
							"status",
							text47
						},
						{
							"Cash",
							dataTable30.Rows[num6]["Cash"].ToString()
						},
						{
							"Credit",
							dataTable30.Rows[num6]["Credit"].ToString()
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_user_consumelog", "", "", strFieldArray17, null, CommandOperationType.ExecuteNonQuery);
				}
			}
			backgroundWorker1.ReportProgress(80);
			string sql45 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO as SNO FROM old.ECRTRHS where HGBCNO = 'REPAYMENT' and HGTYPE = 0 order by HDATE, HTIME ";
			DataTable dataTable31 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql45, null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable31.Rows.Count > 0)
			{
				for (int num7 = 0; num7 < dataTable31.Rows.Count; num7++)
				{
					string[] strParameterArray32 = new string[1]
					{
						dataTable31.Rows[num7]["SNO"].ToString()
					};
					string sql46 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRHDHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					DataTable dataTable32 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql46, strParameterArray32, CommandOperationType.ExecuteReaderReturnDataTable);
					string[] strParameterArray33 = new string[1]
					{
						dataTable31.Rows[num7]["SNO"].ToString()
					};
					string sql47 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRPYHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					DataTable dataTable33 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql47, strParameterArray33, CommandOperationType.ExecuteReaderReturnDataTable);
					string[] strParameterArray34 = new string[1]
					{
						dataTable31.Rows[num7]["SNO"].ToString()
					};
					string sql48 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM ECRTRHS where {0} = (SHOPNO||'_'||HWSNO||'_'||HDATE||'_'||HSEQNO)";
					_ = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql48, strParameterArray34, CommandOperationType.ExecuteReaderReturnDataTable);
					string text49 = "";
					string[] strParameterArray35 = new string[1]
					{
						dataTable32.Rows[0]["HCU_NO"].ToString()
					};
					string sql49 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT VipNo FROM hypos_CUST_RTL where oldVIPNO LIKE {0}";
					DataTable dataTable34 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql49, strParameterArray35, CommandOperationType.ExecuteReaderReturnDataTable);
					text49 = ((dataTable34.Rows.Count <= 0) ? "" : dataTable34.Rows[0]["VipNo"].ToString());
					string str6 = Convert.ToDateTime(dataTable32.Rows[0]["HDATE"].ToString()).ToString("yyyy-MM-dd");
					string[,] strFieldArray18 = new string[7, 2]
					{
						{
							"memberId",
							text49
						},
						{
							"sellNo",
							""
						},
						{
							"editdate",
							str6 + " " + dataTable32.Rows[0]["HTIME"].ToString()
						},
						{
							"sellType",
							"2"
						},
						{
							"status",
							"0"
						},
						{
							"Cash",
							"0"
						},
						{
							"Credit",
							Math.Abs(ConvertToInt(dataTable33.Rows[0]["HPYAMT"].ToString())).ToString()
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_user_consumelog", "", "", strFieldArray18, null, CommandOperationType.ExecuteNonQuery);
				}
			}
			backgroundWorker1.ReportProgress(85);
			string sql50 = "SELECT a.memberId, (select sum(b.Cash + b.Credit) from hypos_user_consumelog as b where b.sellType <> 2 and a.memberId = b.memberId) as Total, (select sum(b.Credit) from hypos_user_consumelog as b where b.sellType <> 2 and a.memberId = b.memberId) as TotCredit, (select sum(b.Credit) from hypos_user_consumelog as b where b.sellType = 2 and a.memberId = b.memberId) as repayment, (select editdate from hypos_user_consumelog as b where b.sellType = 2 and a.memberId = b.memberId order by editdate desc) as RepayDate FROM hypos_user_consumelog as a where a.memberId <> '' group by a.memberId";
			DataTable dataTable35 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql50, null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable35.Rows.Count > 0)
			{
				for (int num8 = 0; num8 < dataTable35.Rows.Count; num8++)
				{
					string text50 = dataTable35.Rows[num8]["memberId"].ToString();
					int num9 = ConvertToInt(dataTable35.Rows[num8]["Total"].ToString());
					int num10 = ConvertToInt(dataTable35.Rows[num8]["TotCredit"].ToString());
					int num11 = ConvertToInt(string.IsNullOrEmpty(dataTable35.Rows[num8]["repayment"].ToString()) ? "0" : dataTable35.Rows[num8]["repayment"].ToString());
					string text51 = dataTable35.Rows[num8]["RepayDate"].ToString();
					if ("".Equals(text51))
					{
						string[] strParameterArray36 = new string[3]
						{
							text50,
							num9.ToString(),
							(num10 - num11).ToString()
						};
						string sql51 = "UPDATE hypos_CUST_RTL  SET VipNo = {0}, Total = {1}, Credit = {2} WHERE VipNo = {0}";
						DataBaseUtilities.DBOperation(Program.ConnectionString, sql51, strParameterArray36, CommandOperationType.ExecuteNonQuery);
					}
					else
					{
						string[] strParameterArray37 = new string[4]
						{
							text50,
							num9.ToString(),
							(num10 - num11).ToString(),
							text51
						};
						string sql52 = "UPDATE hypos_CUST_RTL  SET VipNo = {0}, Total = {1}, Credit = {2}, RepayDate = {3} WHERE VipNo = {0}";
						DataBaseUtilities.DBOperation(Program.ConnectionString, sql52, strParameterArray37, CommandOperationType.ExecuteNonQuery);
					}
				}
			}
			backgroundWorker1.ReportProgress(90);
			string sql53 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT * FROM CHGBKTOP order by UPDDATE";
			DataTable dataTable36 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql53, null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable36.Rows.Count > 0)
			{
				for (int num12 = 0; num12 < dataTable36.Rows.Count; num12++)
				{
					string newPurchaseNo = getNewPurchaseNo();
					string text52 = DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd HH:mm:ss");
					string text53 = "";
					string[] strParameterArray38 = new string[1]
					{
						dataTable36.Rows[num12]["VENNO"].ToString()
					};
					string sql54 = "SELECT SupplierNo FROM hypos_Supplier where oldVENDO = {0}";
					DataTable dataTable37 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql54, strParameterArray38, CommandOperationType.ExecuteReaderReturnDataTable);
					text53 = ((dataTable37.Rows.Count <= 0) ? "" : dataTable37.Rows[0]["SupplierNo"].ToString());
					string text54 = Convert.ToDateTime(string.IsNullOrEmpty(dataTable36.Rows[num12]["VENSDATE"].ToString()) ? "1970-01-01" : dataTable36.Rows[num12]["VENSDATE"].ToString()).ToString("yyyy-MM-dd");
					string text55 = Convert.ToDateTime(dataTable36.Rows[num12]["UPDDATE"].ToString()).ToString("yyyy-MM-dd hh:mm:ss");
					string[,] strFieldArray19 = new string[9, 2]
					{
						{
							"PurchaseNo",
							newPurchaseNo
						},
						{
							"oldCBNO",
							dataTable36.Rows[num12]["CBNO"].ToString()
						},
						{
							"PurchaseCustomNo",
							dataTable36.Rows[num12]["VENSNO"].ToString()
						},
						{
							"SupplierNo",
							text53
						},
						{
							"Total",
							ConvertToInt(dataTable36.Rows[num12]["CBTOT"].ToString()).ToString()
						},
						{
							"Status",
							"0"
						},
						{
							"PurchaseDate",
							text54
						},
						{
							"CreateDate",
							text55
						},
						{
							"UpdateDate",
							text52
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_PurchaseGoods_Master", "", "", strFieldArray19, null, CommandOperationType.ExecuteNonQuery);
					string sql55 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT CBNO,ITEMORD,GDSNO,PCKPRC,PCKQTY,PCKSTT FROM CHGBKDET where CBNO = {0} order by ItemOrd";
					string[] strParameterArray39 = new string[1]
					{
						dataTable36.Rows[num12]["CBNO"].ToString()
					};
					DataTable dataTable38 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql55, strParameterArray39, CommandOperationType.ExecuteReaderReturnDataTable);
					if (dataTable38.Rows.Count > 0)
					{
						for (int num13 = 0; num13 < dataTable38.Rows.Count; num13++)
						{
							string text56 = "";
							string[] strParameterArray40 = new string[1]
							{
								dataTable38.Rows[num13]["GDSNO"].ToString()
							};
							string sql56 = "SELECT GDSNO FROM hypos_GOODSLST where oldGDSNO = {0}";
							DataTable dataTable39 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql56, strParameterArray40, CommandOperationType.ExecuteReaderReturnDataTable);
							text56 = ((dataTable39.Rows.Count <= 0) ? "" : dataTable39.Rows[0]["GDSNO"].ToString());
							strFieldArray19 = new string[5, 2]
							{
								{
									"PurchaseNo",
									newPurchaseNo
								},
								{
									"GDSNO",
									text56
								},
								{
									"Cost",
									ConvertToInt(dataTable38.Rows[num13]["PCKPRC"].ToString()).ToString()
								},
								{
									"Quantity",
									dataTable38.Rows[num13]["PCKQTY"].ToString()
								},
								{
									"GoodsTotalCountLog",
									""
								}
							};
							DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_PurchaseGoods_Detail", "", "", strFieldArray19, null, CommandOperationType.ExecuteNonQuery);
						}
					}
				}
			}
			string sql57 = "ATTACH DATABASE 'C:\\Hypos\\Old_db.db3' AS [old] KEY '1031'; SELECT GDSNO,NOWQTY FROM GDSQTY where GDSNO in (SELECT GDSNO FROM CHGBKDET group by GDSNO order by CBNO desc)";
			DataTable dataTable40 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql57, null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable40.Rows.Count > 0)
			{
				for (int num14 = 0; num14 < dataTable40.Rows.Count; num14++)
				{
					string[] strParameterArray41 = new string[2]
					{
						dataTable40.Rows[num14]["GDSNO"].ToString(),
						dataTable40.Rows[num14]["NOWQTY"].ToString()
					};
					string sql58 = "UPDATE hypos_PurchaseGoods_Detail SET GoodsTotalCountLog = {1} WHERE GDSNO in (SELECT GDSNO FROM hypos_GOODSLST where oldGDSNO like {0} or GDSNO like {0})";
					DataBaseUtilities.DBOperation(Program.ConnectionString, sql58, strParameterArray41, CommandOperationType.ExecuteNonQuery);
					DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_GOODSLST set inventory = {1} WHERE GDSNO like {0} or oldGDSNO like {0}", strParameterArray41, CommandOperationType.ExecuteNonQuery);
				}
			}
			backgroundWorker1.ReportProgress(100);
			AutoClosingMessageBox.Show("資料移轉結束");
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Main發生例外狀況:「{ex.ToString()}」");
		}
	}

	public static int ConvertToInt(string value)
	{
		try
		{
			return (int)float.Parse(value);
		}
		catch (Exception)
		{
			return 0;
		}
	}

	public static string getNewGDSNO()
	{
		string sql = "SELECT GDSNO FROM hypos_GOODSLST where GDSNO like 'G" + Program.SiteNo.ToString() + "%' order by GDSNO desc limit 1";
		string text = Convert.ToString(DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteScalar));
		string text2 = DateTime.Now.AddDays(-1.0).Year.ToString().Substring(2, 2);
		if ("-1".Equals(text))
		{
			return $"G{Program.SiteNo}{text2}00000001";
		}
		string value = text.Substring(3, 2);
		if (!text2.Equals(value))
		{
			return $"G{Program.SiteNo}{text2}00000001";
		}
		string arg = $"{ConvertToInt(text.Substring(5, 8)) + 1:00000000}";
		return $"G{Program.SiteNo}{text2}{arg}";
	}

	public static string getNewVipNo()
	{
		string sql = "SELECT VipNo FROM hypos_CUST_RTL where VipNo like 'M" + Program.SiteNo.ToString() + "%' order by VipNo desc limit 1";
		string text = Convert.ToString(DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteScalar));
		string text2 = DateTime.Now.AddDays(-1.0).Year.ToString().Substring(2, 2);
		if ("-1".Equals(text))
		{
			return $"M{Program.SiteNo.PadLeft(2, '0')}{text2}00001";
		}
		string value = text.Substring(3, 2);
		if (!text2.Equals(value))
		{
			return $"M{Program.SiteNo}{text2}00001";
		}
		string arg = $"{ConvertToInt(text.Substring(5, 5)) + 1:00000}";
		return $"M{Program.SiteNo}{text2}{arg}";
	}

	public static string getSupplierNo()
	{
		string sql = "SELECT SupplierNo FROM hypos_Supplier where SupplierNo like 'S" + Program.SiteNo.ToString() + "%' order by SupplierNo desc limit 1";
		string text = Convert.ToString(DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteScalar));
		string text2 = DateTime.Now.AddDays(-1.0).Year.ToString().Substring(2, 2);
		if ("-1".Equals(text))
		{
			return $"S{Program.SiteNo}{text2}00001";
		}
		string value = text.Substring(3, 2);
		if (!text2.Equals(value))
		{
			return $"S{Program.SiteNo}{text2}00001";
		}
		string arg = $"{ConvertToInt(text.Substring(5, 5)) + 1:00000}";
		return $"S{Program.SiteNo}{text2}{arg}";
	}

	public static string getHseqNo()
	{
		string text = "";
		DateTime dateTime = DateTime.Now.AddDays(-1.0);
		string text2 = dateTime.ToString("yyyyMMdd");
		string str = dateTime.ToString("yyyy-MM-dd");
		string[] strWhereParameterArray = new string[1]
		{
			text2
		};
		string strWhereClause = "editDate like '%" + str + "%'";
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "*", "hypos_main_sell", strWhereClause, "", null, strWhereParameterArray, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count > 0)
		{
			string text3 = (dataTable.Rows.Count + 1).ToString();
			if (text3.Length == 1)
			{
				return Program.LincenseCode + Program.SiteNo + text2 + "000" + text3;
			}
			if (text3.Length == 2)
			{
				return Program.LincenseCode + Program.SiteNo + text2 + "00" + text3;
			}
			if (text3.Length == 3)
			{
				return Program.LincenseCode + Program.SiteNo + text2 + "0" + text3;
			}
			return Program.LincenseCode + Program.SiteNo + text2 + text3;
		}
		return Program.LincenseCode + Program.SiteNo + text2 + "0001";
	}

	public static string getNewPurchaseNo()
	{
		string sql = "SELECT PurchaseNo FROM hypos_PurchaseGoods_Master where PurchaseNo like '" + Program.SiteNo.ToString() + "%' order by PurchaseNo desc limit 1";
		string text = Convert.ToString(DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteScalar));
		DateTime dateTime = DateTime.Now.AddDays(-1.0);
		string text2 = dateTime.Year.ToString().Substring(2, 2) + $"{dateTime.Month:00}";
		if ("-1".Equals(text))
		{
			return $"{Program.SiteNo}{text2}0001";
		}
		string value = text.Substring(2, 4);
		if (!text2.Equals(value))
		{
			return $"{Program.SiteNo}{text2}0001";
		}
		string arg = $"{ConvertToInt(text.Substring(6, 4)) + 1:0000}";
		return $"{Program.SiteNo}{text2}{arg}";
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
		progressBar1 = new System.Windows.Forms.ProgressBar();
		label1 = new System.Windows.Forms.Label();
		backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		label2 = new System.Windows.Forms.Label();
		SuspendLayout();
		progressBar1.Location = new System.Drawing.Point(35, 79);
		progressBar1.Name = "progressBar1";
		progressBar1.Size = new System.Drawing.Size(424, 40);
		progressBar1.TabIndex = 0;
		label1.AutoSize = true;
		label1.Location = new System.Drawing.Point(33, 53);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(33, 12);
		label1.TabIndex = 1;
		label1.Text = "label1";
		backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
		backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
		backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
		label2.AutoSize = true;
		label2.Location = new System.Drawing.Point(33, 22);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(33, 12);
		label2.TabIndex = 2;
		label2.Text = "label2";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(497, 144);
		base.ControlBox = false;
		base.Controls.Add(label2);
		base.Controls.Add(label1);
		base.Controls.Add(progressBar1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Name = "dbDataTransfer";
		Text = "舊POS資料移轉程序";
		ResumeLayout(false);
		PerformLayout();
	}
}
