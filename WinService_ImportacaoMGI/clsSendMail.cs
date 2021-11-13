using System;
using System.Web.Mail;

namespace WinService_ImportacaoMGI2
{
    /// <summary>
    /// Summary description for clsSendMail.
    /// </summary>
    public class clsSendMail
    {
        public static void SendMail(string strBody)
        {
            string strTo = ImportacaoMGI.CLS.clsParametro.getParametro("MailMessageTo");
            clsSendMail.SendMail(strBody, strTo);
        }
        public static void SendMail(string strBody, string strTo)
        {
            //======================Obtem valores do Config
            String emailRemetente = System.Configuration.ConfigurationManager.AppSettings["MailMessageFrom"];
            String server = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"];
            String User = System.Configuration.ConfigurationManager.AppSettings["SmtpUser"];
            String Password = System.Configuration.ConfigurationManager.AppSettings["SmtpPwd"];
            String porta = System.Configuration.ConfigurationManager.AppSettings["SmtpPorta"];
            String emailComCopia = System.Configuration.ConfigurationManager.AppSettings["MailMessageCc"];
            String emailComCopiaOculta = System.Configuration.ConfigurationManager.AppSettings["MailMessageCco"];
            String assuntoMensagem = System.Configuration.ConfigurationManager.AppSettings["MailSubject"].ToString() + " - " + DateTime.Now.ToString("dd/MM/yy HH:mm");
            if (porta == "")
            {
                porta = "25";
            }


            //=========================Cria objeto com dados do e-mail.
            System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage();
            //========================Config Email
            objEmail.From = new System.Net.Mail.MailAddress(emailRemetente);

            string[] ListaDestinarios = strTo.Replace(";", ",").Split(',');
            foreach (string dest in ListaDestinarios)
            {
                objEmail.To.Add(dest);
            };

            if (!String.IsNullOrEmpty(emailComCopia))
            {
                string[] ListaCopias = emailComCopia.Replace(";", ",").Split(',');
                foreach (string strCopia in ListaCopias)
                {
                    objEmail.CC.Add(strCopia);
                };
            }

            if (!String.IsNullOrEmpty(emailComCopiaOculta))
            {
                string[] ListaCopiasOcultas = emailComCopiaOculta.Replace(";", ",").Split(',');
                foreach (string strCOpiaOculta in ListaCopiasOcultas)
                {
                    objEmail.CC.Add(strCOpiaOculta);
                };
            }


            objEmail.Priority = System.Net.Mail.MailPriority.High;
            objEmail.IsBodyHtml = true;
            objEmail.Subject = assuntoMensagem;
            objEmail.Body = strBody;
            objEmail.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            objEmail.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

            //================================Anexo
            // Caso queira enviar um arquivo anexo
            //Caminho do arquivo a ser enviado como anexo
            //string arquivo = Server.MapPath("arquivo.jpg");
            // Ou especifique o caminho manualmente
            //string arquivo = @"e:\home\LoginFTP\Web\arquivo.jpg";
            // Cria o anexo para o e-mail
            //if (!String.IsNullOrEmpty(Anexo))
            //{
            //    Attachment Atach = new Attachment(Anexo, System.Net.Mime.MediaTypeNames.Application.Octet);
            //    objEmail.Attachments.Add(Atach);
            //}


            //===================================Cliente do email
            System.Net.Mail.SmtpClient objSmtp = new System.Net.Mail.SmtpClient();
            objSmtp.Host = server;
            objSmtp.Credentials = new System.Net.NetworkCredential(User, Password);
            objSmtp.Port = int.Parse(porta);

            try
            {
                objSmtp.Send(objEmail);
            }
            catch (Exception Rx)
            {
                throw new Exception(Rx.Message);
            }
            finally
            {
                objEmail.Dispose();
            }
            //public static void SendMail(string strBody, string strTo)
            //{
            //	try
            //	{
            //		if ( strTo.Length > 0 )
            //		{
            //			//Building a message to send
            //			MailMessage m = new MailMessage();

            //			m.From = ImportacaoMGI.CLS.clsParametro.getParametro("MailMessageFrom");
            //			m.To = strTo;
            //			m.Cc = ImportacaoMGI.CLS.clsParametro.getParametro("MailMessageCc");
            //			m.Bcc = ImportacaoMGI.CLS.clsParametro.getParametro("MailMessageCco");
            //			m.Subject = ImportacaoMGI.CLS.clsParametro.getParametro("MailSubject");
            //			m.Subject += " - " + DateTime.Now.ToString("dd/MM/yy HH:mm");
            //			m.Body = ImportacaoMGI.CLS.clsParametro.getParametro("MailBody");
            //			m.Body += strBody;
            //			m.BodyFormat = MailFormat.Html; 

            //			//Send a message with a Smtp object
            //			SmtpMail.SmtpServer = ImportacaoMGI.CLS.clsParametro.getParametro("SmtpServer");
            //			SmtpMail.Send(m);

            //			clsLOG.psuLog("Enviando EMail para: " + strTo.ToString(),false);
            //		}
            //	}
            //	catch (Exception ex)
            //	{
            //		clsLOG.psuLog("** Exception ** clsSendMail.SendMail() >> " + ex.Message.ToString(),false);
            //	}
            //}
        }
    }
}
