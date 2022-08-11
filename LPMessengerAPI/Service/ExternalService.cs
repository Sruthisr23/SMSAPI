using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using RestSharp;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace LPMessengerAPI.Service
{
    public class ExternalService : IExternalService
    {
        private readonly lpmessengerdbContext _context;
        private readonly IOptions<AppSettings> _appSettings;
        public ExternalService(lpmessengerdbContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }
        public string PostSms(SmsRequestDto model, string msgUniqueId)
        {
            try
            {
                
                SmsRequestAuthDto smsRequestAuthDto = new SmsRequestAuthDto();
                smsRequestAuthDto.UserName = _appSettings.Value.SmsUserName;
                smsRequestAuthDto.Password = _appSettings.Value.SmsPassword;
                smsRequestAuthDto.PhoneNumber = model.PhoneNumber;
                smsRequestAuthDto.SenderName = model.SenderName;
                smsRequestAuthDto.Message = model.Message;
                smsRequestAuthDto.MessageSendType = 1;
                smsRequestAuthDto.MsgUniqueId = msgUniqueId;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_appSettings.Value.SmsBaseUrl);
                    var url = _appSettings.Value.SmsBaseUrl+ _appSettings.Value.SmsSendUrl;
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
                    {
                        string jsonBody = JsonConvert.SerializeObject(smsRequestAuthDto);
                        request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                        //HttpResponseMessage response = client.SendAsync(request).Result;
                        using (HttpResponseMessage response = client.SendAsync(request).Result)
                        {                            
                            string responseString = response.Content.ReadAsStringAsync().Result;

                            //var transactionResponse = JsonConvert.DeserializeObject<dynamic>(responseString);

                            return responseString;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string PostwhatsAppToken()
        {
            var urlForm = _appSettings.Value.WhatsAppBaseUrl+ _appSettings.Value.WhatsAppTokenUrl;
            HttpClientHandler handler = new HttpClientHandler();
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(_appSettings.Value.WhatsAppUserName + ":" + _appSettings.Value.WhatsAppPassword));
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            try
            {
                HttpResponseMessage messge = client.PostAsync(urlForm, null).Result;
                var serverResponse = messge.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<dynamic>(serverResponse);
                return result.accessToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string postWhatsApp(string token, WhatsAppRequestDto model)
        {
            try
            {
                WhatsAppMainRequestDto whatsAppMainRequestDto = new WhatsAppMainRequestDto();

                string[] destinationPhoneNumbers = model.Phone.Split(',');

                whatsAppMainRequestDto.Title = model.Title;
                whatsAppMainRequestDto.Description = model.Description;
                whatsAppMainRequestDto.MessageTemplateId = model.TemplateId;
                whatsAppMainRequestDto.MediaMessage = model.MediaUrl;
                whatsAppMainRequestDto.TextMessage = model.Message;
                whatsAppMainRequestDto.DestinationPhoneNumbers = destinationPhoneNumbers;
                whatsAppMainRequestDto.RedirectUrl = _appSettings.Value.WhatsAppRedirectUrl;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    client.BaseAddress = new Uri(_appSettings.Value.WhatsAppBaseUrl);
                    var urlForm = _appSettings.Value.WhatsAppBaseUrl+ _appSettings.Value.WhatsAppSendUrl;
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, urlForm))
                    {
                        string jsonBody = JsonConvert.SerializeObject(whatsAppMainRequestDto);
                        request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                        using (HttpResponseMessage response = client.SendAsync(request).Result)
                        {
                            //if (!response.IsSuccessStatusCode)
                            //{
                            //    string responseContent = response.Content.ReadAsStringAsync().Result;
                            //    ExceptionMessage errMessage = JsonConvert.DeserializeObject<ExceptionMessage>(responseContent);
                            //    throw new Exception(errMessage.message);
                            //}
                            string responseString = response.Content.ReadAsStringAsync().Result;

                            var transactionResponse = JsonConvert.DeserializeObject<dynamic>(responseString);

                            return transactionResponse;
                        }
                    }
                }
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

        public async Task<string> RandomNumberAsync(int? length)
        {
            var start = new System.Text.StringBuilder();
            var end = new System.Text.StringBuilder();           
            for (int i = 1; i < length; i++)
            {
                start.Append(0.ToString());
                end.Append(9.ToString());
            }
            var s = "1"+(start.ToString());
            var e = "9"+(end.ToString());

            var random = new Random();
            int randomNumber = random.Next(Int32.Parse(s), Int32.Parse(e));
            return randomNumber.ToString();
        }

        public async Task<string> RandomAlphaNumeric(int? length)
        {
            //var random = new Random();
            Random RNG = new Random();
            const string range = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, (int)length).Select(x => range[RNG.Next(0, range.Length)]);
            
            return new string(chars.ToArray());
        }
        
        public async Task SaveFileInFolder(IFormFile formFile)
        {
            try
            {
                string filePath = _appSettings.Value.Location;

                using (var stream = System.IO.File.Create(@filePath + formFile.FileName))
                {
                    await formFile.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        public async Task DeleteFileInFolder(IFormFile formFile)
        {
            try
            {
                string filePath = _appSettings.Value.Location;

                if ((System.IO.File.Exists(@filePath + formFile.FileName)))
                {
                    System.IO.File.Delete(@filePath + formFile.FileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> MoveFileToFolder(string fullPath)
        {
            try
            {
                List<string> result = new List<string>();

                string filePath = _appSettings.Value.Location;

                string remoteUri = fullPath;  //"https://www.collinsdictionary.com/images/full/lp_51033703_1000.jpg";

                string[] subs = fullPath.Split('/');

                var sb = new System.Text.StringBuilder();

                for (var i = 0 ; i < subs.Length-1; i++)
                {
                    sb.Append(subs[i].ToString() + "/");
                }
                int length = subs.Length;
                int correctedLength = length - 1;
                string fileName = subs[correctedLength];
                if ((System.IO.File.Exists(@filePath + fileName)))
                {
                    return filePath + fileName;                    
                }
                else
                {
                    string myStringWebResource = null;
                    // Create a new WebClient instance.
                    WebClient myWebClient = new WebClient();
                    myStringWebResource = sb.ToString() + fileName;

                    myWebClient.DownloadFile(myStringWebResource, @filePath + fileName);
                    return filePath + fileName;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


//foreach (var data in subs)
//{
//    sb.Append(data+"/");
//}