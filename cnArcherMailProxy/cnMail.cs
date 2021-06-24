using System;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.Web.SessionState;

namespace cnArcherMailProxy
{
    public class cnMail : IHttpHandler, IRequiresSessionState
    {
        private static string _cnMailServer = ConfigurationManager.AppSettings["cnMailServer"];
        private static string _cnUsername = ConfigurationManager.AppSettings["cnUsername"];
        private static string _cnPassword = ConfigurationManager.AppSettings["cnPassword"];
        private static string _cnFromAddress = ConfigurationManager.AppSettings["cnFromAddress"];
        private static string _cnMailIsSSL = ConfigurationManager.AppSettings["cnMailIsSSL"];
        private static string _cnMailPort = ConfigurationManager.AppSettings["cnMailPort"];

        public cnMail()
        {
            if (String.IsNullOrEmpty(_cnMailServer) || String.IsNullOrEmpty(_cnUsername) || String.IsNullOrEmpty(_cnPassword) || String.IsNullOrEmpty(_cnFromAddress))
                throw new Exception("You must provide cnMailServer, cnMailPort, cnMailIsSSL, cnUsername, cnPassword, cnFromAddress as part of webconfig.");
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context is null  || context.Session is null)
            {
                throw new ArgumentNullException(nameof(context), "The context or session was empty!");
            }

            if (context.Session["UserID"] is null)
            {
                throw new UnauthorizedAccessException("You must be logged into EngageIP to use the cnMail Endpoint.");
            }

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            StreamReader stream = new StreamReader(Request.InputStream);
            MailRequest jsonRequest = JsonConvert.DeserializeObject<MailRequest>(stream.ReadToEnd());
            stream.Dispose();

            var result = composeMailAndSend(jsonRequest);
            if (result)
            {
                Response.ContentType = "text/json";
                Response.Write(jsonRequest.ToCnArcherJSON());
            } else
            {
                throw new SmtpException("Failed to send message!");
            }
        }

        public static string phonesWithLinks(string phones)
        {
            string[] phoneArray = phones?.Split(',');
            for (int i = 0; i < phoneArray.Length; i++)
            {
                phoneArray[i] = $"<a href='tel:{phoneArray[i]}'>{phoneArray[i]}</a>";
            }
            return String.Join(", ", phoneArray);
        }

        public static bool composeMailAndSend(MailRequest mailInfo)
        {
            if (mailInfo is null)
            {
                throw new ArgumentNullException("Mail Request is null");
            }

            var MailText = $"<p><span style='font-size:24px;'>cn<strong>Archer</span></p>" +
                $"<p><b>EIP/AccountID:</b> {mailInfo.EIP} / {mailInfo.Account}<br/>" +
                $"<b>Customer:</b> {mailInfo.Name}<br/>" +
                $"<b>Company:</b> {mailInfo.Company}<br/>" +
                $"<b>Phone:</b> {phonesWithLinks(mailInfo.Phones)}<br/>" +
                $"<b>Installation Date:</b> {mailInfo.Date}<br/>" +
                $"<b>Address:</b> <br/><div style='margin-left: 20px; margin-top:0;padding-top:0;'>{mailInfo.Address.Replace("\n", "<br/>")}</div>" +
                $"<p><b>ESN:</b> {mailInfo.ESN}</p><p>" + 
                $"<b>Package:</b> {mailInfo.Package}<br/>" +
                $"<b>Type:</b> {(mailInfo.VLAN == "20" ? "PPPoE" : "StaticIP")}" +
                (mailInfo.VLAN == "20" ? $"<br/><b>Username:</b> {mailInfo.Username}<br/><b>Password</b>: {mailInfo.Password}" :"");

            MailText += $"</p><p><b>Notes:</b> {mailInfo.Notes}</p>";

            MailMessage _m = new MailMessage();
            _m.IsBodyHtml = true;
            _m.From = new MailAddress(_cnFromAddress, "cnArcher Workorders");
            _m.To.Add(new MailAddress(mailInfo.TechEmail, mailInfo.Tech));
            _m.Subject = $"New Workorder [{mailInfo.Date}]: {mailInfo.Name} ({mailInfo.EIP})";
            _m.Body = MailText;

            var textBytes = System.Text.Encoding.UTF8.GetBytes(mailInfo.ToCnArcherJSON());
            MemoryStream ms = new MemoryStream(textBytes, 0, textBytes.Length);
            _m.Attachments.Add(new Attachment(ms, $"{mailInfo.ESN}.cnarcher"));

            SmtpClient _smtp = new SmtpClient();
            _smtp.UseDefaultCredentials = false;
            _smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtp.Host = _cnMailServer;
            _smtp.Port = Convert.ToInt16(_cnMailPort);
            _smtp.EnableSsl = Convert.ToBoolean(_cnMailIsSSL);
            _smtp.Credentials = new NetworkCredential(_cnUsername, _cnPassword);
            _smtp.Timeout = 10000;

            try
            {
                _smtp.Send(_m);
            }
            catch (SmtpException e)
            {
                throw;
            }
            finally
            {
                _m.Dispose();
                _smtp.Dispose();
            }

            return true;
        }
    }
}
