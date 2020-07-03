using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Helpers
{
    public class SentMail
    {


        public static Boolean MailSent(string server, int port, bool security, string userauten, string pwdauten, string mailsend, string mailto, string subject, string body, string mailCC)
        {

            try
            {

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(mailsend));
                message.Subject = subject;
                // message.To.Add(new MailboxAddress(mailto));
                // message.To.Add(new MailboxAddress(mailto2));
                string[] Multi = mailto.Split(';', ',', ':', ' ');
                foreach (var toMulti in Multi)
                {
                    //mailto = tomail + ";";
                    message.To.Add(new MailboxAddress(toMulti));
                }
                if (mailCC == "")
                {

                }
                else
                {
                    string[] MultiCC = mailCC.Split(';', ',', ':', ' ');
                    foreach (var toMultiCC in MultiCC)
                    {
                        //mailto = tomail + ";";
                        message.Cc.Add(new MailboxAddress(toMultiCC));
                    }


                }


                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = body;

                message.Body = bodyBuilder.ToMessageBody();
                //message.Body = new TextPart("plain")
                //{
                //    //data
                //    Text = "ทดสอบบบบบบบบบบบบบบบบ"
                //};
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    var servers = server;
                    var ports = port;
                    var securitys = security;

                    client.Connect(servers, ports, securitys);
                    if (securitys == false)
                    {

                    }
                    else
                    {
                        client.Authenticate(userauten, pwdauten);
                    }

                    client.Send(message);
                    client.Disconnect(true);
                    Console.WriteLine("จดหมายถูกส่งเรียบร้อยแล้ว !!");
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
