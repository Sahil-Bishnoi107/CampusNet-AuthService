using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthService.Domain.Dtos
{
    public class NotificationMessage
    {

        [JsonPropertyName("to")]
        public string To { get; set; }
        [JsonPropertyName("subject")]
        public string? Subject { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("correlationId")]
        public string CorrelationId { get; set; }

        public NotificationMessage(string to, string subject, string message,string coid) {
            this.To = to;
            this.Subject = subject;
            this.Message = message;
            this.CorrelationId = coid;
        }
        
    }
}
