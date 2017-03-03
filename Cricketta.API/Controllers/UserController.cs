using Cricketta.API.Models;
using Cricketta.Data.Base;
using Cricketta.Data.Data;
using Cricketta.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cricketta.API.Controllers
{
    public class UserController : ApiController
    {
        private IUserRepository userRepository;
        private IUnitofWork unitofWork;

        public UserController(IUnitofWork unitofWork, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.unitofWork = unitofWork;
        }

        public async Task<IHttpActionResult> Get()
        {
            return Ok(userRepository.GetAll());
        }

        public async Task<IHttpActionResult> Post(UserModel user)
        {
            var obj = userRepository.Add(new User
            {
                UserName = user.UserName,
                FacebookId = user.FacebookId,
                ProfileImage = user.ProfileImage
            });

            unitofWork.SaveChanges();
            return Ok(obj);
        }
    }
}
