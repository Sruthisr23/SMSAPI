using CsvHelper;
using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LPMessengerAPI.Service
{
    public class SmsService : ISmsService
    {
        private readonly lpmessengerdbContext _context;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ISmsSendService _smsSendService;
        private readonly IGroupMasterService _groupMasterService;
        public SmsService(lpmessengerdbContext context,
            IOptions<AppSettings> appSettings, ISmsSendService smsSendService, IGroupMasterService groupMasterService)
        {
            _context = context;
            _appSettings = appSettings;
            _smsSendService = smsSendService;
            _groupMasterService = groupMasterService;
        }

        public async Task<SmsTransactionMessageStatus> SendSms(SmsRequestDto smsRequestDto)
        {
            try
            {
                //if (!String.IsNullOrEmpty(smsRequestDto.PhoneNumber))
                {
                    var urlForm = _appSettings.Value.SmsBaseUrl;
                    HttpClientHandler handler = new HttpClientHandler();
                    //handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(_appSettings.Value.SmsUserName + ":" + _appSettings.Value.SmsPassword));
                    HttpClient client = new HttpClient(handler);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");

                    try 
                    {
                        HttpResponseMessage messge = client.GetAsync(urlForm).Result;
                        var serverResponse = messge.Content.ReadAsStringAsync().Result;
                        var response = JsonConvert.DeserializeObject<SmsTransactionMessageStatus>(serverResponse);
                        return (response);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }

            }
            catch(Exception ex)
            {
                throw ex; 
            }

            return null;
        }

        public async Task<string> ImportExcelFile(ExcelFileUploadDto excelFileUploadDto)
        {
            try
            {
                List<SmsExcelFileUploadDto> records = new List<SmsExcelFileUploadDto>();

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
                        records = csvReader.GetRecords<SmsExcelFileUploadDto>().ToList();
                    }

                }

                foreach (var data in records)
                {
                    //var maildetails = await (from m in _context.SmsSends
                    //                         where m.Phone == data.ReceiverPhone
                    //                         && m.GroupId != 0
                    //                         select m).FirstOrDefaultAsync();

                    if (null == null) //maildetails
                    {
                        SmsRequestDto smsdetail = new SmsRequestDto();
                        smsdetail.Message = data.Message;
                        smsdetail.PhoneNumber = data.ReceiverPhone;

                        var sendmail = await _smsSendService.PostSmsSend(smsdetail, excelFileUploadDto.GroupId);

                    }
                    //else
                    //{
                    //    return "sms exist for the same group";
                    //}
                }

                return "Sms uploaded successfully";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SmsRequestDto>> GetGroupmessage(int? groupId, int? templateId)//
        {
            try
            {
                var sendmail = await (from s in _context.SmsSends
                                      join t in _context.TemplateMasters on s.TemplateId equals t.Id
                                      where s.GroupId == groupId && s.TemplateId == templateId
                                      select  new SmsRequestDto
                                      {
                                          SenderName = "LucidPlus",
                                          Message = t.Body,
                                          PhoneNumber = s.Phone.ToString()
                                      }).ToListAsync();

                return sendmail;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> RandomNumber()
        {
            var random = new Random();
            int randomNumber = random.Next(100000, 999999);
            return randomNumber;
        }

    }
}
