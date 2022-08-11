using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service
{
    public class WhatsAppSendService : IWhatsAppSendService
    {
        private readonly lpmessengerdbContext _context;
        public WhatsAppSendService(lpmessengerdbContext context)
        {
            _context = context;
        }

        public async Task<WhatsappSend> PostWhatsAppSend(WhatsAppRequestDto whatsAppRequestDto)
        {
            try
            {
                WhatsappSend whatsappSend = new WhatsappSend();

                whatsappSend.Phone = Int64.Parse(whatsAppRequestDto.Phone);
                whatsappSend.Message = whatsAppRequestDto.Message;
                whatsappSend.MediaUrl = whatsAppRequestDto.MediaUrl;
                whatsappSend.MessageType = whatsAppRequestDto.MessageType;
                whatsappSend.TemplateId = whatsAppRequestDto.TemplateId;
                whatsappSend.Description = whatsAppRequestDto.Description;
                whatsappSend.Title = whatsAppRequestDto.Title;

                var result =  _context.WhatsappSends.Add(whatsappSend);

                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<WhatsappSend> PostWhatsAppBulkSend(WhatsappSend whatsAppRequestDto)
        {
            try
            {
                WhatsappSend whatsappSend = new WhatsappSend();

                whatsappSend.Phone = whatsAppRequestDto.Phone;
                whatsappSend.Message = whatsAppRequestDto.Message;
                whatsappSend.MediaUrl = whatsAppRequestDto.MediaUrl;
                whatsappSend.MessageType = whatsAppRequestDto.MessageType;
                whatsappSend.TemplateId = whatsAppRequestDto.TemplateId;
                whatsappSend.Description = whatsAppRequestDto.Description;
                whatsappSend.Title = whatsAppRequestDto.Title;
                whatsappSend.IsSend = false;
                whatsappSend.SenderPhoneCofigId = whatsAppRequestDto.SenderPhoneCofigId;
                whatsappSend.ChatTemplateId = whatsAppRequestDto.ChatTemplateId;

                var result = _context.WhatsappSends.Add(whatsappSend);

                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<WhatsAppRequestDto>> GetWhatsAppSendData(int? groupId)
        {
            try
            {
                var result = await (from w in _context.WhatsappSends
                                    where w.IsSend == false && w.GroupId == groupId
                                    select new WhatsAppRequestDto
                                    {
                                        Description = w.Description,
                                        Message = w.Message,
                                        MediaUrl = w.MediaUrl,
                                        MessageType = w.MessageType,
                                        Phone = w.Phone.ToString(),
                                        TemplateId = w.TemplateId,
                                        Title = w.Title
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
