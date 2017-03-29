using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Model
{

    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FacebookId { get; set; }

        public int ProfileImage { get; set; }

        public String DeviceToken { get; set; }
    }
}
