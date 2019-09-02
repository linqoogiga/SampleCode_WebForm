using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
//===
using System.Net.Mail;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
//===
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DataBaseKernel
{
    public class CTool
    {
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return true; }
        public static string Get_WebAPI_Data(string TargetAPIServerURL, string pRequestMethod)
        {
            string strResult = "";
            //===
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);//驗證服務器證書回調自動驗證 
            //解決[要求已經中止: 無法建立 SSL/TLS 的安全通道。]的Error(FlightStats_API會發生此現像)。
            //===
            Encoding myEncoding = Encoding.GetEncoding("utf-8");
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(TargetAPIServerURL);
            req.Method = pRequestMethod;
            req.KeepAlive = true; //是否保持連線
            req.ContentType = "text/json";
            //req.ServerCertificateValidationCallback = delegate { return true; };
            //===
            //req.Accept = "application/json";
            //req.Headers.Set("accept-encoding", "gzip");
            //req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) advanced-rest-client/12.1.4 Chrome/61.0.3163.100 Electron/2.0.2 Safari/537.36";
            req.Headers.Set("Customer-Ip", "61.31.232.240");
            //===
            //string Authorization = CTool.Get_AuthHeaderValue();
            //req.Headers.Set("Authorization", Authorization);

            //===
            using (WebResponse wr = req.GetResponse())
            {
                using (StreamReader myStreamReader = new StreamReader(wr.GetResponseStream(), myEncoding))
                {
                    //獲得回傳資料
                    strResult = myStreamReader.ReadToEnd();
                    myStreamReader.Close();
                }
            }
            return strResult;
        }

        public static string Get_UnixTimeStamp()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp.ToString();
        }

        public static string Get_SHA512_Encrypt(string p_InputText)
        {
            SHA512 sha512 = new SHA512CryptoServiceProvider();//建立一個SHA512
            byte[] source = Encoding.UTF8.GetBytes(p_InputText);//將字串轉為Byte[]
            byte[] crypto = sha512.ComputeHash(source);//進行SHA512加密
                                                       //var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                                                       //foreach (var b in crypto)
                                                       //    hashedInputStringBuilder.Append(b.ToString("X2"));                       
                                                       //string Encrypt_Result = hashedInputStringBuilder.ToString();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < crypto.Length; i++)
            {
                sb.Append(crypto[i].ToString("x2"));
            }
            string Encrypt_Result = sb.ToString();
            return Encrypt_Result;
        }

        public static string Get_AuthHeaderValue()
        {
            string apiKey = "dibb0r7f7i3tg3v7jt3uee64b";
            string secret = "dhhc9bcll6jdk";
            string Tp_UnixTimeStamp = "1559814414";
            string Tp_Mac = apiKey + secret + Tp_UnixTimeStamp;
            //EAN APIKey=dibb0r7f7i3tg3v7jt3uee64b,Signature=2c7dbf484dcc2bb5f3dfe84969f8af011db137b939089b84ec9534bbc052e7151d5a22ca2b39356fe8f4c9f84842f91dfae0dfd3fb7dc94d5c25115e21b0473b,timestamp=1559792721
            string Tp_Encrypt_Mac = CTool.Get_SHA512_Encrypt(Tp_Mac);
            //===
            string authHeaderValue = "EAN APIKey=" + apiKey + ",Signature=" + Tp_Encrypt_Mac + ",timestamp=" + Tp_UnixTimeStamp;
            //===
            return authHeaderValue;
        }

        public static string Get_HelloWorld(string str) => CTool.Get_Do_HelloWorld() + str;

        public static string Get_Do_HelloWorld()
        {
            return "Hello World ";
        }

        public static double TakeSquareRoot(int x)
        {
            return Math.Sqrt(x);
        }

        public static void SendEmaill(string p_To, string p_Subject, string p_Body, string p_CC = "", string[] filePaths = null)
        {
            try
            {
                SmtpClient smtpServer = new SmtpClient();
                smtpServer.Host = "10.5.1.11";
                //smtpServer.Port = 587;
                //smtpServer.EnableSsl = true;
                smtpServer.Credentials = new NetworkCredential("starit", "StAr3100");
                //By_JohnLin_20180315[四]_Jonathan將密碼由foru247tek 改成 forutek247。
                //SMTP 伺服器需要安全連接，或用戶端未經驗證。 伺服器回應為: 5.5.1 Authentication Required. Learn more at
                //通常就是[密碼錯誤]。
                //-------------------------------------------
                MailMessage myMail = new MailMessage();
                myMail.From = new MailAddress("starit@startravel.com.tw");
                myMail.To.Add(new MailAddress(p_To));
                //---
                if (p_CC != string.Empty)
                {
                    string[] Tp_Ary = p_CC.Split(';');
                    //---
                    for (int i = 0; i < Tp_Ary.Length; i++)
                    {
                        myMail.CC.Add(new MailAddress(Tp_Ary[i]));
                    }
                    //---
                }
                //---
                Attachment file = null;//宣告在這裡，待會要釋放物件用
                if (filePaths != null)//防呆
                {//有夾帶檔案
                    for (int i = 0; i < filePaths.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(filePaths[i].Trim()))
                        {
                            file = new Attachment(filePaths[i].Trim());
                            //加入信件的夾帶檔案
                            myMail.Attachments.Add(file);
                        }
                    }
                }
                //---
                myMail.Priority = MailPriority.Normal;
                myMail.Subject = p_Subject;
                myMail.Body = p_Body;
                myMail.IsBodyHtml = true;
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                smtpServer.Send(myMail);
                //===
                smtpServer.Dispose();

            }
            catch (SmtpException se)
            {
                //
            }
            catch (Exception ex)
            {
                //
            }
        }

        public static int Test_Split_String_To_Array(string p_Str)
        {
            int ret = 0;
            string[] Tp_Ary = null;
            try
            {
                Tp_Ary = p_Str.Split(',');
                Tp_Ary[2] = "za";
                ret = 1;
            }
            catch (Exception ex)
            {
                string Tp_ExMsg = ex.Message;
                ret = -1;
                //throw ex;
            }
            return ret;
        }
    }
}