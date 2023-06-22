using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using DataService.Repositories.UnitOfWork;
using MimeKit;
using System.IO;


namespace Services.TokenServices
{
    public interface ITokenService
    {
        Task<bool> SendMailConfirmAsync(string mailReciver, string tokenEmail);
        Task<bool> ConfirmEmailAsync(string mailToken);
        Task<bool> ReSendMailConfirmAsync(string mailReciver);
    }
    public class TokenService : ITokenService
    {
        private readonly string PathConfirmApiUrl = "https://localhost:7031/api/Employee/ConfirmEmail?mailToken=";
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uow;
        public TokenService(IConfiguration configuration, IUnitOfWork unitOfWork) { 
            _configuration = configuration;
            _uow = unitOfWork;
        }  

        public async Task<bool> SendMailConfirmAsync(string mailReciver, string tokenEmail)
        {
            if (!string.IsNullOrEmpty(mailReciver) && !string.IsNullOrEmpty(tokenEmail))
            {
                try
                {
                    var urlApiConfimMail = PathConfirmApiUrl + tokenEmail;
                    var email = new MimeMessage();
                    email.From.Add(new MailboxAddress("Sender", _configuration["Accountmail:Username"]));
                    email.To.Add(new MailboxAddress("Recipient", mailReciver));
                    email.Subject = "Confirm Email";

                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = GetEmailBody(mailReciver, urlApiConfimMail);

                    email.Body = bodyBuilder.ToMessageBody();

                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);
                        client.Authenticate(_configuration["Accountmail:Username"], _configuration["Accountmail:Password"]);
                        client.Send(email);
                        client.Disconnect(true);                      
                    }
                    return true;
                }
                catch (Exception ex)
                {           
                    throw new ArgumentException("Error when send mail");                
                }
               
            }
            else return false;
       
        }

        private string GetEmailBody(string email,string pathApiConfirm)
        {
            string buttonHtml = $@" <table border=""0"" cellpadding=""0"" cellspacing=""0"" id=""divMain"">
                                    <tr>
                                        <td align=""center"" bgcolor=""#E5E5E5"" style=""border-radius: 5px;"">
                                            <a href=""{pathApiConfirm}"" target=""_blank"" style=""padding: 10px 20px; font-size: 16px; font-family: Arial, sans-serif; color: black; text-decoration: none; display: inline-block; border-radius: 5px;""><span style='color: darkgreen;font-size: large;'>✔ </span> Yes, It mine.</a>                                                                                
                                        </td>
                                    </tr>
                                    <tr style=""height: 10px;""></tr>
                                    <tr>
                                        <td align=""center"" bgcolor=""#E5E5E5"" style=""border-radius: 5px;"">
                                            <a href=""{pathApiConfirm}"" target=""_blank"" style=""padding: 10px 20px; font-size: 16px; font-family: Arial, sans-serif; color: black; text-decoration: none; display: inline-block; border-radius: 5px;""><span style='color: darkred;font-size: large;'>✘ </span> No, It not me!</a>                                                                                
                                        </td>
                                    </tr>
                                </table>";

            string emailBody = @"<!DOCTYPE html>
                            <html>
                            <head>
                                <style>
                                     #divMain a{
                                            opacity: 0.7;
                                        }
                                        #divMain a:hover{
                                            opacity: 1;
                                        }
                                </style>
                            </head>
                            <body>
                                <h1>Confirm Email "+email+@", To Sign-Up Account.</h1>
                                <p>(if this is not you - Please click 'No')</p>
                                " + buttonHtml + @"
                            </body>
                            </html>";

            return emailBody;
        }

        public async Task<bool> ReSendMailConfirmAsync(string mailReciver)
        {
            var currentEmployee = await _uow.Employees.FirstOfDefaultAsync(p => p.Email == mailReciver, "Acc");
            if (currentEmployee != null)
            {
                var currentToken = await _uow.Tokens.FirstOfDefaultAsync(p => p.AccId == currentEmployee.AccId && !currentEmployee.Acc.IsActive && p.EtcreatedDate < DateTime.Now);
                if (currentToken != null)
                {
                    string newEToken = Guid.NewGuid().ToString();                 
                    currentToken.EmailToken = newEToken;
                    currentToken.EtcreatedDate = DateTime.Now;
                    _uow.Tokens.Udpate(currentToken);
                    await _uow.SaveAsync();
                    await SendMailConfirmAsync(currentEmployee.Email, newEToken);
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> ConfirmEmailAsync(string mailToken)
        {
            var currentToken = await _uow.Tokens.FirstOfDefaultAsync(p => p.EmailToken == mailToken);
            if (currentToken != null)
            {
                var expiredTime = currentToken.EtcreatedDate.AddMinutes(15);
                if(currentToken.EtcreatedDate <= expiredTime)
                {
                    currentToken.EtisActive = true;
                    _uow.Tokens.Udpate(currentToken);
                    await _uow.SaveAsync();
                    return true;
                }else return false;
            }
            else return false;
        }
    }
}
