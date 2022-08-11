using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service
{
    public class MailService : IMailService
    {
        private readonly lpmessengerdbContext _context;
        private IConfiguration _configuration;
        public MailService(lpmessengerdbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<MlMail> PostMail(MlMail mail)
        {
            try
            {
                //MlMail mlMail = new MlMail();

                //mlMail.ReceiverEmail = mail.ReceiverEmail;
                //mlMail.SenderName = mail.SenderName;
                //mlMail.SenderEmail = mail.SenderEmail; 
                //mlMail.Subject = mail.Subject;
                //mlMail.Body = mail.Body;
                //mlMail.IsBodyHtml = mail.IsBodyHtml;

                var result = _context.MlMails.Add(mail);

                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MlMail> UpdateMail(MlMail mail)
        {
            try
            {
                //MlMail mlMail = new MlMail();

                //mlMail.ReceiverEmail = mail.ReceiverEmail;
                //mlMail.SenderName = mail.SenderName;
                //mlMail.SenderEmail = mail.SenderEmail; 
                //mlMail.Subject = mail.Subject;
                //mlMail.Body = mail.Body;
                //mlMail.IsBodyHtml = mail.IsBodyHtml;
                                
                await _context.SaveChangesAsync();

                return mail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
        public async Task<List<MlMail>> GetMailListViaGroupAndTemplate(int? groupId, int? templateId)
        {
            List<MlMail> result = new List<MlMail>();
            try
            {
                 

                var template = await (from t in _context.TemplateMasters                                          
                                      where t.Id == templateId
                                      select t).FirstOrDefaultAsync();
                if(template != null)
                {
                    result = await (from m in _context.MlMails
                                        where m.GroupId == groupId
                                        select new MlMail
                                        {
                                            Id = m.Id,
                                            Body = template.Body,
                                            GroupId = m.GroupId,
                                            SenderEmail = m.SenderEmail,
                                            SenderName = m.SenderName,
                                            IsBodyHtml = template.IsBodyHtml == null ? false : (bool)(template.IsBodyHtml),
                                            ReceiverEmail = m.ReceiverEmail,
                                            Subject = template.Subject,
                                            TemplateId = m.TemplateId
                                        }).ToListAsync();

                }
                else
                {
                    result = await (from m in _context.MlMails
                                    where m.GroupId == groupId
                                    select new MlMail
                                    {
                                        Id = m.Id,
                                        Body = m.Body,
                                        GroupId = m.GroupId,
                                        SenderEmail = m.SenderEmail,
                                        SenderName = m.SenderName,
                                        IsBodyHtml = m.IsBodyHtml == null ? false : (bool)(m.IsBodyHtml),
                                        ReceiverEmail = m.ReceiverEmail,
                                        Subject = m.Subject,
                                        TemplateId = m.TemplateId
                                    }).ToListAsync();                    
                }
                                
                                
                                

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ValidateOtp(string otp, string referenceId)
        {
            try
            {
                var result = await (from m in _context.MlMails
                                    where m.Otp == otp
                                    && m.ReferenceId == referenceId
                                    select m).FirstOrDefaultAsync();

                if(result!=null && result.Id > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<MlMailDto>> GetGroupMailList(int? groupId)
        {
            try
            {
                var result = await (from m in _context.MlMails
                                    join g in _context.GroupMasters on m.GroupId equals g.Id
                                    where groupId == null || m.GroupId == groupId
                                    select new MlMailDto
                                    { 
                                        Id = m.Id,
                                        ReceiverEmail = m.ReceiverEmail,
                                        GroupId = m.GroupId,
                                        GroupName = g.Name
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
