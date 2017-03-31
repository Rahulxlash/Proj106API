using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cricketta.API.Models
{
    public class Notification
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public IPayload Payload{ get; set; }
    }

    public interface IPayload
    {
        string Tag { get; set; }
        int userId {get;set;}
        string click_action { get; set; }
    }
    
    public class TossPayload : IPayload
    {
        public string Tag { get; set; }
        public string click_action { get; set; }
        public int userId { get; set; }
        public int matchId { get; set; }
    }
}