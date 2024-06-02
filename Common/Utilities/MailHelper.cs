using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;


namespace Common.Utilities
{
    /// <summary>
    /// 信件相關通用方法
    /// </summary>
    public class MailHelper
    {
        private readonly IConfiguration _configuration;


        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 異步寄送信件
        /// </summary>
        /// <param name="targetAddress">目標信箱</param>
        /// <param name="subject">信件主旨</param>
        /// <param name="body"> 信件內容</param>
        /// <returns></returns>
        public async Task SendMail(string targetAddress, string subject, string body)
        {
            string host = _configuration.GetValue<string>("MailServerSetting:Host"); // 送信郵件主機
            int port = _configuration.GetValue<int>("MailServerSetting:Post"); // 送信郵件主機連接埠
            string account = _configuration.GetValue<string>("MailServerSetting:Account"); // 帳號
            string password = _configuration.GetValue<string>("MailServerSetting:Password"); // 密碼
            string mailServerName = _configuration.GetValue<string>("MailServerSetting:MailServerName"); // 寄信者名稱
            string mailServerAddress = _configuration.GetValue<string>("MailServerSetting:MailServerAddress");  // 寄送者信箱

            MimeMessage message = new();
            message.From.Add(new MailboxAddress(mailServerName, mailServerAddress));
            message.To.Add(MailboxAddress.Parse(targetAddress));
            message.Subject = subject;

            BodyBuilder bodyBuilder = new()
            {
                HtmlBody = body
            };
            message.Body = bodyBuilder.ToMessageBody();


            using SmtpClient client = new();
            await client.ConnectAsync(host, port, false);
            await client.AuthenticateAsync(account, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

        }

    }
}
