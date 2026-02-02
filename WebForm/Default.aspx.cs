using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using DataBaseKernel;
using PublicTool;


public class Root
{
    public string A { get; set; }
    public Links _link { get; set; }
}

public class Links
{
    public string Method { get; set; }
    public string Href { get; set; }
}


public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string str = "isSuccess=N&sendType=FuBonServer&replyMsg=[失敗原因]無法連接至遠端伺服器&STATUS=M";
        string[] TpAry = str.Split('&');
        //===
        const string isSuccess1 = "isSuccess=Y";
        const string isSuccess2 = "isSuccess=N";
        const string isSuccess3 = "isSuccess=";
        //===
        int pos1 = Array.IndexOf(TpAry, isSuccess1);
        int pos2 = Array.IndexOf(TpAry, isSuccess2);
        int pos3 = Array.IndexOf(TpAry, isSuccess3);
        TextBox1.Text = pos1.ToString() + "," + pos2.ToString() + "," + pos3.ToString();
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        string TpSQL = "select * from ORDERS where order_no in (?,?)";
        //===
        OleDbParameter[] parameters = {
            new OleDbParameter("ZAorder_no","SPO0000000323"),
            new OleDbParameter("ZAorder_no","SPO0004493209")
        };
        //parameters[0].Value = "SPO0000000323";
        //parameters[1].Value = "SPO0004493209";
        //===
        DataSet TpDS = CStarTravel_DBAccess.GetDataSet(TpSQL, parameters);
        GridView1.DataSource = TpDS;
        GridView1.DataBind();
        //===
        TextBox2.Text = TpDS.Tables[0].Columns[0].ColumnName;
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        TextBox2.Text = string.Empty;
        //===
        List<int> integers = new List<int>();
        integers.Add(1);
        integers.Add(2);
        Converter<int, double> converter = CTool.TakeSquareRoot;
        List<double> d_list = integers.ConvertAll<double>(converter);
        foreach (double d in d_list)
        {
            TextBox2.Text += d + "\r\n";
        }
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        CExcelBase Tp_ExcelObj = new CExcelBase();
        CExcelBE be = new CExcelBE(1, 1, "100000000", "A1", "B1", "GRAY", true, 12, "", null);
        Tp_ExcelObj.InsertData(be);
    }

    protected void Btn_SednMail_Click(object sender, EventArgs e)
    {
        string[] p_Ary_Attachment_FileList = new string[] { "D:\\SVN\\Fubon\\StarEDI\\insurance\\bin\\Debug\\ExportExcel\\2019\\08\\08\\2019-08-08-1442-fubonserver.xlsx" };
        CTool.SendEmaill("john4944@startravel.com.tw", "富邦旅責險自動投保申報排程", "投保資料如附檔", string.Empty, p_Ary_Attachment_FileList);
    }

    protected void Button5_Click(object sender, EventArgs e)
    {
        //string str = "";
        //CTest obj = new CTest();
        //obj.za = "ZAOOP";
        //str = $"{obj.za}/1-1-add.asp";
        //TextBox1.Text = str;
        //===
        //CTest obj = new CTest("AAA") { B = "bbb" };
        //TextBox1.Text = obj.A + "," + obj.B;
        //===        
        TextBox2.Text = string.Empty;
        string ZA = @"TEAMNO,ROWNO,TRANNUM,POLYNO,PERSONNAME,PERSONID,PERSONBIRTHDATE,GRP_NO,PROD_NO,ORDER_NO,LST_SEQNO,CUS_SEQNO,LST_STATUS,PROD_S_DATE,PROD_E_DATE,FLAG,CUS_NAME_C_FIRST,CUS_NAME_C_LAST,MSG,CREATE_TIME,MODER,UPDATE_TIME";
        string[] Tp_Ary = ZA.Split(',');
        for (int i = 0; i < Tp_Ary.Length; i++)
        {
            TextBox2.Text += " public string " + Tp_Ary[i] + " { get; set; }" + "\r\n\r\n";
        }
    }

    protected void Button6_Click(object sender, EventArgs e)
    {
        TextBox2.Text = "";
        //===
        StreamReader file = new StreamReader(@"D:\Sample_Code\WebForm\WebForm\Data\#1_國泰旅責險_ErrorMessage_明文表.txt", Encoding.GetEncoding("big5"));
        string line = string.Empty;
        //===
        //X1
        //未輸入任何出團資訊，請檢查!!
        //
        //[EnumRCode("X83"), EnumDesc("此資料並不存在無法異動，請至保險證新增作業進行資料新增!!")]
        //rCodeX83,
        //===
        int Tp_Line = 0;
        string StrA = string.Empty;
        string StrB = string.Empty;
        while ((line = file.ReadLine()) != null)
        {
            Tp_Line++;
            //===
            if (Tp_Line % 2 == 1)
            {
                StrA = "[EnumRCode(\"" + line + "\"), EnumDesc(\"{0}\")]";
                StrB = "rCode" + line + ",";
            }
            else
            {
                StrA = string.Format(StrA, line);
                TextBox2.Text += StrA + "\r\n" + StrB + "\r\n" + "\r\n";
                StrA = string.Empty;
                StrB = string.Empty;
            }
        }
        file.Close();
    }

    protected void Button7_Click(object sender, EventArgs e)
    {
        int Tp_Ret = 0;
        string Tp_ExMsg = string.Empty;
        //try
        //{
            Tp_Ret = CTool.Test_Split_String_To_Array("A,B");
        //}
        //catch (Exception ex)
        //{
        //    Tp_ExMsg = ex.Message;            
        //}
        TextBox2.Text = Tp_Ret.ToString() + "\r\n" + Tp_ExMsg;
        //20251218 @branch:master
        //===
        //20260202:Branch->HR:Ver1
    }
}