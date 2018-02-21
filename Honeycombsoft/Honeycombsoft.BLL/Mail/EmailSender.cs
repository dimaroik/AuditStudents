using Honeycombsoft.DAL.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Honeycombsoft.BLL.Mail
{
    public class EmailSender 
    {
        IUnitOfWork database;

        public EmailSender()
        {

        }
        public EmailSender(IUnitOfWork uow)
        {
            database = uow;
        }
        
     
        public static async Task SendEmail(string To ,string message)
        {

            MailMessage mm = new MailMessage("mityaroik@gmail.com", To);
            mm.Subject = "Hi, " + To;
            mm.Body = message;
            mm.IsBodyHtml = false;

            SmtpClient smpt = new SmtpClient();
            smpt.Host = "smtp.gmail.com";
            smpt.Port = 587;
            smpt.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("mityaroik@gmail.com", "135246_dima");
            smpt.UseDefaultCredentials = true;
            smpt.Credentials = nc;
            await smpt.SendMailAsync(mm);

        }

     
        public static async Task SendEmailAsync(string Email, string callbackUrl)
        {
            await SendEmail(Email ,
                       "Для завершения регистрации перейдите по ссылке:: <a href=\""
                                                       + callbackUrl + "\">завершить регистрацию</a>");
        }

    }
}
