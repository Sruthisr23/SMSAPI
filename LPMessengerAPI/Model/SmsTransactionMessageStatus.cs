using System;
using System.Collections.Generic;

#nullable disable

namespace LPMessengerAPI.Model
{
    public partial class SmsTransactionMessageStatus
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string RequestId { get; set; }
        public string TransactionId { get; set; }
        public string PhoneNumber { get; set; }
        public string MessageText { get; set; }
        public int? SendType { get; set; }
        public int? ServiceChannel { get; set; }
        public int? MessageDlrStatus { get; set; }
        public string Otp { get; set; }
        public string RefferenceNumber { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? DeliveredOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
