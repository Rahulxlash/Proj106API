using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cricketta.API.Models
{
    public class RegisterDeviceModel
    {
        public int UserId { get; set; }
        public String DeviceToken { get; set; }
    }
}