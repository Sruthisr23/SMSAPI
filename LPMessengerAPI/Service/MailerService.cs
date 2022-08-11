using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using System.IO;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace LPMessengerAPI.Service
{
    public class MailerService : IMailerService
    {
        private readonly string _token;
        private readonly lpmessengerdbContext _context;
        private IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IGroupMasterService _groupMasterService;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IExternalService _externalService;
        public MailerService(lpmessengerdbContext context, 
            IConfiguration configuration,
            IMailService mailService,
            IGroupMasterService groupMasterService,
            IOptions<AppSettings> appSettings,
            IExternalService externalService)
        {
            _context = context;
            _configuration = configuration;
            _mailService = mailService;
            _groupMasterService = groupMasterService;
            _appSettings = appSettings;
            _externalService = externalService;
        }

        public async Task<MlMail> SaveMail(MailDto mail, string referenceId)
        {
            try
            {               
                MlMail mlMail = new MlMail();

                mlMail.ReceiverEmail = mail.ReceiverEmail;
                mlMail.SenderName = mail.SenderName;
                mlMail.SenderEmail = mail.SenderEmail;
                mlMail.Subject = mail.Subject;
                mlMail.Body = mail.Body;
                mlMail.IsBodyHtml = mail.IsBodyHtml;
                mlMail.ReferenceId = referenceId;

                var result = _context.MlMails.Add(mlMail);

                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendMail(MlMail mail, List<IFormFile> attachment, List<string> attachmentPath)
        {
            #region temporary data
            String FROM = "sales@lucidplus.com";// "Binu.Paul@Lucidplus.com";//"sales @lucidplus.com";
            String FROMNAME = "sales lucidplus";

            #region BODY Content
            //String BODY =
            //    "<p>Hi there,<br><br>Hope you are doing fine. I wanted to take this opportunity to introduce you to <b>LucidPlus IT Solutions</b> – the company over eleven years’ experience in the Fixed Asset Management Solutions market.<br><br> We are the developers of <b>Ensemble</b>, an end-to-end asset management solution for businesses of all sizes.<br><br> As you are no doubt aware, proactive asset management can save your company a lot of money over the next few years – through regular tracking and audit of your assets, timely servicing and warranty renewal and making proactive plans for replacement of obsolete assets. <br><br>Over the last several years, we have helped a number of businesses (both big and small) save both time and manpower efforts by automating their asset management.<br><br> We have a highly experienced professional team of engineers and technology professionals who are dedicated to providing state-of-the-art, custom solutions to help our clients improve their business processes.<br><br> We can confidently say that Ensemble can eliminate all your asset management problems and make it a simple, cost-effective and seamless process.<br><br>We would be delighted to provide you a free demo of Ensemble. Just head over to our <a href='https://lp-ensemble.com/'>website</a> and let us know what you are looking for and our experts will get in touch with you to help you. You can also send an email to sales@lucidplus.com.<br><br>Link to request a free demo: <a href='https://lp-ensemble.com/'>https://lp-ensemble.com/</a>. <br><br>Looking forward to working with you.<br><br>Best Regards,<br><br>Manager Sales<br><br>Lucid Plus It Solutions<br><br></p>";
            #endregion BODY Content
            //// Replace recipient@example.com with a "To" address. If your account 
            //// is still in the sandbox, this address must be verified.
            ////String TO = "solomon.s@lucidplus.com";

            //String TO = "joseph.eldho@lucidplus.com"; //model.To; //

            //// The subject line of the email
            //String SUBJECT =
            //    "The Perfect Solution For All Your Asset Management Issues";

            //// The body of the email

            //Create and build a new MailMessage object
            //MailMessage message = new MailMessage();
            //message.IsBodyHtml = true;
            //message.From = new MailAddress(FROM, FROMNAME);
            //message.To.Add(new MailAddress(TO));
            //message.Subject = SUBJECT;
            //message.Body = BODY;

            //String SMTP_USERNAME = "AKIA2FR4PHD7IVO3Y27K";

            //String SMTP_PASSWORD = "BErb6iyJM9P8XUwHiuCC52CzdD3dOeTijFnadZE+ImZz";

            //String HOST = "email-smtp.us-east-2.amazonaws.com";

            //int PORT = 587;

            //bool EnableSslEncryption = true;
            #endregion
            MailMessage message = new MailMessage();

            if (attachment == null && attachmentPath != null && attachmentPath.Count > 0)
            {
                foreach (var attach in attachmentPath)
                {
                    var localPath = await _externalService.MoveFileToFolder(attach);

                    Attachment data = new Attachment(localPath, MediaTypeNames.Application.Octet);
                    //message.Attachments.Add(); //"https://www.collinsdictionary.com/images/full/lp_51033703_1000.jpg"
                    message.Attachments.Add(data);
                }
            }
            else if (attachment != null)
            {
                string[] path = await SaveFileInFolder(attachment);

                foreach (var attach in path)
                {
                    Attachment data = new Attachment(attach, MediaTypeNames.Application.Octet);
                    //message.Attachments.Add(new Attachment("https://www.collinsdictionary.com/images/full/lp_51033703_1000.jpg"));
                    message.Attachments.Add(data);
                }
            }


            //Attachment data = new Attachment("C:\\Joseph\\22072022\\whatsapp.csv", MediaTypeNames.Application.Octet);
            message.IsBodyHtml = true; //mail.IsBodyHtml;
            message.From = new MailAddress(FROM, FROMNAME);
            message.To.Add(new MailAddress(mail.ReceiverEmail));
            message.Subject =  mail.Subject;
            message.Body =  mail.Body;
            //message.ReplyToList.Add(new MailAddress(mail.ReceiverEmail));
            //  message.Attachments.Add(data);


            String SMTP_USERNAME = _appSettings.Value.SmtpUserName;

            String SMTP_PASSWORD = _appSettings.Value.SmtpPassword;

            String HOST = _appSettings.Value.MailHost;

            int PORT = _appSettings.Value.MailPort;          

            bool EnableSslEncryption = _appSettings.Value.EnableSslEncryption;

            using (var client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                // Enable SSL encryption
                client.EnableSsl = EnableSslEncryption;

                try
                {
                    client.Send(message);
                    await SendMailStatus(mail.Id, true);
                }
                catch(Exception ex)
                {
                    await SendMailStatus(mail.Id, false);
                }
            }
        }
        public async Task SendMailStatus(int? mlmailId, bool status)
        {
            try
            {
                MlSendmail mlSendmail = new MlSendmail();
                mlSendmail.MailId = mlmailId;
                mlSendmail.MailSendStatus = status;
                _context.MlSendmails.Add(mlSendmail);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<string> ImportExcelFile(ExcelFileUploadDto excelFileUploadDto)
        {
            try
            {
                List<ExcelMailTemplateDtos> records = new List<ExcelMailTemplateDtos>();

                if (excelFileUploadDto.GroupId == 0 || excelFileUploadDto.GroupId == null)
                {
                    GroupMaster groupMaster = new GroupMaster();

                    groupMaster.Name = excelFileUploadDto.GroupName;
                    groupMaster.ServiceId = excelFileUploadDto.serviceId;

                    var groupResult = await _groupMasterService.SaveGroup(groupMaster);

                    if (groupResult != null && groupResult.Id != 0)
                    {
                        excelFileUploadDto.GroupId = groupResult.Id;
                    }
                }

                string baseLocation = _appSettings.Value.Location;

                using (var steram = new StreamReader(@baseLocation + excelFileUploadDto.FileName))
                {
                    using (var csvReader = new CsvReader(steram, System.Globalization.CultureInfo.InvariantCulture))
                    {
                        records = csvReader.GetRecords<ExcelMailTemplateDtos>().ToList();
                    }

                }

                foreach (var data in records)
                {
                    var maildetails = await (from m in _context.MlMails
                                             where m.ReceiverEmail == data.EMAIL
                                             && m.GroupId == excelFileUploadDto.GroupId
                                             && m.GroupId != 0
                                             select m).FirstOrDefaultAsync();

                    if (maildetails == null)
                    {
                        MlMail maildetail = new MlMail();
                        maildetail.ReceiverEmail = data.EMAIL;
                        maildetail.GroupId = excelFileUploadDto.GroupId;
                        maildetail.IsBodyHtml = true;

                        var sendmail = await _mailService.PostMail(maildetail);
                        //var sendmails =  SendMail(maildetail);

                    }
                    //else
                    //{
                    //    return "Mails exist for the same group";
                    //}
                }

                return "Mails uploaded successfully";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string[]> SaveFileInFolder(List<IFormFile> formFiles)
        {
            try
            {
                string filePath = _appSettings.Value.Location;

                List<string> path = new List<string>();

                foreach (var formFile in formFiles)
                {
                    using (var stream = System.IO.File.Create(@filePath + formFile.FileName))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    path.Add(filePath + formFile.FileName);
                }

                return path.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteFileInFolder(List<IFormFile> formFiles)
        {
            try
            {
                string filePath = _appSettings.Value.Location;

                foreach (var formFile in formFiles)
                {
                    if ((System.IO.File.Exists(@filePath + formFile.FileName)))
                    {
                        System.IO.File.Delete(@filePath + formFile.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
