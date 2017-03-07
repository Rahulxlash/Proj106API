using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cricketta.API.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string FacebookId { get; set; }
        public int  ProfileImage { get; set; }
    }
}