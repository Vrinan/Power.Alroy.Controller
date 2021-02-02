using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Power.Alroy.Controller
{
    public class MailHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="MessageFrom">发件人邮箱</param>
        /// <param name="MessageTo">收件人邮箱</param>
        /// <param name="MessageSubject">主题</param>
        /// <param name="MessageBody">内容</param>
        /// <returns></returns>
        public bool SendMail(MailAddress MessageFrom, string MessageTo, string MessageSubject, string MessageBody)   //发送验证邮件
        {
            MailMessage message = new MailMessage();
            message.To.Add(MessageTo);
            message.From = MessageFrom;
            message.Subject = MessageSubject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.Body = MessageBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true; //是否为html格式 
            message.Priority = MailPriority.High; //发送邮件的优先等级 
            SmtpClient sc = new SmtpClient();
            sc.EnableSsl = true;//是否SSL加密

            //sc.UseDefaultCredentials = true;
            //sc.DeliveryMethod= SmtpDeliveryMethod.Network;//指定电子邮件发送方式        

            sc.Host = "smtp.qq.com"; //指定发送邮件的服务器地址或IP 
            sc.Port = 587; //指定发送邮件端口 
            sc.Credentials = new System.Net.NetworkCredential("824559791@qq.com", "elggfvpbrvtnbfbe"); //指定登录服务器的用户名和密码(注意：这里的密码是开通上面的pop3/smtp服务提供给你的授权密码，不是你的qq密码)
            try
            {
                sc.Send(message); //发送邮件 
            }
            catch (Exception e)
            {
                //HttpResponse对象
                //Response.Write(e.Message);
                return false;
            }
            return true;
        }
    }
}
