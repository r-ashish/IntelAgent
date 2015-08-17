//using System;
//using System.Collections.Generic;
//using System.Linq;
////using System.Net.Mail;
//using System.Text;
//using System.Threading.Tasks;

//namespace CodeFunDo
//{
//    class email
//    {
//        private static void sendEmail()
//        {
//            try
//            {
//                MailMessage mail = new MailMessage();
//                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

//                mail.From = new MailAddress("farzi.mail.hai@gmail.com");
//                mail.To.Add("ashishranjan2912@gmail.com");
//                mail.Subject = "Test Mail";
//                mail.Body = "This is for testing SMTP mail from GMAIL";

//                SmtpServer.Port = 587;
//                SmtpServer.Credentials = new System.Net.NetworkCredential("farzi.mail.hai", "farzi123");
//                SmtpServer.EnableSsl = true;

//                SmtpServer.Send(mail);
//                // Console.WriteLine("mail Sent");
//            }
//            catch (Exception ex)
//            {
//                //Console.WriteLine(ex.ToString());
//            }
//            //Console.ReadLine();
//        }
//    }
//}
