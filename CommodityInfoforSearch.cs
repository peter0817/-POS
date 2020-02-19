using POS_Client;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class CommodityInfoforSearch : UserControl, INotifyPropertyChanged
{
	private frmMainShopSimple fms;

	public string barcode = "";

	private IContainer components;

	private Label l_VIPNO;

	private Label l_IDNO;

	private Label commodityName;

	private Label commodityClass;

	private Label commodityPrice;

	private Label lb_SubsideMoney;

	public event PropertyChangedEventHandler PropertyChanged;

	public event EventHandler OnClickMember;

	public CommodityInfoforSearch()
	{
		InitializeComponent();
	}

	public void setfms(frmMainShopSimple fms)
	{
		this.fms = fms;
	}

	public void setMemberIdNo(string IdNo)
	{
		l_IDNO.Text = IdNo;
	}

	public void setMemberVipNo(string VipNo)
	{
		l_VIPNO.Text = VipNo;
	}

	public void setCommodityName(string commodityName)
	{
		this.commodityName.Text = commodityName;
	}

	public void setCommodityClass(string commodityClass)
	{
		this.commodityClass.Text = commodityClass;
	}

	public void setcommodityPrice(string commodityPrice)
	{
		this.commodityPrice.Text = commodityPrice;
	}

	public void setSubsideMoney(string SubsideMoney)
	{
		lb_SubsideMoney.Text = SubsideMoney;
	}

	public string getcommodityName()
	{
		return commodityName.Text.ToString();
	}

	public string getcommodityPrice()
	{
		return commodityPrice.Text.ToString();
	}

	private void UC_Member_MouseEnter(object sender, EventArgs e)
	{
		BackColor = Color.FromArgb(255, 255, 204);
	}

	private void UC_Member_MouseLeave(object sender, EventArgs e)
	{
		BackColor = Control.DefaultBackColor;
	}

	private void UC_Member_Click(object sender, EventArgs e)
	{
		this.OnClickMember(this, null);
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
		l_VIPNO = new System.Windows.Forms.Label();
		l_IDNO = new System.Windows.Forms.Label();
		commodityName = new System.Windows.Forms.Label();
		commodityClass = new System.Windows.Forms.Label();
		commodityPrice = new System.Windows.Forms.Label();
		lb_SubsideMoney = new System.Windows.Forms.Label();
		SuspendLayout();
		l_VIPNO.AutoSize = true;
		l_VIPNO.Font = new System.Drawing.Font("微軟正黑體", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		l_VIPNO.ForeColor = System.Drawing.Color.FromArgb(0, 126, 25);
		l_VIPNO.Location = new System.Drawing.Point(10, 9);
		l_VIPNO.Name = "l_VIPNO";
		l_VIPNO.Size = new System.Drawing.Size(47, 16);
		l_VIPNO.TabIndex = 0;
		l_VIPNO.Text = "店內碼:";
		l_VIPNO.Click += new System.EventHandler(UC_Member_Click);
		l_VIPNO.MouseEnter += new System.EventHandler(UC_Member_MouseEnter);
		l_IDNO.AutoSize = true;
		l_IDNO.Font = new System.Drawing.Font("微軟正黑體", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		l_IDNO.ForeColor = System.Drawing.Color.FromArgb(0, 126, 25);
		l_IDNO.Location = new System.Drawing.Point(169, 9);
		l_IDNO.Name = "l_IDNO";
		l_IDNO.Size = new System.Drawing.Size(59, 16);
		l_IDNO.TabIndex = 1;
		l_IDNO.Text = "商品條碼:";
		l_IDNO.Click += new System.EventHandler(UC_Member_Click);
		l_IDNO.MouseEnter += new System.EventHandler(UC_Member_MouseEnter);
		commodityName.AutoSize = true;
		commodityName.Font = new System.Drawing.Font("微軟正黑體", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		commodityName.Location = new System.Drawing.Point(2, 38);
		commodityName.Name = "commodityName";
		commodityName.Size = new System.Drawing.Size(69, 25);
		commodityName.TabIndex = 2;
		commodityName.Text = "label1";
		commodityName.Click += new System.EventHandler(UC_Member_Click);
		commodityName.MouseEnter += new System.EventHandler(UC_Member_MouseEnter);
		commodityClass.AutoSize = true;
		commodityClass.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		commodityClass.ForeColor = System.Drawing.Color.FromArgb(195, 186, 157);
		commodityClass.Location = new System.Drawing.Point(3, 73);
		commodityClass.Name = "commodityClass";
		commodityClass.Size = new System.Drawing.Size(54, 20);
		commodityClass.TabIndex = 3;
		commodityClass.Text = "label2";
		commodityClass.Click += new System.EventHandler(UC_Member_Click);
		commodityClass.MouseEnter += new System.EventHandler(UC_Member_MouseEnter);
		commodityPrice.AutoSize = true;
		commodityPrice.Font = new System.Drawing.Font("微軟正黑體", 12f);
		commodityPrice.ForeColor = System.Drawing.Color.Red;
		commodityPrice.Location = new System.Drawing.Point(253, 73);
		commodityPrice.Name = "commodityPrice";
		commodityPrice.Size = new System.Drawing.Size(0, 20);
		commodityPrice.TabIndex = 4;
		lb_SubsideMoney.AutoSize = true;
		lb_SubsideMoney.Font = new System.Drawing.Font("微軟正黑體", 10f);
		lb_SubsideMoney.Location = new System.Drawing.Point(150, 75);
		lb_SubsideMoney.Name = "lb_SubsideMoney";
		lb_SubsideMoney.Size = new System.Drawing.Size(0, 18);
		lb_SubsideMoney.TabIndex = 5;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(lb_SubsideMoney);
		base.Controls.Add(commodityPrice);
		base.Controls.Add(commodityClass);
		base.Controls.Add(commodityName);
		base.Controls.Add(l_IDNO);
		base.Controls.Add(l_VIPNO);
		Cursor = System.Windows.Forms.Cursors.Hand;
		MaximumSize = new System.Drawing.Size(398, 102);
		MinimumSize = new System.Drawing.Size(398, 102);
		base.Name = "CommodityInfoforSearch";
		base.Size = new System.Drawing.Size(396, 100);
		base.Click += new System.EventHandler(UC_Member_Click);
		base.MouseEnter += new System.EventHandler(UC_Member_MouseEnter);
		base.MouseLeave += new System.EventHandler(UC_Member_MouseLeave);
		ResumeLayout(false);
		PerformLayout();
	}
}
